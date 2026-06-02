using System;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading.Tasks;

public sealed class BackupMysql
{
    private const int KeySizeBits       = 256;
    private const int BlockSizeBits     = 128;
    private const int KdfIterations     = 100_000; 
    private const int StreamBufferBytes = 256 * 1024;
    
    public static void ExecutarBackup(
        string connectionString,
        string caminhoMysqlDump,
        string caminhoDestinoBackup,
        string senhaCriptografia)
    {
        var (servidor, nomeBanco, usuario, senhaBanco, porta) =
            ParseConnectionString(connectionString);

        ValidarExecutavel(caminhoMysqlDump);

        byte[] salt  = GerarBytesAleatorios(16);
        byte[] iv    = GerarBytesAleatorios(16);
        byte[] chave = DerivarChave(senhaCriptografia, salt);

        var startInfo = CriarProcessInfo(caminhoMysqlDump, $"-h {servidor} -P {porta} -u {usuario} {nomeBanco}", senhaBanco,
            redirectStdIn:  false,
            redirectStdOut: true,
            redirectStdErr: true);

        using (var process = new Process { StartInfo = startInfo })
        using (var fsDest  = new FileStream(caminhoDestinoBackup, FileMode.Create, FileAccess.Write, FileShare.None, StreamBufferBytes))
        {
            fsDest.Write(salt, 0, salt.Length);
            fsDest.Write(iv,   0, iv.Length);

            using (var aes       = CriarAes(chave, iv))
            using (var encryptor = aes.CreateEncryptor())
                
            using (var cryptoStream = new CryptoStream(fsDest, encryptor, CryptoStreamMode.Write))
            using (var zipStream    = new GZipStream(cryptoStream, CompressionMode.Compress))
            {
                process.Start();

                var stderrTask = Task.Run(() => process.StandardError.ReadToEnd());

                using (var stdOut = process.StandardOutput.BaseStream)
                {
                    stdOut.CopyTo(zipStream, StreamBufferBytes);
                }

                string stderr = stderrTask.Result;
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new BackupException($"mysqldump falhou (exit {process.ExitCode}): {stderr}");
            }
        }
    }
    
    public static void ExecutarImportacao(
        string connectionString,
        string caminhoMysql,
        string caminhoOrigemBackup,
        string senhaCriptografia)
    {
        var (servidor, nomeBanco, usuario, senhaBanco, porta) =
            ParseConnectionString(connectionString);

        ValidarExecutavel(caminhoMysql);

        if (!File.Exists(caminhoOrigemBackup))
            throw new FileNotFoundException("Arquivo de backup não encontrado.", caminhoOrigemBackup);

        using (var fsBackup = new FileStream(
                   caminhoOrigemBackup,
                   FileMode.Open, FileAccess.Read, FileShare.Read,
                   StreamBufferBytes))
        {
            byte[] salt = LerBytesExatos(fsBackup, 16);
            byte[] iv = LerBytesExatos(fsBackup, 16);
            byte[] chave = DerivarChave(senhaCriptografia, salt);

            var startInfo = CriarProcessInfo(caminhoMysql, $"-h {servidor} -P {porta} -u {usuario} {nomeBanco}", senhaBanco,
                redirectStdIn:  true,
                redirectStdOut: false,
                redirectStdErr: true);

            using (var aes = CriarAes(chave, iv))
            using (var decryptor = aes.CreateDecryptor())
            using (var cryptoStream = new CryptoStream(fsBackup, decryptor, CryptoStreamMode.Read))
            using (var zipStream = new GZipStream(cryptoStream, CompressionMode.Decompress))
            using (var process = new Process { StartInfo = startInfo })
            {
                process.Start();

                var stderrTask = Task.Run(() => process.StandardError.ReadToEnd());

                using (var stdIn = process.StandardInput.BaseStream)
                {
                    zipStream.CopyTo(stdIn, StreamBufferBytes);
                }

                string stderr = stderrTask.Result;
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new BackupException(
                        $"mysql.exe falhou (exit {process.ExitCode}): {stderr}");
            }
        }
    }
    
    private static Tuple<string, string, string, string, string>
        ParseConnectionString(string connectionString)
    {
        var b = new DbConnectionStringBuilder { ConnectionString = connectionString };

        string nomeBanco = b.ContainsKey("DATABASE") ? b["DATABASE"].ToString() : string.Empty;
        if (string.IsNullOrWhiteSpace(nomeBanco))
            throw new ArgumentException("Connection string não contém DATABASE.");

        return Tuple.Create(
            b.ContainsKey("SERVER") ? b["SERVER"].ToString() : "localhost",
            nomeBanco,
            b.ContainsKey("UID") ? b["UID"].ToString() : "root",
            b.ContainsKey("PWD") ? b["PWD"].ToString() : string.Empty,
            b.ContainsKey("PORT") ? b["PORT"].ToString() : "3306"
        );
    }

    private static void ValidarExecutavel(string caminho)
    {
        if (!File.Exists(caminho))
            throw new FileNotFoundException("Executável não encontrado.", caminho);
    }

    private static ProcessStartInfo CriarProcessInfo(
        string fileName, string arguments, string senhaBanco,
        bool redirectStdIn, bool redirectStdOut, bool redirectStdErr)
    {
        var info = new ProcessStartInfo
        {
            FileName = fileName,
            Arguments = arguments,
            RedirectStandardInput = redirectStdIn,
            RedirectStandardOutput = redirectStdOut,
            RedirectStandardError = redirectStdErr,
            UseShellExecute = false,
            CreateNoWindow = true
        };
        
        info.EnvironmentVariables["MYSQL_PWD"] = senhaBanco;

        return info;
    }

    private static byte[] DerivarChave(string senha, byte[] salt)
    {
        using (var rfc = new Rfc2898DeriveBytes(senha, salt, KdfIterations))
        {
            return rfc.GetBytes(KeySizeBits / 8);
        }
    }

    private static Aes CriarAes(byte[] chave, byte[] iv)
    {
        var aes = Aes.Create();
        aes.KeySize = KeySizeBits;
        aes.BlockSize = BlockSizeBits;
        aes.Key = chave;
        aes.IV = iv;
        aes.Mode = CipherMode.CBC;
        aes.Padding = PaddingMode.PKCS7;
        return aes;
    }

    private static byte[] GerarBytesAleatorios(int tamanho)
    {
        byte[] bytes = new byte[tamanho];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(bytes);
        }
        return bytes;
    }
    
    private static byte[] LerBytesExatos(Stream stream, int count)
    {
        byte[] buffer = new byte[count];
        int lido = 0;
        while (lido < count)
        {
            int n = stream.Read(buffer, lido, count - lido);
            if (n == 0)
                throw new EndOfStreamException($"Backup corrompido: esperado {count} bytes no cabeçalho, lido {lido}.");
            lido += n;
        }
        return buffer;
    }
}

public sealed class BackupException : Exception
{
    public BackupException(string message) : base(message) { }
}
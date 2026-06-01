using System;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Data.Common;

namespace AssisTec.Repository
{
    public class BackupRepository
    {
        private readonly string _connectionString;
        private readonly string _mysqldumpPath;
        private readonly string _mysqlPath;

        public BackupRepository(
            string connectionString,
            string mysqldumpPath = @"C:\xampp\mysql\bin\mysqldump.exe",
            string mysqlPath     = @"C:\xampp\mysql\bin\mysql.exe")
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException("connectionString");

            _connectionString = connectionString;
            _mysqldumpPath    = mysqldumpPath;
            _mysqlPath        = mysqlPath;
        }

        public void CriarBackup(string caminhoDestino, string senhaCriptografia)
        {
            ValidarParametros(caminhoDestino, senhaCriptografia);

            DbConnectionStringBuilder builder = CriarConnectionStringBuilder(_connectionString);
            string tempFile = Path.Combine(Path.GetTempPath(), string.Format("bkp_{0}.tmp", Guid.NewGuid().ToString("N")));

            try
            {
                ExecutarMysqldump(builder, tempFile);

                if (!File.Exists(tempFile) || new FileInfo(tempFile).Length == 0)
                    throw new InvalidOperationException("O arquivo de backup gerado pelo MySQL está vazio ou não foi encontrado.");

                CriptografarArquivo(tempFile, caminhoDestino, senhaCriptografia);
            }
            finally
            {
                DeletarArquivoSeguro(tempFile);
            }
        }

        public void ImportarBackup(string caminhoOrigem, string senhaCriptografia)
        {
            ValidarParametros(caminhoOrigem, senhaCriptografia);

            if (!File.Exists(caminhoOrigem))
                throw new FileNotFoundException("Arquivo de backup não encontrado.", caminhoOrigem);

            DbConnectionStringBuilder builder = CriarConnectionStringBuilder(_connectionString);
            string tempFile = Path.Combine(Path.GetTempPath(), string.Format("imp_{0}.tmp", Guid.NewGuid().ToString("N")));

            try
            {
                DescriptografarArquivo(caminhoOrigem, tempFile, senhaCriptografia);
                ExecutarMysqlImport(builder, tempFile);
            }
            finally
            {
                DeletarArquivoSeguro(tempFile);
            }
        }

        private void ExecutarMysqldump(DbConnectionStringBuilder builder, string arquivoTemp)
        {
            string server   = ObterValor(builder, "server", "data source", "localhost");
            string port     = ObterValor(builder, "port", "port", "3306");
            string user     = ObterValor(builder, "uid", "user id", "");
            string database = ObterValor(builder, "database", "initial catalog", "");
            string password = ObterValor(builder, "pwd", "password", "");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName               = _mysqldumpPath,
                Arguments              = string.Format("-h {0} -P {1} -u {2} {3}", server, port, user, database),
                RedirectStandardOutput = true,
                RedirectStandardError  = true,
                UseShellExecute        = false,
                CreateNoWindow         = true
            };

            psi.EnvironmentVariables["MYSQL_PWD"] = password;

            using (Process process = Process.Start(psi))
            {
                if (process == null)
                    throw new InvalidOperationException("Não foi possível iniciar o mysqldump.");

                using (FileStream fs = new FileStream(arquivoTemp, FileMode.Create, FileAccess.Write))
                {
                    process.StandardOutput.BaseStream.CopyTo(fs);
                }

                string erros = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new InvalidOperationException("mysqldump falhou: " + erros);
            }
        }

        private void ExecutarMysqlImport(DbConnectionStringBuilder builder, string arquivoTemp)
        {
            string server   = ObterValor(builder, "server", "data source", "localhost");
            string port     = ObterValor(builder, "port", "port", "3306");
            string user     = ObterValor(builder, "uid", "user id", "");
            string database = ObterValor(builder, "database", "initial catalog", "");
            string password = ObterValor(builder, "pwd", "password", "");

            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName              = _mysqlPath,
                Arguments             = string.Format("-h {0} -P {1} -u {2} {3}", server, port, user, database),
                RedirectStandardInput = true,
                RedirectStandardError = true,
                UseShellExecute       = false,
                CreateNoWindow        = true
            };

            psi.EnvironmentVariables["MYSQL_PWD"] = password;

            using (Process process = Process.Start(psi))
            {
                if (process == null)
                    throw new InvalidOperationException("Não foi possível iniciar o mysql.");

                using (FileStream fs = new FileStream(arquivoTemp, FileMode.Open, FileAccess.Read))
                {
                    fs.CopyTo(process.StandardInput.BaseStream);
                }

                process.StandardInput.Close();

                string erros = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                    throw new InvalidOperationException("mysql import falhou: " + erros);
            }
        }

        private static void CriptografarArquivo(string origem, string destino, string senha)
        {
            byte[] salt = new byte[32];
            byte[] iv = new byte[16];

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(salt);
                rng.GetBytes(iv);
            }

            using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
            {
                DerivarChave(aes, senha, salt, iv);

                using (FileStream fsOutput = new FileStream(destino, FileMode.Create, FileAccess.Write))
                {
                    fsOutput.Write(salt, 0, salt.Length);
                    fsOutput.Write(iv, 0, iv.Length);

                    using (CryptoStream cs = new CryptoStream(fsOutput, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fsInput = new FileStream(origem, FileMode.Open, FileAccess.Read))
                        {
                            fsInput.CopyTo(cs);
                        }
                    }
                }
            }
        }

        private static void DescriptografarArquivo(string origem, string destino, string senha)
        {
            using (FileStream fsInput = new FileStream(origem, FileMode.Open, FileAccess.Read))
            {
                byte[] salt = new byte[32];
                byte[] iv = new byte[16];

                if (fsInput.Read(salt, 0, salt.Length) != salt.Length ||
                    fsInput.Read(iv, 0, iv.Length) != iv.Length)
                    throw new InvalidDataException("Arquivo de backup corrompido ou inválido.");

                using (AesCryptoServiceProvider aes = new AesCryptoServiceProvider())
                {
                    DerivarChave(aes, senha, salt, iv);

                    using (CryptoStream cs = new CryptoStream(fsInput, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (FileStream fsOutput = new FileStream(destino, FileMode.Create, FileAccess.Write))
                        {
                            cs.CopyTo(fsOutput);
                        }
                    }
                }
            }
        }

        private static void DerivarChave(AesCryptoServiceProvider aes, string senha, byte[] salt, byte[] iv)
        {
            using (Rfc2898DeriveBytes rfc = new Rfc2898DeriveBytes(senha, salt, 100000))
            {
                aes.Key = rfc.GetBytes(32);
                aes.IV = iv;
            }
        }

        private static DbConnectionStringBuilder CriarConnectionStringBuilder(string connectionString)
        {
            return new DbConnectionStringBuilder { ConnectionString = connectionString };
        }

        private static string ObterValor(DbConnectionStringBuilder builder, string chavePrimaria, string chaveSecundaria, string valorPadrao)
        {
            object valor;
            if (builder.TryGetValue(chavePrimaria, out valor))
                return valor.ToString();

            if (builder.TryGetValue(chaveSecundaria, out valor))
                return valor.ToString();

            return valorPadrao;
        }

        private static void ValidarParametros(string caminho, string senha)
        {
            if (string.IsNullOrWhiteSpace(caminho))
                throw new ArgumentNullException("caminho");
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentNullException("senha");
        }

        private static void DeletarArquivoSeguro(string caminho)
        {
            try { if (File.Exists(caminho)) File.Delete(caminho); }
            catch { }
        }
    }
}
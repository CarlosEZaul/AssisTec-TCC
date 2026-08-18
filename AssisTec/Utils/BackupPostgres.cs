using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;
using Npgsql;

public sealed class BackupPostgres
{
    private const int KeySizeBits = 256;
    private const int BlockSizeBits = 128;
    private const int KdfIterations = 100_000;
    private const int StreamBufferBytes = 256 * 1024;

    public static void ExecutarBackup(
        string connectionString,
        string caminhoDestinoBackup,
        string senhaCriptografia)
    {
        byte[] salt  = GerarBytesAleatorios(16);
        byte[] iv = GerarBytesAleatorios(16);
        byte[] chave = DerivarChave(senhaCriptografia, salt);

        using (var conn = new NpgsqlConnection(connectionString))
        {
            conn.Open();

            var tabelas = ObterTabelasEmOrdemDeDependencia(conn);

            using (var fsDest = new FileStream(caminhoDestinoBackup, FileMode.Create, FileAccess.Write, FileShare.None, StreamBufferBytes))
            {
                fsDest.Write(salt, 0, salt.Length);
                fsDest.Write(iv,   0, iv.Length);

                using (var aes  = CriarAes(chave, iv))
                using (var encryptor = aes.CreateEncryptor())
                using (var cryptoStream = new CryptoStream(fsDest, encryptor, CryptoStreamMode.Write))
                using (var zipStream = new GZipStream(cryptoStream, CompressionMode.Compress))
                using (var writer  = new BinaryWriter(zipStream, Encoding.UTF8, leaveOpen: true))
                {
                    writer.Write(tabelas.Count);

                    foreach (var tabela in tabelas)
                    {
                        writer.Write(tabela);
                        string csv;
                        using (var exportReader = conn.BeginTextExport($"COPY \"{tabela}\" TO STDOUT WITH (FORMAT csv)"))
                        {
                            csv = exportReader.ReadToEnd();
                        }

                        byte[] dados = Encoding.UTF8.GetBytes(csv);
                        writer.Write(dados.Length);
                        writer.Write(dados);
                    }
                }
            }
        }
    }

    public static void ExecutarImportacao(
        string connectionString,
        string caminhoOrigemBackup,
        string senhaCriptografia)
    {
        if (!File.Exists(caminhoOrigemBackup))
            throw new FileNotFoundException("Arquivo de backup não encontrado.", caminhoOrigemBackup);

        using (var fsBackup = new FileStream(
                   caminhoOrigemBackup,
                   FileMode.Open, FileAccess.Read, FileShare.Read,
                   StreamBufferBytes))
        {
            byte[] salt  = LerBytesExatos(fsBackup, 16);
            byte[] iv = LerBytesExatos(fsBackup, 16);
            byte[] chave = DerivarChave(senhaCriptografia, salt);

            using (var aes = CriarAes(chave, iv))
            using (var decryptor = aes.CreateDecryptor())
            using (var cryptoStream = new CryptoStream(fsBackup, decryptor, CryptoStreamMode.Read))
            using (var zipStream = new GZipStream(cryptoStream, CompressionMode.Decompress))
            using (var reader = new BinaryReader(zipStream, Encoding.UTF8, leaveOpen: true))
            {
                int qntTabelas = reader.ReadInt32();
                var conteudoPorTabela = new List<(string tabela, string csv)>();

                for (int i = 0; i < qntTabelas; i++)
                {
                    string tabela = reader.ReadString();
                    int tamanho = reader.ReadInt32();
                    byte[] dados = reader.ReadBytes(tamanho);
                    conteudoPorTabela.Add((tabela, Encoding.UTF8.GetString(dados)));
                }

                using (var conn = new NpgsqlConnection(connectionString))
                {
                    conn.Open();

                    using (var tx = conn.BeginTransaction())
                    {
                        try
                        {
                            string listaTabelas = string.Join(", ", conteudoPorTabela.ConvertAll(t => $"\"{t.tabela}\""));
                            using (var cmd = new NpgsqlCommand($"TRUNCATE {listaTabelas} RESTART IDENTITY CASCADE;", conn, tx))
                            {
                                cmd.ExecuteNonQuery();
                            }

                            foreach (var (tabela, csv) in conteudoPorTabela)
                            {
                                if (string.IsNullOrEmpty(csv)) continue;

                                using (var importWriter = conn.BeginTextImport($"COPY \"{tabela}\" FROM STDIN WITH (FORMAT csv)"))
                                {
                                    importWriter.Write(csv);
                                }
                            }

                            tx.Commit();
                        }
                        catch (Exception ex)
                        {
                            tx.Rollback();
                            throw new BackupException("Falha ao restaurar backup: " + ex.Message);
                        }
                    }
                }
            }
        }
    }
    
    private static List<string> ObterTabelasEmOrdemDeDependencia(NpgsqlConnection conn)
    {
        var todasTabelas = new List<string>();
        var dependencias = new Dictionary<string, List<string>>();

        using (var cmd = new NpgsqlCommand(
            "SELECT tablename FROM pg_tables WHERE schemaname = 'public' ORDER BY tablename;", conn))
        using (var rdr = cmd.ExecuteReader())
        {
            while (rdr.Read())
            {
                string nome = rdr.GetString(0);
                todasTabelas.Add(nome);
                dependencias[nome] = new List<string>();
            }
        }

        using (var cmd = new NpgsqlCommand(@"
            SELECT
                tc.table_name AS tabela_filha,
                ccu.table_name AS tabela_pai
            FROM information_schema.table_constraints tc
            JOIN information_schema.constraint_column_usage ccu
                ON tc.constraint_name = ccu.constraint_name
            WHERE tc.constraint_type = 'FOREIGN KEY'
              AND tc.table_schema = 'public';", conn))
        using (var rdr = cmd.ExecuteReader())
        {
            while (rdr.Read())
            {
                string filha = rdr.GetString(0);
                string pai   = rdr.GetString(1);
                if (filha != pai && dependencias.ContainsKey(filha))
                    dependencias[filha].Add(pai);
            }
        }

        var ordenadas = new List<string>();
        var visitadas = new HashSet<string>();

        void Visitar(string tabela)
        {
            if (visitadas.Contains(tabela)) return;
            visitadas.Add(tabela);

            foreach (var pai in dependencias[tabela])
                Visitar(pai);

            ordenadas.Add(tabela);
        }

        foreach (var tabela in todasTabelas)
            Visitar(tabela);

        return ordenadas;
    }

    private static byte[] DerivarChave(string senha, byte[] salt)
    {
        using (var rfc = new Rfc2898DeriveBytes(senha, salt, KdfIterations))
            return rfc.GetBytes(KeySizeBits / 8);
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
            rng.GetBytes(bytes);
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
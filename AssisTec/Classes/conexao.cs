using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;
using System.Security.Cryptography;
using System.IO;
using Microsoft.VisualBasic;
namespace AssisTec
{
    internal class conexao
    {
        public string connection = "SERVER=localhost; DATABASE=assistec; UID=root; PWD=; PORT=3306;";

        public MySqlConnection con = null;
        
        
        

        public void OpenConnection()
        {
            try
            {
                con = new MySqlConnection(connection);
                con.Open();

            }
            catch (Exception ex) {
                MessageBox.Show("Erro com o servidor \n" + ex.Message);    
            }   
        }

        public void CloseConnection() {
            try
            {
                con = new MySqlConnection(connection);
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao fechar banco");
            }
        }

        public void backupBanco()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Encrypted Backup|*.bak"; // Extenção .bak 
            string senha = Interaction.InputBox("Digite uma senha para criptografar o arquivo", "Segurança", ""); //Senha para o arquivo criptografado

            if (String.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Operação cancelada pela senha ser nula", "", MessageBoxButtons.OK);
                return;
            }    
            
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string caminhoFinal = saveFileDialog1.FileName;
                string caminhoTemporario = caminhoFinal + ".tmp";
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connection))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup backup = new MySqlBackup(cmd)) //Cria o arquivo temporario
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                backup.ExportToFile(caminhoTemporario);
                                conn.Close();
                            }
                        }
                    }
                    Encriptar(caminhoTemporario, caminhoFinal, senha); //Criptografa
                    File.Delete(caminhoTemporario);
                    
                    MessageBox.Show("Backup criptografado realizado com sucesso!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao realizar Backup", ex.Message, MessageBoxButtons.OK);
                }
            }
        }

        private void Encriptar(string inputFile, string outputFile, string senha)
        {
            byte[] salt =  new byte[] {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08};

            using (Aes aes = Aes.Create())
            {
                var key = new Rfc2898DeriveBytes(senha, salt, 1000); //Embaralha a senha 
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                using (FileStream fs = new FileStream(outputFile, FileMode.Create))
                {
                    using (CryptoStream cs = new CryptoStream(fs, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (FileStream fs2 = new FileStream(inputFile, FileMode.Open))
                        {
                            fs2.CopyTo(cs);
                        }
                    }
                }
            }
        }

        public void importarBackup()
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "Backup File|*.bak";
            openFileDialog1.Title = "Import Backup";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string caminhoCriptografado = openFileDialog1.FileName;
                string caminhoTemporario= caminhoCriptografado + ".tmp";

                string senha = Interaction.InputBox("Digite a senha usada no arquivo");
                if (String.IsNullOrEmpty(senha))
                {
                    MessageBox.Show("Operação cancelada pela senha ser nula", "", MessageBoxButtons.OK);
                    return;
                }

                try
                {
                    Descriptografar(caminhoCriptografado, caminhoTemporario, senha);
                    using (MySqlConnection conn = new MySqlConnection(connection))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup restore = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                restore.ImportFromFile(caminhoTemporario);
                                conn.Close();
                            }
                        }
                    }

                    MessageBox.Show("Importação realizado realizado com sucesso!");
                }
                catch (CryptographicException)
                {
                    MessageBox.Show("Senha incorreta", "Erro", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao importar Backup", ex.Message, MessageBoxButtons.OK);
                }
                finally
                {
                    if (File.Exists(caminhoTemporario))
                    {
                        File.Delete(caminhoTemporario);
                    }
                }
            }
        }

        private void Descriptografar(string inputFile, string outputFile, string senha)
        {
            byte[] salt = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08 };

            using (Aes aes = Aes.Create())
            {
                // Gera a chave a partir da senha e do salt
                var key = new Rfc2898DeriveBytes(senha, salt, 1000);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                using (FileStream fsIn = new FileStream(inputFile, FileMode.Open, FileAccess.Read))
                {
                    using (FileStream fsOut = new FileStream(outputFile, FileMode.Create, FileAccess.Write))
                    {
                        // Decryptor transforma os dados embaralhados em texto legível
                        using (CryptoStream cs = new CryptoStream(fsIn, aes.CreateDecryptor(), CryptoStreamMode.Read))
                        {
                            cs.CopyTo(fsOut);
                        }
                    }
                }
            }
        }
    }
}

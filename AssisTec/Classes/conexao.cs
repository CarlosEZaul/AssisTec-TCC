using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using MySql.Data;

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
            saveFileDialog1.Filter = "SQL FILE|*.sql";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string caminho = saveFileDialog1.FileName;
                try
                {
                    using (MySqlConnection conn = new MySqlConnection(connection))
                    {
                        using (MySqlCommand cmd = new MySqlCommand())
                        {
                            using (MySqlBackup backup = new MySqlBackup(cmd))
                            {
                                cmd.Connection = conn;
                                conn.Open();
                                backup.ExportToFile(caminho);
                                conn.Close();
                            }
                        }
                    }
                    MessageBox.Show("Backup realizado com sucesso!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao realizar Backup", ex.Message, MessageBoxButtons.OK);
                }
            }
        }

        public void importarBackup()
        {
            
            DialogResult dialogResult = MessageBox.Show("Deseja realizar importação do backup?", "Atenção",  MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                using (OpenFileDialog openFileDialog1 = new OpenFileDialog())
                {
                    openFileDialog1.Filter = "SQL FILE|*.sql";
                    if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            using (MySqlConnection conn = new MySqlConnection(connection))
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    using (MySqlBackup backup = new MySqlBackup(cmd))
                                    {
                                        cmd.Connection = conn;
                                        conn.Open();
                                        backup.ImportFromFile(openFileDialog1.FileName);
                                        conn.Close();
                                        MessageBox.Show("Importação do Backup realizado com sucesso!");
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Erro ao importar Backup", ex.Message, MessageBoxButtons.OK);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Operação cancelada!");
            }
            
            
            
        }
    }
}

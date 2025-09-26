using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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
    }
}

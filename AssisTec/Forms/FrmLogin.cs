using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace AssisTec
{
    public partial class FrmLogin : Form
    {
        conexao con = new conexao();
        private string sql;
        MySqlCommand cmd;
        public FrmLogin()
        {
            
            InitializeComponent();
            ApplyDesing();
            mtbCPF.Focus();
        }

        private void ApplyDesing()
        {
            StyleButton(btnLogin, Color.FromArgb(0, 120, 215));
        }
        private void StyleButton(Button button, Color backgroundColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = backgroundColor;
            button.ForeColor = Color.White;
            button.Font = new Font("Comic Sans MS", 12, FontStyle.Bold);
            
            button.Cursor = Cursors.Hand;
                // Não alterar o tamanho para evitar problemas de layout
        }
        private void cbSenha_CheckedChanged(object sender, EventArgs e)
        {
            if (cbSenha.Checked==true)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
            {
                txtPassword.PasswordChar = '*';
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(mtbCPF.Text.Trim()) || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Preencha todos os campos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                con.OpenConnection();
                string cpf = mtbCPF.Text.Replace(".", "").Replace("-", "").Replace(",", "").Trim();
                
                sql = @"SELECT nome FROM usuarios 
                            WHERE REPLACE(REPLACE(REPLACE(cpf, '.', ''), '-', ''), ',', '') = @cpf 
                            AND TRIM(senha) = @senha";
                cmd = new MySqlCommand(sql, con.con);
    
                cmd.Parameters.AddWithValue("@cpf", cpf);
                cmd.Parameters.AddWithValue("@senha", txtPassword.Text.Trim());

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string nome = reader.GetString("nome");
                    

                    reader.Close();
                    con.CloseConnection();

                    MessageBox.Show($"Bem-vindo, {nome}!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Gerenciador_Clientes form =  new Gerenciador_Clientes();
                    form.Show();
                    this.Hide();
                    
                    
                }
                else
                {
                    reader.Close();
                    con.CloseConnection();
                    MessageBox.Show("CPF ou senha inválidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                con.CloseConnection();
                MessageBox.Show("Erro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.UserControls;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Usuarios;
using MySql.Data.MySqlClient;


namespace AssisTec
{
    public partial class FrmLogin : Form
    {
        conexao con = new conexao();
        private string sql;
        MySqlCommand cmd;
        private Usuario user = new Usuario();
        public FrmLogin()
        {
            
            InitializeComponent();
            ApplyDesing();
            mtbCPF.Focus();
            VerificarGerentePadrão();
        }

        #region DesingModerno

        private void ApplyDesing()
        {
            DesingComponentes.StyleButton(btnLogin, Color.FromArgb(0, 120, 215));
           
        }
        
        #endregion

        
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
                VerificarGerentePadrão();
                con.OpenConnection();
                string cpf = mtbCPF.Text
                    .Replace(".", "")
                    .Replace("-", "")
                    .Replace(",", "")
                    .Trim();
                
                sql = @"SELECT nome, senha FROM usuarios 
                            WHERE REPLACE(REPLACE(REPLACE(cpf, '.', ''), '-', ''), ',', '') = @cpf";

                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@cpf", cpf);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string nome = reader.GetString("nome");
                    string hashSenha = reader.GetString("senha");

                    reader.Close();
                    con.CloseConnection();
                    
                    bool senhaValida = BCrypt.Net.BCrypt.Verify(txtPassword.Text.Trim(), hashSenha);

                    if (senhaValida)
                    {
                        MessageBox.Show($"Bem-vindo, {nome}!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        this.DialogResult = DialogResult.OK;
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("CPF ou senha inválidos.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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

        #region Funções ou métodos

        private void VerificarGerentePadrão()
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT COUNT(*) FROM usuarios where nivel = 1 AND status = 'Ativo'";

                using (MySqlCommand cmd =  new MySqlCommand(sql, con.con))
                {
                    int total = Convert.ToInt32(cmd.ExecuteScalar());

                    if (total == 0)
                    {
                        MessageBox.Show("Nenhum usuário encontrado no sistema, faça seu cadastro!", "Primeiro Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        ucFormularioUsuarios ucFormularioUsuarios = new ucFormularioUsuarios(0, 3, new DataGridView());
                        
                        ucFormularioUsuarios.Disposed += (senderControl, args) =>
                        {
                            this.Size = new Size(751, 409);
                        };
                        
                        this.Size = new Size(ucFormularioUsuarios.Width, ucFormularioUsuarios.Height + 30);
                        ucFormularioUsuarios.BorderStyle = BorderStyle.FixedSingle;
                        this.Controls.Add(ucFormularioUsuarios);
                        
                        ucFormularioUsuarios.BringToFront();
                        ucFormularioUsuarios.Show();
                        
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Falha no primeiro login: " + e.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            
        }
        

        #endregion
    }
}
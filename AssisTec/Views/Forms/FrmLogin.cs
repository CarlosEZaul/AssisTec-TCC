using System;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.Repository;
using AssisTec.Models;
using AssisTec.Repository;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Usuarios;

namespace AssisTec
{
    public partial class FrmLogin : Form
    {
        private readonly IUsuarioReposity repository;
        private readonly UsuarioService service;
        private bool _primeiroAcessoVerificado = false;

        public FrmLogin()
        {
            InitializeComponent();
            
            var context = new AppDbContext();
            this.repository = new UsuarioRepository(context);
            this.service = new UsuarioService(this.repository);

            ApplyDesign();
            mtbCPF.Focus();
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            if (!_primeiroAcessoVerificado)
            {
                _primeiroAcessoVerificado = true;
                VerificarPrimeiroAcesso();
            }
        }

        private void VerificarPrimeiroAcesso()
        {
            if (repository.ExisteGerenteAtivo() == false)
            {
                MessageBox.Show(
                    "Nenhum gerente encontrado no sistema!\nFaça seu cadastro para começar.",
                    "Primeiro Acesso",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                
                this.Hide();

                ucFormularioUsuarios ucFormularioUsuarios = new ucFormularioUsuarios(0, 3, null);
                
                this.Width = ucFormularioUsuarios.Width;
                this.Height = ucFormularioUsuarios.Height + 20;
                
                this.Controls.Add(ucFormularioUsuarios);
                ucFormularioUsuarios.BringToFront();
                ucFormularioUsuarios.Left = (this.ClientSize.Width - ucFormularioUsuarios.Width) / 2;
                ucFormularioUsuarios.Top = (this.ClientSize.Height - ucFormularioUsuarios.Height) / 2;
                
                ucFormularioUsuarios.Disposed += (s, ev) =>
                {
                    Application.Restart();
                };

                ucFormularioUsuarios.Show();
            }
        }

        private void ApplyDesign()
        {
            DesingComponentes.StyleButton(btnLogin, Color.FromArgb(0, 120, 215));
        }

        private void cbSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = cbSenha.Checked ? '\0' : '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var (sucesso, mensagem, usuario) = service.RealizarLogin(mtbCPF.Text,txtPassword.Text);
           
            if (sucesso)
            {
                MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Sessao.usuarioLogado = usuario;
                this.DialogResult = DialogResult.OK;
                this.Hide();
            }
            else
            {
                MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                mtbCPF.Focus();
            }
        }
    }
}
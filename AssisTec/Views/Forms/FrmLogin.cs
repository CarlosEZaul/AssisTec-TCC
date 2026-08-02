using System;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.Repository;
using AssisTec.Models;
using AssisTec.UserControls;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Usuarios;
using AssisTec.UserControls.SubUserControl_do_Login;

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

                ucFormularioUsuarios ucFormularioUsuarios = new ucFormularioUsuarios(0, 3, null, service);
                
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
            DesignComponentes.StyleButton(btnLogin, Color.FromArgb(0, 120, 215));
        }

        private void cbSenha_CheckedChanged(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = cbSenha.Checked ? '\0' : '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var (sucesso, mensagem, usuario) = service.RealizarLogin(mtbCPF.Text, txtPassword.Text);
   
            if (sucesso)
            {
                

                ucAutenticacao uc2FA = new ucAutenticacao(service, usuario);
        
                int larguraOriginal = this.Width;
                int alturaOriginal = this.Height;

                this.Width = uc2FA.Width;
                this.Height = uc2FA.Height + 35;
        
                this.Controls.Add(uc2FA);
                uc2FA.BringToFront();
                uc2FA.Left = (this.ClientSize.Width - uc2FA.Width) / 2;
                uc2FA.Top = (this.ClientSize.Height - uc2FA.Height) / 2;
        
                uc2FA.Disposed += (s, ev) =>
                {
                    if (Sessao.usuarioLogado == null)
                    {
                        this.Width = larguraOriginal;
                        this.Height = alturaOriginal;
                    }
                };
            }
            else
            {
                MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                mtbCPF.Focus();
            }
        }

        private void lblEsqueciASenha_Click(object sender, EventArgs e)
        {
            int larguraOriginal = this.Width;
            int alturaOriginal = this.Height;

            ucEsqueciASenha ucEsqueciASenha = new ucEsqueciASenha(service);
                
            this.Width = ucEsqueciASenha.Width;
            this.Height = ucEsqueciASenha.Height + 35;
                
            this.Controls.Add(ucEsqueciASenha);
            ucEsqueciASenha.BringToFront();
            ucEsqueciASenha.Left = (this.ClientSize.Width - ucEsqueciASenha.Width) / 2;
            ucEsqueciASenha.Top = (this.ClientSize.Height - ucEsqueciASenha.Height) / 2;
            
            ucEsqueciASenha.Disposed += (s, ev) =>
            {
                this.Width = larguraOriginal;
                this.Height = alturaOriginal;
            };
        }
    }
}
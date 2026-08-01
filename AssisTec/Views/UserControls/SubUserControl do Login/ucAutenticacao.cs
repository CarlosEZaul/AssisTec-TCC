using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.Models;

namespace AssisTec.UserControls
{
    public partial class ucAutenticacao : UserControl
    {
        private readonly UsuarioService _usuarioService;
        private readonly EmailService _emailService;
        private readonly Usuario _usuarioPendente;
        
        private string _codigoGerado;

        public ucAutenticacao(UsuarioService usuarioService, Usuario usuario)
        {
            InitializeComponent();
            _usuarioService = usuarioService;
            _usuarioPendente = usuario;
            _emailService = new EmailService("carlosezzddomingos@gmail.com", "yahs ubev npto pewg");

            mtbCodigo.Mask = "000-000";
            mtbCodigo.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
        }

        private async void ucAutenticacao_Load(object sender, EventArgs e)
        {
            await EnviarCodigoAsync();
        }

        private async Task EnviarCodigoAsync()
        {
            if (_usuarioPendente == null || string.IsNullOrEmpty(_usuarioPendente.Email))
            {
                MessageBox.Show("Usuário sem e-mail cadastrado para autenticação.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                lblReenviarCodigo.Enabled = false;

                _codigoGerado = _emailService.GerarCodigoVerificacao();

                bool enviado = await Task.Run(() => _emailService.EnviarCodigoVerificacao(_usuarioPendente.Email, _codigoGerado));

                if (enviado)
                {
                    MessageBox.Show("Código de verificação enviado para o seu e-mail!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    mtbCodigo.Clear();
                    mtbCodigo.Focus();
                }
                else
                {
                    MessageBox.Show("Falha ao enviar o e-mail. Verifique a conexão com a internet.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao enviar o código: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                lblReenviarCodigo.Enabled = true;
            }
        }

        private void btnVerificarCodigo_Click(object sender, EventArgs e)
        {
            string codigoDigitado = mtbCodigo.Text.Trim();

            if (!mtbCodigo.MaskCompleted)
            {
                MessageBox.Show("Digite o código completo de 6 dígitos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (codigoDigitado == _codigoGerado)
            {
                MessageBox.Show("Autenticação concluída com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                Sessao.usuarioLogado = _usuarioPendente;
                
                Form parentForm = this.FindForm();
                if (parentForm != null)
                {
                    parentForm.DialogResult = DialogResult.OK;
                    parentForm.Close();
                }
            }
            else
            {
                MessageBox.Show("Código incorreto. Tente novamente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mtbCodigo.Clear();
                mtbCodigo.Focus();
            }
        }

        private async void lblReenviarCodigo_Click(object sender, EventArgs e)
        {
            await EnviarCodigoAsync();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        
    }
}
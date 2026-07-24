using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Login
{
    public partial class ucEsqueciASenha : UserControl
    {
        private readonly UsuarioService _usuarioService;
        private readonly EmailService _emailService;
        
        private string _codigoGerado;
        private string _emailDestino;

        public ucEsqueciASenha(UsuarioService usuarioService)
        {
            InitializeComponent();
            _usuarioService = usuarioService;
            
            _emailService = new EmailService("carlosezzddomingos@gmail.com", "yahs ubev npto pewg");

            mtbCodigo.Mask = "000-000";
            mtbCodigo.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;

            EstadoInicial();
        }

        private void AtualizarVisibilidadeLinks()
        {
            lblAlterarEmail.Enabled = mtbCodigo.Enabled;
            lblReenviarCodigo.Enabled = mtbCodigo.Enabled;
        }

        private void EstadoInicial()
        {
            txtEmail.Enabled = true;
            btnBuscar.Enabled = true;

            mtbCodigo.Enabled = false;
            btnVerificarCodigo.Enabled = false;

            txtSenha.Enabled = false;
            btnAlterarSenha.Enabled = false;

            AtualizarVisibilidadeLinks();
        }

        private void EstadoCodigoEnviado()
        {
            txtEmail.Enabled = false;
            btnBuscar.Enabled = false;

            mtbCodigo.Enabled = true;
            btnVerificarCodigo.Enabled = true;

            mtbCodigo.Clear();
            mtbCodigo.Focus();

            AtualizarVisibilidadeLinks();
        }

        private void EstadoCodigoConfirmado()
        {
            mtbCodigo.Enabled = false;
            btnVerificarCodigo.Enabled = false;

            txtSenha.Enabled = true;
            btnAlterarSenha.Enabled = true;

            txtSenha.Focus();

            AtualizarVisibilidadeLinks();
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            _emailDestino = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(_emailDestino))
            {
                MessageBox.Show("Informe o e-mail.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                this.Cursor = Cursors.WaitCursor;
                btnBuscar.Enabled = false;

                if (!_usuarioService.ExisteEmail(_emailDestino))
                {
                    MessageBox.Show("E-mail não cadastrado no sistema.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _codigoGerado = _emailService.GerarCodigoVerificacao();

                bool enviado = await Task.Run(() => _emailService.EnviarCodigoVerificacao(_emailDestino, _codigoGerado));

                if (enviado)
                {
                    MessageBox.Show("Código enviado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    EstadoCodigoEnviado();
                }
                else
                {
                    MessageBox.Show("Falha ao enviar e-mail. Verifique a conexão e as credenciais do remetente.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao processar a solicitação: {ex.Message}", "Erro inesperado", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                this.Cursor = Cursors.Default;
                btnBuscar.Enabled = true;
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
                MessageBox.Show("Código correto! Digite a nova senha.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EstadoCodigoConfirmado();
            }
            else
            {
                MessageBox.Show("Código incorreto. Tente novamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Error);
                mtbCodigo.Clear();
                mtbCodigo.Focus();
            }
        }

        private void btnAlterarSenha_Click(object sender, EventArgs e)
        {
            string novaSenha = txtSenha.Text;

            if (string.IsNullOrEmpty(novaSenha))
            {
                MessageBox.Show("Digite a nova senha.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var resultado = _usuarioService.AlterarSenha(_emailDestino, novaSenha);

            if (resultado.sucesso)
            {
                MessageBox.Show(resultado.mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            }
            else
            {
                MessageBox.Show(resultado.mensagem, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void lblReenviarCodigo_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_emailDestino))
            {
                MessageBox.Show("Informe o e-mail primeiro.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            _codigoGerado = _emailService.GerarCodigoVerificacao();

            if (_emailService.EnviarCodigoVerificacao(_emailDestino, _codigoGerado))
            {
                MessageBox.Show("Um novo código foi enviado para o seu e-mail!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                mtbCodigo.Clear();
                mtbCodigo.Focus();
            }
            else
            {
                MessageBox.Show("Falha ao reenviar o e-mail. Verifique a conexão.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lblAlterarEmail_Click(object sender, EventArgs e)
        {
            EstadoInicial();
            txtEmail.Focus();
            txtEmail.SelectAll();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
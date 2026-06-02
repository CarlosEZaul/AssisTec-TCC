using System.Drawing;
using System.Windows.Forms;

namespace AssisTec
{
    internal sealed partial class PasswordForm_ : Form
    {
        private readonly Label  _lblInfo;
        private readonly TextBox _txtSenha;
        private readonly Button  _btnOk;
        private readonly Button  _btnCancelar;

        public string Senha => _txtSenha.Text;

        public PasswordForm_(string mensagem)
        {
            Text = "Senha de Criptografia";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            ClientSize = new Size(360, 140);
            BackColor = Color.FromArgb(39, 55, 76);

            _lblInfo = new Label
            {
                Text      = mensagem,
                ForeColor = Color.White,
                Location  = new Point(12, 15),
                Size      = new Size(336, 32),
                AutoSize  = false
            };

            _txtSenha = new TextBox
            {
                Location     = new Point(12, 55),
                Size         = new Size(336, 23),
                PasswordChar = '*',         
                MaxLength    = 128
            };

            _btnOk = new Button
            {
                Text         = "Confirmar",
                DialogResult = DialogResult.OK,
                Location     = new Point(192, 95),
                Size         = new Size(75, 28),
                BackColor    = Color.FromArgb(50, 70, 95),
                ForeColor    = Color.White,
                FlatStyle    = FlatStyle.Flat
            };

            _btnCancelar = new Button
            {
                Text         = "Cancelar",
                DialogResult = DialogResult.Cancel,
                Location     = new Point(273, 95),
                Size         = new Size(75, 28),
                BackColor    = Color.FromArgb(50, 70, 95),
                ForeColor    = Color.White,
                FlatStyle    = FlatStyle.Flat
            };

            AcceptButton = _btnOk;
            CancelButton = _btnCancelar;

            Controls.AddRange(new Control[]
                { _lblInfo, _txtSenha, _btnOk, _btnCancelar });
        }
    }
}
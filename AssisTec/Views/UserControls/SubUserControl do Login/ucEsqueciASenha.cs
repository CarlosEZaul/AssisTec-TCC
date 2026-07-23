using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Login
{
    public partial class ucEsqueciASenha : UserControl
    {
        public ucEsqueciASenha(UsuarioService usuarioService)
        {
            InitializeComponent();
        }

        private void mtbCodigo_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
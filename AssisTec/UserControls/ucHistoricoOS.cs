using System;
using System.Windows.Forms;

namespace AssisTec.UserControls
{
    public partial class ucHistoricoOS : UserControl
    {
        public ucHistoricoOS(int id_usuario)
        {
            InitializeComponent();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
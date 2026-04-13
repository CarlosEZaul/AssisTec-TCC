using System;
using System.Windows.Forms;
using AssisTec.UserControls.SubUserControl_do_Financeiro;

namespace AssisTec.UserControls
{
    public partial class ucContasReceber : UserControl
    {
        public ucContasReceber()
        {
            InitializeComponent();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ucRegistrarEntradaFinanceiro ucRegistrarEntrada = new ucRegistrarEntradaFinanceiro();
            this.Controls.Add(ucRegistrarEntrada);
            ucRegistrarEntrada.BringToFront();
            ucRegistrarEntrada.Left = (this.ClientSize.Width - ucRegistrarEntrada.Width)/2;
            ucRegistrarEntrada.Top = (this.ClientSize.Height - ucRegistrarEntrada.Height)/2;
            ucRegistrarEntrada.Show();
        }
    }
}
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AssisTec.UserControls.SubUserControl_do_Financeiro
{
    public partial class ucRegistrarPagamento : UserControl
    {
        private int idConta;
        
        public ucRegistrarPagamento(int _idConta)
        {
            InitializeComponent();
            applyModernDesing();
            idConta = _idConta;
            mtbDataPagamento.Text = DateTime.Now.ToShortDateString();
            mtbDataPagamento.Enabled = false;
        }

        #region DesingModerno

        private void applyModernDesing()
        {
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.StyleComboBox(cbFormaPagamento);
            DesingComponentes.StyleButton(btnFechar, Color.Red);
        }
        

        #endregion

        #region Funções dos componentes
        private void btnSave_Click(object sender, EventArgs e)
        {
            
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        
        #endregion
    }
}
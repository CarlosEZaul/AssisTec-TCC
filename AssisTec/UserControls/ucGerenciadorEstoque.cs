using System;
using System.Windows.Forms;

namespace AssisTec.UserControls
{
    public partial class ucGerenciadorEstoque : UserControl
    {
        public ucGerenciadorEstoque()
        {
            InitializeComponent();
            DesingModerno();
        }

        #region DesingModerno

        private void DesingModerno()
        {
            DesingComponentes.centralizarPanelBotoes(panelBotoes, this.Width);
        }
        

        #endregion

        private void btnNew_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
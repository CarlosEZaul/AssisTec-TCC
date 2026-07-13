using System;
using System.Drawing;
using System.Windows.Forms;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque
{
    public partial class ucMovimentaçãoEstoque : UserControl
    {
        public ucMovimentaçãoEstoque()
        {
            InitializeComponent();
            DesingModerno();
        }

        #region Funções

        private void DesingModerno()
        {
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.StyleDataGridView(dgvMovimentacao,DataGridViewAutoSizeColumnsMode.Fill);
            DesingComponentes.StyleButton(btnFechar, Color.Red);
        }
        

        #endregion

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
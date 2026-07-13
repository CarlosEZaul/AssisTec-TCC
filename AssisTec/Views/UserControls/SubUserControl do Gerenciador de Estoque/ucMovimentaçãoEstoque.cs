using System;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque
{
    public partial class ucMovimentaçãoEstoque : UserControl
    {
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;
        public ucMovimentaçãoEstoque(MovimentacaoEstoqueService  movimentacaoEstoqueService)
        {
            InitializeComponent();
            _movimentacaoEstoqueService = movimentacaoEstoqueService ??  throw new ArgumentNullException(nameof(movimentacaoEstoqueService));
            DesingModerno();
            AtualizarGrid();
        }

        #region Funções

        private void DesingModerno()
        {
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.StyleDataGridView(dgvMovimentacao,DataGridViewAutoSizeColumnsMode.Fill);
            DesingComponentes.StyleButton(btnFechar, Color.Red);
        }

        private void AtualizarGrid()
        {
            dgvMovimentacao.DataSource = _movimentacaoEstoqueService.ListarMovimentacaoEstoque();
            //FormatGrid();
        }

        private void FormatGrid()
        {
            
            if (dgvMovimentacao.Columns.Count <= 0) return;

            dgvMovimentacao.Columns[0].HeaderText = "ID_Movimentação";
            dgvMovimentacao.Columns[1].HeaderText = "ID_Produto";
            dgvMovimentacao.Columns[2].HeaderText = "Data";
            dgvMovimentacao.Columns[3].HeaderText = "Quantidade";
            dgvMovimentacao.Columns[4].HeaderText = "Valor";
            dgvMovimentacao.Columns[5].HeaderText = "Descrição";
            dgvMovimentacao.Columns[6].HeaderText = "Tipo da Movimentação";
        }
        

        #endregion

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
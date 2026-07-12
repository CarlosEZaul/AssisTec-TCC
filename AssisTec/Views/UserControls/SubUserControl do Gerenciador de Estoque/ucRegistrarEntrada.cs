using System;
using System.Windows.Forms;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque
{
    public partial class ucRegistrarEntrada : UserControl
    {
        private readonly int _idProduto;
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;
        private readonly ProdutoService _produtoService;
        public ucRegistrarEntrada(int idProduto,ProdutoService produtoService, MovimentacaoEstoqueService movimentacaoEstoqueService)
        {
            InitializeComponent();
            _idProduto = idProduto;
            _movimentacaoEstoqueService = movimentacaoEstoqueService ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueService));
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
        }

        #region Funções

        private void configurarComponentes()
        {
            cbMotivo.Items.Add("Compra de mercadoria");
            cbMotivo.Items.Add("Devolução de Cliente");
            cbMotivo.Items.Add("Ajuste de Inventário / Correção de Saldo");
        }
        

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
using System;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque
{
    public partial class ucFormularioProduto : UserControl
    {
        private readonly ProdutoService  _produtoService;
        private int idProduto;
        private readonly Produto _produto;
        private readonly int modo;
            
            
        public ucFormularioProduto(int idProduto, int modo, ProdutoService produtoService)
        {
            InitializeComponent();
            _produtoService =  produtoService ??  throw new ArgumentNullException(nameof(produtoService));
            this.idProduto = idProduto;
            this.modo = modo;
            _produto = new Produto();
            ApplyDesing();
            ConfigurarComponentes();
        }
        

        private void ApplyDesing()
        {
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
        }

        private void ConfigurarComponentes()
        {
            if (modo == 2)
            {
                txtQuantidade.Enabled = false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _produto.descricao = txtDescricao.Text.Trim();
                _produto.unidade = cbUnidade.SelectedValue.ToString();
                _produto.quantidade = Convert.ToInt32(txtQuantidade.Text);
                _produto.quantidade_minima = Convert.ToInt32(txtQuantidadeMinima.Text);
                _produto.preco_compra = Convert.ToDecimal(mtbPrecoCompra.Text);
                _produto.preco_venda = Convert.ToDecimal(mtbPrecoVenda.Text);
                if (modo == 1)
                {
                    _produtoService.Salvar(_produto);   
                }

                if (modo == 2)
                {
                    _produtoService.atualizarProduto(_produto);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
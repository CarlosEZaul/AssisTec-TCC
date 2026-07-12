using System;
using System.Globalization;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque
{
    public partial class ucRegistrarEntrada : UserControl
    {
        private readonly int _idProduto;
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;
        private readonly MovimentacaoEstoque _MovimentacaoEstoque;
        private readonly ProdutoService _produtoService;
        private Produto _produto;
        private decimal _valor;
        public ucRegistrarEntrada(int idProduto,ProdutoService produtoService, MovimentacaoEstoqueService movimentacaoEstoqueService)
        {
            InitializeComponent();
            _idProduto = idProduto;
            _MovimentacaoEstoque = new MovimentacaoEstoque();
            _movimentacaoEstoqueService = movimentacaoEstoqueService ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueService));
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            _produto = _produtoService.ObterProdutoPorId(idProduto);
            configurarComponentes();
        }

        #region Funções

        private void configurarComponentes()
        {
            cbMotivo.Items.Add("Compra de mercadoria");
            cbMotivo.Items.Add("Devolução de Cliente");
            cbMotivo.Items.Add("Ajuste de Inventário / Correção de Saldo");

            mtbValor.Enabled = false;
            txtEstoque.Enabled = false;
            
            mtbValor.Mask = null; 
            mtbValor.Text = "0,00";
            mtbValor.Enabled = true;
            
            txtNomeProduto.Text = _produto.descricao;
            txtEstoque.Text = _produto.quantidade.ToString();
            
        }

        
        
        
        

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtQuantidade.Text, out int quantidade) || quantidade <= 0)
            {
                MessageBox.Show("Por favor, insira uma quantidade válida maior que zero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbMotivo.SelectedItem == null)
            {
                MessageBox.Show("Por favor, selecione um motivo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var novaMovimentacao = new MovimentacaoEstoque
            {
                valor = _valor,
                quantidade = quantidade,
                data = DateTime.Now,
                descricao = cbMotivo.SelectedItem.ToString(),
                tipoMovimentacao = "ENTRADA",
                idProduto = _idProduto
            };

            if (_produtoService.darEntradaProduto(_idProduto, quantidade))
            {
                _movimentacaoEstoqueService.NovaMovimentacaoEstoque(novaMovimentacao);
        
                MessageBox.Show("Entrada de estoque realizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
        
                this.Dispose();
            }
            
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtQuantidade_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(txtQuantidade.Text, out int quantidade))
            {
                _valor = quantidade * _produto.preco_compra;
                mtbValor.Text = _valor.ToString("F2");
            }
            else
            {
                mtbValor.Text = "0,00";
            }
            
        }

        private void mtbValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.') e.KeyChar = ',';
    
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == ',')
            {
                if (mtbValor.Text.Contains(","))
                {
                    e.Handled = true;
                }
                return;
            }

            if (char.IsDigit(e.KeyChar))
            {
                int posicaoVirgula = mtbValor.Text.IndexOf(',');
                if (posicaoVirgula != -1 && mtbValor.SelectionStart > posicaoVirgula)
                {
                    string[] partes = mtbValor.Text.Split(',');
                    if (partes.Length > 1 && partes[1].Length >= 2 && mtbValor.SelectionLength == 0)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void mtbValor_Leave(object sender, EventArgs e)
        {
            string texto = mtbValor.Text.Trim();

            if (string.IsNullOrWhiteSpace(texto))
            {
                mtbValor.Text = "0,00";
                return;
            }

            if (decimal.TryParse(texto, NumberStyles.Currency, new CultureInfo("pt-BR"), out decimal valor))
            {
                mtbValor.Text = valor.ToString("N2", new CultureInfo("pt-BR"));
            }
            else
            {
                mtbValor.Text = "0,00";
            }
        }
    }
}
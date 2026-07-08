using System;
using System.Globalization;
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
            ConfigurarMascaraValor();
            ConfigurarComponentes();
        }
        

        private void ApplyDesing()
        {
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
        }

        private void carregarProduto()
        {
            Produto produto = _produtoService.ObterProdutoPorId(idProduto);
            txtDescricao.Text = produto.descricao;
            cbUnidade.Text = produto.unidade;
            txtQuantidade.Text = produto.quantidade.ToString();
            txtQuantidadeMinima.Text = produto.quantidade_minima.ToString();
            mtbPrecoCompra.Text = produto.preco_compra.ToString();
            mtbPrecoVenda.Text = produto.preco_venda.ToString();


        }

        private void ConfigurarComponentes()
        {
            if (modo == 2)
            {
                txtQuantidade.Enabled = false;
                carregarProduto();
            }

            cbUnidade.Items.Add("Quant.");
            cbUnidade.Items.Add("Metros");
            cbUnidade.Items.Add("KG.");
        }
        private void ConfigurarMascaraValor()
        {
            mtbPrecoCompra.Mask = null; 
            mtbPrecoCompra.Text = "0,00";
            mtbPrecoCompra.Enabled = true;
            
            mtbPrecoVenda.Mask = null; 
            mtbPrecoVenda.Text = "0,00";
            mtbPrecoVenda.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                

                if (cbUnidade.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, selecione uma unidade.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtQuantidade.Text, out int qtd) || 
                    !int.TryParse(txtQuantidadeMinima.Text, out int qtdMin) ||
                    !decimal.TryParse(mtbPrecoCompra.Text, out decimal precoCompra) || 
                    !decimal.TryParse(mtbPrecoVenda.Text, out decimal precoVenda))
                {
                    MessageBox.Show("Preencha os campos numéricos e de preço corretamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                _produto.idProduto = idProduto;
                _produto.descricao = txtDescricao.Text.Trim();
                _produto.unidade = cbUnidade.SelectedItem.ToString();
                _produto.quantidade = qtd;
                _produto.quantidade_minima = qtdMin;
                _produto.preco_compra = precoCompra;
                _produto.preco_venda = precoVenda;
        
                bool sucesso = false;

                if (modo == 1)
                {
                    sucesso = _produtoService.Salvar(_produto);
                }
                else if (modo == 2)
                {
                    sucesso = _produtoService.atualizarProduto(_produto);
                }

                if (sucesso)
                {
                    MessageBox.Show("Produto salvo com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Dispose(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void mtbPrecoCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.') e.KeyChar = ',';
    
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == ',')
            {
                if (mtbPrecoCompra.Text.Contains(","))
                {
                    e.Handled = true;
                }
                return;
            }

            if (char.IsDigit(e.KeyChar))
            {
                int posicaoVirgula = mtbPrecoCompra.Text.IndexOf(',');
                if (posicaoVirgula != -1 && mtbPrecoCompra.SelectionStart > posicaoVirgula)
                {
                    string[] partes = mtbPrecoCompra.Text.Split(',');
                    if (partes.Length > 1 && partes[1].Length >= 2 && mtbPrecoCompra.SelectionLength == 0)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void mtbPrecoVenda_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.') e.KeyChar = ',';
    
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == ',')
            {
                if (mtbPrecoVenda.Text.Contains(","))
                {
                    e.Handled = true;
                }
                return;
            }

            if (char.IsDigit(e.KeyChar))
            {
                int posicaoVirgula = mtbPrecoVenda.Text.IndexOf(',');
                if (posicaoVirgula != -1 && mtbPrecoVenda.SelectionStart > posicaoVirgula)
                {
                    string[] partes = mtbPrecoVenda.Text.Split(',');
                    if (partes.Length > 1 && partes[1].Length >= 2 && mtbPrecoVenda.SelectionLength == 0)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void mtbPrecoVenda_Leave(object sender, EventArgs e)
        {
            string texto = mtbPrecoVenda.Text.Trim();

            if (string.IsNullOrWhiteSpace(texto))
            {
                mtbPrecoVenda.Text = "0,00";
                return;
            }

            if (decimal.TryParse(texto, NumberStyles.Currency, new CultureInfo("pt-BR"), out decimal valor))
            {
                mtbPrecoVenda.Text = valor.ToString("N2", new CultureInfo("pt-BR"));
            }
            else
            {
                mtbPrecoVenda.Text = "0,00";
            }
        }

        private void mtbPrecoCompra_Leave(object sender, EventArgs e)
        {
            string texto = mtbPrecoCompra.Text.Trim();

            if (string.IsNullOrWhiteSpace(texto))
            {
                mtbPrecoCompra.Text = "0,00";
                return;
            }

            if (decimal.TryParse(texto, NumberStyles.Currency, new CultureInfo("pt-BR"), out decimal valor))
            {
                mtbPrecoCompra.Text = valor.ToString("N2", new CultureInfo("pt-BR"));
            }
            else
            {
                mtbPrecoCompra.Text = "0,00";
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtDescricao.Clear();
            ConfigurarMascaraValor();
            cbUnidade.SelectedIndex = 0;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
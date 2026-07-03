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

        private void ConfigurarComponentes()
        {
            if (modo == 2)
            {
                txtQuantidade.Enabled = false;
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
using System;
using System.Globalization;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque
{
    public partial class ucFormularioProduto : UserControl
    {
        private readonly ProdutoService _produtoService;
        private readonly ContasPagarService _contasPagarService;
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;
        private int idProduto;
        private readonly int modo;
        private readonly CultureInfo _cultureBr = new CultureInfo("pt-BR");

        public ucFormularioProduto(int idProduto, int modo, ProdutoService produtoService, ContasPagarService contasPagarService, MovimentacaoEstoqueService movimentacaoEstoqueService)
        {
            InitializeComponent();
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            _contasPagarService = contasPagarService ?? throw new ArgumentNullException(nameof(contasPagarService));
            _movimentacaoEstoqueService = movimentacaoEstoqueService ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueService));
            this.idProduto = idProduto;
            this.modo = modo;
    
            ConfigurarMascaraValor(); 
            ApplyDesing();            
            ConfigurarComponentes();
        }

        private void ApplyDesing()
        {
            DesignComponentes.centralizarPanel(panelBotoes, this.Width);
        }

        private void carregarProduto()
        {
            Produto produto = _produtoService.ObterProdutoPorId(idProduto);
            if (produto != null)
            {
                txtDescricao.Text = produto.descricao;
                cbUnidade.SelectedItem = produto.unidade;
        
                if (cbUnidade.SelectedIndex == -1)
                {
                    cbUnidade.Text = produto.unidade;
                }

                txtQuantidade.Text = produto.quantidade.ToString();
                txtQuantidadeMinima.Text = produto.quantidade_minima.ToString();
        
                mtbPrecoCompra.Text = produto.preco_compra.ToString("N2", _cultureBr);
                mtbPrecoVenda.Text = produto.preco_venda.ToString("N2", _cultureBr);
            }
        }

        private void ConfigurarComponentes()
        {
            cbUnidade.Items.Clear();
            cbUnidade.Items.Add("Quant.");
            cbUnidade.Items.Add("Metros");
            cbUnidade.Items.Add("KG.");

            if (modo == 2)
            {
                txtQuantidade.Enabled = false;
                carregarProduto();
            }
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
                string unidadeSelecionada = cbUnidade.SelectedIndex != -1 ? cbUnidade.SelectedItem.ToString() : cbUnidade.Text.Trim();

                if (string.IsNullOrEmpty(unidadeSelecionada))
                {
                    MessageBox.Show("Por favor, selecione uma unidade.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!int.TryParse(txtQuantidade.Text, out int qtd) ||
                    !int.TryParse(txtQuantidadeMinima.Text, out int qtdMin) ||
                    !decimal.TryParse(mtbPrecoCompra.Text, NumberStyles.Any, _cultureBr, out decimal precoCompra) ||
                    !decimal.TryParse(mtbPrecoVenda.Text, NumberStyles.Any, _cultureBr, out decimal precoVenda))
                {
                    MessageBox.Show("Preencha os campos numéricos e de preço corretamente.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Produto _produto = new Produto
                {
                    idProduto = idProduto,
                    descricao = txtDescricao.Text.Trim(),
                    unidade = unidadeSelecionada,
                    quantidade = qtd,
                    quantidade_minima = qtdMin,
                    preco_compra = precoCompra,
                    preco_venda = precoVenda,
                    status = "Ativado"
                };

                bool sucesso = false;

                if (modo == 1)
                {
                    sucesso = _produtoService.Salvar(_produto);

                    if (!sucesso)
                    {
                        MessageBox.Show("Falha ao salvar o produto no banco de dados.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    ContasPagar _contasPagar = new ContasPagar
                    {
                        descricao = $"Registro do produto: {_produto.descricao} no estoque",
                        valor = _produto.preco_compra * _produto.quantidade,
                        status = "PAGA",
                        data_emissao = DateTime.Today,
                        data_pagamento = DateTime.Today,
                        data_vencimento = DateTime.Today,
                        id_forma_pagamento_fk = 1,
                        observacoes = $"Registro do produto: {_produto.descricao} no estoque"
                    };

                    _contasPagarService.Salvar(_contasPagar, true);

                    MovimentacaoEstoque _movimentacaoEstoque = new MovimentacaoEstoque
                    {
                        descricao = $"Registro do produto: {_produto.descricao} no estoque",
                        data = DateTime.Today,
                        quantidade = _produto.quantidade,
                        valor = _produto.preco_compra,
                        tipoMovimentacao = "ENTRADA",
                        idProduto = _produto.idProduto,
                        idUsuario = Sessao.usuarioLogado.Id
                        
                    };
                    _movimentacaoEstoqueService.NovaMovimentacaoEstoque(_movimentacaoEstoque);
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

        private void ValidarPrecoKeyPress(TextBoxBase txt, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.') e.KeyChar = ',';

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == ',')
            {
                if (txt.Text.Contains(","))
                {
                    e.Handled = true;
                }
                return;
            }

            if (char.IsDigit(e.KeyChar))
            {
                int posicaoVirgula = txt.Text.IndexOf(',');
                if (posicaoVirgula != -1 && txt.SelectionStart > posicaoVirgula)
                {
                    string[] partes = txt.Text.Split(',');
                    if (partes.Length > 1 && partes[1].Length >= 2 && txt.SelectionLength == 0)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void FormatarPrecoLeave(TextBoxBase txt)
        {
            string texto = txt.Text.Trim();

            if (string.IsNullOrWhiteSpace(texto))
            {
                txt.Text = "0,00";
                return;
            }

            if (decimal.TryParse(texto, NumberStyles.Currency, _cultureBr, out decimal valor))
            {
                txt.Text = valor.ToString("N2", _cultureBr);
            }
            else
            {
                txt.Text = "0,00";
            }
        }

        private void mtbPrecoCompra_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarPrecoKeyPress((TextBoxBase)sender, e);
        }

        private void mtbPrecoVenda_KeyPress(object sender, KeyPressEventArgs e)
        {
            ValidarPrecoKeyPress((TextBoxBase)sender, e);
        }

        private void mtbPrecoVenda_Leave(object sender, EventArgs e)
        {
            FormatarPrecoLeave((TextBoxBase)sender);
        }

        private void mtbPrecoCompra_Leave(object sender, EventArgs e)
        {
            FormatarPrecoLeave((TextBoxBase)sender);
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
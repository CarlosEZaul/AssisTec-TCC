using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque
{
    public partial class ucRegistrarEntrada : UserControl
    {
        private int _idProduto;
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;
        private readonly ProdutoService _produtoService;
        private readonly ContasPagarService _contasPagarService;
        private Produto _produto;
        private decimal _valor;

        public ucRegistrarEntrada(int idProduto, ProdutoService produtoService, MovimentacaoEstoqueService movimentacaoEstoqueService, ContasPagarService contasPagarService)
        {
            InitializeComponent();
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            _movimentacaoEstoqueService = movimentacaoEstoqueService ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueService));
            _contasPagarService = contasPagarService ?? throw new ArgumentNullException(nameof(contasPagarService));
            _idProduto = idProduto;
    
            configurarComponentes();
        }
        public ucRegistrarEntrada(ProdutoService produtoService, MovimentacaoEstoqueService movimentacaoEstoqueService, ContasPagarService contasPagarService)
        {
            InitializeComponent();
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            _movimentacaoEstoqueService = movimentacaoEstoqueService ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueService));
            _contasPagarService = contasPagarService ?? throw new ArgumentNullException(nameof(contasPagarService));
    
            configurarComponentes();
        }

        #region Funções ou métodos

        private void configurarComponentes()
        {
            List<Produto> produtos = _produtoService.ObterProdutos()
                .Where(p => p.status == "Ativado")
                .ToList();
    
            cbProduto.SelectedIndexChanged -= cbProduto_SelectedIndexChanged;

            cbProduto.DataSource = null;
            cbProduto.DisplayMember = "descricao";
            cbProduto.ValueMember = "idProduto";
            cbProduto.DataSource = produtos;
    
            if (_idProduto > 0)
            {
                cbProduto.SelectedValue = _idProduto;
            }
            else
            {
                cbProduto.SelectedIndex = -1;
            }

            cbProduto.SelectedIndexChanged += cbProduto_SelectedIndexChanged;

            cbMotivo.Items.Clear();
            cbMotivo.Items.Add("Compra de mercadoria");
            cbMotivo.Items.Add("Devolução de Cliente");
            cbMotivo.Items.Add("Retorno de Consignação / Demonstração");
            cbMotivo.Items.Add("Ajuste de Inventário / Correção de Saldo");

            mtbValor.Enabled = false;
            txtEstoque.Enabled = false;
    
            mtbValor.Mask = null; 
            mtbValor.Text = "0,00";

            AtualizarProdutoSelecionado();
        }

        private void AtualizarProdutoSelecionado()
        {
            if (cbProduto.SelectedValue != null && int.TryParse(cbProduto.SelectedValue.ToString(), out int idSelecionado) && idSelecionado > 0)
            {
                _idProduto = idSelecionado;
                _produto = _produtoService.ObterProdutoPorId(_idProduto);

                if (_produto != null)
                {
                    txtEstoque.Text = _produto.quantidade.ToString();
                    RecalcularTotalEntrada();
                }
            }
            else
            {
                _idProduto = 0;
                _produto = null;
                txtEstoque.Text = "0";
                mtbValor.Text = "0,00";
                _valor = 0;
            }
        }

        private void RecalcularTotalEntrada()
        {
            if (_produto == null || !int.TryParse(txtQuantidade.Text, out int quantidade) || cbMotivo.SelectedItem == null)
            {
                mtbValor.Text = "0,00";
                _valor = 0;
                return;
            }

            string motivo = cbMotivo.SelectedItem.ToString();

            if (motivo == "Devolução de Cliente")
            {
                _valor = quantidade * _produto.preco_venda;
                mtbValor.Enabled = false;
            }
            else if (motivo == "Compra de mercadoria" || motivo == "Retorno de Consignação / Demonstração")
            {
                _valor = quantidade * _produto.preco_compra;
                mtbValor.Enabled = false;
            }
            else
            {
                _valor = 0;
                mtbValor.Enabled = true;
            }

            mtbValor.Text = _valor.ToString("N2", new CultureInfo("pt-BR"));
        }

        #endregion

        #region Funções dos componentes

        private void cbProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarProdutoSelecionado();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_idProduto <= 0 || _produto == null)
            {
                MessageBox.Show("Por favor, selecione um produto válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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

            if (!decimal.TryParse(mtbValor.Text, out decimal valorDigitado))
            {
                MessageBox.Show("Por favor, insira um valor válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _valor = valorDigitado;

            string motivoSelecionado = cbMotivo.SelectedItem.ToString();

            var novaMovimentacao = new MovimentacaoEstoque
            {
                valor = _valor,
                quantidade = quantidade,
                data = DateTime.Now,
                descricao = motivoSelecionado,
                tipoMovimentacao = "ENTRADA",
                idProduto = _idProduto
            };

            if (_produtoService.darEntradaProduto(_idProduto, quantidade))
            {
                _movimentacaoEstoqueService.NovaMovimentacaoEstoque(novaMovimentacao);

                if (motivoSelecionado == "Compra de mercadoria")
                {
                    var contaPagar = new ContasPagar
                    {
                        descricao = motivoSelecionado,
                        valor = _valor,
                        data_emissao = DateTime.Today,
                        data_pagamento = DateTime.Today,
                        data_vencimento = DateTime.Today,
                        status = "PAGA",
                        observacoes = $"Entrada no estoque do produto {_produto.descricao}",
                        id_forma_pagamento_fk = 1
                    };
                    _contasPagarService.Salvar(contaPagar, true);
                }

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
            RecalcularTotalEntrada();
        }

        private void cbMotivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            RecalcularTotalEntrada();
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
                _valor = valor;
            }
            else
            {
                mtbValor.Text = "0,00";
                _valor = 0;
            }
        }

        #endregion
    }
}
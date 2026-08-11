using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque
{
    public partial class ucRegistrarSaida : UserControl
    {
        private int _idProduto;
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;
        private readonly ProdutoService _produtoService;
        private readonly ContasReceberService _contasReceberService;
        private Produto _produto;
        private DataTable _dtFormasPagamento;
        private decimal _valor;
        
        public ucRegistrarSaida(int idProduto, ProdutoService produtoService, MovimentacaoEstoqueService movimentacaoEstoqueService, ContasReceberService contasReceberService)
        {
            InitializeComponent();
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            _movimentacaoEstoqueService = movimentacaoEstoqueService ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueService));
            _contasReceberService = contasReceberService ?? throw new ArgumentNullException(nameof(contasReceberService));
            _idProduto = idProduto;
            
            configurarComponentes();
        }
        
        public ucRegistrarSaida(ProdutoService produtoService, MovimentacaoEstoqueService movimentacaoEstoqueService, ContasReceberService contasReceberService)
        {
            InitializeComponent();
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            _movimentacaoEstoqueService = movimentacaoEstoqueService ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueService));
            _contasReceberService = contasReceberService ?? throw new ArgumentNullException(nameof(contasReceberService));
            
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
            cbProduto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbProduto.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbProduto.DropDownStyle = ComboBoxStyle.DropDown;

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
            cbMotivo.Items.Add("Venda de mercadoria");
            cbMotivo.Items.Add("Devolução a Fornecedores");
            cbMotivo.Items.Add("Doação / Brinde");
            cbMotivo.Items.Add("Consumo Interno");
            cbMotivo.Items.Add("Avaria ou Validade");
            cbMotivo.Items.Add("Furto ou Roubo");
            cbMotivo.Items.Add("Ajuste de Inventário / Correção de Saldo");

            mtbValor.Enabled = false;
            txtEstoque.Enabled = false;
            
            mtbValor.Mask = null; 
            mtbValor.Text = "0,00";
            
            _dtFormasPagamento = _contasReceberService.CarregarFormasPagamento(incluirOpcaoTodas: false);
            
            cbStatus.SelectedIndexChanged -= cbStatus_SelectedIndexChanged;
            cbStatus.Items.Clear();
            cbStatus.Items.AddRange(new[] { "PENDENTE", "PAGA" });
            cbStatus.SelectedIndex = 0;
            cbStatus.SelectedIndexChanged += cbStatus_SelectedIndexChanged;

            AtualizarProdutoSelecionado();
            ControlarVisibilidadeFinanceiro();
        }

        private void ControlarVisibilidadeFinanceiro()
        {
            bool eVenda = cbMotivo.SelectedItem?.ToString() == "Venda de mercadoria";
            
            cbStatus.Enabled = eVenda;

            if (!eVenda)
            {
                cbFormaPagamento.Enabled = false;
                cbFormaPagamento.DataSource = _dtFormasPagamento;
                cbFormaPagamento.DisplayMember = "exibicao";
                cbFormaPagamento.ValueMember = "id_forma_pagamento";
                cbFormaPagamento.SelectedIndex = -1;
                cbStatus.SelectedIndex = 0;
            }
            else
            {
                AtualizarRegraFormaPagamento();
            }
        }

        private void AtualizarRegraFormaPagamento()
        {
            if (_dtFormasPagamento == null || _dtFormasPagamento.Rows.Count == 0) return;

            string status = cbStatus.SelectedItem?.ToString();

            if (status == "PENDENTE")
            {
                DataView dv = new DataView(_dtFormasPagamento);
                cbFormaPagamento.DataSource = dv;
                cbFormaPagamento.DisplayMember = "exibicao";
                cbFormaPagamento.ValueMember = "id_forma_pagamento";

                cbFormaPagamento.SelectedIndex = 0;
                cbFormaPagamento.Enabled = false;
            }
            else if (status == "PAGA")
            {
                DataView dv = new DataView(_dtFormasPagamento);
        
                string primeiraChave = _dtFormasPagamento.Columns[0].ColumnName;
                dv.RowFilter = $"{primeiraChave} <> 0 AND exibicao <> '---'";

                cbFormaPagamento.DataSource = dv;
                cbFormaPagamento.DisplayMember = "exibicao";
                cbFormaPagamento.ValueMember = "id_forma_pagamento";

                cbFormaPagamento.Enabled = true;

                if (dv.Count > 0)
                {
                    cbFormaPagamento.SelectedIndex = 0;
                }
                else
                {
                    cbFormaPagamento.SelectedIndex = -1;
                }
            }
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
                    RecalcularTotal();
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

        private void RecalcularTotal()
        {
            if (_produto == null || !int.TryParse(txtQuantidade.Text, out int quantidade) || cbMotivo.SelectedItem == null)
            {
                if (cbMotivo.SelectedItem?.ToString() != "Ajuste de Inventário / Correção de Saldo")
                {
                    mtbValor.Text = "0,00";
                    _valor = 0;
                }
                return;
            }

            string motivo = cbMotivo.SelectedItem.ToString();

            if (motivo == "Venda de mercadoria")
            {
                _valor = quantidade * _produto.preco_venda;
                mtbValor.Enabled = false; 
                mtbValor.Text = _valor.ToString("N2", new CultureInfo("pt-BR"));
            }
            else if (motivo == "Devolução a Fornecedores" || 
                     motivo == "Doação / Brinde" || 
                     motivo == "Consumo Interno" || 
                     motivo == "Avaria ou Validade" || 
                     motivo == "Furto ou Roubo")
            {
                _valor = quantidade * _produto.preco_compra;
                mtbValor.Enabled = false;
                mtbValor.Text = _valor.ToString("N2", new CultureInfo("pt-BR"));
            }
            else
            {
                mtbValor.Enabled = true;
            }
        }
        #endregion
        
        #region Funções dos componentes

        private void cbProduto_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarProdutoSelecionado();
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarRegraFormaPagamento();
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

            if (!decimal.TryParse(mtbValor.Text, NumberStyles.Currency, new CultureInfo("pt-BR"), out decimal valorDigitado))
            {
                MessageBox.Show("Por favor, insira um valor válido.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            _valor = valorDigitado;

            if (quantidade > _produto.quantidade)
            {
                MessageBox.Show("Quantidade de saída maior do que a disponível no estoque.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string motivoSelecionado = cbMotivo.SelectedItem.ToString();

            if (motivoSelecionado == "Venda de mercadoria")
            {
                if (cbStatus.SelectedItem == null)
                {
                    MessageBox.Show("Por favor, selecione o status do pagamento.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cbStatus.Text == "PAGA" && (cbFormaPagamento.SelectedValue == null || Convert.ToInt32(cbFormaPagamento.SelectedValue) <= 0))
                {
                    MessageBox.Show("Por favor, selecione uma forma de pagamento válida para a venda.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            var novaMovimentacao = new MovimentacaoEstoque
            {
                valor = _valor,
                quantidade = quantidade,
                data = DateTime.Now,
                descricao = motivoSelecionado,
                tipoMovimentacao = "SAIDA",
                idProduto = _idProduto,
                idUsuario = Sessao.usuarioLogado.Id
            };

            if (_produtoService.darSaidaProduto(_idProduto, quantidade))
            {
                _movimentacaoEstoqueService.NovaMovimentacaoEstoque(novaMovimentacao);

                if (motivoSelecionado == "Venda de mercadoria")
                {
                    string statusPagamento = cbStatus.Text;

                    var contaReceber = new ContasReceber
                    {
                        descricao = $"{motivoSelecionado} - {_produto.descricao}",
                        valor = _valor,
                        data_emissao = DateTime.Today,
                        data_vencimento = DateTime.Today.AddDays(3),
                        data_pagamento = statusPagamento == "PAGA" ? (DateTime?)DateTime.Today : null,
                        status = statusPagamento,
                        observacoes = $"Saída no estoque do produto {_produto.descricao} registrada pelo usuário {Sessao.usuarioLogado.Nome}",
                        id_forma_pagamento_fk = Convert.ToInt32(cbFormaPagamento.SelectedValue)
                    };
                    _contasReceberService.Salvar(contaReceber, true);
                }

                MessageBox.Show("Saída no estoque realizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtQuantidade_TextChanged(object sender, EventArgs e)
        {
            RecalcularTotal();
        }

        private void cbMotivo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ControlarVisibilidadeFinanceiro();
            RecalcularTotal();
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
                _valor = 0;
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
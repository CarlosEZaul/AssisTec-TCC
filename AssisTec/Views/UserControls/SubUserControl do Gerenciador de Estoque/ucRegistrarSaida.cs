using System;
using System.Globalization;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Estoque
{
    public partial class ucRegistrarSaida : UserControl
    {
        private readonly int _idProduto;
        private readonly MovimentacaoEstoqueService _movimentacaoEstoqueService;
        private readonly ProdutoService _produtoService;
        private readonly ContasReceberService _contasReceberService;
        private Produto _produto;
        private decimal _valor;
        
        public ucRegistrarSaida(int idProduto, ProdutoService produtoService, MovimentacaoEstoqueService movimentacaoEstoqueService, ContasReceberService contasReceberService)
        {
            InitializeComponent();
            _produtoService = produtoService ?? throw new ArgumentNullException(nameof(produtoService));
            _movimentacaoEstoqueService = movimentacaoEstoqueService ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueService));
            _contasReceberService = contasReceberService ?? throw new ArgumentNullException(nameof(contasReceberService));
            _idProduto = idProduto;
            _produto = _produtoService.ObterProdutoPorId(idProduto);
            
            configurarComponentes();
        }
        
        #region Funções ou métodos

        private void configurarComponentes()
        {
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
            txtNomeProduto.Enabled = false;
            
            mtbValor.Mask = null; 
            mtbValor.Text = "0,00";
            
            if (_produto != null)
            {
                txtNomeProduto.Text = _produto.descricao;
                txtEstoque.Text = _produto.quantidade.ToString();
            }
        }

        private void RecalcularTotal()
        {
            if (!int.TryParse(txtQuantidade.Text, out int quantidade) || cbMotivo.SelectedItem == null)
            {
                mtbValor.Text = "0,00";
                _valor = 0;
                return;
            }

            string motivo = cbMotivo.SelectedItem.ToString();

            if (motivo == "Venda de mercadoria")
            {
                _valor = quantidade * _produto.preco_venda;
                mtbValor.Enabled = false; 
            }
            else if (motivo == "Devolução a Fornecedores" || 
                     motivo == "Doação / Brinde" || 
                     motivo == "Consumo Interno" || 
                     motivo == "Avaria ou Validade" || 
                     motivo == "Furto ou Roubo")
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

            if (!decimal.TryParse(mtbValor.Text, out decimal valorDigitado))
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

            var novaMovimentacao = new MovimentacaoEstoque
            {
                valor = _valor,
                quantidade = quantidade,
                data = DateTime.Now,
                descricao = motivoSelecionado,
                tipoMovimentacao = "SAIDA",
                idProduto = _idProduto
            };

            if (_produtoService.darSaidaProduto(_idProduto, quantidade))
            {
                _movimentacaoEstoqueService.NovaMovimentacaoEstoque(novaMovimentacao);

                if (motivoSelecionado == "Venda de mercadoria")
                {
                    var contaReceber = new ContasReceber
                    {
                        descricao = motivoSelecionado,
                        valor = _valor,
                        data_emissao = DateTime.Today,
                        data_pagamento = DateTime.Today,
                        data_vencimento = DateTime.Today,
                        status = "PAGA",
                        observacoes = $"Saída no estoque do produto {_produto.descricao}",
                        id_forma_pagamento_fk = 1
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
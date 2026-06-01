using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.Models;

namespace AssisTec.UserControls.SubUserControl_do_Financeiro
{
    public partial class ucRegistrarEntradaFinanceiro : UserControl
    {
        private readonly ContasReceberService _service;
        private readonly DataGridView _dgv;
        private readonly int _id;
        private readonly int _modo;
        private readonly List<Label> _listaLabels;
        private DataTable _dtFormaPagamento;
        private ContasReceber _contaAtual;

        public ucRegistrarEntradaFinanceiro(DataGridView dgv, int id, int modo, List<Label> listaLabels)
        {
            InitializeComponent();
            
            _service = new ContasReceberService();
            _dgv = dgv;
            _id = id;
            _modo = modo;
            _listaLabels = listaLabels;
            _contaAtual = new ContasReceber();
        }

        private void ucRegistrarEntradaFinanceiro_Load(object sender, EventArgs e)
        {
            ConfigurarComponentesIniciais();
            
            if (_modo == 2)
            {
                CarregarDadosParaEdicao();
            }
        }

        private void ConfigurarComponentesIniciais()
        {
            try
            {
                _dtFormaPagamento = _service.CarregarFormasPagamentoComPadrao();
                cbFormaPagamento.DataSource = _dtFormaPagamento;
                cbFormaPagamento.DisplayMember = "exibicao";
                cbFormaPagamento.ValueMember = "id_forma_pagamento";

                cbFormaPagamento.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cbFormaPagamento.AutoCompleteSource = AutoCompleteSource.ListItems;

                cbStatus.Items.Clear();
                cbStatus.Items.Add("PENDENTE");
                cbStatus.Items.Add("PAGA");

                mtbDataEmissao.Text = DateTime.Today.ToString("dd/MM/yyyy");
                cbStatus.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao inicializar componentes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CarregarDadosParaEdicao()
        {
            try
            {
                _contaAtual = _service.ObterContaPorId(_id);

                txtDescricao.Text = _contaAtual.descricao;
                txtValor.Text = _contaAtual.valor.ToString("F2");
                mtbDataEmissao.Text = _contaAtual.data_emissao.ToString("dd/MM/yyyy");
                mtbDataVencimento.Text = _contaAtual.data_vencimento.ToString("dd/MM/yyyy");
                txtObservacoes.Text = _contaAtual.observacoes;
                cbStatus.Text = _contaAtual.status;
                
                if (_contaAtual.id_forma_pagamento_fk.HasValue)
                {
                    cbFormaPagamento.SelectedValue = _contaAtual.id_forma_pagamento_fk.Value;
                }
                
                if (_contaAtual.data_pagamento.HasValue)
                {
                    mtbDataPagamento.Text = _contaAtual.data_pagamento.Value.ToString("dd/MM/yyyy");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao carregar dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Fechar();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _contaAtual.descricao = txtDescricao.Text.Trim();
                _contaAtual.observacoes = txtObservacoes.Text.Trim();
                _contaAtual.status = cbStatus.Text;

                if (decimal.TryParse(txtValor.Text, out decimal valorConvertido))
                    _contaAtual.valor = valorConvertido;

                if (DateTime.TryParse(mtbDataEmissao.Text, out DateTime emissao))
                    _contaAtual.data_emissao = emissao;

                if (DateTime.TryParse(mtbDataVencimento.Text, out DateTime vencimento))
                    _contaAtual.data_vencimento = vencimento;

                if (cbStatus.Text == "PAGA" && DateTime.TryParse(mtbDataPagamento.Text, out DateTime pagamento))
                    _contaAtual.data_pagamento = pagamento;
                else
                    _contaAtual.data_pagamento = null;

                int.TryParse(cbFormaPagamento.SelectedValue?.ToString(), out int idForma);
                _contaAtual.id_forma_pagamento_fk = idForma > 0 ? (int?)idForma : null;

                _service.SalvarContaReceber(_contaAtual, _modo);

                MessageBox.Show("Operação realizada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                AtualizarComponentesExternos();
                Fechar();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStatus.SelectedItem?.ToString() == "PENDENTE")
            {
                _dtFormaPagamento.DefaultView.RowFilter = "";
                cbFormaPagamento.SelectedValue = 0; 
                cbFormaPagamento.Enabled = false;

                mtbDataPagamento.Text = null;
                mtbDataPagamento.Enabled = false;
            }
            else 
            {
                _dtFormaPagamento.DefaultView.RowFilter = "id_forma_pagamento <> 0";
                cbFormaPagamento.Enabled = true;
                mtbDataPagamento.Enabled = true;

                if (cbFormaPagamento.SelectedIndex == -1)
                {
                    cbFormaPagamento.SelectedIndex = 0;
                }
                mtbDataPagamento.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }

        private void AtualizarComponentesExternos()
        {
            _dgv.DataSource = _service.CarregarTodasContas();
            var totais = _service.ObterTotaisPadrao();

            _listaLabels[0].Text = totais.totalGeral.ToString("C2");
            _listaLabels[1].Text = totais.totalRecebido.ToString("C2");
            _listaLabels[2].Text = totais.totalPendente.ToString("C2");
            _listaLabels[3].Text = totais.totalAtrasado.ToString("C2");
        }

        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.') e.KeyChar = ',';

            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8 && e.KeyChar != ',')
            {
                e.Handled = true;
                return;
            }

            if (e.KeyChar == ',' && txtValor.Text.Contains(","))
            {
                e.Handled = true;
                return;
            }

            if (txtValor.Text.Contains(","))
            {
                string[] partes = txtValor.Text.Split(',');
                if (partes.Length > 1 && txtValor.SelectionStart > txtValor.Text.IndexOf(","))
                {
                    if (partes[1].Length >= 2)
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtDescricao.Text = null;
            txtValor.Text = null;
            mtbDataPagamento.Text = null;
            mtbDataVencimento.Text = null;
            cbStatus.SelectedIndex = 0;
            cbFormaPagamento.SelectedIndex = 0;
            txtObservacoes.Text = null;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Fechar();
        }

        private void Fechar()
        {
            this.Hide();
        }
    }
}
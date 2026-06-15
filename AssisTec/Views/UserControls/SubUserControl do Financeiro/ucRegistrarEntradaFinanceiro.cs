using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.Models;

namespace AssisTec.UserControls.SubUserControl_do_Financeiro
{
    public partial class ucRegistrarEntradaFinanceiro : UserControl
    {
        private readonly ContasReceberService _service;
        private readonly int _id;
        private readonly bool _ehInsercao;
        private ContasReceber _contaAtual;
        private DataTable _dtFormasPagamento;

        public ucRegistrarEntradaFinanceiro(int id, int modo, ContasReceberService service)
        {
            InitializeComponent();

            _service = service ?? throw new ArgumentNullException(nameof(service));
            _id = id;
            _ehInsercao = modo == 1;

            _contaAtual = new ContasReceber();

            ConfigurarMascaraValor();
        }

        private void ConfigurarMascaraValor()
        {
            mtbValor.Mask = null; 
            mtbValor.Text = "0,00";
            mtbValor.Enabled = true;
        }

        private void ucRegistrarEntradaFinanceiro_Load(object sender, EventArgs e)
        {
            CarregarFormasPagamento();

            cbStatus.Items.AddRange(new[] { "PENDENTE", "PAGA" });
            cbStatus.SelectedIndex = 0;
            mtbDataEmissao.Text = DateTime.Today.ToString("dd/MM/yyyy");

            if (!_ehInsercao) CarregarDadosParaEdicao();
        }

        private void CarregarFormasPagamento()
        {
            _dtFormasPagamento = _service.CarregarFormasPagamento(incluirOpcaoTodas: false);
            cbFormaPagamento.DataSource = _dtFormasPagamento;
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";
        }

        private void CarregarDadosParaEdicao()
        {
            try
            {
                _contaAtual = _service.ObterPorId(_id);

                txtDescricao.Text = _contaAtual.descricao;
                mtbValor.Text  = _contaAtual.valor.ToString("N2", new CultureInfo("pt-BR"));
                mtbDataEmissao.Text  = _contaAtual.data_emissao.ToString("dd/MM/yyyy");
                mtbDataVencimento.Text  = _contaAtual.data_vencimento.ToString("dd/MM/yyyy");
                txtObservacoes.Text = _contaAtual.observacoes;
                cbStatus.Text = _contaAtual.status;

                if (_contaAtual.id_forma_pagamento_fk.HasValue)
                    cbFormaPagamento.SelectedValue = _contaAtual.id_forma_pagamento_fk.Value;

                if (_contaAtual.data_pagamento.HasValue)
                    mtbDataPagamento.Text = _contaAtual.data_pagamento.Value.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar dados para edição: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                _contaAtual.descricao   = txtDescricao.Text.Trim();
                _contaAtual.observacoes = txtObservacoes.Text.Trim();
                _contaAtual.status      = cbStatus.Text;

                string valorTexto = mtbValor.Text.Trim();
                if (decimal.TryParse(valorTexto, NumberStyles.Currency, new CultureInfo("pt-BR"), out decimal v))
                {
                    _contaAtual.valor = v;
                }
                else
                {
                    throw new ArgumentException("O valor informado possui um formato inválido.");
                }

                _contaAtual.data_emissao = DateTime.TryParseExact(mtbDataEmissao.Text, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime em)
                    ? em : DateTime.MinValue;

                _contaAtual.data_vencimento = DateTime.TryParseExact(mtbDataVencimento.Text, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ven)
                    ? ven : DateTime.MinValue;

                _contaAtual.data_pagamento = (cbStatus.Text == "PAGA" &&
                    DateTime.TryParseExact(mtbDataPagamento.Text, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime pag))
                    ? pag : (DateTime?)null;

                _contaAtual.id_forma_pagamento_fk = int.TryParse(
                    cbFormaPagamento.SelectedValue?.ToString(), out int idF) ? idF : (int?)null;

                _service.Salvar(_contaAtual, _ehInsercao);

                MessageBox.Show("Operação realizada com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information); 
                this.Dispose();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Dados inválidos",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar: {ex.Message}", "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStatus.SelectedItem == null || _dtFormasPagamento == null) return;

            bool ehPendente = cbStatus.SelectedItem.ToString() == "PENDENTE";
            cbFormaPagamento.Enabled = !ehPendente;
            mtbDataPagamento.Enabled = !ehPendente;

            if (ehPendente)
            {
                _dtFormasPagamento.DefaultView.RowFilter = string.Empty;

                if (cbFormaPagamento.Items.Count > 0)
                    cbFormaPagamento.SelectedIndex = 0;

                mtbDataPagamento.Clear();
            }
            else
            {
                object valorSelecionadoAntes = cbFormaPagamento.SelectedValue;

                _dtFormasPagamento.DefaultView.RowFilter = "id_forma_pagamento <> 1";

                if (valorSelecionadoAntes != null && valorSelecionadoAntes.ToString() != "1")
                {
                    cbFormaPagamento.SelectedValue = valorSelecionadoAntes;
                }
                else if (cbFormaPagamento.Items.Count > 0)
                {
                    cbFormaPagamento.SelectedIndex = 0; 
                }

                mtbDataPagamento.Text = DateTime.Today.ToString("dd/MM/yyyy");
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

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtDescricao.Clear();
            ConfigurarMascaraValor();
            txtObservacoes.Clear();
            cbStatus.SelectedIndex = 0;
        }

        private void btnFechar_Click(object sender, EventArgs e) => this.Dispose();
    }
}
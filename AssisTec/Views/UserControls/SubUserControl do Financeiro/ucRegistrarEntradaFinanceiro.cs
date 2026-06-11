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

        // CORRIGIDO: Removidos parâmetros não utilizados (dgv e listaLabels).
        // Se forem necessários futuramente, readicione com uso explícito.
        public ucRegistrarEntradaFinanceiro(int id, int modo, ContasReceberService service)
        {
            InitializeComponent();

            _service = service ?? throw new ArgumentNullException(nameof(service));
            _id = id;
            _ehInsercao = modo == 1;

            // CORRIGIDO: Garante que _contaAtual nunca seja null ao entrar no btnSave_Click
            _contaAtual = _ehInsercao ? new ContasReceber() : new ContasReceber();
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
            cbFormaPagamento.DataSource = _service.CarregarFormasPagamento(incluirOpcaoTodas: false);
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";
        }

        private void CarregarDadosParaEdicao()
        {
            // CORRIGIDO: try/catch para evitar crash silencioso deixando _contaAtual null
            try
            {
                _contaAtual = _service.ObterPorId(_id);

                txtDescricao.Text       = _contaAtual.descricao;
                txtValor.Text           = _contaAtual.valor.ToString("F2");
                mtbDataEmissao.Text     = _contaAtual.data_emissao.ToString("dd/MM/yyyy");
                mtbDataVencimento.Text  = _contaAtual.data_vencimento.ToString("dd/MM/yyyy");
                txtObservacoes.Text     = _contaAtual.observacoes;
                cbStatus.Text           = _contaAtual.status;

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

                // CORRIGIDO: CultureInfo explícito para garantir parse correto da vírgula decimal
                _contaAtual.valor = decimal.TryParse(txtValor.Text,
                    NumberStyles.Number, new CultureInfo("pt-BR"), out decimal v) ? v : 0;

                // CORRIGIDO: ParseExact com formato e cultura explícitos para evitar ambiguidade
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

                // CORRIGIDO: Notifica o controle pai para fechar/remover este UserControl,
                // em vez de chamar this.Dispose() diretamente.
                ParentForm?.Close();
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
            bool ehPendente = cbStatus.SelectedItem?.ToString() == "PENDENTE";
            cbFormaPagamento.Enabled = !ehPendente;
            mtbDataPagamento.Enabled = !ehPendente;

            if (ehPendente)
            {
                // CORRIGIDO: Removido SelectedValue = 1 (hardcoded e frágil).
                // Reseta para o primeiro item disponível com segurança.
                if (cbFormaPagamento.Items.Count > 0)
                    cbFormaPagamento.SelectedIndex = 0;

                mtbDataPagamento.Clear();
            }
            else
            {
                mtbDataPagamento.Text = DateTime.Today.ToString("dd/MM/yyyy");
            }
        }

        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.') e.KeyChar = ',';
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8 && e.KeyChar != ',') e.Handled = true;
            if (e.KeyChar == ',' && txtValor.Text.Contains(",")) e.Handled = true;
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtDescricao.Clear();
            txtValor.Clear();
            txtObservacoes.Clear();
            cbStatus.SelectedIndex = 0;
        }

        private void btnFechar_Click(object sender, EventArgs e) => ParentForm?.Close();
    }
}
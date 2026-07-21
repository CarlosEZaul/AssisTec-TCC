using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Financeiro
{
    public partial class ucRegistrarPagamentoSaida : UserControl
    {
        private readonly ContasPagarService _service;
        private readonly PagamentoService _pagamentoService;
        private readonly int _idConta;

        public ucRegistrarPagamentoSaida(int idConta, ContasPagarService service, PagamentoService pagamentoService)
        {
            InitializeComponent();
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _pagamentoService = pagamentoService ?? throw new ArgumentNullException(nameof(pagamentoService));
            _idConta = idConta;
            DesingModerno();
            CarregarFormasPagamento();
        }

        private void DesingModerno()
        {
            DesignComponentes.centralizarPanel(panelBotoes, this.Width);
            DesignComponentes.StyleButton(btnFechar, Color.Red);
            mtbDataPagamento.Text = DateTime.Today.ToString("dd/MM/yyyy");
            mtbDataPagamento.Enabled = false;
        }

        private void CarregarFormasPagamento()
        {
            var dt = _pagamentoService.CarregarFormasPagamento(incluirOpcaoTodas: false);
            cbFormaPagamento.DataSource = dt;
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(cbFormaPagamento.SelectedValue?.ToString(), out int idForma))
            {
                MessageBox.Show("Forma de pagamento inválida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!DateTime.TryParse(mtbDataPagamento.Text, out DateTime dataPagamento))
            {
                MessageBox.Show("Data de pagamento inválida.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _pagamentoService.RegistrarPagamentoSaida(_idConta, idForma, dataPagamento);
                MessageBox.Show("Pagamento registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao Registrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
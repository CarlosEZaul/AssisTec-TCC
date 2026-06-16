using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Service;

namespace AssisTec.UserControls.SubUserControl_do_Financeiro
{
    public partial class ucRegistrarPagamento : UserControl
    {
        private readonly ContasReceberService _service;
        private readonly PagamentoService _pagamentoService;
        private readonly int _idConta;

        public event EventHandler PagamentoRegistrado;

        public ucRegistrarPagamento(int idConta, ContasReceberService service, PagamentoService pagamentoService)
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
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.StyleButton(btnFechar, Color.Red);
            
            mtbDataPagamento.Text = DateTime.Today.ToString("dd/MM/yyyy");
            mtbDataPagamento.Enabled = false;
        }

        private void CarregarFormasPagamento()
        {
            var dt = _service.CarregarFormasPagamento(incluirOpcaoTodas: false);
    
            if (dt != null && dt.Rows.Count > 0)
            {
                dt.Rows[0].Delete();
                dt.AcceptChanges();
            }

            cbFormaPagamento.DataSource = dt;
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(cbFormaPagamento.SelectedValue?.ToString(), out int idForma))
            {
                MessageBox.Show("Forma de pagamento inválida.");
                return;
            }

            DateTime.TryParse(mtbDataPagamento.Text, out DateTime dataPagamento);

            try
            {
                _pagamentoService.RegistrarPagamentoEntrada(_idConta, idForma, dataPagamento);
                MessageBox.Show("Pagamento registrado com sucesso!");
                
                PagamentoRegistrado?.Invoke(this, EventArgs.Empty);
                this.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnFechar_Click(object sender, EventArgs e) => this.Dispose();
    }
}
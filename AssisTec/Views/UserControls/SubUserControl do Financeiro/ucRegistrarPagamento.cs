using System;
using System.Collections.Generic;
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
        private readonly DataGridView _dgv;
        private readonly List<Label> _listaLabels;

        public ucRegistrarPagamento(int idConta, DataGridView dgv, List<Label> labels, ContasReceberService service)
        {
            InitializeComponent();
            
            _service = service;
            _service = service;
            _idConta = idConta;
            _dgv = dgv;
            _listaLabels = labels;

            ConfigurarInterface();
            CarregarFormasPagamento();
        }

        private void ConfigurarInterface()
        {
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.StyleButton(btnFechar, Color.Red);
            
            mtbDataPagamento.Text = DateTime.Today.ToString("dd/MM/yyyy");
            mtbDataPagamento.Enabled = false;
        }

        private void CarregarFormasPagamento()
        {
            var dt = _service.CarregarFormasPagamento(incluirOpcaoTodas: false);
            cbFormaPagamento.DataSource = dt;
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int.TryParse(cbFormaPagamento.SelectedValue?.ToString(), out int idForma);
            DateTime.TryParse(mtbDataPagamento.Text, out DateTime dataPagamento);

            _pagamentoService.RegistrarPagamentoEntrada(_idConta, idForma, dataPagamento);

            MessageBox.Show("Pagamento registrado com sucesso!");
            
            AtualizarComponentesExternos();
            this.Dispose();
        }

        private void AtualizarComponentesExternos()
        {
            _dgv.DataSource = _service.CarregarTodas();
            
            var totais = _service.ObterTotaisPadrao();
            _listaLabels[0].Text = totais.TotalGeral.ToString("C2");
            _listaLabels[1].Text = totais.TotalRecebido.ToString("C2");
            _listaLabels[2].Text = totais.TotalPendente.ToString("C2");
            _listaLabels[3].Text = totais.TotalAtrasado.ToString("C2");
        }

        private void btnFechar_Click(object sender, EventArgs e) => this.Dispose();
    }
}
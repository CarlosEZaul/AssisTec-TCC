using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.Reports;

namespace AssisTec.UserControls.SubUserControl_do_Financeiro
{
    public partial class ucRegistrarPagamento : UserControl
    {
        private readonly ContasReceberService _service;
        //private readonly ContasReceberRelatorio _relatorio;
        private PagamentoService pagamentoService = new PagamentoService();
        private readonly int _idConta;
        private readonly DataGridView _dgvContasReceber;
        private readonly List<Label> _listLabels;

        public ucRegistrarPagamento(int idConta, DataGridView dgvContasReceber, List<Label> listLabels)
        {
            InitializeComponent();
            
            _service = new ContasReceberService();
            //_relatorio = new ContasReceberRelatorio();
            
            _idConta = idConta;
            _dgvContasReceber = dgvContasReceber;
            _listLabels = listLabels;

            ApplyModernDesign();
            ConfigurarCombobox();
        }

        private void ApplyModernDesign()
        {
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.StyleButton(btnFechar, Color.Red);
            
            mtbDataPagamento.Text = DateTime.Today.ToString("dd/MM/yyyy");
            mtbDataPagamento.Enabled = false;
        }

        private void ConfigurarCombobox()
        {
            try
            {
                DataTable dtFormaPagamento = _service.CarregarFormasPagamentoComPadrao();
                
                dtFormaPagamento.DefaultView.RowFilter = "id_forma_pagamento <> 0";

                cbFormaPagamento.DataSource = dtFormaPagamento;
                cbFormaPagamento.DisplayMember = "exibicao";
                cbFormaPagamento.ValueMember = "id_forma_pagamento";

                cbFormaPagamento.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cbFormaPagamento.AutoCompleteSource = AutoCompleteSource.ListItems;

                if (cbFormaPagamento.SelectedIndex == -1 && cbFormaPagamento.Items.Count > 0)
                {
                    cbFormaPagamento.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar formas de pagamento: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int.TryParse(cbFormaPagamento.SelectedValue?.ToString(), out int idFormaPagamento);
        
                if (!DateTime.TryParse(mtbDataPagamento.Text, out DateTime dataPagamento))
                {
                    dataPagamento = DateTime.Today;
                }

                pagamentoService.RegistrarPagamentoEntrada(_idConta, idFormaPagamento, dataPagamento);

                MessageBox.Show("Pagamento registrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

               // _relatorio.GerarRecibo(_idConta);

                AtualizarComponentesExternos();
                this.Dispose();
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AtualizarComponentesExternos()
        {
            _dgvContasReceber.DataSource = _service.CarregarTodasContas();
            
            var totais = _service.ObterTotaisPadrao();
            _listLabels[0].Text = totais.totalGeral.ToString("C2");
            _listLabels[1].Text = totais.totalRecebido.ToString("C2");
            _listLabels[2].Text = totais.totalPendente.ToString("C2");
            _listLabels[3].Text = totais.totalAtrasado.ToString("C2");
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
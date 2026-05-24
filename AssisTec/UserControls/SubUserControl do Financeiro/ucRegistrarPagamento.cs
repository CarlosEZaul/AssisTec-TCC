using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.Data;
using AssisTec.Reports;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls.SubUserControl_do_Financeiro
{
    public partial class ucRegistrarPagamento : UserControl
    {
        private int idConta;
        LancamentoFinanceiro lf  = new LancamentoFinanceiro();
        ContasReceber  contasReceber = new ContasReceber();
        ContasReceberRelatorio  contasReceberRelatorio = new ContasReceberRelatorio();
        PagamentoRepository PagamentoRepository = new PagamentoRepository();
        ContasReceberRepositoy ContasReceberRepositoy = new ContasReceberRepositoy();
        private conexao con = new conexao();
        private string sql;
        MySqlCommand cmd;
        DataTable dtFormaPagamento;
        DataGridView dgvContasReceber;
        List<Label> listLabels = new List<Label>();
        
        public ucRegistrarPagamento(int _idConta, DataGridView _dgvContasReceber, List<Label>  _listLabels)
        {
            InitializeComponent();
            applyModernDesing();
            idConta = _idConta;
            mtbDataPagamento.Text = DateTime.Now.ToShortDateString();
            mtbDataPagamento.Enabled = false;
            ConfigurarCombobox();
            dgvContasReceber = _dgvContasReceber;
            listLabels = _listLabels;
        }

        #region DesingModerno

        private void applyModernDesing()
        {
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            //DesingComponentes.StyleComboBox(cbFormaPagamento);
            DesingComponentes.StyleButton(btnFechar, Color.Red);
        }
        

        #endregion

        #region Funções dos componentes
        private void btnSave_Click(object sender, EventArgs e)
        {
            PagamentoRepository.registrarPagamentoEntrada(idConta, Convert.ToInt32(cbFormaPagamento.SelectedValue),
                mtbDataPagamento.Text);
            var totais = ContasReceberRepositoy.AtualizarTotais(contasReceber);
            listLabels[0].Text = totais.totalGeral.ToString("C2");
            listLabels[1].Text = totais.totalRecebido.ToString("C2");
            listLabels[2].Text = totais.totalPendente.ToString("C2");
            listLabels[3].Text = totais.totalAtrasado.ToString("C2");
            contasReceberRelatorio.GerarRecibo(idConta);
            dgvContasReceber.DataSource = ContasReceberRepositoy.AtualizarTotais(contasReceber);
            this.Dispose();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        
        #endregion

        #region Funções ou métodos

        private void ConfigurarCombobox()
        {
            cbFormaPagamento.Items.Clear();
            dtFormaPagamento = PagamentoRepository.carregarFormasPamento();

            cbFormaPagamento.DataSource = dtFormaPagamento;
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";

            cbFormaPagamento.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbFormaPagamento.AutoCompleteSource = AutoCompleteSource.ListItems;
            
            dtFormaPagamento.DefaultView.RowFilter = 
                "exibicao IS NOT NULL AND exibicao <> '' AND exibicao <> '---'";

            cbFormaPagamento.Enabled = true;
            mtbDataPagamento.Enabled = true;

            // Garante seleção válida
            if (cbFormaPagamento.SelectedIndex == -1)
            {
                cbFormaPagamento.SelectedIndex = 0;
            }
            mtbDataPagamento.Text = DateTime.Now.ToShortDateString();

            con.CloseConnection();
            
            
        }


        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls.SubUserControl_do_Financeiro
{
    public partial class ucRegistrarPagamento : UserControl
    {
        private int idConta;
        LancamentoFinanceiro lf  = new LancamentoFinanceiro();
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
            lf.registrarPagamentoEntrada(idConta, Convert.ToInt32(cbFormaPagamento.SelectedValue),
                mtbDataPagamento.Text, dgvContasReceber);
            var totais = lf.AtualizarTotais();
            listLabels[0].Text = totais.totalGeral.ToString("C2");
            listLabels[1].Text = totais.totalRecebido.ToString("C2");
            listLabels[2].Text = totais.totalPendente.ToString("C2");
            listLabels[3].Text = totais.totalAtrasado.ToString("C2");
            lf.GerarReciboContaReceberPDF(idConta);
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
            con.OpenConnection();

            sql = @"SELECT id_forma_pagamento, CONCAT(descricao) AS exibicao 
                    FROM forma_pagamento 
                    ORDER BY descricao;";

            cmd = new MySqlCommand(sql, con.con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            dtFormaPagamento = new DataTable();
            da.Fill(dtFormaPagamento);

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
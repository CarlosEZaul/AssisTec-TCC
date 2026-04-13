using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls.SubUserControl_do_Financeiro
{
    public partial class ucRegistrarEntradaFinanceiro : UserControl
    {
        private conexao con  = new conexao();
        private string sql;
        private MySqlCommand cmd;
        
        public ucRegistrarEntradaFinanceiro()
        {
            InitializeComponent();
            ConfigurarCombobox();
        }
        
        #region metodos ou funcoes

        private void ConfigurarCombobox()
        {
            cbFormaPagamento.Items.Clear();
            con.OpenConnection();
            
            sql = @"SELECT id_forma_pagamento, CONCAT(descricao) AS exibicao FROM forma_pagamento ORDER BY descricao;";
            cmd = new MySqlCommand(sql, con.con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cbFormaPagamento.DataSource = dt;
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";
            
            cbFormaPagamento.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbFormaPagamento.AutoCompleteSource = AutoCompleteSource.ListItems;
            
        }

        private void fechar()
        {
            this.Hide();
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LancamentoFinanceiro lf = new LancamentoFinanceiro();
            lf.pagamento = new Pagamento();
            lf.tipo = 1;
            lf.valor = Convert.ToDecimal(txtValor.Text);
            lf.descricao = txtDescricao.Text;
            lf.dataEmissao = DateTime.Now.ToShortTimeString();
            lf.dataVencimento = mtbDataVencimento.Text;
            lf.dataPagamento = mtbDataPagamento.Text;
            lf.obervacoes = txtObservacoes.Text;
            if (cbFormaPagamento.SelectedValue == null)
            {
                MessageBox.Show("Selecione uma forma de pagamento");
                return;
            }
            lf.pagamento.id_pagamento = Convert.ToInt32(cbFormaPagamento.SelectedValue);
            
            
            lf.SalvarLancamentoFinanceiro();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            fechar();
        }

        private void ucRegistrarEntradaFinanceiro_Load(object sender, EventArgs e)
        {
            mtbDataEmissao.Text = DateTime.Now.ToShortDateString();
        }


        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }
            
            if (!char.IsDigit(e.KeyChar) &&
                e.KeyChar != (char)8 &&
                e.KeyChar != ',')
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
                
                if (partes.Length > 1)
                {
                    if (txtValor.SelectionStart > txtValor.Text.IndexOf(","))
                    {
                        if (partes[1].Length >= 2)
                        {
                            e.Handled = true;
                        }
                    }
                }
            }
        }
    }
}
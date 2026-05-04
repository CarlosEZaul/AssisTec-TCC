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
        DataTable dtFormaPagamento;
        private DataGridView dgv;
        public ucRegistrarEntradaFinanceiro(DataGridView _dgv)
        {
            InitializeComponent();
            ConfigurarCombobox();
            dgv = _dgv;
            
        }
        
        #region metodos ou funcoes
        private void fechar()
        {
            this.Hide();
        }

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
            
            con.CloseConnection();
            
            cbStatus.Items.Clear();
            cbStatus.Items.Add("PENDENTE");
            cbStatus.Items.Add("PAGA");

        }

        
        #endregion

        #region Função dos componentes

        

        
        private void btnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (string.IsNullOrWhiteSpace(txtValor.Text) || string.IsNullOrWhiteSpace(txtDescricao.Text) ||
                    string.IsNullOrWhiteSpace(txtDescricao.Text) ||
                    string.IsNullOrWhiteSpace(mtbDataEmissao.Text) ||
                    string.IsNullOrWhiteSpace(mtbDataVencimento.Text)||
                    string.IsNullOrWhiteSpace(cbStatus.Text))
                {
                    MessageBox.Show("Preencha todos os campos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                
                LancamentoFinanceiro lf = new LancamentoFinanceiro();
                lf.pagamento = new Pagamento();
                lf.tipo = 1;
                lf.valor = Convert.ToDecimal(txtValor.Text);
                lf.descricao = txtDescricao.Text;
                lf.dataEmissao = DateTime.Now.ToShortDateString();
                lf.dataVencimento = mtbDataVencimento.Text;
                lf.dataPagamento = mtbDataPagamento.Text;
                lf.status = cbStatus.Text;
                lf.obervacoes = txtObservacoes.Text;
                lf.pagamento.id_pagamento = Convert.ToInt32(cbFormaPagamento.SelectedValue);
            
            
                lf.SalvarEntrada();
                dgv.DataSource = lf.atualizarContasReceber();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao registrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
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

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbStatus.SelectedItem.ToString() == "PENDENTE")
            {
                // Mostra tudo (inclusive vazio)
                dtFormaPagamento.DefaultView.RowFilter = "";

                cbFormaPagamento.SelectedValue = 4;
                cbFormaPagamento.Enabled = false;

                mtbDataPagamento.Text = null;
                mtbDataPagamento.Enabled = false;
            }
            else // PAGA
            {
                // Oculta o "___"
                dtFormaPagamento.DefaultView.RowFilter = 
                    "exibicao IS NOT NULL AND exibicao <> '' AND exibicao <> '---'";

                cbFormaPagamento.Enabled = true;
                mtbDataPagamento.Enabled = true;

                // Garante seleção válida
                if (cbFormaPagamento.SelectedIndex == -1)
                    cbFormaPagamento.SelectedIndex = 0;
            }
        }
        #endregion

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
    }
}
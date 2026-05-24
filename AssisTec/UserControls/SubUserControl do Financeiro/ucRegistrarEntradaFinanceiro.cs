using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using AssisTec.Business;
using AssisTec.Data;
using AssisTec.Reports;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls.SubUserControl_do_Financeiro
{
    public partial class ucRegistrarEntradaFinanceiro : UserControl
    {
        private conexao con  = new conexao();
        PagamentoRepository pagamentoRepository = new PagamentoRepository();
        ContasReceber contasReceber = new ContasReceber();
        ContasReceberRepositoy ContasReceberRepositoy =  new ContasReceberRepositoy();
        ContasReceberRelatorio contasReceberRelatorio = new ContasReceberRelatorio();
        ContasReceberService contasReceberService = new ContasReceberService();
        private string sql;
        private MySqlCommand cmd;
        DataTable dtFormaPagamento;
        private DataGridView dgv;
        private int id;
        private int modo;
        private List<Label> listalabels = new List<Label>();
        
        public ucRegistrarEntradaFinanceiro(DataGridView _dgv, int _id, int _modo, List<Label> _listaLabels)
        {
            InitializeComponent();
            ConfigurarCombobox();
            dgv = _dgv;
            id = _id;
            modo = _modo;
            listalabels= _listaLabels;
            
        }
        
        #region metodos ou funcoes

        private void atualizarLabels()
        {
            var totais = ContasReceberRepositoy.AtualizarTotais(contasReceber);
            listalabels[0].Text = totais.totalGeral.ToString("C2");
            listalabels[1].Text = totais.totalRecebido.ToString("C2");
            listalabels[2].Text = totais.totalPendente.ToString("C2");
            listalabels[3].Text = totais.totalAtrasado.ToString("C2");
            
        }
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

        private void carregarDados()
        {
            contasReceber = ContasReceberRepositoy.carregarContaReceber(id);

            id = contasReceber.id_conta;
            txtDescricao.Text = contasReceber.descricao;
            txtValor.Text = contasReceber.valor.ToString();
            mtbDataEmissao.Text = contasReceber.dataEmissao;
            mtbDataPagamento.Text = contasReceber.dataPagamento;
            mtbDataVencimento.Text = contasReceber.dataVencimento;
            cbStatus.Text = contasReceber.status;
            cbFormaPagamento.Text = contasReceber.pagamento.forma_pagamento;
            txtObservacoes.Text = contasReceber.obervacoes;
        }

        
        #endregion

        #region Função dos componentes
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtValor.Text) || string.IsNullOrWhiteSpace(txtDescricao.Text) ||
                string.IsNullOrWhiteSpace(txtDescricao.Text) ||
                string.IsNullOrWhiteSpace(mtbDataEmissao.Text.Replace("/","").Trim()) ||
                string.IsNullOrWhiteSpace(mtbDataVencimento.Text.Replace("/","").Trim())||
                string.IsNullOrWhiteSpace(cbStatus.Text))
            {
                MessageBox.Show("Preencha todos os campos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            
            if (modo == 1)
            {
                try
                {
                    contasReceber.pagamento = new Pagamento();
                    contasReceber.tipo = 1;
                    contasReceber.valor = Convert.ToDecimal(txtValor.Text);
                    contasReceber.descricao = txtDescricao.Text;
                    contasReceber.dataEmissao = DateTime.Now.ToShortDateString();
                    contasReceber.dataVencimento = mtbDataVencimento.Text;
                    contasReceber.dataPagamento = mtbDataPagamento.Text;
                    contasReceber.status = cbStatus.Text;
                    contasReceber.obervacoes = txtObservacoes.Text;
                    contasReceber.pagamento.id_pagamento = Convert.ToInt32(cbFormaPagamento.SelectedValue);
                    
                    var (sucesso, mensagem) = contasReceberService.CadastrarContasReceber(contasReceber);

                    if (sucesso)
                    {
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    
                    dgv.DataSource = ContasReceberRepositoy.CarregarTodasContasReceber();
                    atualizarLabels();
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro ao registrar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (modo == 2)
            {
                try
                {
                    contasReceber.id_conta = id;
                    contasReceber.pagamento = new Pagamento();
                    contasReceber.tipo = 1;
                    contasReceber.valor = Convert.ToDecimal(txtValor.Text);
                    contasReceber.descricao = txtDescricao.Text;
                    contasReceber.dataEmissao = DateTime.Now.ToShortDateString();
                    contasReceber.dataVencimento = mtbDataVencimento.Text;
                    contasReceber.dataPagamento = mtbDataPagamento.Text;
                    contasReceber.status = cbStatus.Text;
                    contasReceber.obervacoes = txtObservacoes.Text;
                    contasReceber.pagamento.id_pagamento = Convert.ToInt32(cbFormaPagamento.SelectedValue);

                   contasReceber.pagamento = new Pagamento { id_pagamento = Convert.ToInt32(cbFormaPagamento.SelectedValue) };

                   
                   var (sucesso, mensagem) = contasReceberService.EditarContasReceber(contasReceber);

                   if (sucesso)
                   {
                       MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       fechar();
                   }
                   else
                   {
                       MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   }
                   
                    dgv.DataSource = ContasReceberRepositoy.CarregarTodasContasReceber();
                    atualizarLabels();
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao alterar conta " + ex, "Erro ao alterar conta", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
                
            }
            
            
            
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            fechar();
        }

        private void ucRegistrarEntradaFinanceiro_Load(object sender, EventArgs e)
        {
            mtbDataEmissao.Text = DateTime.Now.ToShortDateString();
            if (modo == 2)
            {
                carregarDados();
                
            }
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
                {
                    cbFormaPagamento.SelectedIndex = 0;
                }
                mtbDataPagamento.Text = DateTime.Now.ToShortDateString();
                    
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
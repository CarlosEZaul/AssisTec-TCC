using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using AssisTec.UserControls.SubUserControl_do_Financeiro;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls
{
    public partial class ucContasReceber : UserControl
    {
        public ucContasReceber()
        {
            
            InitializeComponent();
            apllyDesingModerno();
            configurarComboBox();
            listgrid();
            formatgrid();
        }

        private string sql;
        private MySqlCommand cmd;
        conexao con = new conexao();
        LancamentoFinanceiro lancamentoFinanceiro = new LancamentoFinanceiro();
        DataTable dtFormaPagamento;
        
        #region DesingModerno

        private void apllyDesingModerno()
        {
            this.Text = "Contas a Receber";
            this.BackColor = Color.FromArgb(39, 55, 76);
            DesingComponentes.StyleDataGridView(dgvContasReceber, DataGridViewAutoSizeColumnsMode.Fill);
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.centralizarPanel(panelExibicao,this.Width);
        }
        
        
        #endregion

        #region Funções ou métodos

        private void carregarlabels()
        {
            
        }

        

        private void filtro()
        {
            try
            {
                con.OpenConnection();

                int idForma = 0;

                bool temFormaPagamento = cbFormaPagamento.SelectedValue != null
                    && int.TryParse(cbFormaPagamento.SelectedValue.ToString(), out idForma)
                    && idForma > 0;

                sql = @"SELECT 
                    cr.id_conta_receber,        
                    cr.id_os_fk,                   
                    cr.descricao,               
                    cr.valor,                   
                    cr.data_emissao,            
                    cr.data_pagamento,          
                    cr.data_vencimento,         
                    cr.status,                  
                    fp.descricao AS forma_pagamento, 
                    cr.observacoes              
                FROM contas_receber cr
                INNER JOIN forma_pagamento fp 
                    ON cr.id_forma_pagamento_fk = fp.id_forma_pagamento
                WHERE 1=1";

                bool temDataInicio = mtbDataInicio.Text.Replace("/", "").Replace("_", "").Trim().Length > 0;
                bool temDataFim = mtbDataFim.Text.Replace("/", "").Replace("_", "").Trim().Length > 0;

                if (temDataInicio)
                {
                    if (DateTime.TryParseExact(mtbDataInicio.Text, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataInicio))
                    {
                        sql += " AND cr.data_vencimento >= @DataInicio";
                    }
                    else
                    {
                        MessageBox.Show("Data de início inválida!", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (temDataFim)
                {
                    if (DateTime.TryParseExact(mtbDataFim.Text, "dd/MM/yyyy",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFim))
                    {
                        sql += " AND cr.data_vencimento <= @DataFim";
                    }
                    else
                    {
                        MessageBox.Show("Data fim inválida!", "Aviso",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                if (!string.IsNullOrWhiteSpace(txtBusca.Text))
                    sql += " AND cr.descricao LIKE @Descricao";

                if (cbStatus.SelectedIndex > 0)
                    sql += " AND cr.status = @Status";

                if (temFormaPagamento)
                    sql += " AND cr.id_forma_pagamento_fk = @FormaPagamento";

                sql += " ORDER BY cr.data_vencimento ASC";

                MySqlCommand cmd = new MySqlCommand(sql, con.con);

                if (temDataInicio)
                    cmd.Parameters.AddWithValue("@DataInicio",
                        DateTime.ParseExact(mtbDataInicio.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date);

                if (temDataFim)
                    cmd.Parameters.AddWithValue("@DataFim",
                        DateTime.ParseExact(mtbDataFim.Text, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date.AddDays(1).AddSeconds(-1));

                if (!string.IsNullOrWhiteSpace(txtBusca.Text))
                    cmd.Parameters.AddWithValue("@Descricao", $"%{txtBusca.Text.Trim()}%");

                if (cbStatus.SelectedIndex > 0)
                    cmd.Parameters.AddWithValue("@Status", cbStatus.SelectedItem.ToString());

                if (temFormaPagamento)
                    cmd.Parameters.AddWithValue("@FormaPagamento", idForma);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                dgvContasReceber.DataSource = dt;
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao filtrar", "Erro", MessageBoxButtons.OK);
                throw;
            }
            finally
            {
                con.CloseConnection();
                formatgrid();
            }
        }

        private void configurarComboBox()
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
            
            DataRow dr = dtFormaPagamento.NewRow();
            dr["id_forma_pagamento"] = 0;
            dr["exibicao"] = "Todas as formas de pagamento";
            dtFormaPagamento.Rows.InsertAt(dr, 0);

            cbFormaPagamento.DataSource = dtFormaPagamento;
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";

            cbFormaPagamento.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbFormaPagamento.AutoCompleteSource = AutoCompleteSource.ListItems;
            
            con.CloseConnection();

            cbStatus.Items.Add("Todos os Status");
            cbStatus.Items.Add("PENDENTE");
            cbStatus.Items.Add("PAGA");
            cbStatus.Items.Add("ATRASADO");
        }
        private void listgrid()
        {
            try
            {
                lancamentoFinanceiro.alterarParaAtrasado();
                dgvContasReceber.DataSource = lancamentoFinanceiro.atualizarContasReceber();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void formatgrid()
        {
            try
            {
                if (dgvContasReceber.Columns.Count <= 0) return;

                dgvContasReceber.Columns[0].HeaderText = "ID_CONTA";
                dgvContasReceber.Columns[1].HeaderText = "ID_OS";
                dgvContasReceber.Columns[2].HeaderText = "Descrição";
                dgvContasReceber.Columns[3].HeaderText = "Valor";
                dgvContasReceber.Columns[4].HeaderText = "Data de Emissão";
                dgvContasReceber.Columns[5].HeaderText = "Data de Pagamento";
                dgvContasReceber.Columns[6].HeaderText = "Data de Vencimento";
                dgvContasReceber.Columns[7].HeaderText = "Status";
                dgvContasReceber.Columns[8].HeaderText = "Forma de Pagamento";
                dgvContasReceber.Columns[9].HeaderText = "Observações";
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao carregar dados: " + e.Message, "Erro", MessageBoxButtons.OK);
            }
            
        }
        

        #endregion

        

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ucRegistrarEntradaFinanceiro ucRegistrarEntrada = new ucRegistrarEntradaFinanceiro(dgvContasReceber);
            this.Controls.Add(ucRegistrarEntrada);
            ucRegistrarEntrada.BringToFront();
            ucRegistrarEntrada.Left = (this.ClientSize.Width - ucRegistrarEntrada.Width)/2;
            ucRegistrarEntrada.Top = (this.ClientSize.Height - ucRegistrarEntrada.Height)/2;
            ucRegistrarEntrada.Show();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listgrid();
            cbStatus.SelectedIndex = 0;
            cbFormaPagamento.SelectedIndex = 0;
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            filtro();
        }
    }
}
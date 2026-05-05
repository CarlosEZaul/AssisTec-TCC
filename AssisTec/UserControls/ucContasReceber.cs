using System;
using System.Collections.Generic;
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
            atualizar();
            listaLabelsTotais = new List<Label> { 
                lblTotalReceber, 
                lblRecebido, 
                lblPendente, 
                lblAtrasado 
            };
        }

        private string sql;
        private MySqlCommand cmd;
        conexao con = new conexao();
        LancamentoFinanceiro lancamentoFinanceiro = new LancamentoFinanceiro();
        DataTable dtFormaPagamento;
        private int idConta;
        private List<Label> listaLabelsTotais;
        
        #region DesingModerno

        private void apllyDesingModerno()
        {
            this.Text = "Contas a Receber";
            this.BackColor = Color.FromArgb(39, 55, 76);
            DesingComponentes.StyleDataGridView(dgvContasReceber, DataGridViewAutoSizeColumnsMode.Fill);
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.centralizarPanel(panelExibicao,this.Width);
            DesingComponentes.StyleButton(btnDelete, Color.Red);
        }
        
        
        #endregion

        #region Funções ou métodos
        
        private void filtro()
        {
            // Valida datas antes de passar para a classe
            bool temDataInicio = mtbDataInicio.Text.Replace("/", "").Replace("_", "").Trim().Length > 0;
            bool temDataFim    = mtbDataFim.Text.Replace("/", "").Replace("_", "").Trim().Length > 0;

            if (temDataInicio && !DateTime.TryParseExact(mtbDataInicio.Text, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                MessageBox.Show("Data de início inválida!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (temDataFim && !DateTime.TryParseExact(mtbDataFim.Text, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                MessageBox.Show("Data fim inválida!", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int idForma = 0;
            int.TryParse(cbFormaPagamento.SelectedValue?.ToString(), out idForma);

            LancamentoFinanceiro lf = new LancamentoFinanceiro
            {
                // Limpa a máscara antes de passar, se estiver vazia vira null
                filtroDataInicio = mtbDataInicio.Text.Replace("/", "").Replace("_", "").Trim().Length > 0
                    ? mtbDataInicio.Text : null,

                filtroDataFim = mtbDataFim.Text.Replace("/", "").Replace("_", "").Trim().Length > 0
                    ? mtbDataFim.Text : null,

                filtroDescricao        = txtBusca.Text,
                filtroStatus           = cbStatus.SelectedIndex > 0 ? cbStatus.SelectedItem.ToString() : null,
                filtroIdFormaPagamento = idForma
            };

            dgvContasReceber.DataSource = lf.FiltrarContasReceber();
            formatgrid();

            var (totalGeral, totalRecebido, totalPendente, totalAtrasado) = lf.AtualizarTotais();
            lblTotalReceber.Text = $"R$ {totalGeral:N2}";
            lblRecebido.Text     = $"R$ {totalRecebido:N2}";
            lblPendente.Text     = $"R$ {totalPendente:N2}";
            lblAtrasado.Text     = $"R$ {totalAtrasado:N2}";
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
            
            cbStatus.SelectedIndex = 0;
            cbFormaPagamento.SelectedIndex = 0;
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

        private void atualizar()
        {
            listgrid();
            cbStatus.SelectedIndex = 0;
            cbFormaPagamento.SelectedIndex = 0;
            var (totalGeral, totalRecebido, totalPendente, totalAtrasado) = lancamentoFinanceiro.AtualizarTotais();

            lblTotalReceber.Text = $"R$ {totalGeral:N2}";
            lblRecebido.Text     = $"R$ {totalRecebido:N2}";
            lblPendente.Text     = $"R$ {totalPendente:N2}";
            lblAtrasado.Text     = $"R$ {totalAtrasado:N2}";
            formatgrid();
        }

        private void EnableBtn()
        {
            btnEditar.Enabled = true;
            btnDelete.Enabled = true;
            btnRecibo.Enabled = true;
        }

        private void DisableBtn()
        {
            btnEditar.Enabled = false;
            btnDelete.Enabled = false;
            btnRecibo.Enabled = false;
        }
        

        #endregion

        #region Funções dos componentes

        

        

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ucRegistrarEntradaFinanceiro ucRegistrarEntrada = new ucRegistrarEntradaFinanceiro(dgvContasReceber, idConta, 1);
            this.Controls.Add(ucRegistrarEntrada);
            ucRegistrarEntrada.BringToFront();
            ucRegistrarEntrada.Left = (this.ClientSize.Width - ucRegistrarEntrada.Width)/2;
            ucRegistrarEntrada.Top = (this.ClientSize.Height - ucRegistrarEntrada.Height)/2;
            ucRegistrarEntrada.Show();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            atualizar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            int idForma = 0;
            int.TryParse(cbFormaPagamento.SelectedValue?.ToString(), out idForma);

            LancamentoFinanceiro lf = new LancamentoFinanceiro
            {
                // Limpa a máscara antes de passar, se estiver vazia vira null
                filtroDataInicio = mtbDataInicio.Text.Replace("/", "").Replace("_", "").Trim().Length > 0
                    ? mtbDataInicio.Text : null,

                filtroDataFim = mtbDataFim.Text.Replace("/", "").Replace("_", "").Trim().Length > 0
                    ? mtbDataFim.Text : null,

                filtroDescricao        = txtBusca.Text,
                filtroStatus           = cbStatus.SelectedIndex > 0 ? cbStatus.SelectedItem.ToString() : null,
                filtroIdFormaPagamento = idForma
            };

            var (totalGeral, totalRecebido, totalPendente, totalAtrasado) = lf.AtualizarTotais();

            lblTotalReceber.Text = $"R$ {totalGeral:N2}";
            lblRecebido.Text     = $"R$ {totalRecebido:N2}";
            lblPendente.Text     = $"R$ {totalPendente:N2}";
            lblAtrasado.Text     = $"R$ {totalAtrasado:N2}";

            filtro();
            
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            int idForma = 0;
            int.TryParse(cbFormaPagamento.SelectedValue?.ToString(), out idForma);

            LancamentoFinanceiro lf = new LancamentoFinanceiro
            {
                filtroDataInicio       = mtbDataInicio.Text.Replace("/","").Replace("_","").Trim().Length > 0 ? mtbDataInicio.Text : null,
                filtroDataFim          = mtbDataFim.Text.Replace("/","").Replace("_","").Trim().Length > 0 ? mtbDataFim.Text : null,
                filtroDescricao        = txtBusca.Text,
                filtroStatus           = cbStatus.SelectedIndex > 0 ? cbStatus.SelectedItem.ToString() : null,
                filtroIdFormaPagamento = idForma
            };

            DataTable dados = lf.FiltrarContasReceber();
            var (totalGeral, totalRecebido, totalPendente, totalAtrasado) = lf.AtualizarTotais();

            lf.GerarRelatorioContasReceberPDF(dados, totalGeral, totalRecebido, totalPendente, totalAtrasado);
        }

        private void dgvContasReceber_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvContasReceber.Rows.Count > 0)
            {
                try
                {
                    EnableBtn();
                    idConta = Convert.ToInt32(dgvContasReceber.Rows[e.RowIndex].Cells[0].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }

        private void btnRecibo_Click(object sender, EventArgs e)
        {
            lancamentoFinanceiro.GerarReciboContaReceberPDF(idConta);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ucRegistrarEntradaFinanceiro ucRegistrarEntrada = new ucRegistrarEntradaFinanceiro(dgvContasReceber, idConta, 2);
            this.Controls.Add(ucRegistrarEntrada);
            ucRegistrarEntrada.BringToFront();
            ucRegistrarEntrada.Left = (this.ClientSize.Width - ucRegistrarEntrada.Width) / 2;
            ucRegistrarEntrada.Top = (this.ClientSize.Height - ucRegistrarEntrada.Height) / 2;
            ucRegistrarEntrada.Show();
        }
        #endregion

        private void dgvContasReceber_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvContasReceber.Columns[e.ColumnIndex].Index == 7 && e.Value != null)
            {
                string status = e.Value.ToString();

                // Se o status for 'Atrasado', pinta a LINHA toda de vermelho
                if (status == "ATRASADO")
                {
                    dgvContasReceber.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Tomato;
                    dgvContasReceber.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
                
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            lancamentoFinanceiro.deletarContaReceber(idConta);
            dgvContasReceber.DataSource = lancamentoFinanceiro.atualizarContasReceber();
            DisableBtn();
            
        }
    }
}
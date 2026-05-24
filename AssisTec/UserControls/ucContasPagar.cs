using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using AssisTec.Data;
using AssisTec.UserControls.SubUserControl_do_Financeiro;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls
{
    public partial class ucContasPagar : UserControl
    {
        public ucContasPagar()
        {
            InitializeComponent();
            listaLabelsTotais = new List<Label> { 
                lblTotalPagar, 
                lblPago, 
                lblPendente, 
                lblAtrasado 
            };
            applyDesingModerno();
            configurarComboBox();
            atualizar();
        }

        private string sql;
        private MySqlCommand cmd;
        conexao con = new conexao();
        PagamentoRepository pagamentoRepository = new PagamentoRepository();
        LancamentoFinanceiro lancamentoFinanceiro = new LancamentoFinanceiro();
        Pagamento pagamento = new Pagamento();
        private int idConta;
        private List<Label> listaLabelsTotais;
        
        #region DesingModerno

        private void applyDesingModerno()
        {
            this.Text = "Contas a Pagar";
            this.BackColor = Color.FromArgb(39, 55, 76);
            DesingComponentes.StyleDataGridView(dgvContasPagar, DataGridViewAutoSizeColumnsMode.Fill);
            DesingComponentes.centralizarPanel(panelBotoes, this.Width);
            DesingComponentes.centralizarPanel(panelExibicao, this.Width);
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

            dgvContasPagar.DataSource = lf.FiltrarContasPagar();
            formatgrid();

            var (totalGeral, totalPago, totalPendente, totalAtrasado) = lf.AtualizarTotaisPagar();
            lblTotalPagar.Text = $"R$ {totalGeral:N2}";
            lblPago.Text       = $"R$ {totalPago:N2}";
            lblPendente.Text   = $"R$ {totalPendente:N2}";
            lblAtrasado.Text   = $"R$ {totalAtrasado:N2}";
        }

        private void configurarComboBox()
        {
            cbFormaPagamento.Items.Clear();
            DataTable dtFormaPagamento = pagamentoRepository.carregarFormasPamento();

            cbFormaPagamento.DataSource = dtFormaPagamento;
            cbFormaPagamento.DisplayMember = "exibicao";
            cbFormaPagamento.ValueMember = "id_forma_pagamento";
            
            DataRow dr = dtFormaPagamento.NewRow();
            dr["id_forma_pagamento"] = 0;
            dr["exibicao"] = "Todas as formas de pagamento";
            dtFormaPagamento.Rows.InsertAt(dr, 0);

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
        
        private void listGrid()
        {
            try
            {
                lancamentoFinanceiro.alterarParaAtrasadoPagar();
                dgvContasPagar.DataSource = lancamentoFinanceiro.atualizarContasPagar();
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
                if (dgvContasPagar.Columns.Count <= 0) return;

                dgvContasPagar.Columns[0].HeaderText = "ID_CONTA";
                dgvContasPagar.Columns[1].HeaderText = "Descrição";
                dgvContasPagar.Columns[2].HeaderText = "Valor";
                dgvContasPagar.Columns[3].HeaderText = "Data de Emissão";
                dgvContasPagar.Columns[4].HeaderText = "Data de Pagamento";
                dgvContasPagar.Columns[5].HeaderText = "Data de Vencimento";
                dgvContasPagar.Columns[6].HeaderText = "Status";
                dgvContasPagar.Columns[7].HeaderText = "Forma de Pagamento";
                dgvContasPagar.Columns[8].HeaderText = "Observações";
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao formatar grid: " + e.Message, "Erro", MessageBoxButtons.OK);
            }
        }

        private void atualizar()
        {
            listGrid();
            cbStatus.SelectedIndex = 0;
            cbFormaPagamento.SelectedIndex = 0;
            var totais = lancamentoFinanceiro.AtualizarTotaisPagar();

            listaLabelsTotais[0].Text = totais.totalGeral.ToString("C2");
            listaLabelsTotais[1].Text = totais.totalPago.ToString("C2");
            listaLabelsTotais[2].Text = totais.totalPendente.ToString("C2");
            listaLabelsTotais[3].Text = totais.totalAtrasado.ToString("C2");
            formatgrid();
        }

        private void EnableBtn()
        {
            btnEditar.Enabled = true;
            btnDelete.Enabled = true;
            btnRecibo.Enabled = true;
            btnRegistrarPagamento.Enabled = true;
        }

        private void DisableBtn()
        {
            btnEditar.Enabled = false;
            btnDelete.Enabled = false;
            btnRecibo.Enabled = false;
            btnRegistrarPagamento.Enabled = false;
        }

        #endregion

        #region Funções dos componentes

        private void dgvContasPagar_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvContasPagar.Columns[e.ColumnIndex].Index == 6 && e.Value != null)
            {
                string status = e.Value.ToString();

                // Se o status for 'Atrasado', pinta a linha de vermelho
                if (status == "ATRASADO")
                {
                    dgvContasPagar.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                    dgvContasPagar.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            lancamentoFinanceiro.deletarContaPagar(idConta);
            DisableBtn();
            atualizar();
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ucRegistrarSaidaFinanceiro ucRegistrarSaida = new ucRegistrarSaidaFinanceiro(dgvContasPagar, idConta, 1, listaLabelsTotais);
            this.Controls.Add(ucRegistrarSaida);
            ucRegistrarSaida.BringToFront();
            ucRegistrarSaida.Left = (this.ClientSize.Width - ucRegistrarSaida.Width) / 2;
            ucRegistrarSaida.Top = (this.ClientSize.Height - ucRegistrarSaida.Height) / 2;
            ucRegistrarSaida.Show();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            atualizar();
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            filtro();
        }

        private void btnRelatorio_Click(object sender, EventArgs e)
        {
            int idForma = 0;
            int.TryParse(cbFormaPagamento.SelectedValue?.ToString(), out idForma);

            LancamentoFinanceiro lf = new LancamentoFinanceiro
            {
                filtroDataInicio = mtbDataInicio.Text.Replace("/","").Replace("_","").Trim().Length > 0 ? mtbDataInicio.Text : null,
                filtroDataFim = mtbDataFim.Text.Replace("/","").Replace("_","").Trim().Length > 0 ? mtbDataFim.Text : null,
                filtroDescricao = txtBusca.Text,
                filtroStatus = cbStatus.SelectedIndex > 0 ? cbStatus.SelectedItem.ToString() : null,
                filtroIdFormaPagamento = idForma
            };

            DataTable dados = lf.FiltrarContasPagar();
            var (totalGeral, totalPago, totalPendente, totalAtrasado) = lf.AtualizarTotaisPagar();

            lf.GerarRelatorioContasPagarPDF(dados, totalGeral, totalPago, totalPendente, totalAtrasado);
        }

        private void dgvContasPagar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvContasPagar.Rows.Count > 0)
            {
                try
                {
                    EnableBtn();
                    idConta = Convert.ToInt32(dgvContasPagar.Rows[e.RowIndex].Cells[0].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRecibo_Click(object sender, EventArgs e)
        {
            lancamentoFinanceiro.GerarReciboContaPagarPDF(idConta);
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ucRegistrarSaidaFinanceiro ucRegistrarSaida = new ucRegistrarSaidaFinanceiro(dgvContasPagar, idConta, 2, listaLabelsTotais);
            this.Controls.Add(ucRegistrarSaida);
            ucRegistrarSaida.BringToFront();
            ucRegistrarSaida.Left = (this.ClientSize.Width - ucRegistrarSaida.Width) / 2;
            ucRegistrarSaida.Top = (this.ClientSize.Height - ucRegistrarSaida.Height) / 2;
            ucRegistrarSaida.Show();
        }

        private void btnRegistrarPagamento_Click(object sender, EventArgs e)
        {
            if (dgvContasPagar.CurrentRow.Cells["Status"].Value.ToString() == "PAGA")
            {
                MessageBox.Show("Registro de Pagamento apenas para contas não pagas", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            ucRegistrarPagamento ucRegistrarPagamento = new ucRegistrarPagamento(idConta, dgvContasPagar, listaLabelsTotais);
            this.Controls.Add(ucRegistrarPagamento);
            ucRegistrarPagamento.BringToFront();
            ucRegistrarPagamento.Left = (this.ClientSize.Width - ucRegistrarPagamento.Width) / 2;
            ucRegistrarPagamento.Top = (this.ClientSize.Height - ucRegistrarPagamento.Height) / 2;
            ucRegistrarPagamento.Show();
        }

        #endregion
    }
}
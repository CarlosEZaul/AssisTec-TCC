using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.UserControls.SubUserControl_do_Financeiro;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls
{
    public partial class ucContasReceber : UserControl
    {
        public ucContasReceber()
        {
            //formatgrid();
            InitializeComponent();
            apllyDesingModerno();
            listgrid();
            formatgrid();
        }

        private string sql;
        private MySqlCommand cmd;
        conexao con = new conexao();
        LancamentoFinanceiro lancamentoFinanceiro = new LancamentoFinanceiro();
        
        #region DesingModerno

        private void apllyDesingModerno()
        {
            this.Text = "Contas a Receber";
            this.BackColor = Color.FromArgb(39, 55, 76);
            DesingComponentes.StyleDataGridView(dgvContasReceber, DataGridViewAutoSizeColumnsMode.Fill);
            DesingComponentes.centralizarPanelBotoes(panelBotoes, this.Width);
        }
        
        
        #endregion
        

        private void listgrid()
        {
            try
            {
                LancamentoFinanceiro lancamentoFinanceiro = new LancamentoFinanceiro();
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
                // Headers
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

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ucRegistrarEntradaFinanceiro ucRegistrarEntrada = new ucRegistrarEntradaFinanceiro();
            this.Controls.Add(ucRegistrarEntrada);
            ucRegistrarEntrada.BringToFront();
            ucRegistrarEntrada.Left = (this.ClientSize.Width - ucRegistrarEntrada.Width)/2;
            ucRegistrarEntrada.Top = (this.ClientSize.Height - ucRegistrarEntrada.Height)/2;
            ucRegistrarEntrada.Show();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listgrid();
        }
    }
}
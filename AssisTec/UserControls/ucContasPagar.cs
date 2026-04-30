using System;
using System.Windows.Forms;
using AssisTec.UserControls.SubUserControl_do_Financeiro;

namespace AssisTec.UserControls
{
    public partial class ucContasPagar : UserControl
    {
        LancamentoFinanceiro lancamentoFinanceiro = new LancamentoFinanceiro();
        public ucContasPagar()
        {
            InitializeComponent();
            listGrid();
            applyDesing();
        }

        #region DesingModerno

        private void applyDesing()
        {
            DesingComponentes.StyleDataGridView(dgvContasReceber, DataGridViewAutoSizeColumnsMode.Fill);
        }

        #endregion

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            ucRegistrarSaidaFinanceiro ucRegistrarSaida = new ucRegistrarSaidaFinanceiro();
            this.Controls.Add(ucRegistrarSaida);
            ucRegistrarSaida.BringToFront();
            ucRegistrarSaida.Left = (this.ClientSize.Width - ucRegistrarSaida.Width)/2;
            ucRegistrarSaida.Top = (this.ClientSize.Height - ucRegistrarSaida.Height)/2;
            ucRegistrarSaida.Show();
        }

        private void listGrid()
        {
            try
            {
                dgvContasReceber.DataSource = lancamentoFinanceiro.atualizarContasPagar();

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
    }
}
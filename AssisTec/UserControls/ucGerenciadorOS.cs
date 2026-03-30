using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls
{
    public partial class ucGerenciadorOS : UserControl
    {
        conexao con = new conexao();
        string sql;
        private MySqlCommand cmd;
        public ucGerenciadorOS()
        {
            InitializeComponent();
        }
        private void formartGrid()
        {
            if (dgvOS.Columns.Count <= 0) return;
            // Headers
            dgvOS.Columns[0].HeaderText = "ID_OS";
            dgvOS.Columns[1].HeaderText = "Cliente";
            dgvOS.Columns[2].HeaderText = "Técnico Responsável";
            dgvOS.Columns[3].HeaderText = "Equipamento";
            dgvOS.Columns[4].HeaderText = "Status";
            dgvOS.Columns[5].HeaderText = "Data de abertura";
            dgvOS.Columns[6].HeaderText = "Ultima Atualização";
            dgvOS.Columns[7].HeaderText = "Data de fechamento";
            dgvOS.Columns[8].HeaderText = "Valor Mão de Obra";
            dgvOS.Columns[9].HeaderText = "Valor p/ peça";
            dgvOS.Columns[10].HeaderText = "Valor total";
            dgvOS.Columns[11].HeaderText = "Problema relatado";
            dgvOS.Columns[12].HeaderText = "Diagnóstico";
            dgvOS.Columns[13].HeaderText = "Observações";
        }
        public void listGrid()
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM usuarios ORDER BY NOME ASC";
                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvOS.DataSource = dt;
                con.CloseConnection();
                formartGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
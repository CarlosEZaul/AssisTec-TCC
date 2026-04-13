using System;
using System.Data;
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
        }

        private string sql;
        private MySqlCommand cmd;
        conexao con = new conexao();
        

        private void listgrid()
        {
            try
            {
                con.OpenConnection();
                sql = @"SELECT 
                            cr.id_conta_receber,
                            cr.descricao,
                            cr.valor,
                            cr.data_emissao,
                            cr.data_pagamento,
                            




";
                            
                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvContasReceber.DataSource = dt;
                con.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void formatgrid()
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
            dgvContasReceber.Columns[8].HeaderText = "Problema relatado";
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
    }
}
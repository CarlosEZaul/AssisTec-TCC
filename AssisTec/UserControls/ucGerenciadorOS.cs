using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using AssisTec.SubForms_do_Gerenciador_de_Pedidos;
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
            ApplyModernDesign();
            listGrid();
        }

        #region DesingModerno

        private void ApplyModernDesign()
        {
            try
            {
                this.Text = "Gerenciador de Ordens de Serviço";
                this.BackColor = Color.FromArgb(39, 55, 76);

                // Labels
                // DesingComponentes.ApplyLabelStyles(this);

                // TextBox
                DesingComponentes.StyleTextBox(txtBusca);
                 
                DesingComponentes.centralizarPanel(panelBotoes, this.Width);
                // Botões
                DesingComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnDelete, Color.FromArgb(209, 17, 65));

                // DataGridView
                DesingComponentes.StyleDataGridView(dgvOS, DataGridViewAutoSizeColumnsMode.Fill);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message);
            }
        }

        #endregion

        private void enableBtn()
        {
            btnGerenciar.Enabled =  true;
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
            dgvOS.Columns[6].HeaderText = "Última Atualização";
            dgvOS.Columns[7].HeaderText = "Valor total";
            dgvOS.Columns[8].HeaderText = "Problema relatado";
        }

        private void listGrid()
        {
            try
            {
                con.OpenConnection();
                sql = @"SELECT 
                            os.id_os,
                            c.nome as cliente,
                            us.nome as usuario,
                            e.descricao as equipamento,
                            os.status,
                            os.data_abertura,
                            os.data_atualizacao,
                            os.valor_total,
                            os.problema_relatado
                        FROM ordem_servico os
                        INNER JOIN clientes c ON os.id_cliente = c.id_cliente
                        INNER JOIN usuarios us ON os.id_tecnico  = us.id_usuario
                        INNER JOIN equipamentos e ON os.id_equipamento = e.id_equipamento
                        ORDER BY os.id_os DESC";
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
            ucFormularioOS ucFormularioOs = new ucFormularioOS(dgvOS);
            this.Controls.Add(ucFormularioOs);
            ucFormularioOs.BringToFront();
            ucFormularioOs.Left = (this.ClientSize.Width - ucFormularioOs.Width)/2;
            ucFormularioOs.Top = (this.ClientSize.Height - ucFormularioOs.Height)/2;
            ucFormularioOs.Show();
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listGrid();
        }

        private void btnGerenciar_Click(object sender, EventArgs e)
        {
            OrdemServico ordemServico = new OrdemServico();
            FrmEditarPedido frmEditarPedido = new FrmEditarPedido(ordemServico);
            
            frmEditarPedido.Left = (this.ClientSize.Width - frmEditarPedido.Width)/2;
            frmEditarPedido.Top = (this.ClientSize.Height - frmEditarPedido.Height)/2;
            frmEditarPedido.Show();
        }

        private void dgvOS_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            enableBtn();
        }
    }
}
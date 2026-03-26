using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.AtendeClienteService;
using MySql.Data.MySqlClient;
using Exception = System.Exception;

namespace AssisTec.UserControls
{
    public partial class ucGerenciador_Clientes : UserControl
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int modo;
        private int id;
        private string uf;
        private bool okCep;

        public ucGerenciador_Clientes()
        {
            InitializeComponent();
            
            ApplyModernDesign();
        }

        private void ucGerenciadorClientes_Load(object sender, EventArgs e)
        {
            btnNew.Focus();
            listGrid();
        }
        
        #region Design Moderno

        private void ApplyModernDesign()
        {
            try
            {
                this.Text = "Gerenciador de Clientes";
                this.BackColor = Color.FromArgb(240, 240, 240);

                // Labels
                //DesingComponentes.ApplyLabelStyles(this);

                // TextBox
                DesingComponentes.StyleTextBox(txtBusca);
                
                // Botões
                DesingComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnDelete, Color.FromArgb(209, 17, 65));

                // DataGridView
                DesingComponentes.StyleDataGridView(dgvClientes, DataGridViewAutoSizeColumnsMode.Fill);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message);
            }
        }

        #endregion

        #region Métodos de Manipulação de Dados

        
        
        private void formartGrid()
        { 
            if (dgvClientes.Columns.Count > 0)
            {
                dgvClientes.Columns[0].HeaderText = "ID";
                dgvClientes.Columns[0].Visible = false;
                dgvClientes.Columns[1].HeaderText = "NOME";
                dgvClientes.Columns[2].HeaderText = "CPF";
                dgvClientes.Columns[3].HeaderText = "TELEFONE";
                dgvClientes.Columns[4].HeaderText = "DATA DE NASC.";
                dgvClientes.Columns[5].HeaderText = "CEP";
                dgvClientes.Columns[6].HeaderText = "RUA";
                dgvClientes.Columns[7].HeaderText = "NUMERO";
                dgvClientes.Columns[8].HeaderText = "CIDADE";
                dgvClientes.Columns[9].HeaderText = "BAIRRO";
                dgvClientes.Columns[10].HeaderText = "ESTADO";
                dgvClientes.Columns[11].HeaderText = "COMPLEMENTO";
            }
            
            
        }
        
        private void listGrid()
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM clientes ORDER BY NOME ASC";
                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvClientes.DataSource = dt;
                con.CloseConnection();
                formartGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Cliente cliente =  new Cliente();
            cliente.id = id;
            cliente.deletarCLiente(id);
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listGrid();
        }

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvClientes.Rows.Count > 0)
            {
                try
                {
                    id = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells[0].Value);
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao selecionar registro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM clientes WHERE nome LIKE @nome ORDER BY NOME ASC";  
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", txtBusca.Text + "%");
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                dgvClientes.DataSource = dt;
                con.CloseConnection();
                formartGrid();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Erro na busca: " + ex.Message);
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
        #endregion
}

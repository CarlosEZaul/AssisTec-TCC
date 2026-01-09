using System.Windows.Forms;
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
using AssisTec.SubForms_do_Gerenciador_de_Pedidos;
using MySql.Data.MySqlClient;
using Refit;
using Exception = System.Exception;
namespace AssisTec
{
    public partial class Gerenciador_Pedidos : Form
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int id;
        private string uf;
        private bool okCep;
        
        public Gerenciador_Pedidos()
        {
            InitializeComponent();
            ApplyModernDesign();
            
            
        }

        private void Gerenciador_Pedidos_Load(object sender, EventArgs e)
        {
            btnNew.Focus();
            listGrid();
            formatGrid();
        }
        #region Design Moderno

        private void ApplyModernDesign()
        {
            try
            {
                // Propriedades do formulário (específicas deste form)
                this.Text = "Gerenciador de Clientes";
                this.BackColor = Color.FromArgb(240, 240, 240);
                this.Font = new Font("Segoe UI", 9F);
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
                this.StartPosition = FormStartPosition.CenterScreen;

                // Estilo dos painéis (específicos deste form)
                panel1.BackColor = Color.FromArgb(39, 54, 77);
                panel2.BackColor = Color.FromArgb(32, 45, 64);
        
                // Estilo das labels: Usando o método estático
                DesingComponentes.ApplyLabelStyles(this);

                // Estilo dos cabeçalhos de seção (específicos deste form)
                label1.Font = new Font("Segoe UI Semibold", 14F);
                label1.ForeColor = Color.White;
                
                DesingComponentes.StyleTextBox(txtBusca);

                // Estilo das caixas de texto com máscara: Usando o método estático para cada controle
                

                // Estilo dos botões: Usando o método estático para cada controle
                DesingComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnCancel, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnImprimir, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnDelete, Color.FromArgb(209, 17, 65));
                // ... (outros Buttons)

                // Estilo do DataGridView: Usando o método estático (se o form tiver um DataGridView)
                DesingComponentes.StyleDataGridView(dgvPedidos, DataGridViewAutoSizeColumnsMode.AllCells); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion
        
        #region Métodos de Manipulação de Dados

        private void enableBtn()
        {
            btnDelete.Enabled = true;
            btnGerenciar.Enabled = true;
            btnCancel.Enabled = true;
        }

        private void disableBtn()
        {
            btnDelete.Enabled = false;
            btnGerenciar.Enabled = false;
            btnCancel.Enabled = false;
        }
        
        private Pedido formPedido()
        {
            Pedido pedido = new Pedido();
            pedido.id_pedido = id;
            return pedido;
            
        }
        
        private void formatGrid()
        {
            if (dgvPedidos.Columns.Count > 0)
            {
                dgvPedidos.Columns[0].HeaderText = "ID PEDIDO";
                dgvPedidos.Columns[1].HeaderText = "CLIENTE";
                dgvPedidos.Columns[2].HeaderText = "TÉCNICO";
                dgvPedidos.Columns[3].HeaderText = "EQUIPAMENTO";
                dgvPedidos.Columns[4].HeaderText = "PROBLEMA_RELATADO";
                dgvPedidos.Columns[5].HeaderText = "DIAGNÓTICO";
                dgvPedidos.Columns[6].HeaderText = "STATUS";
                dgvPedidos.Columns[7].HeaderText = "DATA_ABERTURA";
                dgvPedidos.Columns[8].HeaderText = "DATA_ATUALIZAÇÃO";
                dgvPedidos.Columns[9].HeaderText = "DATA_FECHAMENTO";
                dgvPedidos.Columns[10].HeaderText = "VALOR_OBRA";
                dgvPedidos.Columns[11].HeaderText = "VALOR_PEÇAS";
                dgvPedidos.Columns[12].HeaderText = "VALOR_TOTAL";
                dgvPedidos.Columns[13].HeaderText = "OBSERVAÇÕES";

                dgvPedidos.Columns[3].Visible = false;
                dgvPedidos.Columns[4].Visible = false;
                dgvPedidos.Columns[5].Visible = false;
                dgvPedidos.Columns[13].Visible = false;

            }
        }
        
        private void listGrid()
        {
            try
            {
                con.OpenConnection();

                sql = @" SELECT 
                                p.id_pedido,
                                c.nome AS cliente,
                                u.nome AS tecnico,
                                e.descricao AS equipamento,
                                p.problema_relatado,
                                p.diagnostico,
                                p.status,
                                p.data_abertura,
                                p.data_atualizacao,
                                p.data_fechamento,
                                p.valor_mao_obra,
                                p.valor_pecas,
                                p.valor_total,
                                p.observacoes
                            FROM pedidos p
                            LEFT JOIN clientes c      ON p.id_cliente = c.id_cliente
                            LEFT JOIN usuarios u      ON p.id_tecnico = u.id_usuario
                            LEFT JOIN equipamentos e  ON p.id_equipamento = e.id_equipamento
                            ORDER BY p.id_pedido ASC;
";

                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);

                DataTable dt = new DataTable();
                da.Fill(dt);

                dgvPedidos.DataSource = dt;
                
                con.CloseConnection();
                formatGrid();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        private void btnNew_Click(object sender, EventArgs e)
        {
            Novo_Pedido novo_pedido = new Novo_Pedido();
            novo_pedido.ShowDialog();
            listGrid();
            
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT p.*, e.descricao\nFROM pedidos p\nINNER JOIN equipamentos e \n    ON p.id_equipamento = e.id_equipamento\nWHERE e.descricao LIKE @descricao\nORDER BY e.descricao ASC;\n";  
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", txtBusca.Text + "%");
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                dgvPedidos.DataSource = dt;
                con.CloseConnection();
                formatGrid();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Erro na busca: " + ex.Message);
            }
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listGrid();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja excluir pedido?", "Confirmar Exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    con.OpenConnection();
                    sql = "DELETE FROM pedidos WHERE id_pedido = @id";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                    listGrid();
                }
                catch(Exception exception)
                {
                    MessageBox.Show("Erro ao excluir pedido: " + exception.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
                
        }

        private void dgvPedidos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Pedido pedido = formPedido();
            if (e.RowIndex >= 0 && dgvPedidos.Rows.Count > 0)
            {
                try
                {
                    pedido.id_pedido= id=Convert.ToInt32(dgvPedidos.Rows[e.RowIndex].Cells[0].Value);
                    
                    
                    enableBtn();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao selecionar registro: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            disableBtn();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            Editar_Pedido editar_Pedido = new Editar_Pedido();
            editar_Pedido.ShowDialog();
        }

        private void dgvPedidos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Editar_Pedido editar_Pedido = new Editar_Pedido();
            editar_Pedido.ShowDialog();
        }
    }
}
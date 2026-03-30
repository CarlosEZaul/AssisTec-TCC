using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Usuarios;

namespace AssisTec.UserControls
{
    public partial class ucGerenciador_Usuario : UserControl
    {
        
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int modo;
        private int id;
        private string uf;
        private bool okCep;
        int nivel = 0;
        
        
        
        public ucGerenciador_Usuario()
        {
            InitializeComponent();
        }
        private void ucGerenciador_Usuario_Load(object sender, EventArgs e)
        {
            ConfigurarComboBox();
            ApplyModernDesign();
            listGrid();
            
        }
        #region Design Moderno
        
         private void ApplyModernDesign()
         {
             try
             {
                 this.Text = "Gerenciador de Usuários";
                 this.BackColor = Color.FromArgb(39, 55, 76);

                 // Labels
                // DesingComponentes.ApplyLabelStyles(this);

                 // TextBox
                 DesingComponentes.StyleTextBox(txtBusca);
                 

                 // Botões
                 DesingComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                 DesingComponentes.StyleButton(btnDelete, Color.FromArgb(209, 17, 65));

                 // DataGridView
                 DesingComponentes.StyleDataGridView(dgvUsuarios, DataGridViewAutoSizeColumnsMode.Fill);

             }
             catch (Exception ex)
             {
                 MessageBox.Show("Erro ao aplicar design: " + ex.Message);
             }
         }

        

        #endregion
        

        #region Metodos ou funcoes

        private void ConfigurarComboBox()
        {
            cbNivel.Items.Clear();
            cbNivel.Items.Add("1- Gerente");
            cbNivel.Items.Add("2- Atendente");
            cbNivel.Items.Add("3 - Técnico de TI");
            cbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        
        private void enableBtn()
        {
            btnNew.Enabled = true;
            btnDelete.Enabled = true;
            btnEditar.Enabled = true;
            
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
                dgvUsuarios.DataSource = dt;
                con.CloseConnection();
                formartGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private void formartGrid()
        {
            if (dgvUsuarios.Columns.Count <= 0) return;
            // Headers
            dgvUsuarios.Columns[0].HeaderText = "ID";
            dgvUsuarios.Columns[0].Visible = false;
            dgvUsuarios.Columns[1].HeaderText = "Nome";
            dgvUsuarios.Columns[2].HeaderText = "CPF";
            dgvUsuarios.Columns[3].HeaderText = "Senha";
            dgvUsuarios.Columns[3].Visible = false;
            dgvUsuarios.Columns[4].HeaderText = "Telefone";
            dgvUsuarios.Columns[5].HeaderText = "Nível";
            dgvUsuarios.Columns[6].HeaderText = "Status";
            dgvUsuarios.Columns[7].HeaderText = "CEP";
            dgvUsuarios.Columns[8].HeaderText = "Rua";
            dgvUsuarios.Columns[9].HeaderText = "Número";
            dgvUsuarios.Columns[10].HeaderText = "Cidade";
            dgvUsuarios.Columns[11].HeaderText = "Bairro";
            dgvUsuarios.Columns[12].HeaderText = "Estado";
            dgvUsuarios.Columns[13].HeaderText = "Complemento";
            
        }
        

        
        
        private void btnNew_Click(object sender, EventArgs e)
        {
            ucFormularioUsuarios ucFormularioUsuarios = new ucFormularioUsuarios(id, modo = 1, dgvUsuarios);
            this.Controls.Add(ucFormularioUsuarios);
            ucFormularioUsuarios.BringToFront();
            ucFormularioUsuarios.Left = (this.ClientSize.Width - ucFormularioUsuarios.Width)/2;
            ucFormularioUsuarios.Top = (this.ClientSize.Height - ucFormularioUsuarios.Height)/2;
            ucFormularioUsuarios.Show();
        }
        
        
        //
        #endregion

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ucFormularioUsuarios ucFormularioUsuarios = new ucFormularioUsuarios(id, modo = 2, dgvUsuarios);
            this.Controls.Add(ucFormularioUsuarios);
            ucFormularioUsuarios.BringToFront();
            ucFormularioUsuarios.Left = (this.ClientSize.Width - ucFormularioUsuarios.Width)/2;
            ucFormularioUsuarios.Top = (this.ClientSize.Height - ucFormularioUsuarios.Height)/2;
            ucFormularioUsuarios.Show(); 
        }
        

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvUsuarios.Rows.Count > 0)
            {
                try
                {
                    enableBtn();
                    btnEditar.Enabled = true;
                    

                    id = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells[0].Value);

                    /*int nivel = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells["nivel"].Value);
                    foreach (var item in cbNivel.Items)
                    {
                        if (item.ToString().StartsWith(nivel.ToString()))
                        {
                            cbNivel.SelectedItem = item;
                            break;
                        }
                    }*/
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja excluir usuário?", "Confirmar Exclusão", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.OpenConnection();
                    sql = "DELETE FROM usuarios WHERE id_usuario = @id";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                    listGrid();
                    
                    
                    MessageBox.Show("Cliente excluído com sucesso!", "Sucesso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    listGrid();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Erro ao excluir cliente!\n" + exception.Message, "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            btnDelete.Enabled = false;
            btnEditar.Enabled = false;
        }
        


        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listGrid();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM usuarios WHERE nome LIKE @nome ORDER BY NOME ASC";  
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", txtBusca.Text + "%");
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                dgvUsuarios.DataSource = dt;
                con.CloseConnection();
                formartGrid();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine("Erro na busca: " + ex.Message);
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }


        
    }
}
    

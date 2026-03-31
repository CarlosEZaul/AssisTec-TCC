using System;
using System.Collections.Generic;
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
            var lista = new List<dynamic>()
            {
                new { Id = 0, Nome = "Todos" },
                new { Id = 1, Nome = "1- Gerente" },
                new { Id = 2, Nome = "2- Atendente" },
                new { Id = 3, Nome = "3- Técnico" }
            };
            cbNivel.DataSource = lista;
            cbNivel.DisplayMember = "Nome";
            cbNivel.ValueMember = "Id";
            
            
            
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
                txtBusca.Text = null;
                cbNivel.SelectedValue = 0;
                cbInativo.Checked = false;
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

        private void Filtro()
        {
            try
            {
                con.OpenConnection();

                string sql = "SELECT * FROM usuarios WHERE 1=1";

                // 🔍 Filtro nome (só aplica se tiver texto)
                if (!string.IsNullOrWhiteSpace(txtBusca.Text))
                {
                    sql += " AND nome LIKE @nome";
                }

                // ☑ Filtro status
                if (cbInativo.Checked)
                {
                    sql += " AND status = 'Desativado'";
                }

                // 👤 Filtro nível
                int nivelSelecionado = 0;

                if (cbNivel.SelectedValue != null &&
                    int.TryParse(cbNivel.SelectedValue.ToString(), out nivelSelecionado))
                {
                    if (nivelSelecionado != 0) // 0 = Todos
                    {
                        sql += " AND nivel = @nivel";
                    }
                }

                sql += " ORDER BY nome ASC";

                cmd = new MySqlCommand(sql, con.con);

                // Parâmetros
                if (!string.IsNullOrWhiteSpace(txtBusca.Text))
                {
                    cmd.Parameters.AddWithValue("@nome", txtBusca.Text + "%");
                }

                if (nivelSelecionado != 0)
                {
                    cmd.Parameters.AddWithValue("@nivel", nivelSelecionado);
                }

                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);

                dgvUsuarios.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro: " + ex.Message);
            }
            finally
            {
                con.CloseConnection();
                formartGrid();
            }
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
            Filtro();
        }

        private void label5_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Filtro();
        }

        private void cbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Filtro();
        }

        private void cbNivel_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Filtro();
        }
    }
}
    

using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Refit;

namespace AssisTec
{
    public partial class Gerenciador_Usuarios : Form
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int modo;
        private int id;
        private string uf;
        private bool okCep;
        int nivel = 0;

        // Novos controles para melhor organização
        private Panel pnlFormulario;
        private Panel pnlBotoes;
        private Panel pnlLista;
        private Panel pnlBusca;
        private GroupBox gbDadosUsuario;
        private GroupBox gbListaUsuarios;
        private Label lblTitulo;
        private Label lblSubtitulo;
        private Button btnTogglePassword;
        private PictureBox picIcone;

        public Gerenciador_Usuarios()
        {
            InitializeComponent();
            disabletxt();
            ApplyModernDesign();
            
        }

        private void Gerenciador_Usuarios_Load(object sender, EventArgs e)
        {
            ConfigurarComboBoxes();
            listGrid();
        }
         #region Design Moderno

        private void ApplyModernDesign()
        {
            try
            {
                // Propriedades do formulário.
                this.Text = "Gerenciador de Clientes";
                this.BackColor = Color.FromArgb(240, 240, 240);
                this.Font = new Font("Segoe UI", 9F);
                this.FormBorderStyle = FormBorderStyle.FixedSingle;
                this.MaximizeBox = false;
                this.StartPosition = FormStartPosition.CenterScreen;

                // Estilo dos painéis
                panel1.BackColor = Color.FromArgb(39, 54, 77);
                panel2.BackColor = Color.FromArgb(32, 45, 64);

                // Estilo das labels
                foreach (Control control in this.Controls)
                {
                    if (control is Panel panel)
                    {
                        foreach (Control panelControl in panel.Controls)
                        {
                            if (panelControl is Label label)
                            {
                                label.ForeColor = Color.White; // branco puro
                                label.Font = new Font("Segoe UI", 9F);
                            }
                        }
                    }
                }

                // Estilo dos cabeçalhos de seção
                label4.Font = new Font("Segoe UI Semibold", 14F);
                label4.ForeColor = Color.White;
                
                // Estilo das caixas de texto
                StyleTextBox(txtName);
                StyleTextBox(txtBusca);
                StyleTextBox(txtSenha);
                StyleTextBox(txtBusca);

                // Estilo das caixas de texto com máscara
                StyleMaskedTextBox(mtbCPF);
                StyleMaskedTextBox(mtbTel);

                // Estilo dos botões
                StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                StyleButton(btnEditar, Color.FromArgb(0, 120, 215));
                StyleButton(btnSave, Color.FromArgb(0, 120, 215));
                StyleButton(btnDelete, Color.FromArgb(209, 17, 65));
                StyleButton(btnCancel, Color.FromArgb(100, 100, 100));

                // Estilo do DataGridView
                StyleDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void StyleDataGridView()
        {
            dgvUsuarios.BorderStyle = BorderStyle.None;
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvUsuarios.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsuarios.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 232, 255);
            dgvUsuarios.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvUsuarios.BackgroundColor = Color.White;
            dgvUsuarios.RowHeadersVisible = false;
            dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.RowTemplate.Height = 35;
            dgvUsuarios.EnableHeadersVisualStyles = false;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dgvUsuarios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsuarios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10);
            dgvUsuarios.ColumnHeadersHeight = 40;
            dgvUsuarios.DefaultCellStyle.Font = new Font("Segoe UI", 9);
        }

        private void StyleTextBox(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.BackColor = Color.White;
            textBox.Font = new Font("Segoe UI", 9F);
            textBox.ForeColor = Color.FromArgb(60, 60, 60);
        }

        private void StyleMaskedTextBox(MaskedTextBox maskedTextBox)
        {
            maskedTextBox.BorderStyle = BorderStyle.FixedSingle;
            maskedTextBox.BackColor = Color.White;
            maskedTextBox.Font = new Font("Segoe UI", 9F);
            maskedTextBox.ForeColor = Color.FromArgb(60, 60, 60);
        }

        private void StyleButton(Button button, Color backgroundColor)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = backgroundColor;
            button.ForeColor = Color.White;
            button.Font = new Font("Segoe UI Semibold", 9F);
            button.Cursor = Cursors.Hand;
            // Não alterar o tamanho para evitar problemas de layout
        }

        #endregion
        

        #region Metodos ou funcoes

        private void ConfigurarComboBoxes()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Ativo");
            cbStatus.Items.Add("Desativado");
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            cbNivel.Items.Clear();
            cbNivel.Items.Add("1- Apenas Visualização");
            cbNivel.Items.Add("2- Gestor/Funcionário");
            cbNivel.Items.Add("3- Gerente");
            cbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        
        
        private void enableBtn()
        {
            btnNew.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = true;
            btnEditar.Enabled = true;
            btnSave.Enabled = true;
        }

        private void desableBtn()
        {
            btnNew.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            btnEditar.Enabled = false;
            btnSave.Enabled = false;
        }

        private void disable()
        {
            txtName.Enabled = false;
            mtbCPF.Enabled = false;
            mtbTel.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            cbStatus.Enabled = false;
            cbStatus.DropDownStyle = ComboBoxStyle.Simple;
            cbNivel.Enabled = false;
            cbNivel.DropDownStyle = ComboBoxStyle.Simple;
        }

        private void disabletxt()
        {
            txtName.Enabled = false;
            mtbCPF.Enabled = false;
            mtbTel.Enabled = false;
            txtSenha.Enabled = false;
            cbStatus.Enabled = false;
            cbStatus.DropDownStyle = ComboBoxStyle.Simple;
            cbNivel.Enabled = false;
            cbNivel.DropDownStyle = ComboBoxStyle.Simple;
        }

        private void deleteAll()
        {
            txtName.Text = "";
            mtbCPF.Text = "";
            mtbTel.Text = "";
            txtSenha.Text = "";
            txtBusca.Text = "";
            cbStatus.SelectedText = null;
            cbNivel.SelectedText = null;
            
        }

        private void enableTxt()
        {
            txtName.Enabled = true;
            mtbCPF.Enabled = true;
            txtSenha.Enabled = true;
            mtbTel.Enabled = true;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            cbStatus.Enabled = true;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDown;
            cbNivel.Enabled = true;
            cbNivel.DropDownStyle = ComboBoxStyle.DropDown;
        }
        private void listGrid()
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
            if (dgvUsuarios.Columns.Count > 0)
            {
                dgvUsuarios.Columns[0].HeaderText = "ID";
                dgvUsuarios.Columns[0].Visible = false;
                dgvUsuarios.Columns[1].HeaderText = "NOME";
                dgvUsuarios.Columns[1].Width = 200;
                dgvUsuarios.Columns[2].HeaderText = "CPF";
                dgvUsuarios.Columns[2].Width = 120;
                dgvUsuarios.Columns[3].HeaderText = "TELEFONE";
                dgvUsuarios.Columns[3].Width = 120;
                dgvUsuarios.Columns[4].HeaderText = "SENHA";
                dgvUsuarios.Columns[4].Width = 120;
                dgvUsuarios.Columns[5].HeaderText = "STATUS";
                dgvUsuarios.Columns[5].Width = 80;
                dgvUsuarios.Columns[6].HeaderText = "NÍVEL";
                dgvUsuarios.Columns[6].Width = 50;
                
            }
        }
        Usuario formUsuario()
        {
            Usuario user = new Usuario();
            user.id = id;
            user.nome = txtName.Text;
            user.cpf = mtbCPF.Text.Replace(",","").Replace(".","").Replace("-","").Trim();
            user.telefone = mtbTel.Text;
            user.senha = txtSenha.Text;
            
            if (cbNivel.SelectedItem != null)
            {
                string texto = cbNivel.SelectedItem.ToString();
                user.nivel = int.Parse(texto.Split('-')[0].Trim());
            }
            
            user.status = cbStatus.SelectedItem?.ToString() ?? "";
            return user;
        }

        private bool usuarioExiste( string cpf, int? ignorarId=null)
        {
            con.OpenConnection();
            sql = "SELECT * FROM usuarios WHERE cpf = @cpf";
            if (ignorarId.HasValue)
            {
                sql += " AND id <> @id"; 
            }

            using (MySqlCommand cmd = new MySqlCommand(sql, con.con))
            {
                cmd.Parameters.AddWithValue("@cpf", cpf);
                if (ignorarId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@id", ignorarId.Value);
                    
                }
                int cout = Convert.ToInt32(cmd.ExecuteScalar());
                return cout > 0;
            }
            
        }
        private void editarUsuario()
        {
            Usuario user = formUsuario();
            if (user == null)
            {
                MessageBox.Show("Tentativa de cadastro com campo inválido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                if (usuarioExiste(user.cpf, user.id))
                {
                    MessageBox.Show("Usuário já existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                con.OpenConnection();
                sql = "update usuarios set nome=@nome, cpf=@cpf, telefone = @tel, nivel=@nivel,senha=@senha, status=@status where id=@id";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", user.nome);
                cmd.Parameters.AddWithValue("@cpf", user.cpf);
                cmd.Parameters.AddWithValue("@tel", user.telefone);
                cmd.Parameters.AddWithValue("@nivel", user.nivel);
                cmd.Parameters.AddWithValue("@senha", user.senha);
                cmd.Parameters.AddWithValue("@status", user.status);
                cmd.Parameters.AddWithValue("@id", user.id);
                cmd.ExecuteNonQuery();
                con.CloseConnection();
                MessageBox.Show("Usuário editado com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listGrid();
                
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void novoUsuario()
        {
            Usuario user = formUsuario();
            try
            {
                con.OpenConnection();
                sql =
                    "insert into usuarios nome, cpf, telefone,senha, nivel, status values (@nome, @cpf, @telefone, @senha, @nivel, @status)";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", user.nome);
                cmd.Parameters.AddWithValue("@cpf", user.cpf);
                cmd.Parameters.AddWithValue("@telefone", user.telefone);
                cmd.Parameters.AddWithValue("@senha", user.senha);
                cmd.Parameters.AddWithValue("@nivel", user.nivel);
                cmd.Parameters.AddWithValue("@status", user.status);

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar usuário "+ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void btnNew_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            btnEditar.Enabled = false;
            modo = 2;
            enableTxt();
            deleteAll();
            txtName.Focus();
            
        }
        
        async Task BuscarCep(string cep)
        {
            try
            {
                
                Cursor = Cursors.WaitCursor;
                
                var cepBuscar = RestService.For<ICepApiService>("http://viacep.com.br");
                var endereco = await cepBuscar.GetAdressAsync(cep);
                
                txtCidade.Text = endereco.cidade;
                txtRua.Text = endereco.rua;
                txtBairro.Text = endereco.bairro;
                txtEstado.Text = endereco.estado + " - " + endereco.uf;
                okCep = true;
                
                if (endereco.cidade == null && endereco.rua == null && endereco.bairro == null)
                {
                    MessageBox.Show("Falha ao localizar CEP!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    okCep = false;
                }

                uf = endereco.uf;
            }
            catch (Exception ex)
            {
                MessageBox.Show("CEP inválido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                okCep = false;
            }
            finally
            {
                
                Cursor = Cursors.Default;
            }
        }
        
        #endregion

        private void btnEditar_Click(object sender, EventArgs e)
        {
            enableTxt();
            btnCancel.Enabled = true;
            btnNew.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;
            btnEditar.Enabled = false;
            modo = 1;   
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtName.Text)|| string.IsNullOrEmpty(txtSenha.Text)|| !mtbCPF.MaskFull|| !mtbTel.MaskFull ||
               string.IsNullOrEmpty(cbNivel.Text)|| string.IsNullOrEmpty(cbStatus.Text))
            {
                MessageBox.Show("Preencha todos os campos corretamente", "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            if (modo == 1)
            {
                editarUsuario();
                modo = 0;
                disable();
                btnNew.Enabled = true;
                deleteAll();
                
            }

            if (modo == 2)
            {
                novoUsuario();
                modo = 0;
            }
            
            
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvUsuarios.Rows.Count > 0)
            {
                try
                {
                    enableBtn();
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    id = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells[0].Value.ToString());
                    txtName.Text = dgvUsuarios.Rows[e.RowIndex].Cells[1].Value.ToString();
                    mtbCPF.Text = dgvUsuarios.Rows[e.RowIndex].Cells[2].Value.ToString();
                    mtbTel.Text = dgvUsuarios.Rows[e.RowIndex].Cells[3].Value.ToString();
                    txtSenha.Text = dgvUsuarios.Rows[e.RowIndex].Cells[4].Value.ToString();
                    
                    int nivel=  Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells[6].Value.ToString());
                    foreach (var item in cbNivel.Items)
                    {
                        if (item.ToString().StartsWith(nivel.ToString()))
                        {
                            cbNivel.SelectedItem = item;
                            break;
                        }
                    }
                    
                    cbStatus.SelectedItem = dgvUsuarios.Rows[e.RowIndex].Cells[5].Value.ToString();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            
        }

        private void mtbCep_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(mtbCep.Text) && mtbCep.Text.Replace("-", "").Length == 8)
            {
                BuscarCep(mtbCep.Text);
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
                    sql = "DELETE FROM usuarios WHERE id = @id";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                    listGrid();
                    deleteAll();
                    
                    MessageBox.Show("Cliente excluído com sucesso!", "Sucesso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            deleteAll();
            disable();
            desableBtn();
            btnNew.Enabled = true;
            btnDelete.Enabled = false;
            btnBuscar.Enabled = false;
        }

        
    }
}
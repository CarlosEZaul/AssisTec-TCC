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
        
        public Gerenciador_Usuarios()
        {
            InitializeComponent();
            disabletxt();
            ApplyModernDesign();

        }

        private void Gerenciador_Usuarios_Load(object sender, EventArgs e)
        {
            ConfigurarComboBox();
            listGrid();
            
        }
         #region Design Moderno
        
         private void ApplyModernDesign()
         {
             try
             {
                 this.Text = "Gerenciador de Usuários";
                 this.BackColor = Color.FromArgb(240, 240, 240);

                 // Labels
                 DesingComponentes.ApplyLabelStyles(this);

                 // TextBox
                 DesingComponentes.StyleTextBox(txtNome);
                 DesingComponentes.StyleTextBox(txtRua);

                 // MaskedTextBox
                 DesingComponentes.StyleMaskedTextBox(mtbCPF);
                 DesingComponentes.StyleMaskedTextBox(mtbCep);

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
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Ativo");
            cbStatus.Items.Add("Desativado");
            cbStatus.DropDownStyle = ComboBoxStyle.DropDownList;

            cbNivel.Items.Clear();
            cbNivel.Items.Add("1- Apenas Visualização");
            cbNivel.Items.Add("2- Gestor/Funcionário");
            cbNivel.Items.Add("3 - Técnico de TI");
            cbNivel.Items.Add("4 - Gerente");
            cbNivel.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        
        
        private void enableBtn()
        {
            btnNew.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = true;
            btnEditar.Enabled = true;
            btnSave.Enabled = true;
            btnBuscar.Enabled = true;
        }

        private void desableBtn()
        {
            btnNew.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            btnEditar.Enabled = false;
            btnSave.Enabled = false;
            btnBuscar.Enabled = false;
        }

        private void disable()
        {
            txtNome.Enabled = false;
            mtbCPF.Enabled = false;
            mtbTel.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            cbStatus.Enabled = false;
            btnBuscar.Enabled = false;
            cbStatus.DropDownStyle = ComboBoxStyle.Simple;
            cbNivel.Enabled = false;
            cbNivel.DropDownStyle = ComboBoxStyle.Simple;
        }

        private void disabletxt()
        {
            txtNome.Enabled = false;
            mtbCPF.Enabled = false;
            mtbTel.Enabled = false;
            txtSenha.Enabled = false;
            txtRua.Enabled = false;
            txtBairro.Enabled = false;
            txtCidade.Enabled = false;
            txtComp.Enabled = false;
            txtEstado.Enabled=false;
            txtNumber.Enabled = false;
            cbStatus.Enabled = false;
            cbStatus.DropDownStyle = ComboBoxStyle.Simple;
            cbNivel.Enabled = false;
            cbNivel.DropDownStyle = ComboBoxStyle.Simple;
        }

        private void deleteAll()
        {
            txtNome.Text = "";
            txtSenha.Text = "";
            mtbCPF.Text = "";
            mtbTel.Text = "";
            txtSenha.Text = "";
            txtBusca.Text = "";
            mtbCep.Text = "";
            txtRua.Text = "";
            txtBairro.Text = "";
            txtCidade.Text = "";
            txtComp.Text = "";
            txtNumber.Text = "";
            txtEstado.Text = "";
            cbStatus.SelectedText = null;
            cbNivel.SelectedText = null;
            
        }

        private void enableTxt()
        {
            txtNome.Enabled = true;
            mtbCPF.Enabled = true;
            txtSenha.Enabled = true;
            mtbTel.Enabled = true;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            cbStatus.Enabled = true;
            txtRua.Enabled = true;
            txtBairro.Enabled = true;
            txtCidade.Enabled = true;
            txtComp.Enabled = true;
            txtNumber.Enabled = true;
            txtEstado.Enabled=true;
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
            if (dgvUsuarios.Columns.Count <= 0) return;
            // Headers
            dgvUsuarios.Columns[0].HeaderText = "ID";
            dgvUsuarios.Columns[0].Visible = false;
            dgvUsuarios.Columns[1].HeaderText = "Nome";
            dgvUsuarios.Columns[2].HeaderText = "CPF";
            dgvUsuarios.Columns[3].HeaderText = "Senha";
            dgvUsuarios.Columns[4].HeaderText = "Telefone";
            dgvUsuarios.Columns[5].HeaderText = "Nível";
            dgvUsuarios.Columns[6].HeaderText = "Status";
            dgvUsuarios.Columns[7].HeaderText = "CEP";
            dgvUsuarios.Columns[8].HeaderText = "Rua";
            dgvUsuarios.Columns[9].HeaderText = "Número";
            dgvUsuarios.Columns[10].HeaderText = "Cidade";
            dgvUsuarios.Columns[11].HeaderText = "Bairro";
            dgvUsuarios.Columns[12].HeaderText = "Estado";
            dgvUsuarios.Columns[13].HeaderText = "Complemento"; // Coluna 13
            
        }
        Usuario formUsuario()
        {
            Usuario user = new Usuario();
            user.id = id;
            user.nome = txtNome.Text;
            user.cpf = mtbCPF.Text;
            user.telefone = mtbTel.Text;
            user.senha = txtSenha.Text;

            if (cbNivel.SelectedItem != null)
            {
                string texto = cbNivel.SelectedItem.ToString();
                user.nivel = int.Parse(texto.Split('-')[0].Trim());
            }

            user.status = cbStatus.SelectedItem?.ToString() ?? "";

            
            user.cep = mtbCep.Text;
            user.rua = txtRua.Text;
            user.numero = txtNumber.Text;
            user.cidade = txtCidade.Text;
            user.bairro = txtBairro.Text;
            user.estado = txtEstado.Text;
            user.complemento = txtComp.Text;

            return user;
        }


        private bool usuarioExiste(string cpf, int? ignorarId = null)
        {
            con.OpenConnection();
            sql = "SELECT COUNT(*) FROM usuarios WHERE cpf = @cpf";
            if (ignorarId.HasValue)
            {
                sql += " AND id_usuario <> @id"; 
            }

            using (MySqlCommand cmd = new MySqlCommand(sql, con.con))
            {
                cmd.Parameters.AddWithValue("@cpf", cpf);
                if (ignorarId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@id", ignorarId.Value);
                }

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.CloseConnection();
                return count > 0;
            }
        }
        private bool editarUsuario()
        {
            Usuario user = formUsuario();
            if (user == null)
            {
                MessageBox.Show("Tentativa de cadastro com campo inválido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                if (usuarioExiste(user.cpf, user.id))
                {
                    MessageBox.Show("Usuário já existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
                con.OpenConnection();
                sql = @"UPDATE usuarios 
                SET nome=@nome, cpf=@cpf, telefone=@tel, nivel=@nivel, senha=@senha, status=@status, 
                    cep=@cep, rua=@rua, numero=@numero, cidade=@cidade, bairro=@bairro, estado=@estado, complemento=@complemento
                WHERE id_usuario=@id";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", user.nome);
                cmd.Parameters.AddWithValue("@cpf", user.cpf);
                cmd.Parameters.AddWithValue("@tel", user.telefone);
                cmd.Parameters.AddWithValue("@nivel", user.nivel);
                cmd.Parameters.AddWithValue("@senha", user.senha);
                cmd.Parameters.AddWithValue("@status", user.status);
                cmd.Parameters.AddWithValue("@cep", user.cep);
                cmd.Parameters.AddWithValue("@rua", user.rua);
                cmd.Parameters.AddWithValue("@numero", user.numero);
                cmd.Parameters.AddWithValue("@cidade", user.cidade);
                cmd.Parameters.AddWithValue("@bairro", user.bairro);
                cmd.Parameters.AddWithValue("@estado", user.estado);
                cmd.Parameters.AddWithValue("@complemento", user.complemento);
                cmd.Parameters.AddWithValue("@id", user.id);

                cmd.ExecuteNonQuery();
                con.CloseConnection();

                MessageBox.Show("Usuário editado com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listGrid();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }


        private bool novoUsuario()
        {
            Usuario user = formUsuario();
            
            if (user == null)
            {
                MessageBox.Show("Tentativa de cadastro com campo inválido. Verifique os dados.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            try
            {
                if (usuarioExiste(user.cpf))
                {
                    MessageBox.Show("Usuário com este CPF já existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                
                con.OpenConnection();
                sql = "INSERT INTO usuarios (nome, cpf, telefone, senha, nivel, status, cep, rua, numero, cidade, bairro, estado, complemento) " +
                      "VALUES (@nome, @cpf, @telefone, @senha, @nivel, @status, @cep, @rua, @numero, @cidade, @bairro, @estado, @complemento)";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", user.nome);
                cmd.Parameters.AddWithValue("@cpf", user.cpf);
                cmd.Parameters.AddWithValue("@telefone", user.telefone);
                cmd.Parameters.AddWithValue("@senha", user.senha);
                cmd.Parameters.AddWithValue("@nivel", user.nivel);
                cmd.Parameters.AddWithValue("@status", user.status);
                cmd.Parameters.AddWithValue("@cep", user.cep);
                cmd.Parameters.AddWithValue("@rua", user.rua);
                cmd.Parameters.AddWithValue("@numero", user.numero);
                cmd.Parameters.AddWithValue("@cidade", user.cidade);
                cmd.Parameters.AddWithValue("@bairro", user.bairro);
                cmd.Parameters.AddWithValue("@estado", user.estado);
                cmd.Parameters.AddWithValue("@complemento", user.complemento);

                cmd.ExecuteNonQuery();
                con.CloseConnection();

                MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                listGrid();
                deleteAll();
                return true;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Erro ao cadastrar usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
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
            txtNome.Focus();
            
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
            if(string.IsNullOrEmpty(txtNome.Text)|| string.IsNullOrEmpty(txtSenha.Text)|| !mtbCPF.MaskFull|| !mtbTel.MaskFull ||
               string.IsNullOrEmpty(cbNivel.Text)|| string.IsNullOrEmpty(cbStatus.Text))
            {
                MessageBox.Show("Preencha todos os campos corretamente", "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtNome.Focus();
                return;
            }

            bool success = false; 

            if (modo == 1) 
            {
                success = editarUsuario(); 
            }

            if (modo == 2) 
            {
                success = novoUsuario(); 
            }
    
            
            if (success)
            {
                modo = 0;
                disable();
                btnNew.Enabled = true;
                deleteAll(); 
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

                    id = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells[0].Value);
                    txtNome.Text = dgvUsuarios.Rows[e.RowIndex].Cells["nome"].Value.ToString();
                    mtbCPF.Text = dgvUsuarios.Rows[e.RowIndex].Cells["cpf"].Value.ToString();
                    mtbTel.Text = dgvUsuarios.Rows[e.RowIndex].Cells["telefone"].Value.ToString();
                    mtbCep.Text = dgvUsuarios.Rows[e.RowIndex].Cells["cep"].Value.ToString();
                    txtSenha.Text = dgvUsuarios.Rows[e.RowIndex].Cells["senha"].Value.ToString();
                    cbStatus.SelectedItem = dgvUsuarios.Rows[e.RowIndex].Cells["status"].Value.ToString();

                    int nivel = Convert.ToInt32(dgvUsuarios.Rows[e.RowIndex].Cells["nivel"].Value);
                    foreach (var item in cbNivel.Items)
                    {
                        if (item.ToString().StartsWith(nivel.ToString()))
                        {
                            cbNivel.SelectedItem = item;
                            break;
                        }
                    }

                    // Endereço
                    txtRua.Text = dgvUsuarios.Rows[e.RowIndex].Cells["rua"].Value.ToString();
                    txtNumber.Text = dgvUsuarios.Rows[e.RowIndex].Cells["numero"].Value.ToString();
                    txtCidade.Text = dgvUsuarios.Rows[e.RowIndex].Cells["cidade"].Value.ToString();
                    txtBairro.Text = dgvUsuarios.Rows[e.RowIndex].Cells["bairro"].Value.ToString();
                    txtEstado.Text = dgvUsuarios.Rows[e.RowIndex].Cells["estado"].Value.ToString();
                    txtComp.Text = dgvUsuarios.Rows[e.RowIndex].Cells["complemento"].Value.ToString();
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
                    sql = "DELETE FROM usuarios WHERE id_usuario = @id";
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
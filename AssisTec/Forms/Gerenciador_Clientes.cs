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
using Refit;
using Exception = System.Exception;

namespace AssisTec
{
    public partial class Gerenciador_Clientes : Form
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int modo;
        private int id;
        private string uf;
        private bool okCep;

        public Gerenciador_Clientes()
        {
            InitializeComponent();
            disabletxt();
            ApplyModernDesign(); // Aplicar design moderno
        }

        private void Gerenciador_ClientesLoad(object sender, EventArgs e)
        {
            
            disable();
            btnNew.Focus();
            listGrid();
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

                lblEnd.Font = new Font("Segoe UI Semibold", 14F);
                lblEnd.ForeColor = Color.White;


                // Estilo das caixas de texto: Usando o método estático para cada controle
                DesingComponentes.StyleTextBox(txtName);
                DesingComponentes.StyleTextBox(txtRua);
                // ... (outros TextBoxes)
                DesingComponentes.StyleTextBox(txtBusca);

                // Estilo das caixas de texto com máscara: Usando o método estático para cada controle
                DesingComponentes.StyleMaskedTextBox(mtbCPF);
                // ... (outros MaskedTextBoxes)
                DesingComponentes.StyleMaskedTextBox(mtbCep);

                // Estilo dos botões: Usando o método estático para cada controle
                DesingComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                
                DesingComponentes.StyleButton(btnDelete, Color.FromArgb(209, 17, 65));
                // ... (outros Buttons)

                // Estilo do DataGridView: Usando o método estático (se o form tiver um DataGridView)
                 DesingComponentes.StyleDataGridView(dgvClientes); 
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        #region Métodos de Manipulação de Dados

        private Cliente formCliente()
        {
            Cliente cliente = new Cliente();
            cliente.id = id;
            cliente.nome = txtName.Text;
            cliente.cpf = mtbCPF.Text;
            cliente.telefone = mtbTel.Text;
            cliente.dataNascimento = mtbNasc.Text;
            string dataFormatada = cliente.dataNascimentoFormatada;
            if (dataFormatada == null)
            {
                return null;
            }
            
            cliente.cep = mtbCep.Text;
            cliente.rua = txtRua.Text;
            cliente.numero = Convert.ToInt32(txtNumber.Text);
            cliente.cidade = txtCidade.Text;
            cliente.estado = txtEstado.Text;
            cliente.bairro = txtBairro.Text;
            cliente.complemento = txtComp.Text;
            return cliente;
            
        }
        
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
            dgvClientes.Columns[1].Width = 130;  
            dgvClientes.Columns[2].Width = 100;  
            dgvClientes.Columns[3].Width = 100;  
            dgvClientes.Columns[4].Width = 80;
            dgvClientes.Columns[5].Width = 60;   
            dgvClientes.Columns[6].Width = 140;   
            dgvClientes.Columns[7].Width = 120; 
            dgvClientes.Columns[8].Width = 120;  
            dgvClientes.Columns[9].Width = 100;   
            dgvClientes.Columns[10].Width = 120; 
            dgvClientes.Columns[11].Width = 120; 
            
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
            mtbNasc.Enabled = false;
            mtbTel.Enabled = false;
            mtbCep.Enabled = false;
            txtComp.Enabled = false;
            txtNumber.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            txtEstado.Enabled = false;
            txtRua.Enabled = false;
            txtCidade.Enabled = false;
            txtBairro.Enabled = false;
            btnBuscar.Enabled = false;
        }

        private void disabletxt()
        {
            txtName.Enabled = false;
            mtbCPF.Enabled = false;
            mtbNasc.Enabled = false;
            mtbTel.Enabled = false;
            mtbCep.Enabled = false;
            txtComp.Enabled = false;
            txtNumber.Enabled = false;
            txtEstado.Enabled = false;
            txtRua.Enabled = false;
            txtCidade.Enabled = false;
            txtBairro.Enabled = false;
        }
        
        private void deleteAll()
        {
            txtName.Text = "";
            mtbCPF.Text = "";
            mtbNasc.Text = "";
            txtNumber.Text = "";
            mtbTel.Text = "";
            txtRua.Text = "";
            txtBairro.Text = "";
            txtCidade.Text = "";
            mtbCep.Text = "";
            txtEstado.Text = "";
            txtComp.Text = "";
            txtBusca.Text = "";
        }

        private void enableTxt()
        {
            txtName.Enabled = true;
            mtbCPF.Enabled = true;
            mtbNasc.Enabled = true;
            txtNumber.Enabled = true;
            mtbTel.Enabled = true;
            mtbCep.Enabled = true;
            txtComp.Enabled = true;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            
            txtRua.BackColor = Color.White;
            txtCidade.BackColor = Color.White;
            txtBairro.BackColor = Color.White;
            txtEstado.BackColor = Color.White;
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

        
        
        
        private void editarCliente()
        {
            Cliente cliente = formCliente();
            if (cliente == null)
            {
                MessageBox.Show("Tentativa de cadastro com campo inválido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            try
            {
                con.OpenConnection();
                sql = "update clientes set nome=@nome, cpf=@cpf, telefone=@telefone, datanasc=@datanasc, cep=@cep, rua=@rua, numero=@numero, cidade=@cidade, estado=@estado, bairro=@bairro, complemento=@complemento where id_cliente=@id";
                
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", cliente.id);
                cmd.Parameters.AddWithValue("@nome", cliente.nome);
                cmd.Parameters.AddWithValue("@cpf", cliente.cpf);
                cmd.Parameters.AddWithValue("@telefone", cliente.telefone);
                cmd.Parameters.AddWithValue("@datanasc", cliente.dataNascimentoFormatada);
                cmd.Parameters.AddWithValue("@cep", cliente.cep);
                cmd.Parameters.AddWithValue("@rua", cliente.rua);
                cmd.Parameters.AddWithValue("@numero", cliente.numero);
                cmd.Parameters.AddWithValue("@cidade", cliente.cidade);
                cmd.Parameters.AddWithValue("@estado", cliente.estado);
                cmd.Parameters.AddWithValue("@bairro", cliente.bairro);
                cmd.Parameters.AddWithValue("@complemento", cliente.complemento);

                cmd.ExecuteNonQuery();
                con.CloseConnection();
                modo = 0;
                deleteAll();
                disable();
                listGrid();
                    
                
                MessageBox.Show("Cliente editado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Erro ao editar!\n" + exception.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
        }

        private void novoCliente()
        {
            Cliente cliente = formCliente();
            if (cliente == null)
            {
                MessageBox.Show("Cadastro com um campo inválido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            try
            {
                    
                con.OpenConnection();
                sql = "SELECT COUNT(*) FROM clientes WHERE cpf=@cpf";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@cpf", cliente.cpf);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    MessageBox.Show("Cliente já cadastrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.CloseConnection();
                }
                else
                {
                    disable();
                    btnNew.Enabled = true;
                    btnBuscar.Enabled = false;

                    con.OpenConnection();
                    sql = "insert into clientes (nome, cpf, telefone, datanasc, cep, rua, numero, cidade, estado, bairro, complemento) values (@nome, @cpf, @telefone, @datanasc, @cep, @rua, @numero, @cidade, @estado, @bairro, @complemento)";
                    cmd = new MySqlCommand(sql, con.con);
                        
                    cmd.Parameters.AddWithValue("@nome", cliente.nome);
                    cmd.Parameters.AddWithValue("@cpf", cliente.cpf);
                    cmd.Parameters.AddWithValue("@telefone", cliente.telefone);
                    cmd.Parameters.AddWithValue("@datanasc", cliente.dataNascimentoFormatada);
                    cmd.Parameters.AddWithValue("@cep", cliente.cep);
                    cmd.Parameters.AddWithValue("@rua", cliente.rua);
                    cmd.Parameters.AddWithValue("@numero", cliente.numero);
                    cmd.Parameters.AddWithValue("@cidade", cliente.cidade);
                    cmd.Parameters.AddWithValue("@estado", cliente.estado);
                    cmd.Parameters.AddWithValue("@bairro", cliente.bairro);
                    cmd.Parameters.AddWithValue("@complemento", cliente.complemento);

                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                    
                    
                    MessageBox.Show("Cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    deleteAll();
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    listGrid();
                }
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar cliente\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
            
        }
        
        #endregion

        #region Manipuladores de Eventos

        private void btnNew_Click(object sender, EventArgs e)
        {
            btnBuscar.Enabled = true;
            btnCancel.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            modo = 2;
            enableTxt();
            
            btnDelete.Enabled = false;
            deleteAll();
            
            txtName.Focus();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                !mtbCPF.MaskFull ||
                !mtbTel.MaskFull ||
                !mtbCep.MaskFull ||
                string.IsNullOrWhiteSpace(txtNumber.Text) ||
                !DateTime.TryParseExact(mtbNasc.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios corretamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }
            
            if (modo == 1 && okCep == true) 
            {
                editarCliente();
                
            }
            else if (modo == 2 && okCep == true) 
            {
                novoCliente();
            }
            else if (!okCep)
            {
                MessageBox.Show("Por favor, verifique o CEP informado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            enableTxt();
            btnBuscar.Enabled = true;
            btnCancel.Enabled = true;
            btnNew.Enabled = false;
            btnDelete.Enabled = false;
            btnSave.Enabled = true;
            btnEditar.Enabled = false;
            modo = 1;
        }

        private void mtbCep_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(mtbCep.Text) && mtbCep.Text.Replace("-", "").Length == 8)
            {
                BuscarCep(mtbCep.Text);
            }
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja excluir cliente?", "Confirmar Exclusão", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.OpenConnection();
                    sql = "DELETE FROM clientes WHERE id_cliente = @id";
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

        private void dgvClientes_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
            if (e.RowIndex >= 0 && dgvClientes.Rows.Count > 0)
            {
                try
                {
                    enableBtn();
                    
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    btnEditar.Enabled = true;
                    
                    id = Convert.ToInt32(dgvClientes.Rows[e.RowIndex].Cells[0].Value);
                    txtName.Text = dgvClientes.Rows[e.RowIndex].Cells[1].Value.ToString();
                    mtbCPF.Text = dgvClientes.Rows[e.RowIndex].Cells[2].Value.ToString();
                    mtbTel.Text = dgvClientes.Rows[e.RowIndex].Cells[3].Value.ToString();
                    
                    DateTime data = Convert.ToDateTime(dgvClientes.Rows[e.RowIndex].Cells[4].Value.ToString());
                    mtbNasc.Text = data.ToString("dd/MM/yyyy");
                    
                    mtbCep.Text = dgvClientes.Rows[e.RowIndex].Cells[5].Value.ToString();
                    BuscarCep(mtbCep.Text);
                    
                    txtNumber.Text = dgvClientes.Rows[e.RowIndex].Cells[7].Value.ToString();
                    txtComp.Text = dgvClientes.Rows[e.RowIndex].Cells[11].Value.ToString();
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

        #endregion

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listGrid();
        }


        private void label14_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }
    }
}
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
    public partial class Gerenciador_Usuarios : Form
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int modo;
        private string id;
        private string uf;
        private bool okCep;

        public Gerenciador_Usuarios()
        {
            InitializeComponent();
            disabletxt();
            ApplyModernDesign(); // Aplicar design moderno
        }

        private void Gerenciador_Usuarios_Load(object sender, EventArgs e)
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Ativo");
            cbStatus.Items.Add("Desativado");
            disable();
            btnNew.Focus();
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

                lblendereco.Font = new Font("Segoe UI Semibold", 14F);
                lblendereco.ForeColor = Color.White;


                // Estilo das caixas de texto
                StyleTextBox(txtName);
                StyleTextBox(txtRua);
                StyleTextBox(txtNumber);
                StyleTextBox(txtBairro);
                StyleTextBox(txtCidade);
                StyleTextBox(txtEstado);
                StyleTextBox(txtComp);
                StyleTextBox(txtBusca);

                // Estilo das caixas de texto com máscara
                StyleMaskedTextBox(mtbCPF);
                StyleMaskedTextBox(mtbTel);
                StyleMaskedTextBox(mtbNasc);
                StyleMaskedTextBox(mtbCep);

                // Estilo dos botões
                StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                StyleButton(btnEditar, Color.FromArgb(0, 120, 215));
                StyleButton(btnSave, Color.FromArgb(0, 120, 215));
                StyleButton(btnDelete, Color.FromArgb(209, 17, 65));
                StyleButton(btnCancel, Color.FromArgb(100, 100, 100));
                StyleButton(btnBuscar, Color.FromArgb(0, 120, 215));

                // Estilo do DataGridView
                StyleDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void StyleDataGridView()
        {
            dgvTecnicos.BorderStyle = BorderStyle.None;
            dgvTecnicos.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            dgvTecnicos.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTecnicos.DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 232, 255);
            dgvTecnicos.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvTecnicos.BackgroundColor = Color.White;
            dgvTecnicos.RowHeadersVisible = false;
            dgvTecnicos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTecnicos.RowTemplate.Height = 35;
            dgvTecnicos.EnableHeadersVisualStyles = false;
            dgvTecnicos.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(50, 50, 50);
            dgvTecnicos.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvTecnicos.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10);
            dgvTecnicos.ColumnHeadersHeight = 40;
            dgvTecnicos.DefaultCellStyle.Font = new Font("Segoe UI", 9);
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

        #region Métodos de Manipulação de Dados

        private string DataFormatada(string dat)
        {
            try
            {
                string data = mtbNasc.Text;
                DateTime dataConvertida = DateTime.ParseExact(data, "dd/MM/yyyy", null);
                return dataConvertida.ToString("yyyy/MM/dd");
            }
            catch
            {
                MessageBox.Show("Formato de data inválido. Use DD/MM/AAAA.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        
        private void formartGrid()
        { 
            if (dgvTecnicos.Columns.Count > 0)
            {
                dgvTecnicos.Columns[0].HeaderText = "ID";
                dgvTecnicos.Columns[0].Visible = false;
                dgvTecnicos.Columns[1].HeaderText = "NOME";
                dgvTecnicos.Columns[2].HeaderText = "CPF";
                dgvTecnicos.Columns[3].HeaderText = "TEL.";
                dgvTecnicos.Columns[4].HeaderText = "DATA DE NASC.";
                dgvTecnicos.Columns[5].HeaderText = "CEP";
                dgvTecnicos.Columns[6].HeaderText = "RUA";
                dgvTecnicos.Columns[7].HeaderText = "NUMERO";
                dgvTecnicos.Columns[8].HeaderText = "CIDADE";
                dgvTecnicos.Columns[9].HeaderText = "ESTADO";
                dgvTecnicos.Columns[10].HeaderText = "BAIRRO";
                dgvTecnicos.Columns[11].HeaderText = "COMPLEMENTO";
            }
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
            cbStatus.Enabled = false;
            cbStatus.DropDownStyle = ComboBoxStyle.Simple;
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
            cbStatus.Enabled = false;
            cbStatus.DropDownStyle = ComboBoxStyle.Simple;
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
            cbStatus.SelectedText = null;
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
            cbStatus.Enabled = true;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDown;
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
                sql = "SELECT * FROM tecnicos ORDER BY NOME ASC";
                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvTecnicos.DataSource = dt;
                con.CloseConnection();
                formartGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        async Task BuscarCep(string cep)
        {
            try
            {
                // Mostrar indicador de carregamento
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
            // Validação de campos obrigatórios
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                !mtbCPF.MaskFull ||
                !mtbTel.MaskFull ||
                !mtbCep.MaskFull ||
                string.IsNullOrWhiteSpace(cbStatus.Text)||
                string.IsNullOrWhiteSpace(txtNumber.Text) ||
                !DateTime.TryParseExact(mtbNasc.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios corretamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }

            string dataformat = DataFormatada(mtbNasc.Text);
            if (dataformat == null) return;
            
            if (modo == 1 && okCep == true) // Editar
            {
                try
                {
                    con.OpenConnection();
                    sql = "update tecnicos set nome=@nome, cpf=@cpf, telefone=@telefone, datanasc=@datanasc, cep=@cep, rua=@rua, numero=@numero, cidade=@cidade, estado=@estado, bairro=@bairro, complemento=@complemento, status=@status where id=@id";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@nome", txtName.Text);
                    cmd.Parameters.AddWithValue("@cpf", mtbCPF.Text);
                    cmd.Parameters.AddWithValue("@telefone", mtbTel.Text);
                    cmd.Parameters.AddWithValue("@datanasc", dataformat);
                    cmd.Parameters.AddWithValue("@cep", mtbCep.Text);
                    cmd.Parameters.AddWithValue("@rua", txtRua.Text);
                    cmd.Parameters.AddWithValue("@numero", txtNumber.Text);
                    cmd.Parameters.AddWithValue("@cidade", txtCidade.Text);
                    cmd.Parameters.AddWithValue("@estado", uf);
                    cmd.Parameters.AddWithValue("@bairro", txtBairro.Text);
                    cmd.Parameters.AddWithValue("@complemento", txtComp.Text);
                    cmd.Parameters.AddWithValue("@status", cbStatus.Text);

                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                    modo = 0;
                    deleteAll();
                    disable();
                    listGrid();
                    
                    // Mostrar mensagem de sucesso
                    MessageBox.Show("tecnicos editado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Erro ao editar!\n" + exception.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
            else if (modo == 2 && okCep == true) // Novo 
            {
                try
                {
                    
                    con.OpenConnection();
                    sql = "SELECT COUNT(*) FROM tecnicos WHERE cpf=@cpf";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@cpf", mtbCPF.Text);
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    if (count > 0)
                    {
                        MessageBox.Show("tecnico já cadastrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        con.CloseConnection();
                    }
                    else
                    {
                        disable();
                        btnNew.Enabled = true;
                        btnBuscar.Enabled = false;

                        con.OpenConnection();
                        sql = "insert into tecnicos (nome, cpf, telefone, datanasc, cep, rua, numero, cidade, estado, bairro, complemento, status) values (@nome, @cpf, @telefone, @datanasc, @cep, @rua, @numero, @cidade, @estado, @bairro, @complemento, @status)";
                        cmd = new MySqlCommand(sql, con.con);
                        cmd.Parameters.AddWithValue("@nome", txtName.Text);
                        cmd.Parameters.AddWithValue("@cpf", mtbCPF.Text);
                        cmd.Parameters.AddWithValue("@telefone", mtbTel.Text);
                        cmd.Parameters.AddWithValue("@datanasc", dataformat);
                        cmd.Parameters.AddWithValue("@cep", mtbCep.Text);
                        cmd.Parameters.AddWithValue("@rua", txtRua.Text);
                        cmd.Parameters.AddWithValue("@numero", txtNumber.Text);
                        cmd.Parameters.AddWithValue("@cidade", txtCidade.Text);
                        cmd.Parameters.AddWithValue("@estado", uf);
                        cmd.Parameters.AddWithValue("@bairro", txtBairro.Text);
                        cmd.Parameters.AddWithValue("@complemento", txtComp.Text);
                        cmd.Parameters.AddWithValue("@status", cbStatus.Text);

                        cmd.ExecuteNonQuery();
                        con.CloseConnection();
                    
                        // Mostrar mensagem de sucesso
                        MessageBox.Show("Cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        deleteAll();
                        btnSave.Enabled = false;
                        btnNew.Enabled = true;
                        listGrid();
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao cadastrar técnico\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja excluir tecnico?", "Confirmar Exclusão", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.OpenConnection();
                    sql = "DELETE FROM tecnicos WHERE id = @id";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                    listGrid();
                    deleteAll();
                    
                    MessageBox.Show("Tecnico excluído com sucesso!", "Sucesso", 
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

        private void dgvTecnicos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            if (e.RowIndex >= 0 && dgvTecnicos.Rows.Count > 0)
            {
                try
                {
                    enableBtn();
                    
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    btnEditar.Enabled = true;
                    btnPDF.Enabled = true;
                    id = dgvTecnicos.Rows[e.RowIndex].Cells[0].Value.ToString();
                    txtName.Text = dgvTecnicos.Rows[e.RowIndex].Cells[1].Value.ToString();
                    mtbCPF.Text = dgvTecnicos.Rows[e.RowIndex].Cells[2].Value.ToString();
                    mtbTel.Text = dgvTecnicos.Rows[e.RowIndex].Cells[3].Value.ToString();
                    
                    DateTime data = Convert.ToDateTime(dgvTecnicos.Rows[e.RowIndex].Cells[4].Value.ToString());
                    mtbNasc.Text = data.ToString("dd/MM/yyyy");
                    
                    mtbCep.Text = dgvTecnicos.Rows[e.RowIndex].Cells[5].Value.ToString();
                    BuscarCep(mtbCep.Text);
                    
                    txtNumber.Text = dgvTecnicos.Rows[e.RowIndex].Cells[7].Value.ToString();
                    txtComp.Text = dgvTecnicos.Rows[e.RowIndex].Cells[11].Value.ToString();
                    cbStatus.SelectedItem = dgvTecnicos.Rows[e.RowIndex].Cells[12].Value.ToString();
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
                sql = "SELECT * FROM tecnicos WHERE nome LIKE @nome ORDER BY NOME ASC";  
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", txtBusca.Text + "%");
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                dgvTecnicos.DataSource = dt;
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


        private void btnPDF_Click(object sender, EventArgs e)
        {
            
        }
    }
}
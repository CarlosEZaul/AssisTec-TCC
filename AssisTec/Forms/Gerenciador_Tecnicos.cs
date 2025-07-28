using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Refit;
using Exception = System.Exception;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Mysqlx;
using Font = System.Drawing.Font;
using font = iTextSharp.text.Font;
using Image = iTextSharp.text.Image;
using Rectangle = iTextSharp.text.Rectangle;


namespace AssisTec
{
    public partial class Gerenciador_Tecnicos : Form
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int modo;
        private int id;
        private string uf;
        private bool okCep;

        public Gerenciador_Tecnicos()
        {
            InitializeComponent();
            disabletxt();
            ApplyModernDesign(); // Aplicar design moderno
        }

        private void Gerenciador_Tecnicos_Load(object sender, EventArgs e)
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("Ativo");
            cbStatus.Items.Add("Desativado");
            cbPeriodo.Items.Clear();
            cbPeriodo.Items.Add("08:00 - 14:00");
            cbPeriodo.Items.Add("14:00 - 20:00");
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
                StyleMaskedTextBox(mtbRG);
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
                MessageBox.Show("Erro ao aplicar design: " + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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



        private void formartGrid()
        {
            if (dgvTecnicos.Columns.Count > 0)
            {
                dgvTecnicos.Columns[0].HeaderText = "ID";
                dgvTecnicos.Columns[0].Visible = false;
                dgvTecnicos.Columns[1].HeaderText = "NOME";
                dgvTecnicos.Columns[2].HeaderText = "CPF";
                dgvTecnicos.Columns[3].HeaderText = "RG";
                dgvTecnicos.Columns[4].HeaderText = "TEL.";
                dgvTecnicos.Columns[5].HeaderText = "DATA DE NASC.";
                dgvTecnicos.Columns[6].HeaderText = "CEP";
                dgvTecnicos.Columns[7].HeaderText = "RUA";
                dgvTecnicos.Columns[8].HeaderText = "NUMERO";
                dgvTecnicos.Columns[9].HeaderText = "CIDADE";
                dgvTecnicos.Columns[10].HeaderText = "ESTADO";
                dgvTecnicos.Columns[11].HeaderText = "BAIRRO";
                dgvTecnicos.Columns[12].HeaderText = "COMPLEMENTO";
                dgvTecnicos.Columns[13].HeaderText = "STATUS";
                dgvTecnicos.Columns[14].HeaderText = "PERIODO";
            }
        }

        private void enableBtn()
        {
            btnNew.Enabled = true;
            btnCancel.Enabled = true;
            btnDelete.Enabled = true;
            btnEditar.Enabled = true;
            btnSave.Enabled = true;
            btnPDF.Enabled=true;
        }

        private void desableBtn()
        {
            btnNew.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;
            btnEditar.Enabled = false;
            btnSave.Enabled = false;
            btnPDF.Enabled = false;
        }

        private void disable()
        {
            txtName.Enabled = false;
            mtbCPF.Enabled = false;
            mtbRG.Enabled = false;
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
            btnPDF.Enabled = false;
            cbPeriodo.Enabled = false;
            cbPeriodo.DropDownStyle = ComboBoxStyle.Simple;
        }

        private void disabletxt()
        {
            txtName.Enabled = false;
            mtbCPF.Enabled = false;
            mtbRG.Enabled = false;
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
            cbPeriodo.Enabled = false;
            cbPeriodo.DropDownStyle = ComboBoxStyle.Simple;
        }

        private void deleteAll()
        {
            txtName.Text = "";
            mtbCPF.Text = "";
            mtbRG.Text = "";
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
            cbPeriodo.SelectedText = null;
        }

        private void enableTxt()
        {
            txtName.Enabled = true;
            mtbCPF.Enabled = true;
            mtbRG.Enabled=true;
            mtbNasc.Enabled = true;
            txtNumber.Enabled = true;
            mtbTel.Enabled = true;
            mtbCep.Enabled = true;
            txtComp.Enabled = true;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
            cbStatus.Enabled = true;
            cbStatus.DropDownStyle = ComboBoxStyle.DropDown;
            cbPeriodo.Enabled = true;
            cbPeriodo.DropDownStyle = ComboBoxStyle.DropDown;
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
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        Tecnico formTecnico()
        {
            Tecnico tecnico = new Tecnico();
            tecnico.id = id;
            tecnico.nome = txtName.Text;
            tecnico.cpf = mtbCPF.Text;
            tecnico.rg = mtbRG.Text;
            tecnico.telefone = mtbTel.Text;
            tecnico.dataNascimento = mtbNasc.Text;
            string dataFormatada = tecnico.dataNascimentoFormatada;
            if (dataFormatada == null)
            {
                return null;
            }

            tecnico.cep = mtbCep.Text;
            tecnico.rua = txtRua.Text;
            tecnico.numero = Convert.ToInt32(txtNumber.Text);
            tecnico.cidade = txtCidade.Text;
            tecnico.estado = txtEstado.Text;
            tecnico.bairro = txtBairro.Text;
            tecnico.complemento = txtComp.Text;
            tecnico.status = cbStatus.Text;
            tecnico.periodo = cbPeriodo.Text;
            return tecnico;
        }
        private bool TecnicoJaExiste(string cpf, string rg, int? idIgnorar = null)
        {
            con.OpenConnection();
            string sql = "SELECT COUNT(*) FROM tecnicos WHERE (cpf = @cpf OR rg = @rg)";
            if (idIgnorar.HasValue)
                sql += " AND id <> @id";

            using (MySqlCommand cmd = new MySqlCommand(sql, con.con))
            {
                cmd.Parameters.AddWithValue("@cpf", cpf);
                cmd.Parameters.AddWithValue("@rg", rg);
                if (idIgnorar.HasValue)
                {
                    cmd.Parameters.AddWithValue("@id", idIgnorar.Value);
                }
                
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.CloseConnection();
                return count > 0;
            }
        }

        private void novoTecnico()
        {
            btnNew.Enabled = true;
            btnBuscar.Enabled = false;
            Tecnico tecnico = formTecnico();
            if (tecnico == null)
            {
                MessageBox.Show("Cadastro com um campo inválido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            try
            {
                if (TecnicoJaExiste(tecnico.cpf, tecnico.rg))
                {
                    MessageBox.Show("Técnico já cadastrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }


                
                disable();
                btnNew.Enabled = true;
                btnBuscar.Enabled = false;

                con.OpenConnection();
                sql =
                    "insert into tecnicos (nome, cpf, rg ,telefone, datanasc, cep, rua, numero, cidade, estado, bairro, complemento, status,periodo) values (@nome, @cpf,@rg, @telefone, @datanasc, @cep, @rua, @numero, @cidade, @estado, @bairro, @complemento, @status, @periodo)";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", tecnico.nome);
                cmd.Parameters.AddWithValue("@cpf", tecnico.cpf);
                cmd.Parameters.AddWithValue("@rg", tecnico.rg);
                cmd.Parameters.AddWithValue("@telefone", tecnico.telefone);
                cmd.Parameters.AddWithValue("@datanasc", tecnico.dataNascimentoFormatada);
                cmd.Parameters.AddWithValue("@cep", tecnico.cep);
                cmd.Parameters.AddWithValue("@rua", tecnico.rua);
                cmd.Parameters.AddWithValue("@numero", tecnico.numero);
                cmd.Parameters.AddWithValue("@cidade", tecnico.cidade);
                cmd.Parameters.AddWithValue("@estado", tecnico.estado);
                cmd.Parameters.AddWithValue("@bairro", tecnico.bairro);
                cmd.Parameters.AddWithValue("@complemento", tecnico.complemento);
                cmd.Parameters.AddWithValue("@status", tecnico.status);
                cmd.Parameters.AddWithValue("@periodo", tecnico.periodo);

                cmd.ExecuteNonQuery();
                con.CloseConnection();

                // Mostrar mensagem de sucesso
                MessageBox.Show("Cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                deleteAll();
                btnSave.Enabled = false;
                btnNew.Enabled = true;
                listGrid();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar técnico\n" + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void editarTecnico()
        {
            Tecnico tecnico = formTecnico();
            if (tecnico == null)
            {
                MessageBox.Show("Tentativa de cadastro com campo inválido!", "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            try
            {
                if (TecnicoJaExiste(tecnico.cpf, tecnico.rg, tecnico.id))
                {
                    MessageBox.Show("Técnico já cadastrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
                con.OpenConnection();
                sql =
                    "update tecnicos set nome=@nome, cpf=@cpf,rg=@rg, telefone=@telefone, datanasc=@datanasc, cep=@cep, rua=@rua, numero=@numero, cidade=@cidade, estado=@estado, bairro=@bairro, complemento=@complemento, status=@status, periodo=@periodo where id=@id";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", tecnico.id);
                cmd.Parameters.AddWithValue("@nome", tecnico.nome);
                cmd.Parameters.AddWithValue("@cpf", tecnico.cpf);
                cmd.Parameters.AddWithValue("@rg", tecnico.rg);
                cmd.Parameters.AddWithValue("@telefone", tecnico.telefone);
                cmd.Parameters.AddWithValue("@datanasc", tecnico.dataNascimentoFormatada);
                cmd.Parameters.AddWithValue("@cep", tecnico.cep);
                cmd.Parameters.AddWithValue("@rua", tecnico.rua);
                cmd.Parameters.AddWithValue("@numero", tecnico.numero);
                cmd.Parameters.AddWithValue("@cidade", tecnico.cidade);
                cmd.Parameters.AddWithValue("@estado", tecnico.estado);
                cmd.Parameters.AddWithValue("@bairro", tecnico.bairro);
                cmd.Parameters.AddWithValue("@complemento", tecnico.complemento);
                cmd.Parameters.AddWithValue("@status", tecnico.status);
                cmd.Parameters.AddWithValue("@periodo", tecnico.periodo);

                cmd.ExecuteNonQuery();
                con.CloseConnection();
                modo = 0;
                deleteAll();
                disable();
                btnNew.Enabled = true;
                btnBuscar.Enabled = false;
                listGrid();

                // Mostrar mensagem de sucesso
                MessageBox.Show("tecnicos editado com sucesso!", "Sucesso", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Erro ao editar!\n" + exception.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
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

            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                !mtbCPF.MaskFull ||
                !mtbRG.MaskFull||
                !mtbTel.MaskFull ||
                !mtbCep.MaskFull ||
                string.IsNullOrWhiteSpace(cbStatus.Text) ||
                string.IsNullOrWhiteSpace(cbPeriodo.Text) ||
                string.IsNullOrWhiteSpace(txtNumber.Text) ||
                !DateTime.TryParseExact(mtbNasc.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None,
                    out _))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios corretamente!", "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                txtName.Focus();
                return;
            }


            if (modo == 1 && okCep == true) // Editar
            {
                editarTecnico();

            }

            else if (modo == 2 && okCep == true) // Novo 
            {
                novoTecnico();

            }
            else if (!okCep)
            {
                MessageBox.Show("Por favor, verifique o CEP informado.", "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
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
                    id = Convert.ToInt32(dgvTecnicos.Rows[e.RowIndex].Cells[0].Value.ToString());
                    txtName.Text = dgvTecnicos.Rows[e.RowIndex].Cells[1].Value.ToString();
                    mtbCPF.Text = dgvTecnicos.Rows[e.RowIndex].Cells[2].Value.ToString();
                    mtbRG.Text = dgvTecnicos.Rows[e.RowIndex].Cells[3].Value.ToString();
                    mtbTel.Text = dgvTecnicos.Rows[e.RowIndex].Cells[4].Value.ToString();

                    DateTime data = Convert.ToDateTime(dgvTecnicos.Rows[e.RowIndex].Cells[5].Value.ToString());
                    mtbNasc.Text = data.ToString("dd/MM/yyyy");

                    mtbCep.Text = dgvTecnicos.Rows[e.RowIndex].Cells[6].Value.ToString();
                    BuscarCep(mtbCep.Text);

                    txtNumber.Text = dgvTecnicos.Rows[e.RowIndex].Cells[8].Value.ToString();
                    txtComp.Text = dgvTecnicos.Rows[e.RowIndex].Cells[12].Value.ToString();
                    cbStatus.SelectedItem = dgvTecnicos.Rows[e.RowIndex].Cells[13].Value.ToString();
                    cbPeriodo.SelectedItem = dgvTecnicos.Rows[e.RowIndex].Cells[14].Value.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao selecionar registro: " + ex.Message, "Erro", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            desableBtn();
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
            btnNew.Enabled = true;
        }

        #endregion

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            listGrid();
        }


        private void btnPDF_Click(object sender, EventArgs e)
        {
            if (id == 0)
            {
                MessageBox.Show(("Selecione um Técnico!"));
                return;
            }

            try
            {
                Tecnico tecnico = formTecnico();
                
                SaveFileDialog salvar = new SaveFileDialog();
                salvar.Filter = "PDF FILE|*.pdf";
                salvar.Title = "Salvar PDF do Tecnico";
                salvar.FileName = "Técnico " + tecnico.nome;
                if (salvar.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                
                
                
                string caminho = salvar.FileName;
                Document doc = new Document(PageSize.A4, 30f, 30f, 40f, 40f);
                PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                
                BaseColor corPrimaria = new BaseColor(41, 128, 185);     // Azul elegante
                BaseColor corSecundaria = new BaseColor(52, 73, 94);     // Cinza escuro
                BaseColor corFundo = new BaseColor(236, 240, 241);       // Cinza claro
                BaseColor corTexto = new BaseColor(44, 62, 80);          // Cinza escuro para texto

                
                // Cabeçalho
                PdfPTable cabecalhoPrincipal = new PdfPTable(3);
                cabecalhoPrincipal.WidthPercentage = 100;
                cabecalhoPrincipal.SetWidths(new float[] { 1f, 2f, 1f }); // Logo, Título, Espaço

                // Logo na barra azul
                string caminhoLogo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img\\logo\\logo.png");
                PdfPCell cellLogoHeader;
                try 
                {
                    Image logo = Image.GetInstance(caminhoLogo);
                    logo.ScaleToFit(100f, 80f); // Logo maior
                    cellLogoHeader = new PdfPCell(logo);
                    cellLogoHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellLogoHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cellLogoHeader.PaddingLeft = 20f;
                }
                catch
                {
                    
                    font fontLogoAlt = FontFactory.GetFont("Arial", 16, font.BOLD, BaseColor.WHITE);
                    cellLogoHeader = new PdfPCell(new Phrase("LOGO", fontLogoAlt));
                    cellLogoHeader.HorizontalAlignment = Element.ALIGN_LEFT;
                    cellLogoHeader.VerticalAlignment = Element.ALIGN_MIDDLE;
                    cellLogoHeader.PaddingLeft = 20f;
                }

                cellLogoHeader.BackgroundColor = corPrimaria;
                cellLogoHeader.Border = 0;
                cellLogoHeader.FixedHeight = 80f;
                cabecalhoPrincipal.AddCell(cellLogoHeader);

                // Título 
                font fontTitulo = FontFactory.GetFont("Arial", 28, font.BOLD, BaseColor.WHITE);
                PdfPCell cellTitulo = new PdfPCell(new Phrase("AssisTec", fontTitulo));
                cellTitulo.BackgroundColor = corPrimaria;
                cellTitulo.HorizontalAlignment = Element.ALIGN_CENTER;
                cellTitulo.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellTitulo.Border = 0;
                cellTitulo.FixedHeight = 80f;
                cabecalhoPrincipal.AddCell(cellTitulo);

                
                PdfPCell cellVazia = new PdfPCell(new Phrase(""));
                cellVazia.BackgroundColor = corPrimaria;
                cellVazia.Border = 0;
                cellVazia.FixedHeight = 80f;
                cabecalhoPrincipal.AddCell(cellVazia);

                doc.Add(cabecalhoPrincipal);

                // Subtítulo
                font fontSubtitulo = FontFactory.GetFont("Arial", 14, font.ITALIC, corSecundaria);
                Paragraph subtitulo = new Paragraph("Sistema de Gestão Técnica", fontSubtitulo);
                subtitulo.Alignment = Element.ALIGN_CENTER;
                subtitulo.SpacingAfter = 20f;
                subtitulo.SpacingBefore = 15f;
                doc.Add(subtitulo);

                
                // Cabeçalho2
                PdfPTable cabecalho = new PdfPTable(2);
                cabecalho.WidthPercentage = 100;
                cabecalho.SetWidths(new float[] { 1f, 4f });
                cabecalho.SpacingAfter = 20f;

                
               

                // Título da ficha
                font fontTecnico = FontFactory.GetFont("Arial", 20, font.BOLD, corSecundaria);
                PdfPCell cellTituloSecao = new PdfPCell(new Phrase("FICHA DO TÉCNICO", fontTecnico));
                cellTituloSecao.HorizontalAlignment = Element.ALIGN_CENTER;
                cellTituloSecao.VerticalAlignment = Element.ALIGN_MIDDLE;
                cellTituloSecao.Border = 0;
                cellTituloSecao.PaddingLeft = 20f;
                cabecalho.AddCell(cellTituloSecao);

                doc.Add(cabecalho);

                // Nome do técnico em destaque
                font fontNomeTecnico = FontFactory.GetFont("Arial", 18, font.BOLD, corPrimaria);
                Paragraph nomeTecnico = new Paragraph(tecnico.nome.ToUpper(), fontNomeTecnico);
                nomeTecnico.Alignment = Element.ALIGN_CENTER;
                nomeTecnico.SpacingAfter = 25f;
                nomeTecnico.SpacingBefore = 10f;

                // linha decorativa
                PdfPTable linhaDivisoria = new PdfPTable(1);
                linhaDivisoria.WidthPercentage = 60;
                linhaDivisoria.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell cellLinha = new PdfPCell();
                cellLinha.BackgroundColor = corPrimaria;
                cellLinha.FixedHeight = 3f;
                cellLinha.Border = 0;
                linhaDivisoria.AddCell(cellLinha);

                doc.Add(nomeTecnico);
                doc.Add(linhaDivisoria);
                doc.Add(new Paragraph("\n"));

                // Dados
                font fontSecao = FontFactory.GetFont("Arial", 14, font.BOLD, BaseColor.WHITE);
                PdfPTable tituloSecao1 = new PdfPTable(1);
                tituloSecao1.WidthPercentage = 100;
                tituloSecao1.SpacingBefore = 10f;
                PdfPCell cellSecao1 = new PdfPCell(new Phrase("DADOS PESSOAIS", fontSecao));
                cellSecao1.BackgroundColor = corSecundaria;
                cellSecao1.HorizontalAlignment = Element.ALIGN_CENTER;
                cellSecao1.Padding = 8f;
                cellSecao1.Border = 0;
                tituloSecao1.AddCell(cellSecao1);
                doc.Add(tituloSecao1);

                // Tabela de dados pessoais
                PdfPTable tabelaPessoais = new PdfPTable(2);
                tabelaPessoais.WidthPercentage = 100;
                tabelaPessoais.SpacingAfter = 15f;
                tabelaPessoais.SetWidths(new float[] { 1f, 2f });

                font boldFont = FontFactory.GetFont("Arial", 11, font.BOLD, corTexto);
                font regularFont = FontFactory.GetFont("Arial", 11, font.NORMAL, corTexto);

                void AddLinhaDados(string texto, string valor, bool alternarCor = true)
                {
                    PdfPCell cell1 = new PdfPCell(new Phrase(texto, boldFont));
                    PdfPCell cell2 = new PdfPCell(new Phrase(valor ?? "N/A", regularFont));
                    
                    if (alternarCor)
                    {
                        cell1.BackgroundColor = corFundo;
                        cell2.BackgroundColor = BaseColor.WHITE;
                    }
                    else
                    {
                        cell1.BackgroundColor = BaseColor.WHITE;
                        cell2.BackgroundColor = corFundo;
                    }
                    
                    cell1.Padding = 8f;
                    cell2.Padding = 8f;
                    cell1.Border = Rectangle.BOTTOM_BORDER;
                    cell2.Border = Rectangle.BOTTOM_BORDER;
                    cell1.BorderColor = new BaseColor(189, 195, 199);
                    cell2.BorderColor = new BaseColor(189, 195, 199);
                    
                    tabelaPessoais.AddCell(cell1);
                    tabelaPessoais.AddCell(cell2);
                }

                bool alternar = true;
                AddLinhaDados("Nome Completo:", tecnico.nome, alternar = !alternar);
                AddLinhaDados("CPF:", tecnico.cpf, alternar = !alternar);
                AddLinhaDados("RG: ", tecnico.rg, alternar = !alternar);
                AddLinhaDados("Telefone:", tecnico.telefone, alternar = !alternar);
                AddLinhaDados("Data de Nascimento:", tecnico.dataNascimento, alternar = !alternar);

                doc.Add(tabelaPessoais);

                // info. profissionais
                PdfPTable tituloSecao3 = new PdfPTable(1);
                tituloSecao3.WidthPercentage = 100;
                tituloSecao3.SpacingBefore = 10f;
                PdfPCell cellSecao3 = new PdfPCell(new Phrase("INFORMAÇÕES PROFISSIONAIS", fontSecao));
                cellSecao3.BackgroundColor = corSecundaria;
                cellSecao3.HorizontalAlignment = Element.ALIGN_CENTER;
                cellSecao3.Padding = 8f;
                cellSecao3.Border = 0;
                tituloSecao3.AddCell(cellSecao3);
                doc.Add(tituloSecao3);

                // Tabela de informações profissionais
                PdfPTable tabelaProfissional = new PdfPTable(2);
                tabelaProfissional.WidthPercentage = 100;
                tabelaProfissional.SpacingAfter = 15f;
                tabelaProfissional.SetWidths(new float[] { 1f, 2f });

                
                void AddLinhaProfissional(string texto, string valor, bool alternarCor = true)
                {
                    PdfPCell cell1 = new PdfPCell(new Phrase(texto, boldFont));
                    PdfPCell cell2 = new PdfPCell(new Phrase(valor ?? "N/A", regularFont));
                    
                    if (alternarCor)
                    {
                        cell1.BackgroundColor = corFundo;
                        cell2.BackgroundColor = BaseColor.WHITE;
                    }
                    else
                    {
                        cell1.BackgroundColor = BaseColor.WHITE;
                        cell2.BackgroundColor = corFundo;
                    }
                    
                    cell1.Padding = 8f;
                    cell2.Padding = 8f;
                    cell1.Border = Rectangle.BOTTOM_BORDER;
                    cell2.Border = Rectangle.BOTTOM_BORDER;
                    cell1.BorderColor = new BaseColor(189, 195, 199);
                    cell2.BorderColor = new BaseColor(189, 195, 199);
                    
                    tabelaProfissional.AddCell(cell1);
                    tabelaProfissional.AddCell(cell2);
                }

                // Adicionar os dados profissionais
                alternar = true;
                AddLinhaProfissional("Status:", tecnico.status, alternar = !alternar);
                AddLinhaProfissional("Período:", tecnico.periodo, alternar = !alternar);

                doc.Add(tabelaProfissional);

                // Endereco
                PdfPTable tituloSecao2 = new PdfPTable(1);
                tituloSecao2.WidthPercentage = 100;
                tituloSecao2.SpacingBefore = 10f;
                PdfPCell cellSecao2 = new PdfPCell(new Phrase("ENDEREÇO", fontSecao));
                cellSecao2.BackgroundColor = corSecundaria;
                cellSecao2.HorizontalAlignment = Element.ALIGN_CENTER;
                cellSecao2.Padding = 8f;
                cellSecao2.Border = 0;
                tituloSecao2.AddCell(cellSecao2);
                doc.Add(tituloSecao2);

                // Tabela de endereço
                PdfPTable tabelaEndereco = new PdfPTable(2);
                tabelaEndereco.WidthPercentage = 100;
                tabelaEndereco.SpacingAfter = 20f;
                tabelaEndereco.SetWidths(new float[] { 1f, 2f });

                // Função local para adicionar linhas na tabela de endereço
                void AddLinhaEndereco(string texto, string valor, bool alternarCor = true)
                {
                    PdfPCell cell1 = new PdfPCell(new Phrase(texto, boldFont));
                    PdfPCell cell2 = new PdfPCell(new Phrase(valor ?? "N/A", regularFont));
                    
                    if (alternarCor)
                    {
                        cell1.BackgroundColor = corFundo;
                        cell2.BackgroundColor = BaseColor.WHITE;
                    }
                    else
                    {
                        cell1.BackgroundColor = BaseColor.WHITE;
                        cell2.BackgroundColor = corFundo;
                    }
                    
                    cell1.Padding = 8f;
                    cell2.Padding = 8f;
                    cell1.Border = Rectangle.BOTTOM_BORDER;
                    cell2.Border = Rectangle.BOTTOM_BORDER;
                    cell1.BorderColor = new BaseColor(189, 195, 199);
                    cell2.BorderColor = new BaseColor(189, 195, 199);
                    
                    tabelaEndereco.AddCell(cell1);
                    tabelaEndereco.AddCell(cell2);
                }

                // Agora adicionar os dados do endereço
                alternar = true;
                AddLinhaEndereco("CEP:", tecnico.cep, alternar = !alternar);
                AddLinhaEndereco("Rua:", tecnico.rua, alternar = !alternar);
                AddLinhaEndereco("Número:", tecnico.numero.ToString(), alternar = !alternar);
                AddLinhaEndereco("Bairro:", tecnico.bairro, alternar = !alternar);
                AddLinhaEndereco("Cidade:", tecnico.cidade, alternar = !alternar);
                AddLinhaEndereco("Estado:", tecnico.estado, alternar = !alternar);
                AddLinhaEndereco("Complemento:", tecnico.complemento, alternar = !alternar);

                doc.Add(tabelaEndereco);

                // Rodape
                // Linha decorativa
                PdfPTable linhaRodape = new PdfPTable(1);
                linhaRodape.WidthPercentage = 100;
                linhaRodape.SpacingBefore = 20f;
                PdfPCell cellLinhaRodape = new PdfPCell();
                cellLinhaRodape.BackgroundColor = corPrimaria;
                cellLinhaRodape.FixedHeight = 2f;
                cellLinhaRodape.Border = 0;
                linhaRodape.AddCell(cellLinhaRodape);
                doc.Add(linhaRodape);

                // Informações do rodapé
                font fontRodape = FontFactory.GetFont("Arial", 10, font.NORMAL, corSecundaria);
                Paragraph rodape = new Paragraph("Documento gerado automaticamente em " + DateTime.Now.ToString("dd/MM/yyyy às HH:mm"), fontRodape);
                rodape.Alignment = Element.ALIGN_CENTER;
                rodape.SpacingBefore = 10f;
                doc.Add(rodape);

                // Assinatura
                doc.Add(new Paragraph("\n\n"));
                PdfPTable assinatura = new PdfPTable(1);
                assinatura.WidthPercentage = 50;
                assinatura.HorizontalAlignment = Element.ALIGN_CENTER;
                PdfPCell cellAssinatura = new PdfPCell(new Phrase("_________________________________\nAssinatura do Técnico", fontRodape));
                cellAssinatura.HorizontalAlignment = Element.ALIGN_CENTER;
                cellAssinatura.Border = 0;
                cellAssinatura.PaddingTop = 20f;
                assinatura.AddCell(cellAssinatura);
                doc.Add(assinatura);

                doc.Close();



   
                MessageBox.Show("Documento gerado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch(Exception ex)
            {
                MessageBox.Show("Erro ao gerar PDF Documento!" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
                
        }
    }
}
       
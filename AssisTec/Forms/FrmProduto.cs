using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec
{
    public partial class FrmProduto : Form
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int modo;
        private int id;
        
        public FrmProduto()
        {
            InitializeComponent();
            ApplyModernDesign();
            listGrid();
            formartGrid();
            disable();
            ConfigurarComboBox();
            txtPrecoCompra.Text = 0.ToString();
            txtPrecoVenda.Text = 0.ToString();
        }

        #region Design Moderno

        private void ApplyModernDesign()
        {
            try
            {
                this.Text = "Gerenciador de Produtos";
                this.BackColor = Color.FromArgb(240, 240, 240);

                // Labels
                //DesingComponentes.ApplyLabelStyles(this);

                // TextBox
                DesingComponentes.StyleTextBox(txtDescricao);
                DesingComponentes.StyleTextBox(txtEstoque);
                DesingComponentes.StyleTextBox(txtEstoqueMinimo);

                // Botões
                DesingComponentes.StyleButton(btnNew, Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnDelete, Color.FromArgb(209, 17, 65));
                
                // DataGridView
                DesingComponentes.StyleDataGridView(dgvProdutos, DataGridViewAutoSizeColumnsMode.Fill);



            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message);
            }
        }

        #endregion

        #region Métodos de Manipulação de Dados

        private Produto formProduto()
        {
            Produto prod = new Produto();
            prod.id = id;
            prod.descricao = txtDescricao.Text;
            prod.unidade = cbUnidade.SelectedItem?.ToString() ?? "";
            prod.precoCompra = double.Parse(txtPrecoCompra.Text.Replace("R$", "").Trim());
            prod.precoVenda = double.Parse(txtPrecoVenda.Text.Replace("R$", "").Trim());
            prod.estoque = Convert.ToInt32(txtEstoque.Text);
            prod.estoqueMinimo = Convert.ToInt32(txtEstoqueMinimo.Text);
            return prod;
        }
        
        private void ConfigurarComboBox()
        {
            cbUnidade.Items.Clear();
            cbUnidade.Items.Add("Unidade/Peça");
            cbUnidade.Items.Add("Kg");
            cbUnidade.Items.Add("Litros");
            cbUnidade.Items.Add("Metros");
            cbUnidade.DropDownStyle = ComboBoxStyle.DropDownList;

            
        }

        private void textBoxPrecoCompra_TextChanged(object sender, EventArgs e)
        {
            if (txtPrecoCompra.Text == "") return;

            string valor = txtPrecoCompra.Text.Replace("R$", "")
                .Replace(".", "")
                .Replace(",", "");

            if (decimal.TryParse(valor, out decimal resultado))
            {
                resultado /= 100;
                txtPrecoCompra.Text = resultado.ToString("C2");
                txtPrecoCompra.SelectionStart = txtPrecoCompra.Text.Length;
            }
        }

        private void textBoxPrecoVenda_TextChanged(object sender, EventArgs e)
        {
            if (txtPrecoVenda.Text == "") return;

            string valor = txtPrecoVenda.Text.Replace("R$", "")
                .Replace(".", "")
                .Replace(",", "");

            if (decimal.TryParse(valor, out decimal resultado))
            {
                resultado /= 100;
                txtPrecoVenda.Text = resultado.ToString("C2");
                txtPrecoVenda.SelectionStart = txtPrecoVenda.Text.Length;
            }
        }

        private void formartGrid()
        {
            if (dgvProdutos.Columns.Count > 0)
            {
                dgvProdutos.Columns[0].HeaderText = "ID";
                dgvProdutos.Columns[0].Visible = false;
                dgvProdutos.Columns[1].HeaderText = "Descrição";
                dgvProdutos.Columns[2].HeaderText = "Unidade";
                dgvProdutos.Columns[3].HeaderText = "Preço de Compra";
                dgvProdutos.Columns[4].HeaderText = "Preço de Venda";
                dgvProdutos.Columns[5].HeaderText = "Estoque";
                dgvProdutos.Columns[6].HeaderText = "Estoque Minimo";
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
            txtDescricao.Enabled = false;
            cbUnidade.Enabled = false;
            txtPrecoCompra.Enabled = false;
            txtPrecoVenda.Enabled = false;
            txtEstoque.Enabled = false;
            txtEstoqueMinimo.Enabled = false;
            btnCancel.Enabled = false;
            btnSave.Enabled = false;
            
        }

        private void disabletxt()
        {
            txtDescricao.Enabled = false;
            cbUnidade.Enabled = false;
            txtPrecoCompra.Enabled = false;
            txtPrecoVenda.Enabled = false;
            txtEstoque.Enabled = false;
            txtEstoqueMinimo.Enabled = false;
        }
        
        private void deleteAll()
        {
            txtDescricao.Text = "";
            cbUnidade.Text = "";
            txtPrecoCompra.Text = "";
            txtPrecoVenda.Text = "";
            txtEstoque.Text = "";
            txtEstoqueMinimo.Text = "";
        }

        private void enableTxt()
        {
            txtDescricao.Enabled = true;
            cbUnidade.Enabled = true;
            txtPrecoCompra.Enabled = true;
            txtPrecoVenda.Enabled = true;
            txtEstoque.Enabled = true;
            txtEstoqueMinimo.Enabled = true;
            btnCancel.Enabled = true;
            btnSave.Enabled = true;
        }
        #endregion
        
        private void listGrid()
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM produtos ORDER BY descricao ASC";
                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvProdutos.DataSource = dt;
                con.CloseConnection();
                formartGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void novoProduto()
        {
            Produto produto = formProduto();
            
            if (produto == null)
            {
                MessageBox.Show("Cadastro com um campo inválido", "Erro", MessageBoxButtons.OK);
                return;
            }

            try
            {
                con.OpenConnection();
                disable();
                btnNew.Enabled = true;
                sql = "insert into produtos (descricao, unidade, preco_compra, preco_venda, estoque, estoque_minimo) values (@descricao, @unidade, @preco_compra, @preco_venda, @estoque, @estoque_minimo)";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue(@"descricao", produto.descricao);
                cmd.Parameters.AddWithValue(@"unidade", produto.unidade);
                cmd.Parameters.AddWithValue(@"preco_compra", produto.precoCompra);
                cmd.Parameters.AddWithValue(@"preco_venda", produto.precoVenda);
                cmd.Parameters.AddWithValue(@"estoque", produto.estoque);
                cmd.Parameters.AddWithValue(@"estoque_minimo", produto.estoqueMinimo);
                cmd.ExecuteNonQuery();
                con.CloseConnection();

                MessageBox.Show("Cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                deleteAll();
                btnSave.Enabled = false;
                btnNew.Enabled = true;
                listGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void editarProduto()
        {
            Produto produto = new Produto();
            if (produto == null)
            {
                MessageBox.Show("Tentativa de cadastro com campo inválido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                con.OpenConnection();
                sql = "update produtos set descricao=@descricao, unidade=@unidade, preco_compra=@preco_compra, preco_venda=@preco_venda, estoque=@estoque, estoque_minimo=@estoque_minimo where id_produto=@id_produto";
                
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id_produto", produto.id);
                cmd.Parameters.AddWithValue("@descricao", produto.descricao);
                cmd.Parameters.AddWithValue("@unidade", produto.unidade);
                cmd.Parameters.AddWithValue("@preco_compra", produto.precoCompra);
                cmd.Parameters.AddWithValue("@preco_venda", produto.precoVenda);
                cmd.Parameters.AddWithValue("@estoque", produto.estoque);
                cmd.Parameters.AddWithValue("@estoque_minimo", produto.estoqueMinimo);
                

                cmd.ExecuteNonQuery();
                con.CloseConnection();
                modo = 0;
                deleteAll();
                disable();
                listGrid();
                    
                
                MessageBox.Show("Produto editado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Erro ao editar!\n" + exception.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            btnCancel.Enabled = true;
            btnNew.Enabled = false;
            btnSave.Enabled = true;
            btnDelete.Enabled = false;
            modo = 2;
            enableTxt();
            
            btnDelete.Enabled = false;
            deleteAll();
            
            txtDescricao.Focus();
        }

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
            if (string.IsNullOrWhiteSpace(txtDescricao.Text) ||
                string.IsNullOrWhiteSpace(txtPrecoCompra.Text) ||
                string.IsNullOrWhiteSpace(txtPrecoVenda.Text) ||
                string.IsNullOrWhiteSpace(txtEstoque.Text) ||
                string.IsNullOrWhiteSpace(txtEstoqueMinimo.Text) ||
                string.IsNullOrWhiteSpace(cbUnidade.Text))
            {
                MessageBox.Show("Preencha todos os campos obrigatórios corretamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDescricao.Focus();
                return;
            }
            if (modo == 1) 
            {
                editarProduto();
                
            }
            else if (modo == 2) 
            {
                novoProduto();
            }
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Deseja excluir produto?", "Confirmar Exclusão", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.OpenConnection();
                    sql = "delete from produtos where id_produto=@id_produto";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@id_produto", id);
                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                
                    MessageBox.Show("Produto deleteado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao deletar produto: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    throw;
                }
                btnDelete.Enabled = false;
                btnEditar.Enabled = false;
            }
            
        }

        private void dgvProdutos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvProdutos.Rows.Count > 0)
            {
                try
                {
                    enableBtn();
                    btnSave.Enabled = false;
                    btnNew.Enabled = true;
                    btnEditar.Enabled = true;
                    
                    id = Convert.ToInt32(dgvProdutos.Rows[e.RowIndex].Cells[0].Value);
                    txtDescricao.Text = dgvProdutos.Rows[e.RowIndex].Cells["descricao"].Value.ToString();
                    cbUnidade.SelectedItem = dgvProdutos.Rows[e.RowIndex].Cells["unidade"].Value.ToString();
                    txtPrecoCompra.Text = dgvProdutos.Rows[e.RowIndex].Cells["preco_compra"].Value.ToString();
                    txtPrecoVenda.Text = dgvProdutos.Rows[e.RowIndex].Cells["preco_venda"].Value.ToString();
                    txtEstoque.Text = dgvProdutos.Rows[e.RowIndex].Cells["estoque"].Value.ToString();
                    txtEstoqueMinimo.Text = dgvProdutos.Rows[e.RowIndex].Cells["estoque_minimo"].Value.ToString();

                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }

        private void txtEstoque_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            // Agora validamos se é número, controle ou a vírgula (que pode ter vindo do ponto)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Impede mais de uma vírgula
            if (e.KeyChar == ',' && (sender as TextBox).Text.Contains(","))
            {
                e.Handled = true;
            }
        }

        private void txtEstoqueMinimo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '.')
            {
                e.KeyChar = ',';
            }

            // Agora validamos se é número, controle ou a vírgula (que pode ter vindo do ponto)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ',')
            {
                e.Handled = true;
            }

            // Impede mais de uma vírgula
            if (e.KeyChar == ',' && (sender as TextBox).Text.Contains(","))
            {
                e.Handled = true;
            }
        }

        private void txtBusca_TextChanged(object sender, EventArgs e)
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM produtos WHERE descricao LIKE @descricao ORDER BY descricao ASC";  
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@descricao", txtBusca.Text + "%");
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                dgvProdutos.DataSource = dt;
                con.CloseConnection();
                formartGrid();
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
    }
}
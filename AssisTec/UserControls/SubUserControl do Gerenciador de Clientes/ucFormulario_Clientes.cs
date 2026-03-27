using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Clientes.ucFormulario_Clientes
{
    public partial class ucFormulario_Clientes : UserControl
    {
        conexao con = new conexao();
        MySqlCommand cmd;
        private string sql;
        private int id;
        private int modo;
        private string uf;
        private DataGridView dgvClientes;
        private bool okCep;
        public ucFormulario_Clientes(int _id, int _modo, DataGridView _dgv)
        {
            modo =  _modo;
            id = _id;
            dgvClientes =  _dgv;
            InitializeComponent();
        }
        #region Métodos e funções
        private void ucFormulario_Clientes_Load(object sender, EventArgs e)
        {
            if (modo == 2)
            {
                carregarDados();
            }
        }

        private void fechar()
        {
            this.Hide();
        }
        private void deleteAll()
        {
            txtNome.Text = "";
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
            
        }

        public void carregarDados()
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM clientes WHERE id_cliente = @id";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader  = cmd.ExecuteReader();
                if (reader.Read())
                {
                    id = reader.GetInt32("id_cliente");
                    txtNome.Text = reader["nome"].ToString();
                    mtbCPF.Text = reader["cpf"].ToString();
                    mtbTel.Text = reader["telefone"].ToString();
                    mtbNasc.Text =  reader["datanasc"].ToString();
                    mtbCep.Text = reader["cep"].ToString();
                    txtRua.Text = reader["rua"].ToString();
                    txtNumber.Text = reader["numero"].ToString();
                    txtCidade.Text = reader["cidade"].ToString();
                    txtBairro.Text = reader["bairro"].ToString();
                    txtEstado.Text = reader["estado"].ToString();
                    txtComp.Text = reader["complemento"].ToString();
                }
                
                con.CloseConnection();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados: " + ex.Message);
            }
        }
        
        private Cliente formCliente()
        {
            Cliente cliente = new Cliente();
            cliente.id = id;
            cliente.nome = txtNome.Text;
            cliente.cpf = mtbCPF.Text;
            cliente.dataNascimento = mtbNasc.Text;
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
        

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                Cliente cliente = formCliente();
                BuscarCep(cliente.cep);
                if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                    !mtbCPF.MaskFull ||
                    !mtbTel.MaskFull ||
                    !mtbCep.MaskFull ||
                    string.IsNullOrWhiteSpace(txtNumber.Text) ||
                    !DateTime.TryParseExact(mtbNasc.Text, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out _))
                {
                    MessageBox.Show("Preencha todos os campos obrigatórios corretamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNome.Focus();
                    return;
                }
            
                if (modo == 1 && okCep == true) 
                {
                    cliente.novoCliente(cliente);
                    cliente.atualizarDados(dgvClientes);
                    deleteAll();
                
                }
                else if (modo == 2 && okCep == true) 
                {
                    cliente.editarCliente(cliente);
                    cliente.atualizarDados(dgvClientes);
                    fechar();
                }
                else if (!okCep)
                {
                    MessageBox.Show("Por favor, verifique o CEP informado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro ao gerenciar clientes", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
        }
        
        async Task BuscarCep(string cep)
        {
            try
            {
                
                Cursor = Cursors.WaitCursor;
                
                Cursor = Cursors.WaitCursor;
                
                BuscaCEP buscaCep = new BuscaCEP();
                buscaCep.cep = cep;
                buscaCep.Consultar();
                
                txtCidade.Text = buscaCep.cidade;
                txtRua.Text = buscaCep.rua;
                txtBairro.Text = buscaCep.bairro;
                txtEstado.Text = buscaCep.estado;
                
                okCep = true;
                
                if (buscaCep.rua == null || txtRua.Text == null || txtBairro.Text == null|| txtEstado.Text == null)
                {
                    MessageBox.Show("Falha ao localizar CEP!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    okCep = false;
                }
                
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
        

        private void mtbCep_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(mtbCep.Text) && mtbCep.Text.Replace("-", "").Length == 8)
            {
                BuscarCep(mtbCep.Text);
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            txtNome.Clear();
            mtbCPF.Clear();
            mtbTel.Clear();
            mtbNasc.Clear();
            mtbCep.Clear();
            txtRua.Clear();
            txtNumber.Clear();
            txtCidade.Clear();
            txtEstado.Clear();
            txtBairro.Clear();
            txtComp.Clear();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            fechar();
        }

        
    }
}
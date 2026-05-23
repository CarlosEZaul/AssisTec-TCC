using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.Business;
using AssisTec.Data;
using AssisTec.Reports;
using MySql.Data.MySqlClient;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Clientes.ucFormulario_Clientes
{
    public partial class ucFormulario_Clientes : UserControl
    {
        conexao con = new conexao();
        ClienteRository repositoryCliente = new ClienteRository();
        ClienteService serviceCliente = new ClienteService();
        ClienteRelatorio  relatorioCliente = new ClienteRelatorio();
        
        MySqlCommand cmd;
        private string sql;
        private int id;
        private int modo;
        private string uf;
        private DataGridView dgvClientes;
        private bool okCep;

        public ucFormulario_Clientes(int _id, int _modo, DataGridView _dgv)
        {
            modo = _modo;
            id = _id;
            dgvClientes = _dgv;
            InitializeComponent();
        }

        #region Métodos e funções

        private void ucFormulario_Clientes_Load(object sender, EventArgs e)
        {
            ApplyModernDesign();
            if (modo == 2)
            {
                carregarDados();
            }
        }

        private void ApplyModernDesign()
        {
            try
            {
                this.BackColor = System.Drawing.Color.FromArgb(39, 55, 76);
                DesingComponentes.StyleTextBox(txtNome);
                DesingComponentes.StyleTextBox(txtRua);
                DesingComponentes.StyleTextBox(txtBairro);
                DesingComponentes.StyleTextBox(txtCidade);
                DesingComponentes.StyleTextBox(txtEstado);
                DesingComponentes.StyleTextBox(txtNumber);
                DesingComponentes.StyleTextBox(txtComp);
                DesingComponentes.StyleMaskedTextBox(mtbCPF);
                DesingComponentes.StyleMaskedTextBox(mtbTel);
                DesingComponentes.StyleMaskedTextBox(mtbNasc);
                DesingComponentes.StyleMaskedTextBox(mtbCep);
                DesingComponentes.StyleButton(btnSave, System.Drawing.Color.FromArgb(0, 153, 76));
                DesingComponentes.StyleButton(btnLimpar, System.Drawing.Color.FromArgb(0, 120, 215));
                DesingComponentes.StyleButton(btnFechar, System.Drawing.Color.FromArgb(209, 17, 65));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao aplicar design: " + ex.Message, "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void fechar()
        {
            this.Dispose();
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
            okCep = false;
        }

        public void carregarDados()
        {
            try
            {
                Cliente cliente = new Cliente();
                cliente = repositoryCliente.ObterPorId(id);
                id = cliente.id;
                txtNome.Text = cliente.nome;
                mtbCPF.Text = cliente.cpf;
                mtbTel.Text = cliente.telefone;
                mtbNasc.Text = cliente.dataNascimento;
                mtbCep.Text = cliente.cep;
                txtRua.Text = cliente.rua;
                txtNumber.Text = cliente.numero.ToString();
                txtCidade.Text = cliente.cidade;
                txtBairro.Text = cliente.bairro;
                txtEstado.Text = cliente.estado;
                txtComp.Text = cliente.complemento;
                okCep = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados form: " + ex.Message);
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
            string dataFormatada = cliente.dataNascimentoFormatada;
            if (dataFormatada == null)
            {
                return null;
            }
            cliente.cep = mtbCep.Text;
            cliente.rua = txtRua.Text;
            if (!int.TryParse(txtNumber.Text, out int numero))
            {
                MessageBox.Show("Número do endereço inválido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }
            cliente.numero = numero;
            cliente.cidade = txtCidade.Text;
            cliente.estado = txtEstado.Text;
            cliente.bairro = txtBairro.Text;
            cliente.complemento = txtComp.Text;
            return cliente;
        }

        async Task BuscarCep(string cep)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                okCep = false;
                BuscaCEP buscaCep = new BuscaCEP();
                buscaCep.cep = cep;
                buscaCep.Consultar();
                if (string.IsNullOrWhiteSpace(buscaCep.cidade) ||
                    string.IsNullOrWhiteSpace(buscaCep.rua) ||
                    string.IsNullOrWhiteSpace(buscaCep.bairro))
                {
                    MessageBox.Show("Falha ao localizar CEP!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    okCep = false;
                    return;
                }
                txtCidade.Text = buscaCep.cidade;
                txtRua.Text = buscaCep.rua;
                txtBairro.Text = buscaCep.bairro;
                txtEstado.Text = buscaCep.estado;
                okCep = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("CEP inválido!" + ex, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                okCep = false;
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        #endregion

        #region botões ou componentes

        private async void mtbCep_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(mtbCep.Text) && mtbCep.Text.Replace("-", "").Length == 8)
            {
                await BuscarCep(mtbCep.Text);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;
                if (!okCep && mtbCep.MaskFull)
                {
                    await BuscarCep(mtbCep.Text);
                }
                if (string.IsNullOrWhiteSpace(txtNome.Text) ||
                    !mtbCPF.MaskFull ||
                    !mtbTel.MaskFull ||
                    !mtbCep.MaskFull ||
                    string.IsNullOrWhiteSpace(txtNumber.Text) ||
                    !mtbNasc.MaskFull)
                {
                    MessageBox.Show("Preencha todos os campos obrigatórios corretamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNome.Focus();
                    return;
                }
                if (!okCep)
                {
                    MessageBox.Show("CEP inválido", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Cliente cliente = formCliente();
                if (cliente == null)
                {
                    return;
                }
                if (modo == 1)
                {
                    var (sucesso, mensagem) = serviceCliente.CadastrarCliente(cliente);
                    if (sucesso)
                    {
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK);
                        deleteAll();
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK);
                    }
                    
                    dgvClientes.DataSource = repositoryCliente.ObterTodosClientes();
                    
                }
                else if (modo == 2)
                {
                    var (sucesso, mensagem)  =serviceCliente.EditarCliente(cliente);

                    if (sucesso)
                    {
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK);
                        deleteAll();
                        fechar();
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK);
                    }
                    dgvClientes.DataSource = repositoryCliente.ObterTodosClientes();
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerenciar clientes", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            deleteAll();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            fechar();
        }

        #endregion
    }
}

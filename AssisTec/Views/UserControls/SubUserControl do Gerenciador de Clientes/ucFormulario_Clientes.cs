using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.Service;
using AssisTec.Repository;
using AssisTec.Models;

namespace AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Clientes.ucFormulario_Clientes
{
    public partial class ucFormulario_Clientes : UserControl
    {
        private IClienteRepository repositoryCliente;
        private ClienteService serviceCliente;
        
        private int id;
        private int modo;
        private DataGridView dgvClientes;
        private bool okCep;

        public ucFormulario_Clientes(int _id, int _modo, DataGridView _dgv)
        {
            InitializeComponent();
            this.modo = _modo;
            
            if (modo != 1)
            {
                this.id = _id;    
            }
            
            this.dgvClientes = _dgv;
            CriarNovoContexto();
        }

        private void CriarNovoContexto()
        {
            var context = new AppDbContext();
            this.repositoryCliente = new ClienteRepository(context);
            this.serviceCliente = new ClienteService(this.repositoryCliente);
        }

        #region Métodos de Interface e Dados
        private void ucFormulario_Clientes_Load(object sender, EventArgs e)
        {
            ApplyModernDesign();
            if (modo == 2)
            {
                CarregarDados();
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
                MessageBox.Show("Erro ao aplicar design: " + ex.Message, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void Fechar()
        {
            this.Dispose();
        }

        private void DeleteAll()
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

        public void CarregarDados()
        {
            try
            {
                Cliente cliente = repositoryCliente.ObterPorId(id);
                if (cliente == null) return;

                id = cliente.Id;
                txtNome.Text = cliente.Nome;
                mtbCPF.Text = cliente.Cpf;
                mtbTel.Text = cliente.Telefone;
                mtbNasc.Text = cliente.DataNascimento.Value.ToString("dd/MM/yyyy");
                mtbCep.Text = cliente.Cep;
                txtRua.Text = cliente.Rua;
                txtNumber.Text = cliente.Numero;
                txtCidade.Text = cliente.Cidade;
                txtBairro.Text = cliente.Bairro;
                txtEstado.Text = cliente.Estado;
                txtComp.Text = cliente.Complemento;
                okCep = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados do formulário: " + ex.Message);
            }
        }

        private Cliente FormCliente()
        {
            if (!DateTime.TryParse(mtbNasc.Text, out DateTime dataNasc))
            {
                MessageBox.Show("Data de nascimento inválida.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return null;
            }

            return new Cliente
            {
                Id = id,
                Nome = txtNome.Text,
                Cpf = mtbCPF.Text,
                DataNascimento = dataNasc,
                Telefone = mtbTel.Text,
                Cep = mtbCep.Text,
                Rua = txtRua.Text,
                Numero = txtNumber.Text,
                Cidade = txtCidade.Text,
                Estado = txtEstado.Text,
                Bairro = txtBairro.Text,
                Complemento = txtComp.Text
            };
        }

        private void BuscarCep(string cep)
        {
            try
            {
                Cursor = Cursors.WaitCursor;
                okCep = false;

                var (sucesso, mensagem, rua, bairro, cidade, estado) = serviceCliente.ConsultarCep(cep);

                if (!sucesso)
                {
                    MessageBox.Show(mensagem, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                txtRua.Text = rua;
                txtBairro.Text = bairro;
                txtCidade.Text = cidade;
                txtEstado.Text = estado;
                okCep = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro na interface ao preencher o CEP: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void AtualizarGridPai()
        {
            if (dgvClientes != null)
            {
                using (var ctx = new AppDbContext())
                {
                    var repo = new ClienteRepository(ctx);
                    var srv = new ClienteService(repo);
                    dgvClientes.DataSource = null;
                    dgvClientes.DataSource = srv.ObterTodos();
                }
            }
        }
        #endregion

        #region Eventos dos Componentes
        private void mtbCep_Leave(object sender, EventArgs e)
        {
            string valorCep = mtbCep.Text.Replace("-", "").Replace("_", "").Trim();
            if (valorCep.Length == 8)
            {
                BuscarCep(mtbCep.Text);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                btnSave.Enabled = false;

                if (!okCep && mtbCep.MaskFull)
                {
                    BuscarCep(mtbCep.Text);
                }

                if (string.IsNullOrWhiteSpace(txtNome.Text) || !mtbCPF.MaskFull || !mtbTel.MaskFull || !mtbCep.MaskFull || string.IsNullOrWhiteSpace(txtNumber.Text) || !mtbNasc.MaskFull)
                {
                    MessageBox.Show("Preencha todos os campos obrigatórios corretamente!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtNome.Focus();
                    return;
                }

                if (!okCep)
                {
                    MessageBox.Show("CEP inválido ou não localizado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Cliente cliente = FormCliente();
                if (cliente == null) return;

                if (modo == 1)
                {
                    var (sucesso, mensagem) = serviceCliente.CadastrarCliente(cliente);
                    if (sucesso)
                    {
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AtualizarGridPai();
                        DeleteAll();
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (modo == 2)
                {
                    var (sucesso, mensagem) = serviceCliente.EditarCliente(cliente);
                    if (sucesso)
                    {
                        MessageBox.Show(mensagem, "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AtualizarGridPai();
                        DeleteAll();
                        Fechar();
                    }
                    else
                    {
                        MessageBox.Show(mensagem, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerenciar clientes: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnSave.Enabled = true;
            }
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            DeleteAll();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Fechar();
        }
        #endregion
    }
}
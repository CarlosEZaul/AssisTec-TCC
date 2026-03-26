using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssisTec.UserControls.ucFormulario_Clientes
{
    public partial class ucFormulario_Clientes : UserControl
    {
        private int id;
        private int modo;
        private string uf;
        private bool okCep;
        public ucFormulario_Clientes()
        {
            InitializeComponent();
        }

        #region Métodos e funções
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
        
        private Cliente formCliente()
        {
            Cliente cliente = new Cliente();
            cliente.id = id;
            cliente.nome = txtNome.Text;
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
        

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            Cliente cliente = formCliente();
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
                
            }
            else if (modo == 2 && okCep == true) 
            {
                cliente.editarCliente(cliente);
            }
            else if (!okCep)
            {
                MessageBox.Show("Por favor, verifique o CEP informado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                
                if (buscaCep.cidade == null && buscaCep.rua == null && buscaCep.bairro == null)
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
    }
}
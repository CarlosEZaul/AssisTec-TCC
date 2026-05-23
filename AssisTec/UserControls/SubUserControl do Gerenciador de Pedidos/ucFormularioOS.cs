using System;
using System.Data;
using System.Windows.Forms;
using AssisTec.Business;
using AssisTec.Data;
using AssisTec.Reports;
using MySql.Data.MySqlClient;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucFormularioOS : UserControl
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private DataGridView dgvOS;
        UsuarioRepository repositoryUsuario = new UsuarioRepository();
        UsuarioService Usuarioservice = new UsuarioService();
        UsuarioRelatorio Usuariorelatorio = new UsuarioRelatorio();
        ClienteService clienteService = new ClienteService();
        ClienteRository clienteRository = new ClienteRository();
        ClienteRelatorio clienteRelatorio = new ClienteRelatorio();
        public ucFormularioOS(DataGridView _dgvOS)
        {
            dgvOS = _dgvOS;
            InitializeComponent();
            ConfigurarComboBox();
        }
        private void ConfigurarComboBox()
        {
            cbEstado.Items.Clear();
            cbEstado.Items.Add("Bom estado");
            cbEstado.Items.Add("Algumas avarias");
            cbEstado.Items.Add("Danificado");
            cbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            
            con.OpenConnection();
            string sql = "SELECT id_cliente , CONCAT(nome, ' - ', cpf) AS exibicao FROM clientes ORDER BY nome";

            MySqlCommand cmd = new MySqlCommand(sql, con.con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cbCliente.DataSource = dt;
            cbCliente.DisplayMember = "exibicao";
            cbCliente.ValueMember = "id_cliente";

            cbCliente.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbCliente.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbCliente.SelectedIndex = -1;
            con.CloseConnection();
            
            con.OpenConnection();

            string sql1 = "SELECT id_usuario, CONCAT(nome, ' - ', cpf) AS exibicao FROM usuarios WHERE nivel = 3 ||  nivel = 1  ORDER BY nome";

            MySqlCommand cmd1 = new MySqlCommand(sql1, con.con);
            MySqlDataAdapter da1 = new MySqlDataAdapter(cmd1);
            DataTable dt1 = new DataTable();
            da1.Fill(dt1);

            cbTecnico.DataSource = null;
            cbTecnico.Items.Clear();

            cbTecnico.DataSource = dt1;
            cbTecnico.DisplayMember = "exibicao";
            cbTecnico.ValueMember = "id_usuario";

            cbTecnico.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbTecnico.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbTecnico.SelectedIndex = -1;

            con.CloseConnection();

             

        }

        private void LimparTxt()
        {
            txtDescricao.Text = "";
            txtMarca.Text = "";
            txtModelo.Text = "";
            txtNdeSerie.Text = "";
            txtAcessorio.Text = "";
            cbEstado.Text = "";
            txtObservacoes.Text = "";
            txtProblemas.Text="";
        }

        private void CarregarOS()
        {
            Cliente cliente = new Cliente();
            cliente = clienteRository.ObterPorId(Convert.ToInt32(cbCliente.SelectedValue));
            Usuario tecnico = new Usuario();
            tecnico = repositoryUsuario.ObterPorId(Convert.ToInt32(cbTecnico.SelectedValue));
            Equipamento equipamento = new Equipamento();
            
            OrdemDeServico os = new OrdemDeServico();
            
            

           
        }
        
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cbCliente.Text) || string.IsNullOrWhiteSpace(cbTecnico.Text) ||
                    string.IsNullOrWhiteSpace(txtDescricao.Text) ||
                    string.IsNullOrWhiteSpace(txtModelo.Text) ||
                    string.IsNullOrWhiteSpace(txtNdeSerie.Text) || string.IsNullOrWhiteSpace(txtAcessorio.Text) ||
                    string.IsNullOrWhiteSpace(cbEstado.Text) ||
                    string.IsNullOrWhiteSpace(txtObservacoes.Text) || string.IsNullOrWhiteSpace(txtProblemas.Text))
                {
                    MessageBox.Show("Preencha todos os campos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                OrdemDeServico  os = new OrdemDeServico();
                if (cbCliente.SelectedValue == null)
                {
                    MessageBox.Show("Cliente não selecionado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (cbTecnico.SelectedValue == null)
                {
                    MessageBox.Show("Técnico não selecionado", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                os.Cliente.id = Convert.ToInt32(cbCliente.SelectedValue);
                os.Tecnico.id = Convert.ToInt32(cbTecnico.SelectedValue);
                os.Equipamento.Descricao = txtDescricao.Text;
                os.Equipamento.Marca = txtMarca.Text;
                os.Equipamento.Modelo = txtMarca.Text;
                os.Equipamento.EstadoEntrada = cbEstado.Text;
                os.Equipamento.NumeroSerie = txtNdeSerie.Text;
                os.Equipamento.Acessorio = txtAcessorio.Text;
                os.ProblemaRelatado = txtProblemas.Text;
                os.salvarOS();
                os.atualizarDados(dgvOS);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparTxt();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
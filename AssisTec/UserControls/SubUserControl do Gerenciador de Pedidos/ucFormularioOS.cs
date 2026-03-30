using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucFormularioOS : UserControl
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        public ucFormularioOS()
        {
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
            string sql = "SELECT id_cliente, CONCAT(nome, ' - ', cpf) AS exibicao FROM clientes ORDER BY nome";

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

            string sql1 = "SELECT id_usuario, CONCAT(nome, ' - ', cpf) AS exibicao FROM usuarios WHERE nivel = 3 ORDER BY nome";

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
            txtTipo.Text = "";
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
            cliente = cliente.carregarDados(Convert.ToInt32(cbCliente.SelectedValue));
            Usuario tecnico = new Usuario();
            tecnico = tecnico.carregarDados(Convert.ToInt32(cbTecnico.SelectedValue));
            Equipamento equipamento = new Equipamento();
            
            OrdemDeServico os = new OrdemDeServico();
            
            

           
        }
        
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(cbCliente.Text) || string.IsNullOrWhiteSpace(cbTecnico.Text) ||
                string.IsNullOrWhiteSpace(txtDescricao.Text) ||
                string.IsNullOrWhiteSpace(txtTipo.Text) || string.IsNullOrWhiteSpace(txtMarca.Text) ||
                string.IsNullOrWhiteSpace(txtModelo.Text) ||
                string.IsNullOrWhiteSpace(txtNdeSerie.Text) || string.IsNullOrWhiteSpace(txtAcessorio.Text) ||
                string.IsNullOrWhiteSpace(cbEstado.Text) ||
                string.IsNullOrWhiteSpace(txtObservacoes.Text) || string.IsNullOrWhiteSpace(txtProblemas.Text))
            {
                MessageBox.Show("Preencha todos os campos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            try
            {
                con.OpenConnection();
                sql = "insert into equipamentos (id_cliente, descricao, tipo, marca, modelo, numero_serie, acessorios, estado_entrada, observacoes) values (@id_cliente, @descricao, @tipo, @marca, @modelo, @numero_serie, @acessorios, @estado_entrada, @observacoes)";
                cmd = new MySqlCommand(sql, con.con);
                
                cmd.Parameters.AddWithValue("@id_cliente", cbCliente.SelectedValue);
                cmd.Parameters.AddWithValue("@descricao", txtDescricao.Text);
                cmd.Parameters.AddWithValue("@tipo", txtTipo.Text);
                cmd.Parameters.AddWithValue("@marca", txtMarca.Text);
                cmd.Parameters.AddWithValue("@modelo", txtModelo.Text);
                cmd.Parameters.AddWithValue("@numero_serie", txtNdeSerie.Text);
                cmd.Parameters.AddWithValue("@estado_entrada", cbEstado.SelectedText);
                cmd.Parameters.AddWithValue("@acessorios", txtAcessorio.Text);
                cmd.Parameters.AddWithValue("@observacoes", txtObservacoes.Text);
                cmd.ExecuteNonQuery();

                int idEquipamento = (int)cmd.LastInsertedId;
                var dataAtual = DateTime.Now;
                con.CloseConnection();
                
                con.OpenConnection();
                sql = "insert into pedidos (id_tecnico , id_equipamento , problema_relatado, data_abertura) values (@id_tecnico, @id_equipamento, @problema_relatado, @data_abertura)";
                cmd = new MySqlCommand(sql, con.con);
                
                cmd.Parameters.AddWithValue("@id_tecnico", cbTecnico.SelectedValue);
                cmd.Parameters.AddWithValue("@id_equipamento", idEquipamento);
                cmd.Parameters.AddWithValue("@problema_relatado", txtProblemas.Text);
                cmd.Parameters.AddWithValue("@data_abertura", dataAtual);
                
                
                cmd.ExecuteNonQuery();
                con.CloseConnection();
                MessageBox.Show("Pedido Cadastrado!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
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
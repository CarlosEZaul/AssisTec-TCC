using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class Novo_Pedido : Form
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        public Novo_Pedido()
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

            string sql1 = "SELECT id_usuario, CONCAT(nome, ' - ', cpf) AS exibicao FROM usuarios ORDER BY nome";

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
        
        private void btnSalvar_Click(object sender, EventArgs e)
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
            sql = "insert into pedidos (id_cliente , id_tecnico , id_equipamento , problema_relatado, data_abertura) values (@id_cliente, @id_tecnico, @id_equipamento, @problema_relatado, @data_abertura)";
            cmd = new MySqlCommand(sql, con.con);
            
            cmd.Parameters.AddWithValue("@id_cliente", cbCliente.SelectedValue);            
            cmd.Parameters.AddWithValue("@id_tecnico", cbTecnico.SelectedValue);
            cmd.Parameters.AddWithValue("@id_equipamento", idEquipamento);
            cmd.Parameters.AddWithValue("@problema_relatado", txtProblemas.Text);
            cmd.Parameters.AddWithValue("@data_abertura", dataAtual);
            
            
            cmd.ExecuteNonQuery();
            con.CloseConnection();
            this.Close();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            LimparTxt();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
using System;
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
            cmd.Parameters.AddWithValue("@descricao", txtDescricao);
            cmd.Parameters.AddWithValue("@tipo", txtTipo.Text);
            cmd.Parameters.AddWithValue("@marca", txtMarca.Text);
            cmd.Parameters.AddWithValue("@modelo", txtModelo);
            cmd.Parameters.AddWithValue("@numero_serie", txtNdeSerie.Text);
            cmd.Parameters.AddWithValue("@estado_entrada", cbEstado.SelectedText);
            cmd.Parameters.AddWithValue("@observacoes", txtObservacoes);
            cmd.ExecuteNonQuery();

            int idEquipamento = (int)cmd.LastInsertedId;
            con.CloseConnection();
            
            con.OpenConnection();
            sql = "insert into pedidos (id_cliente , id_tecnico , id_equipamento , problema_relatado, status) values (@id_cliente, @id_tecnico, @id_equipamento, @problema_relatado, @status)";
            cmd = new MySqlCommand(sql, con.con);
            
            cmd.Parameters.AddWithValue("@id_cliente", cbCliente.SelectedValue);            
            cmd.Parameters.AddWithValue("@id_tecnico", cbTecnico.SelectedValue);
            cmd.Parameters.AddWithValue("@id_equipamento", idEquipamento);
            cmd.Parameters.AddWithValue("@problema_relatado", txtProblemas);
            cmd.Parameters.AddWithValue("@status", txtNdeSerie.Text);
            
            cmd.ExecuteNonQuery();
            con.CloseConnection();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
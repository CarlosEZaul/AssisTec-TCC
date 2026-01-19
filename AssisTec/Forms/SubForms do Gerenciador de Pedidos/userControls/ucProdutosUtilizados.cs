using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucProdutosUtilizados : UserControl
    {
        private Pedido _pedido;
        conexao con = new conexao();
        MySqlCommand cmd;
        private int id;
        
        public ucProdutosUtilizados(Pedido pedido)
        {
            InitializeComponent();
            _pedido = pedido;
            ConfigurarComboBox();
            
        }
        
        private void ConfigurarComboBox()
        {
            con.OpenConnection();
            string sql = "SELECT id_produto, CONCAT(descricao, ' - Qnt: ', estoque, ' - Preço de Compra: R$', preco_compra) AS exibicao FROM produtos ORDER BY descricao";

            MySqlCommand cmd = new MySqlCommand(sql, con.con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cbProduto.DataSource = dt;
            cbProduto.DisplayMember = "exibicao";
            cbProduto.ValueMember = "id_produto";

            cbProduto.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbProduto.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbProduto.SelectedIndex = -1;
            con.CloseConnection();
            
        }

        private void txtQntd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
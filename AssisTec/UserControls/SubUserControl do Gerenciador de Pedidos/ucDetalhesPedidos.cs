using System;
using System.Windows.Forms;
using System.Drawing;
using MySql.Data.MySqlClient;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.IO;
using Exception = System.Exception;
using Font = System.Drawing.Font;
using Image = iTextSharp.text.Image;

namespace AssisTec.SubForms_do_Gerenciador_de_Pedidos
{
    public partial class ucDetalhesPedidos : UserControl
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        private int id;
        private string uf;
        private Pedido _pedido;
        
        public ucDetalhesPedidos(Pedido pedido)
        {
            InitializeComponent();
            _pedido = pedido;
        }

        private void ucDetalhesPedidos_Load(object sender, EventArgs e)
        {
            txtId.Text = _pedido.id_pedido.ToString();
            //CarregarDadosPedido();
        }
        

        #region Metodos de Manipulação de Dados

        private void CarregarDadosPedido()
        {
            try
            {
                con.OpenConnection();

                string sql = @"
                                SELECT 
                                    p.id_pedido,
                                    c.nome AS cliente,
                                    u.nome AS tecnico,
                                    e.descricao AS equipamento,
                                    p.status,
                                    p.data_abertura,
                                    p.data_atualizacao,
                                    p.data_fechamento,
                                    p.valor_mao_obra,
                                    p.valor_pecas,
                                    p.valor_total,
                                    p.problema_relatado,
                                    p.diagnostico,
                                    p.observacoes
                                FROM pedidos p
                                LEFT JOIN equipamentos e ON p.id_equipamento = e.id_equipamento
                                LEFT JOIN clientes c ON e.id_cliente = c.id_cliente
                                LEFT JOIN usuarios u ON p.id_tecnico = u.id_usuario
                                WHERE p.id_pedido = @id";
    

                using (MySqlCommand cmd = new MySqlCommand(sql, con.con))
                {
                    cmd.Parameters.AddWithValue("@id", _pedido.id_pedido);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            txtCliente.Text = dr["cliente"]?.ToString();
                            txtTecnico.Text = dr["tecnico"]?.ToString();
                            txtEquipamento.Text = dr["equipamento"]?.ToString();
                            txtStatus.Text = dr["status"]?.ToString();

                            txtDataAbertura.Text = dr["data_abertura"] == DBNull.Value
                                ? ""
                                : Convert.ToDateTime(dr["data_abertura"]).ToString("dd/MM/yyyy");

                            txtUltimaAtualizacao.Text = dr["data_atualizacao"] == DBNull.Value
                                ? ""
                                : Convert.ToDateTime(dr["data_atualizacao"]).ToString("dd/MM/yyyy");

                            txtValorMaoObra.Text = dr["valor_mao_obra"]?.ToString();
                            txtValorPecas.Text = dr["valor_pecas"]?.ToString();
                            txtValorTotal.Text = dr["valor_total"]?.ToString();

                            txtProblema.Text = dr["problema_relatado"]?.ToString();
                            txtDiagnostico.Text = dr["diagnostico"]?.ToString();
                            txtObservacoes.Text = dr["observacoes"]?.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar dados do pedido:\n" + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        

            #endregion
            
       
    }
}
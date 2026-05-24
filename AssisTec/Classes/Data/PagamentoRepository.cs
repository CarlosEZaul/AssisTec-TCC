using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec.Data
{
    public class PagamentoRepository
    {
        conexao con = new conexao();
        MySqlCommand cmd;
        string sql;
        
        public DataTable carregarFormasPamento()
        {
            try
            {
                con.OpenConnection();

                sql = @"SELECT id_forma_pagamento, CONCAT(descricao) AS exibicao 
                    FROM forma_pagamento 
                    ORDER BY descricao;";

                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dtFormaPagamento = new DataTable();
                da.Fill(dtFormaPagamento);
            
                
            
                return dtFormaPagamento;
            }
            catch (Exception e)
            {
                MessageBox.Show("Falha ao carregar formas de pagamento: ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            
        }
        
        public bool registrarPagamentoEntrada(int id, int foma_pagamento, string data_pagamento)
        {
            try
            {
                Pagamento pagamento = new Pagamento();
                con.OpenConnection();
                sql = @"UPDATE contas_receber SET 
                          data_pagamento = @data_pagamento,
                          id_forma_pagamento_fk = @id_forma_pagamento_fk,
                          status = 'PAGA'
                     WHERE id_conta_receber = @id;";
                
                cmd = new MySqlCommand(sql, con.con);
                
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@data_pagamento", pagamento.DataFormatada(data_pagamento) ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@id_forma_pagamento_fk", foma_pagamento);
                
                cmd.ExecuteNonQuery();
                con.CloseConnection();
                
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
            
        }
    }
}
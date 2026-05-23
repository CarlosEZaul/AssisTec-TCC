using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec
{
    public class Pagamento
    {
        conexao con = new conexao();
        string sql;
        private MySqlCommand cmd;
        
        public int id_pagamento{get;set;}
        public string forma_pagamento{get;set;}

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
            
                DataRow dr = dtFormaPagamento.NewRow();
                dr["id_forma_pagamento"] = 0;
                dr["exibicao"] = "Todas as formas de pagamento";
                dtFormaPagamento.Rows.InsertAt(dr, 0);
            
                return dtFormaPagamento;
            }
            catch (Exception e)
            {
                MessageBox.Show("Falha ao carregar formas de pagamento: ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            
        }
    }
}
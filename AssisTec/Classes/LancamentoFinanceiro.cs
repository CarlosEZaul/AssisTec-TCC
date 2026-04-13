using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec
{
    public class LancamentoFinanceiro
    {
        public int id_lancamento{get;set;}
        public OrdemDeServico ordemDeServico{get;set;}
        public Pagamento pagamento{get;set;}
        public string descricao{get;set;}
        public string dataVencimento{get;set;}
        public string dataPagamento{get;set;}
        public string tipo{get;set;}
        public int formaPagamento{get;set;}
        public string status{get;set;}
        public double valor{get;set;}

        private string sql;
        private MySqlCommand cmd;
        private conexao con;

        public void lancamentoFinanceiro()
        {
            try
            {
                con.OpenConnection();
                sql = @"INSERT INTO lancamento_financeiro 
                        (TIPO, DESCRICAO, VALOR, DATA_VENCIMENTO, STATUS, ID_FORMA_PAGAMENTO_FK)
                    VALUES 
                        (@tipo, @descricao,@valor,@data_vencimento,@status,@id_pagamento_fk)";
            
                cmd = new MySqlCommand(sql, con.con);
            
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@descricao", descricao);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@data_vencimento", dataVencimento);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id_pagamento_fk", pagamento.id_pagamento);
                cmd.ExecuteNonQuery();
                con.CloseConnection();
                
                MessageBox.Show("Entrada registrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        public void lancamentoFinanceiroOS()
        {
            try
            {
                con.OpenConnection();
                sql = @"INSERT INTO lancamento_financeiro 
                        (TIPO, DESCRICAO, VALOR, DATA_VENCIMENTO, STATUS, ID_FORMA_PAGAMENTO_FK, ID_OS_FK)
                    VALUES 
                        (@tipo, @descricao,@valor,@data_vencimento,@status,@id_pagamento_fk, @id_os_fk)";
            
                cmd = new MySqlCommand(sql, con.con);
            
                cmd.Parameters.AddWithValue("@tipo", tipo);
                cmd.Parameters.AddWithValue("@descricao", descricao);
                cmd.Parameters.AddWithValue("@valor", valor);
                cmd.Parameters.AddWithValue("@data_vencimento", dataVencimento);
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@id_pagamento_fk", pagamento.id_pagamento);
                cmd.Parameters.AddWithValue("@id_os_fk", ordemDeServico.IdPedido);
                cmd.ExecuteNonQuery();
                con.CloseConnection();
                
                MessageBox.Show("Entrada registrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
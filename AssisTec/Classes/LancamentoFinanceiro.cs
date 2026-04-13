using System;
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
        public string dataEmissao{get;set;}
        public string dataPagamento{get;set;}
        public int tipo{get;set;}
        public string status{get;set;}
        public decimal valor{get;set;}
        public string obervacoes{get;set;}

        private string sql;
        private MySqlCommand cmd;
        private conexao con = new conexao();

        private void carregarContasReeber()
        {
            try
            {
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        
        public string DataFormatada(string DataFornecida, bool obrigatoria = true)
        {
            if (string.IsNullOrWhiteSpace(DataFornecida))
            {
                if (obrigatoria)
                {
                    MessageBox.Show("Data vazia!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return null;
            }

            try
            {
                DateTime dataConvertida = DateTime.ParseExact(DataFornecida, "dd/MM/yyyy", null);
                return dataConvertida.ToString("yyyy-MM-dd");
            }
            catch
            {
                if (obrigatoria)
                {
                    MessageBox.Show("Data inválida!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return null;
            }
        }

        public void SalvarLancamentoFinanceiro()
        {
            if (tipo == 1)
            {
                if (status == "PAGA" && string.IsNullOrWhiteSpace(dataPagamento))
                {
                    MessageBox.Show("Data de pagamento é obrigatória para status PAGA!", 
                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                try
                {
                    con.OpenConnection();
                    sql = @"INSERT INTO contas_receber 
                        (DESCRICAO, VALOR, DATA_EMISSAO, DATA_PAGAMENTO, DATA_VENCIMENTO,STATUS, ID_FORMA_PAGAMENTO_FK, OBSERVACOES)
                    VALUES 
                        (@descricao, @valor, @data_emissao, @data_pagamento, @data_vencimento,@status, @id_pagamento_fk, @observacoes)";
            
                    cmd = new MySqlCommand(sql, con.con);
                
                    cmd.Parameters.AddWithValue("@descricao", descricao);
                    cmd.Parameters.AddWithValue("@valor", valor);
                    cmd.Parameters.AddWithValue("@data_emissao", DataFormatada(dataEmissao));
                    
                    if (string.IsNullOrWhiteSpace(dataPagamento))
                    {
                        cmd.Parameters.AddWithValue("@data_pagamento", DBNull.Value);
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@data_pagamento", DataFormatada(dataPagamento, false));
                    }

                    cmd.Parameters.AddWithValue("@data_vencimento", DataFormatada(dataVencimento));
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@id_pagamento_fk", pagamento.id_pagamento);
                    cmd.Parameters.AddWithValue("@observacoes", obervacoes);
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
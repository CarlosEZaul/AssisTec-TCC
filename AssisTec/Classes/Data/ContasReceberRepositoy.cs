using System;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Exception = AssisTec.AtendeClienteService.Exception;

namespace AssisTec.Data
{
    public class ContasReceberRepositoy
    {
        conexao con  = new conexao();
        string sql;
        MySqlCommand cmd;
        
         public ContasReceber carregarContaReceber(int id)
        {
            try
            {
                con.OpenConnection();

                sql = @"SELECT 
                        cr.id_conta_receber,
                        os.id_os,
                        cr.descricao,
                        cr.valor,
                        cr.data_emissao,
                        cr.data_pagamento,
                        cr.data_vencimento,
                        cr.status,
                        fp.id_forma_pagamento,
                        fp.descricao AS forma_pagamento,
                        cr.observacoes
                    FROM contas_receber cr
                    LEFT JOIN forma_pagamento fp
                        ON cr.id_forma_pagamento_fk = fp.id_forma_pagamento
                    LEFT JOIN ordem_servico os
                        ON cr.id_os_fk = os.id_os
                    WHERE cr.id_conta_receber = @id;";

                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", id);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    return new ContasReceber()
                    {
                        id_conta = rs.GetInt32("id_conta_receber"),
                        descricao = rs.GetString("descricao"),
                        valor = rs.GetDecimal("valor"),

                        dataEmissao = rs.IsDBNull(rs.GetOrdinal("data_emissao"))
                            ? ""
                            : rs.GetDateTime("data_emissao").ToString("dd/MM/yyyy"),
                        dataPagamento = rs.IsDBNull(rs.GetOrdinal("data_pagamento"))
                            ? ""
                            : rs.GetDateTime("data_pagamento").ToString("dd/MM/yyyy"),
                        dataVencimento = rs.IsDBNull(rs.GetOrdinal("data_vencimento"))
                            ? ""
                            : rs.GetDateTime("data_vencimento").ToString("dd/MM/yyyy"),

                        status = rs.GetString("status"),
                        obervacoes = rs.IsDBNull(rs.GetOrdinal("observacoes")) ? "" : rs.GetString("observacoes"),

                        ordemDeServico = new OrdemDeServico
                        {
                            Id_os = rs.IsDBNull(rs.GetOrdinal("id_os")) ? 0 : rs.GetInt32("id_os"),
                        },

                        pagamento = new Pagamento
                        {
                            id_pagamento = rs.IsDBNull(rs.GetOrdinal("id_forma_pagamento"))
                                ? 0
                                : rs.GetInt32("id_forma_pagamento"),
                            forma_pagamento = rs.IsDBNull(rs.GetOrdinal("forma_pagamento"))
                                ? ""
                                : rs.GetString("forma_pagamento")
                        }

                    };
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.CloseConnection();
            }
            return null;
        }
        
        public DataTable CarregarTodasContasReceber()
        {
            DataTable dt = new DataTable();

            try
            {
                con.OpenConnection();
                sql = @"SELECT 
                    cr.id_conta_receber,
                    os.id_os,
                    cr.descricao,
                    cr.valor,
                    cr.data_emissao,
                    cr.data_pagamento,
                    cr.data_vencimento,
                    cr.status,
                    fp.descricao AS forma_pagamento,
                    cr.observacoes
                    
                FROM contas_receber cr
                
                LEFT JOIN forma_pagamento fp
                    ON cr.id_forma_pagamento_fk = fp.id_forma_pagamento
                    
                LEFT JOIN ordem_servico os
                    ON cr.id_os_fk = os.id_os;";

                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                
                return dt;


            }
            catch (Exception ex)
            {
                return dt;
            }
            finally
            {
                con.CloseConnection();
            }
            
        }

        public bool SalvarEntrada(ContasReceber contasReceber)
        {
            
            try
            {
                con.OpenConnection();
                sql = @"INSERT INTO contas_receber 
                (DESCRICAO, VALOR, DATA_EMISSAO, DATA_PAGAMENTO, DATA_VENCIMENTO,STATUS, ID_FORMA_PAGAMENTO_FK, OBSERVACOES)
            VALUES 
                (@descricao, @valor, @data_emissao, @data_pagamento, @data_vencimento,@status, @id_pagamento_fk, @observacoes)";

                cmd = new MySqlCommand(sql, con.con);

                cmd.Parameters.AddWithValue("@descricao", contasReceber.descricao);
                cmd.Parameters.AddWithValue("@valor", contasReceber.valor);
                cmd.Parameters.AddWithValue("@data_emissao", contasReceber.DataFormatada(contasReceber.dataEmissao));

                if (string.IsNullOrWhiteSpace(contasReceber.dataPagamento))
                {
                    cmd.Parameters.AddWithValue("@data_pagamento", DBNull.Value);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@data_pagamento",
                        contasReceber.DataFormatada(contasReceber.dataPagamento, false));
                }

                cmd.Parameters.AddWithValue("@data_vencimento",
                    contasReceber.DataFormatada(contasReceber.dataVencimento));
                cmd.Parameters.AddWithValue("@status", contasReceber.status);
                cmd.Parameters.AddWithValue("@id_pagamento_fk", contasReceber.pagamento.id_pagamento);
                cmd.Parameters.AddWithValue("@observacoes", contasReceber.obervacoes);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch (Exception e)
            {
                return true;
            }
            finally
            {
                con.CloseConnection();
            }
            
        }
        
        
        public bool editarContaReceber(ContasReceber contasReceber)
        {
            try
            {
                con.OpenConnection();
                sql = @"UPDATE contas_receber SET
                    descricao               = @descricao,
                    valor                   = @valor,
                    data_emissao            = @dataEmissao,
                    data_pagamento          = @dataPagamento,
                    data_vencimento         = @dataVencimento,
                    status                  = @status,
                    observacoes             = @observacoes,
                    id_forma_pagamento_fk   = @idFormaPagamento
                WHERE id_conta_receber = @id;";

                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", contasReceber.id_conta);
                cmd.Parameters.AddWithValue("@descricao", contasReceber.descricao);
                cmd.Parameters.AddWithValue("@valor", contasReceber.valor);
                cmd.Parameters.AddWithValue("@status", contasReceber.status);
                cmd.Parameters.AddWithValue("@observacoes", contasReceber.obervacoes);
                cmd.Parameters.AddWithValue("@idFormaPagamento", contasReceber.pagamento != null ? contasReceber.pagamento.id_pagamento : (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dataEmissao", contasReceber.DataFormatada(contasReceber.dataEmissao) ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dataVencimento", contasReceber.DataFormatada(contasReceber.dataVencimento) ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dataPagamento",  contasReceber.DataFormatada(contasReceber.dataPagamento, false) ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
                con.CloseConnection();

                return true;
            }
            catch (Exception ex)
            {
                
                return false;
            }
        }
        
        public bool deletarContaReceber(int idConta)
        {
            try
            {
                con.OpenConnection();
                
                string sqlCheck = @"SELECT os.status 
                    FROM contas_receber cr
                    INNER JOIN ordem_servico os ON cr.id_os_fk = os.id_os
                WHERE cr.id_conta_receber = @id";

                MySqlCommand cmdCheck = new MySqlCommand(sqlCheck, con.con);
                cmdCheck.Parameters.AddWithValue("@id", idConta);
        
                object result = cmdCheck.ExecuteScalar();

                if (result != null)
                {
                    string statusOS = result.ToString().ToUpper();
                    
                    if (statusOS == "EM ANDAMENTO" || statusOS == "PARA RETIRADA")
                    {
                        MessageBox.Show($"Esta conta não pode ser excluída pois a OS vinculada está: {statusOS}. \nConclua a OS primeiro.", 
                            "Aviso de Segurança", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                
                if (MessageBox.Show("Deseja realmente excluir esta conta?", "Confirmar", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    string sqlDelete = "DELETE FROM contas_receber WHERE id_conta_receber = @id";
                    MySqlCommand cmdDelete = new MySqlCommand(sqlDelete, con.con);
                    cmdDelete.Parameters.AddWithValue("@id", idConta);
            
                    cmdDelete.ExecuteNonQuery();
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao deletar conta: " + ex.Message);
                return false;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public DataTable carregarContaReceberdoBD(int idConta)
        {
            con.OpenConnection();

            string sql = @"
                SELECT
                    cr.id_conta_receber,
                    cr.id_os_fk,
                    cr.descricao,
                    cr.valor,
                    cr.data_emissao,
                    cr.data_pagamento,
                    cr.data_vencimento,
                    cr.status,
                    fp.descricao AS forma_pagamento,
                    cr.observacoes
                FROM contas_receber cr
                LEFT JOIN forma_pagamento fp
                    ON cr.id_forma_pagamento_fk =
                       fp.id_forma_pagamento
                WHERE cr.id_conta_receber =
                    @idConta";

            MySqlCommand cmd =
                new MySqlCommand(
                    sql,
                    con.con
                );

            cmd.Parameters.AddWithValue(
                "@idConta",
                idConta
            );

            DataTable dt =
                new DataTable();

            new MySqlDataAdapter(cmd)
                .Fill(dt);

            con.CloseConnection();
            
            return dt;
        }

        /*public void lancamentoFinanceiroOS()
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
                cmd.Parameters.AddWithValue("@id_os_fk", ordemDeServico.Id_os);
                cmd.ExecuteNonQuery();
                con.CloseConnection();
                atualizarContasReceber();
                MessageBox.Show("Entrada registrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public void alterarParaAtrasado()
        {
            try
            {
                con.OpenConnection();
                sql = @"UPDATE contas_receber
                    set status = 'ATRASADO'
                    where status = 'PENDENTE' 
                    and data_vencimento < current_date()
                    ";

                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                cmd.ExecuteNonQuery();
                con.CloseConnection();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao alterar contas atrasadas" + ex.Message, "Erro", MessageBoxButtons.OK);
            }
        }*/
        public bool alterarParaAtrasado()
        {
            try
            {
                con.OpenConnection();
                sql = @"UPDATE contas_receber
                    set status = 'ATRASADO'
                    where status = 'PENDENTE' 
                    and data_vencimento < current_date()
                    ";

                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                cmd.ExecuteNonQuery();
                return true;

            }
            catch (Exception ex)
            {
                return false;
            }
            finally{
                con.CloseConnection();
            }
            return false;
        }
        
        public (decimal totalGeral, decimal totalRecebido, decimal totalPendente, decimal totalAtrasado) AtualizarTotais(ContasReceber contasReceber)
        {
            try
            {
                con.OpenConnection();

                bool temDataInicio = !string.IsNullOrWhiteSpace(contasReceber.filtroDataInicio);
                bool temDataFim = !string.IsNullOrWhiteSpace(contasReceber.filtroDataFim);
                bool temDescricao = !string.IsNullOrWhiteSpace(contasReceber.filtroDescricao);
                bool temStatus = !string.IsNullOrWhiteSpace(contasReceber.filtroStatus);
                bool temFormaPagamento = contasReceber.filtroIdFormaPagamento > 0;

                sql = @"SELECT 
                    SUM(cr.valor) AS total_geral,
                    SUM(CASE WHEN cr.status = 'Paga' THEN cr.valor ELSE 0 END) AS total_recebido,
                    SUM(CASE WHEN cr.status = 'Pendente'  THEN cr.valor ELSE 0 END) AS total_pendente,
                    SUM(CASE WHEN cr.status = 'Atrasado'  THEN cr.valor ELSE 0 END) AS total_atrasado
                FROM contas_receber cr
                WHERE 1=1";

                if (temDataInicio)
                    sql += " AND cr.data_vencimento >= @DataInicio";
                if (temDataFim)
                    sql += " AND cr.data_vencimento <= @DataFim";
                if (temDescricao)
                    sql += " AND cr.descricao LIKE @Descricao";
                if (temStatus)
                    sql += " AND cr.status = @Status";
                if (temFormaPagamento)
                    sql += " AND cr.id_forma_pagamento_fk = @FormaPagamento";

                cmd = new MySqlCommand(sql, con.con);

                if (temDataInicio)
                    cmd.Parameters.AddWithValue("@DataInicio",
                        DateTime.ParseExact(contasReceber.filtroDataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            .Date);

                if (temDataFim)
                    cmd.Parameters.AddWithValue("@DataFim",
                        DateTime.ParseExact(contasReceber.filtroDataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture)
                            .Date.AddDays(1).AddSeconds(-1));

                if (temDescricao)
                    cmd.Parameters.AddWithValue("@Descricao", $"%{contasReceber.filtroDescricao.Trim()}%");

                if (temStatus)
                    cmd.Parameters.AddWithValue("@Status", contasReceber.filtroStatus);

                if (temFormaPagamento)
                    cmd.Parameters.AddWithValue("@FormaPagamento", contasReceber.filtroIdFormaPagamento);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    decimal totalGeral = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                    decimal totalRecebido = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                    decimal totalPendente = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                    decimal totalAtrasado = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);

                    reader.Close();
                    return (totalGeral, totalRecebido, totalPendente, totalAtrasado);
                }

                reader.Close();
                
            }
            catch (Exception ex)
            {
                return (0,0,0,0);
            }
            finally
            {
                con.CloseConnection();
            }
            

            return (0, 0, 0, 0);
        }
        
        public DataTable FiltrarContasReceber(ContasReceber contasReceber)
        {
            DataTable dt = new DataTable();

            try
            {
                con.OpenConnection();

                bool temFormaPagamento = contasReceber.filtroIdFormaPagamento > 0;
                bool temDataInicio     = !string.IsNullOrWhiteSpace(contasReceber.filtroDataInicio);
                bool temDataFim        = !string.IsNullOrWhiteSpace(contasReceber.filtroDataFim);
                bool temDescricao      = !string.IsNullOrWhiteSpace(contasReceber.filtroDescricao);
                bool temStatus         = !string.IsNullOrWhiteSpace(contasReceber.filtroStatus);

                sql = @"SELECT 
                    cr.id_conta_receber,        
                    cr.id_os_fk,                   
                    cr.descricao,               
                    cr.valor,                   
                    cr.data_emissao,            
                    cr.data_pagamento,          
                    cr.data_vencimento,         
                    cr.status,                  
                    fp.descricao AS forma_pagamento, 
                    cr.observacoes              
                FROM contas_receber cr
                INNER JOIN forma_pagamento fp 
                    ON cr.id_forma_pagamento_fk = fp.id_forma_pagamento
                WHERE 1=1";

                if (temDataInicio) sql += " AND cr.data_vencimento >= @DataInicio";
                if (temDataFim)    sql += " AND cr.data_vencimento <= @DataFim";
                if (temDescricao)  sql += " AND cr.descricao LIKE @Descricao";
                if (temStatus)     sql += " AND cr.status = @Status";
                if (temFormaPagamento) sql += " AND cr.id_forma_pagamento_fk = @FormaPagamento";

                sql += " ORDER BY cr.data_vencimento ASC";

                cmd = new MySqlCommand(sql, con.con);

                if (temDataInicio)
                    cmd.Parameters.AddWithValue("@DataInicio",
                        DateTime.ParseExact(contasReceber.filtroDataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date);

                if (temDataFim)
                    cmd.Parameters.AddWithValue("@DataFim",
                        DateTime.ParseExact(contasReceber.filtroDataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date.AddDays(1).AddSeconds(-1));

                if (temDescricao)
                    cmd.Parameters.AddWithValue("@Descricao", $"%{contasReceber.filtroDescricao.Trim()}%");

                if (temStatus)
                    cmd.Parameters.AddWithValue("@Status", contasReceber.filtroStatus);

                if (temFormaPagamento)
                    cmd.Parameters.AddWithValue("@FormaPagamento", contasReceber.filtroIdFormaPagamento);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                con.CloseConnection();
            }

            return dt;
        }
        
        
    }
}
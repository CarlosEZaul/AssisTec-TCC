using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

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
        
        public string filtroDataInicio { get; set; }
        public string filtroDataFim { get; set; }
        public string filtroDescricao { get; set; }
        public string filtroStatus { get; set; }
        public int filtroIdFormaPagamento { get; set; }

            

        #region Contas a Receber


        public LancamentoFinanceiro carregarContaReceber(int id)
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
                    this.id_lancamento = rs.GetInt32("id_conta_receber");
                    this.descricao     = rs.GetString("descricao");
                    this.valor         = rs.GetDecimal("valor");
                    
                    this.dataEmissao    = rs.IsDBNull(rs.GetOrdinal("data_emissao"))    ? "" : rs.GetDateTime("data_emissao").ToString("dd/MM/yyyy");
                    this.dataPagamento  = rs.IsDBNull(rs.GetOrdinal("data_pagamento"))  ? "" : rs.GetDateTime("data_pagamento").ToString("dd/MM/yyyy");
                    this.dataVencimento = rs.IsDBNull(rs.GetOrdinal("data_vencimento")) ? "" : rs.GetDateTime("data_vencimento").ToString("dd/MM/yyyy");

                    this.status         = rs.GetString("status");
                    this.obervacoes     = rs.IsDBNull(rs.GetOrdinal("observacoes")) ? "" : rs.GetString("observacoes");

                    this.ordemDeServico = new OrdemDeServico
                    {
                        Id_os = rs.IsDBNull(rs.GetOrdinal("id_os")) ? 0 : rs.GetInt32("id_os")
                    };

                    this.pagamento = new Pagamento
                    {
                        id_pagamento = rs.IsDBNull(rs.GetOrdinal("id_forma_pagamento")) ? 0 : rs.GetInt32("id_forma_pagamento"),
                        forma_pagamento = rs.IsDBNull(rs.GetOrdinal("forma_pagamento")) ? "" : rs.GetString("forma_pagamento")
                    };
                }

                rs.Close();
                con.CloseConnection();
                return this;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar conta a receber!" + ex, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        
        public DataTable atualizarContasReceber()
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

                con.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar contas a receber \n" + ex.Message);
            }

            return dt;
            
            
        }
        
        
        public DataTable atualizarContasPagar()
        {
            DataTable dt = new DataTable();

            try
            {
                con.OpenConnection();
                sql = @"SELECT 
                    cp.id_conta_pagar,
                    fp.descricao AS forma_pagamento,
                    cp.descricao,
                    cp.valor,
                    cp.data_emissao,
                    cp.data_pagamento,
                    cp.status,
                    cp.observacoes      
                FROM contas_pagar cp

                LEFT JOIN forma_pagamento fp                            
                    ON cp.id_forma_pagamento_fk = fp.id_forma_pagamento ;";

                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);

                con.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar contas a receber \n" + ex.Message);
            }

            return dt;
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

        public void SalvarEntrada()
        {
            if (tipo == 1)
            {
                if (status == "PAGA" && string.IsNullOrWhiteSpace(dataPagamento))
                {
                    MessageBox.Show("Data de pagamento é obrigatória para status PAGA!", 
                        "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Convert.ToDateTime(dataVencimento) < DateTime.Now)
                {
                    MessageBox.Show("A data de vencimento não pode ser uma data passada.", "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        public bool editarContaReceber(LancamentoFinanceiro lancamento)
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
                cmd.Parameters.AddWithValue("@id", lancamento.id_lancamento);
                cmd.Parameters.AddWithValue("@descricao", lancamento.descricao);
                cmd.Parameters.AddWithValue("@valor", lancamento.valor);
                cmd.Parameters.AddWithValue("@status", lancamento.status);
                cmd.Parameters.AddWithValue("@observacoes", lancamento.obervacoes);
                cmd.Parameters.AddWithValue("@idFormaPagamento", lancamento.pagamento != null ? lancamento.pagamento.id_pagamento : (object)DBNull.Value);

                // --- APLICANDO SUA FUNÇÃO DataFormatada ---
                // Se a função retornar null, passamos DBNull.Value para o MySQL
                cmd.Parameters.AddWithValue("@dataEmissao",    DataFormatada(lancamento.dataEmissao) ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@dataVencimento", DataFormatada(lancamento.dataVencimento) ?? (object)DBNull.Value);
        
                // Para a data de pagamento, geralmente ela não é obrigatória (pode estar em aberto)
                cmd.Parameters.AddWithValue("@dataPagamento",  DataFormatada(lancamento.dataPagamento, false) ?? (object)DBNull.Value);

                cmd.ExecuteNonQuery();
                con.CloseConnection();

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao editar conta a receber \n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        }
        
        public (decimal totalGeral, decimal totalRecebido, decimal totalPendente, decimal totalAtrasado) AtualizarTotais()
        {
            try
            {
                con.OpenConnection();

                bool temDataInicio       = !string.IsNullOrWhiteSpace(filtroDataInicio);
                bool temDataFim          = !string.IsNullOrWhiteSpace(filtroDataFim);
                bool temDescricao        = !string.IsNullOrWhiteSpace(filtroDescricao);
                bool temStatus           = !string.IsNullOrWhiteSpace(filtroStatus);
                bool temFormaPagamento   = filtroIdFormaPagamento > 0;

                sql = @"SELECT 
                    SUM(cr.valor) AS total_geral,
                    SUM(CASE WHEN cr.status = 'Paga' THEN cr.valor ELSE 0 END) AS total_recebido,
                    SUM(CASE WHEN cr.status = 'Pendente'  THEN cr.valor ELSE 0 END) AS total_pendente,
                    SUM(CASE WHEN cr.status = 'Atrasado'  THEN cr.valor ELSE 0 END) AS total_atrasado
                FROM contas_receber cr
                WHERE 1=1";

                if (temDataInicio)      sql += " AND cr.data_vencimento >= @DataInicio";
                if (temDataFim)        sql += " AND cr.data_vencimento <= @DataFim";
                if (temDescricao)      sql += " AND cr.descricao LIKE @Descricao";
                if (temStatus)         sql += " AND cr.status = @Status";
                if (temFormaPagamento) sql += " AND cr.id_forma_pagamento_fk = @FormaPagamento";

                cmd = new MySqlCommand(sql, con.con);

                if (temDataInicio)
                    cmd.Parameters.AddWithValue("@DataInicio",
                        DateTime.ParseExact(filtroDataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date);

                if (temDataFim)
                    cmd.Parameters.AddWithValue("@DataFim",
                        DateTime.ParseExact(filtroDataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date.AddDays(1).AddSeconds(-1));

                if (temDescricao)
                    cmd.Parameters.AddWithValue("@Descricao", $"%{filtroDescricao.Trim()}%");

                if (temStatus)
                    cmd.Parameters.AddWithValue("@Status", filtroStatus);

                if (temFormaPagamento)
                    cmd.Parameters.AddWithValue("@FormaPagamento", filtroIdFormaPagamento);

                MySqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    decimal totalGeral    = reader.IsDBNull(0) ? 0 : reader.GetDecimal(0);
                    decimal totalRecebido = reader.IsDBNull(1) ? 0 : reader.GetDecimal(1);
                    decimal totalPendente = reader.IsDBNull(2) ? 0 : reader.GetDecimal(2);
                    decimal totalAtrasado = reader.IsDBNull(3) ? 0 : reader.GetDecimal(3);

                    reader.Close();
                    return (totalGeral, totalRecebido, totalPendente, totalAtrasado);
                }

                reader.Close();
                con.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao calcular totais\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

            return (0, 0, 0, 0);
        }
        
        public DataTable FiltrarContasReceber()
        {
            DataTable dt = new DataTable();

            try
            {
                con.OpenConnection();

                bool temFormaPagamento = filtroIdFormaPagamento > 0;
                bool temDataInicio     = !string.IsNullOrWhiteSpace(filtroDataInicio);
                bool temDataFim        = !string.IsNullOrWhiteSpace(filtroDataFim);
                bool temDescricao      = !string.IsNullOrWhiteSpace(filtroDescricao);
                bool temStatus         = !string.IsNullOrWhiteSpace(filtroStatus);

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
                        DateTime.ParseExact(filtroDataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date);

                if (temDataFim)
                    cmd.Parameters.AddWithValue("@DataFim",
                        DateTime.ParseExact(filtroDataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture).Date.AddDays(1).AddSeconds(-1));

                if (temDescricao)
                    cmd.Parameters.AddWithValue("@Descricao", $"%{filtroDescricao.Trim()}%");

                if (temStatus)
                    cmd.Parameters.AddWithValue("@Status", filtroStatus);

                if (temFormaPagamento)
                    cmd.Parameters.AddWithValue("@FormaPagamento", filtroIdFormaPagamento);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao filtrar\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.CloseConnection();
            }

            return dt;
        }
        
        public void GerarRelatorioContasReceberPDF(DataTable dados, decimal totalGeral, decimal totalRecebido, decimal totalPendente, decimal totalAtrasado)
        {
            try
            {
                con.OpenConnection();

                string caminho = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                              "Relatorio_ContasReceber_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".pdf");

                Document  doc    = new Document(PageSize.A4.Rotate(), 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                //Fontes
                Font fonteAssistec  = new Font(Font.FontFamily.HELVETICA, 28, Font.BOLD,   BaseColor.WHITE);
                Font fonteSubtitulo = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL, new BaseColor(200, 220, 240));
                Font fonteColuna  = new Font(Font.FontFamily.HELVETICA,  9, Font.BOLD,   BaseColor.WHITE);
                Font fonteDado = new Font(Font.FontFamily.HELVETICA,  9, Font.NORMAL, new BaseColor(30, 30, 30));
                Font fonteTotalLabel = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL, BaseColor.WHITE);
                Font fonteTotalValor = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD,  BaseColor.WHITE);

                // Cores
                BaseColor corPrimaria  = new BaseColor(33, 97, 140);
                BaseColor corHeader = new BaseColor(52, 73, 94);
                BaseColor corLinha1 = new BaseColor(245, 248, 250);
                BaseColor corLinha2 = BaseColor.WHITE;
                BaseColor corRecebido = new BaseColor(39, 174, 96);
                BaseColor corPendente = new BaseColor(230, 126, 34);
                BaseColor corAtrasado = new BaseColor(192, 57, 43);
                BaseColor corTotal = new BaseColor(52, 73, 94);

                //Cabeçalho com logo 
                byte[] logoBytes = (byte[])new ImageConverter().ConvertTo(Properties.Resources.logopng, typeof(byte[]));

                PdfPTable headerTable = new PdfPTable(new float[] { 1, 3 });
                headerTable.WidthPercentage = 100;

                try
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoBytes);
                    logo.ScaleToFit(80f, 80f);
                    PdfPCell logoCell = new PdfPCell(logo);
                    logoCell.BackgroundColor = corPrimaria;
                    logoCell.Border = Rectangle.NO_BORDER;
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    logoCell.Padding= 8;
                    headerTable.AddCell(logoCell);
                }
                catch
                {
                    PdfPCell vazia = new PdfPCell(new Phrase(""));
                    vazia.BackgroundColor = corPrimaria;
                    vazia.Border = Rectangle.NO_BORDER;
                    headerTable.AddCell(vazia);
                }

                PdfPCell textoCell = new PdfPCell();
                textoCell.BackgroundColor = corPrimaria;
                textoCell.Border = Rectangle.NO_BORDER;
                textoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                textoCell.Padding = 12;
                textoCell.AddElement(new Paragraph("ASSISTEC", fonteAssistec) { SpacingAfter = 2 });
                textoCell.AddElement(new Paragraph("Relatório de Contas a Receber", fonteSubtitulo));

                if (!string.IsNullOrWhiteSpace(filtroDataInicio) || !string.IsNullOrWhiteSpace(filtroDataFim))
                    textoCell.AddElement(new Paragraph(
                        $"Período: {filtroDataInicio ?? "início"} até {filtroDataFim ?? "hoje"}", fonteSubtitulo));

                headerTable.AddCell(textoCell);
                doc.Add(headerTable);
                doc.Add(new Paragraph(" "));

                // Cards de totais
                PdfPTable tblTotais = new PdfPTable(4);
                tblTotais.WidthPercentage = 100;
                tblTotais.SetWidths(new float[] { 25f, 25f, 25f, 25f });

                void AddCard(string label, decimal valor, BaseColor cor)
                {
                    PdfPCell card = new PdfPCell();
                    card.BackgroundColor  = cor;
                    card.Border  = Rectangle.NO_BORDER;
                    card.BorderWidthRight = 2f;
                    card.BorderColorRight = BaseColor.WHITE;
                    card.Padding = 8f;
                    card.AddElement(new Paragraph(label,             fonteTotalLabel));
                    card.AddElement(new Paragraph($"R$ {valor:N2}", fonteTotalValor));
                    tblTotais.AddCell(card);
                }

                AddCard("Total a Receber", totalGeral,    corTotal);
                AddCard("Recebido", totalRecebido, corRecebido);
                AddCard("Pendente", totalPendente, corPendente);
                AddCard("Atrasado", totalAtrasado, corAtrasado);

                doc.Add(tblTotais);
                doc.Add(new Paragraph(" "));

                //Tabela de dados
                PdfPTable tabela = new PdfPTable(new float[] { 1.5f, 3, 2, 2, 2, 2, 2, 3 });
                tabela.WidthPercentage = 100;

                void AddCabecalho(string texto)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(texto, fonteColuna));
                    cell.BackgroundColor = corHeader;
                    cell.Border = Rectangle.NO_BORDER;
                    cell.Padding = 8;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabela.AddCell(cell);
                }
                
                AddCabecalho("Descrição");
                AddCabecalho("OS"); 
                AddCabecalho("Valor");
                AddCabecalho("Emissão");
                AddCabecalho("Vencimento");
                AddCabecalho("Pagamento");
                AddCabecalho("Status");
                AddCabecalho("Forma Pagamento");

                int linha = 0;
                foreach (DataRow row in dados.Rows)
                {
                    BaseColor cor = linha % 2 == 0 ? corLinha1 : corLinha2;

                    void AddCelula(string valor, int alinhamento = Element.ALIGN_CENTER)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(valor, fonteDado));
                        cell.BackgroundColor = cor;
                        cell.Border = Rectangle.NO_BORDER;
                        cell.Padding = 7;
                        cell.HorizontalAlignment = alinhamento;
                        tabela.AddCell(cell);
                    }

                    string FormatarData(string campo) =>
                        row[campo] != DBNull.Value && DateTime.TryParse(row[campo].ToString(), out DateTime dt)
                            ? dt.ToString("dd/MM/yyyy") : "-";

                    AddCelula(row["descricao"].ToString(), Element.ALIGN_LEFT);
                    AddCelula(row["id_os_fk"] != DBNull.Value ? row["id_os_fk"].ToString() : "-");
                    AddCelula($"R$ {Convert.ToDecimal(row["valor"]):N2}");
                    AddCelula(FormatarData("data_emissao"));
                    AddCelula(FormatarData("data_vencimento"));
                    AddCelula(FormatarData("data_pagamento"));
                    AddCelula(row["status"].ToString());
                    AddCelula(row["forma_pagamento"] != DBNull.Value ? row["forma_pagamento"].ToString() : "-");

                    linha++;
                }

                // Rodapé
                PdfPCell rodape = new PdfPCell(new Phrase("Total de registros: " + linha,
                    new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, new BaseColor(80, 80, 80))));
                rodape.Colspan = 8;
                rodape.BackgroundColor = new BaseColor(214, 234, 248);
                rodape.Border = Rectangle.NO_BORDER;
                rodape.Padding = 8;
                rodape.HorizontalAlignment = Element.ALIGN_RIGHT;
                tabela.AddCell(rodape);

                doc.Add(tabela);
                doc.Close();

                MessageBox.Show("Relatório gerado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(caminho);
                
                con.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar relatório!\n" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        public void GerarReciboContaReceberPDF(int idConta)
        {
            DataRow conta = null;

            try
            {
                con.OpenConnection();
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
                LEFT JOIN forma_pagamento fp
                    ON cr.id_forma_pagamento_fk = fp.id_forma_pagamento
                WHERE cr.id_conta_receber = @idConta";

                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@idConta", idConta);

                DataTable dt = new DataTable();
                new MySqlDataAdapter(cmd).Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Conta não encontrada.", "Atenção",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                conta = dt.Rows[0];
                con.CloseConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao buscar conta!\n" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (conta["status"].ToString().Trim().ToUpper() != "PAGA")
            {
                MessageBox.Show("O recibo só pode ser gerado para contas com status PAGA.",
                    "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                string caminho = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Recibo_" + conta["id_conta_receber"].ToString() + "_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".pdf");

                // Documento A5 retrato 
                Document doc = new Document(PageSize.A5, 40, 40, 40, 40);
                PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                //Fontes
                Font fonteEmpresa   = new Font(Font.FontFamily.HELVETICA, 20, Font.BOLD,   BaseColor.WHITE);
                Font fonteSubtitulo = new Font(Font.FontFamily.HELVETICA,  9, Font.NORMAL, new BaseColor(200, 220, 240));
                Font fonteTitulo = new Font(Font.FontFamily.HELVETICA, 13, Font.BOLD,   new BaseColor(33, 97, 140));
                Font fonteLabel = new Font(Font.FontFamily.HELVETICA,  8, Font.BOLD,   new BaseColor(100, 100, 100));
                Font fonteDado = new Font(Font.FontFamily.HELVETICA,  9, Font.NORMAL, new BaseColor(30,  30,  30));
                Font fonteValorGrande = new Font(Font.FontFamily.HELVETICA, 22, Font.BOLD, new BaseColor(39, 174, 96));
                Font fonteRodape    = new Font(Font.FontFamily.HELVETICA,  7, Font.ITALIC, new BaseColor(150, 150, 150));
                Font fonteNumRecibo = new Font(Font.FontFamily.HELVETICA,  8, Font.NORMAL, new BaseColor(180, 180, 180));

                // Cores 
                BaseColor corPrimaria = new BaseColor(33,  97, 140);
                BaseColor corFundo    = new BaseColor(245, 248, 250);
                BaseColor corBorda    = new BaseColor(200, 220, 240);

                // Cabeçalho 
                byte[] logoBytes = (byte[])new ImageConverter().ConvertTo(Properties.Resources.logopng, typeof(byte[]));

                PdfPTable headerTable = new PdfPTable(new float[] { 1, 3 });
                headerTable.WidthPercentage = 100;

                try
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoBytes);
                    logo.ScaleToFit(55f, 55f);
                    PdfPCell logoCell = new PdfPCell(logo);
                    logoCell.BackgroundColor     = corPrimaria;
                    logoCell.Border              = Rectangle.NO_BORDER;
                    logoCell.VerticalAlignment   = Element.ALIGN_MIDDLE;
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    logoCell.Padding             = 8;
                    headerTable.AddCell(logoCell);
                }
                catch
                {
                    PdfPCell vazia = new PdfPCell(new Phrase(""));
                    vazia.BackgroundColor = corPrimaria;
                    vazia.Border          = Rectangle.NO_BORDER;
                    headerTable.AddCell(vazia);
                }

                PdfPCell textoCell = new PdfPCell();
                textoCell.BackgroundColor   = corPrimaria;
                textoCell.Border            = Rectangle.NO_BORDER;
                textoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                textoCell.Padding           = 10;
                textoCell.AddElement(new Paragraph("ASSISTEC", fonteEmpresa) { SpacingAfter = 2 });
                textoCell.AddElement(new Paragraph("Recibo de Pagamento", fonteSubtitulo));
                headerTable.AddCell(textoCell);
                doc.Add(headerTable);

                // Linha: Nº do recibo + data de emissão 
                PdfPTable tblNumero = new PdfPTable(2);
                tblNumero.WidthPercentage = 100;
                tblNumero.SpacingBefore   = 6f;

                string idOS = conta["id_os_fk"] != DBNull.Value ? "OS #" + conta["id_os_fk"] : "";

                PdfPCell cellNumero = new PdfPCell(new Phrase(
                    "Recibo Nº " + conta["id_conta_receber"].ToString().PadLeft(6, '0') +
                    (string.IsNullOrEmpty(idOS) ? "" : "   |   " + idOS),
                    fonteNumRecibo));
                cellNumero.Border              = Rectangle.NO_BORDER;
                cellNumero.HorizontalAlignment = Element.ALIGN_LEFT;
                cellNumero.Padding             = 4;

                string dataEmissaoFmt = conta["data_emissao"] != DBNull.Value &&
                    DateTime.TryParse(conta["data_emissao"].ToString(), out DateTime de)
                    ? de.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");

                PdfPCell cellData = new PdfPCell(new Phrase("Emitido em: " + dataEmissaoFmt, fonteNumRecibo));
                cellData.Border              = Rectangle.NO_BORDER;
                cellData.HorizontalAlignment = Element.ALIGN_RIGHT;
                cellData.Padding             = 4;

                tblNumero.AddCell(cellNumero);
                tblNumero.AddCell(cellData);
                doc.Add(tblNumero);

                // Card do valor recebido 
                decimal valorConta = Convert.ToDecimal(conta["valor"]);

                PdfPTable tblValor = new PdfPTable(1);
                tblValor.WidthPercentage = 100;
                tblValor.SpacingBefore   = 4f;
                tblValor.SpacingAfter    = 8f;

                PdfPCell cardValor = new PdfPCell();
                cardValor.BackgroundColor     = corFundo;
                cardValor.Border              = Rectangle.BOX;
                cardValor.BorderColor         = corBorda;
                cardValor.BorderWidth         = 1f;
                cardValor.Padding             = 12f;
                cardValor.HorizontalAlignment = Element.ALIGN_CENTER;
                cardValor.AddElement(new Paragraph("VALOR RECEBIDO",
                    new Font(Font.FontFamily.HELVETICA, 8, Font.BOLD, new BaseColor(100, 100, 100)))
                    { Alignment = Element.ALIGN_CENTER });
                cardValor.AddElement(new Paragraph($"R$ {valorConta:N2}", fonteValorGrande)
                    { Alignment = Element.ALIGN_CENTER, SpacingBefore = 4 });
                tblValor.AddCell(cardValor);
                doc.Add(tblValor);

                // Detalhes do lançamento
                PdfPTable tblDetalhes = new PdfPTable(new float[] { 1, 2 });
                tblDetalhes.WidthPercentage = 100;
                tblDetalhes.SpacingAfter    = 8f;

                void AddDetalhe(string label, string valor)
                {
                    PdfPCell cLabel = new PdfPCell(new Phrase(label, fonteLabel));
                    cLabel.BackgroundColor = corFundo;
                    cLabel.Border          = Rectangle.BOTTOM_BORDER;
                    cLabel.BorderColor     = corBorda;
                    cLabel.Padding         = 6;
                    tblDetalhes.AddCell(cLabel);

                    PdfPCell cValor = new PdfPCell(new Phrase(valor, fonteDado));
                    cValor.BackgroundColor = BaseColor.WHITE;
                    cValor.Border          = Rectangle.BOTTOM_BORDER;
                    cValor.BorderColor     = corBorda;
                    cValor.Padding         = 6;
                    tblDetalhes.AddCell(cValor);
                }

                string FormatarData(string campo) =>
                    conta[campo] != DBNull.Value && DateTime.TryParse(conta[campo].ToString(), out DateTime dt)
                        ? dt.ToString("dd/MM/yyyy") : "-";

                AddDetalhe("DESCRIÇÃO", conta["descricao"].ToString());
                AddDetalhe("VENCIMENTO",FormatarData("data_vencimento"));
                AddDetalhe("DATA PAGAMENTO",FormatarData("data_pagamento"));
                AddDetalhe("FORMA PAGAMENTO",conta["forma_pagamento"] != DBNull.Value ? conta["forma_pagamento"].ToString() : "-");
                AddDetalhe("STATUS", conta["status"].ToString());

                if (conta["observacoes"] != DBNull.Value && !string.IsNullOrWhiteSpace(conta["observacoes"].ToString()))
                    AddDetalhe("OBSERVAÇÕES", conta["observacoes"].ToString());

                doc.Add(tblDetalhes);

                // Assinatura
                PdfPTable tblAssinatura = new PdfPTable(2);
                tblAssinatura.WidthPercentage = 100;
                tblAssinatura.SpacingBefore   = 20f;

                void AddLinha(string legenda)
                {
                    PdfPCell cell = new PdfPCell();
                    cell.Border  = Rectangle.NO_BORDER;
                    cell.Padding = 4;
                    cell.AddElement(new Paragraph("_______________________________",
                        new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, new BaseColor(150, 150, 150))));
                    cell.AddElement(new Paragraph(legenda,
                        new Font(Font.FontFamily.HELVETICA, 7, Font.NORMAL, new BaseColor(130, 130, 130)))
                        { Alignment = Element.ALIGN_CENTER });
                    tblAssinatura.AddCell(cell);
                }

                AddLinha("Assinatura do Responsável");
                AddLinha("Assinatura do Cliente");
                doc.Add(tblAssinatura);

                // Rodapé
                doc.Add(new Paragraph(" "));
                Paragraph rodape = new Paragraph(
                    $"Documento gerado em {DateTime.Now:dd/MM/yyyy} às {DateTime.Now:HH:mm}  •  ASSISTEC — Sistema de Gestão de Assistência Técnica",
                    fonteRodape) { Alignment = Element.ALIGN_CENTER };
                doc.Add(rodape);

                doc.Close();

                MessageBox.Show("Recibo gerado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(caminho);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao gerar recibo!\n" + ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }
        #endregion
        
        public void SalvarSaida()
        {
            if (tipo == 2)
            {
                if (status == "PAGA" && string.IsNullOrWhiteSpace(dataPagamento))
                {
                    MessageBox.Show("Data de pagamento é obrigatória para status PAGA!", 
                        "Atenção!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                
                try
                {
                    con.OpenConnection();
                    sql = @"INSERT INTO contas_pagar 
                        (DESCRICAO, VALOR, DATA_EMISSAO, DATA_PAGAMENTO, STATUS, ID_FORMA_PAGAMENTO_FK, OBSERVACOES)
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
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@id_pagamento_fk", pagamento.id_pagamento);
                    cmd.Parameters.AddWithValue("@observacoes", obervacoes);
                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                
                    MessageBox.Show("Saída registrada com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
    
    
    
}
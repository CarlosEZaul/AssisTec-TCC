using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AssisTec
{
    public class Cliente : Pessoa
    {
        private conexao con = new conexao();
        private MySqlCommand cmd;
        private string sql;

        public Cliente carregarDados(int id)
        {
            try
            {
                con.OpenConnection();

                sql = "SELECT * FROM clientes WHERE id_cliente = @id_cliente";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id_cliente", id);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    this.id = rs.GetInt32("id_cliente");
                    this.nome = rs.GetString("nome");
                    this.cpf = rs.GetString("cpf");
                    this.telefone = rs.GetString("telefone");
                    this.dataNascimento = rs.GetDateTime("datanasc").ToString("dd/MM/yyyy");
                    this.cep = rs.GetString("cep");
                    this.rua = rs.GetString("rua");
                    this.numero = Convert.ToInt32(rs.GetString("numero"));
                    this.cidade = rs.GetString("cidade");
                    this.estado = rs.GetString("estado");
                    this.bairro = rs.GetString("bairro");
                    this.complemento = rs.GetString("complemento");
                    
                }

                rs.Close();
                con.CloseConnection();
                return this;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar cliente!" + ex, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        public bool novoCliente(Cliente cliente)
        {

            if (cliente == null)
            {
                MessageBox.Show("Cadastro com um campo inválido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;

            }

            try
            {
                if (!ValidarCPF(cliente.cpf))
                {
                    MessageBox.Show("CPF inválido", "Verifique o CPF", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                    return false;
                }

                if (!ValidarTelefone(cliente.telefone))
                {
                    MessageBox.Show("Número de telefone inválido", "Verifique o número de telefone",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }

                con.OpenConnection();
                sql = "SELECT COUNT(*) FROM clientes WHERE cpf=@cpf";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@cpf", cliente.cpf);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                
                if (count > 0)
                {
                    MessageBox.Show("Cliente já cadastrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.CloseConnection();
                    return false;
                }
                else
                {

                    con.OpenConnection();
                    sql =
                        "insert into clientes (nome, cpf, telefone, datanasc, cep, rua, numero, cidade, estado, bairro, complemento) values (@nome, @cpf, @telefone, @datanasc, @cep, @rua, @numero, @cidade, @estado, @bairro, @complemento)";
                    cmd = new MySqlCommand(sql, con.con);

                    cmd.Parameters.AddWithValue("@nome", cliente.nome);
                    cmd.Parameters.AddWithValue("@cpf", cliente.cpf);
                    cmd.Parameters.AddWithValue("@telefone", cliente.telefone);
                    cmd.Parameters.AddWithValue("@datanasc", cliente.dataNascimentoFormatada);
                    cmd.Parameters.AddWithValue("@cep", cliente.cep);
                    cmd.Parameters.AddWithValue("@rua", cliente.rua);
                    cmd.Parameters.AddWithValue("@numero", cliente.numero);
                    cmd.Parameters.AddWithValue("@cidade", cliente.cidade);
                    cmd.Parameters.AddWithValue("@estado", cliente.estado);
                    cmd.Parameters.AddWithValue("@bairro", cliente.bairro);
                    cmd.Parameters.AddWithValue("@complemento", cliente.complemento);

                    cmd.ExecuteNonQuery();
                    con.CloseConnection();


                    MessageBox.Show("Cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    return true;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar cliente\n" + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            
        }

        public bool editarCliente(Cliente cliente)
        {

            if (cliente == null)
            {
                MessageBox.Show("Tentativa de cadastro com campo inválido!", "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
            

            try
            {
                if (!ValidarCPF(cliente.cpf))
                {
                    MessageBox.Show("CPF inválido", "Verifique o CPF", MessageBoxButtons.OK, MessageBoxIcon.Warning );
                    return false;
                }
                if (!ValidarTelefone(cliente.telefone))
                {
                    MessageBox.Show("Número de telefone inválido", "Verifique o número de telefone",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                con.OpenConnection();
                sql =
                    "update clientes set nome=@nome, cpf=@cpf, telefone=@telefone, datanasc=@datanasc, cep=@cep, rua=@rua, numero=@numero, cidade=@cidade, estado=@estado, bairro=@bairro, complemento=@complemento where id_cliente=@id_cliente";

                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id_cliente", cliente.id);
                cmd.Parameters.AddWithValue("@nome", cliente.nome);
                cmd.Parameters.AddWithValue("@cpf", cliente.cpf);
                cmd.Parameters.AddWithValue("@telefone", cliente.telefone);
                cmd.Parameters.AddWithValue("@datanasc", cliente.dataNascimentoFormatada);
                cmd.Parameters.AddWithValue("@cep", cliente.cep);
                cmd.Parameters.AddWithValue("@rua", cliente.rua);
                cmd.Parameters.AddWithValue("@numero", cliente.numero);
                cmd.Parameters.AddWithValue("@cidade", cliente.cidade);
                cmd.Parameters.AddWithValue("@estado", cliente.estado);
                cmd.Parameters.AddWithValue("@bairro", cliente.bairro);
                cmd.Parameters.AddWithValue("@complemento", cliente.complemento);

                cmd.ExecuteNonQuery();
                con.CloseConnection();



                MessageBox.Show("Cliente editado com sucesso!", "Sucesso", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return true;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Erro ao editar!\n" + exception.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return false;
            }
        }

        public void deletarCLiente(int id)
        {
            DialogResult result = MessageBox.Show("Deseja excluir cliente?", "Confirmar Exclusão", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.OpenConnection();
                    sql = "DELETE FROM clientes WHERE id_cliente = @id";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                    
                    
                    MessageBox.Show("Cliente excluído com sucesso!", "Sucesso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    atualizarDados();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Erro ao excluir cliente!\n" + exception.Message, "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        public DataTable atualizarDados()
        {
            DataTable dt =  new DataTable();
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM clientes ORDER BY NOME ASC";
                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
                con.CloseConnection();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return dt;
        }
        
        public DataTable historicoOs(int id_cliente)
        {
            DataTable dt = new DataTable();
            
            con.OpenConnection();
            sql = @"SELECT 
                os.id_os,
                c.nome          AS nome_cliente,
                u.nome          AS nome_tecnico,
                e.descricao     AS descricao_equipamento,
                os.status,
                os.data_abertura,
                os.data_fechamento,
                os.valor_mao_obra,
                os.valor_pecas,
                os.valor_total,
                os.problema_relatado,
                os.diagnostico,
                os.observacoes
            FROM ordem_servico os
            LEFT JOIN clientes    c ON c.id_cliente    = os.id_cliente
            LEFT JOIN usuarios    u ON u.id_usuario    = os.id_tecnico
            LEFT JOIN equipamentos e ON e.id_equipamento = os.id_equipamento
            WHERE os.id_cliente = @id";
            cmd = new MySqlCommand(sql, con.con);
            cmd.Parameters.AddWithValue("@id", id_cliente);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            con.CloseConnection();
            return dt;
        }

        public void gerarRelatorioTodosClientes()
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM clientes ORDER BY nome";
                cmd = new MySqlCommand(sql, con.con);
                MySqlDataReader reader = cmd.ExecuteReader();

                //Configurar PDF 
                string pasta = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Relatório de Clientes",
                    DateTime.Now.ToString("dd-MM-yyyy")
                );
                Directory.CreateDirectory(pasta);

                string caminho = Path.Combine(
                    pasta,
                    "Relatorio_Clientes_" +
                    ".pdf"
                );


                Document  doc    = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                //Fontes 
                Font fonteAssistec   = new Font(Font.FontFamily.HELVETICA, 28, Font.BOLD,   BaseColor.WHITE);
                Font fonteSubtitulo  = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL, new BaseColor(200, 220, 240));
                Font fonteColuna     = new Font(Font.FontFamily.HELVETICA,  9, Font.BOLD,   BaseColor.WHITE);
                Font fonteDado       = new Font(Font.FontFamily.HELVETICA,  9, Font.NORMAL, new BaseColor(30, 30, 30));

                BaseColor corPrimaria = new BaseColor(33, 97, 140);
                BaseColor corHeader   = new BaseColor(52, 73, 94);
                BaseColor corLinha1   = new BaseColor(245, 248, 250);
                BaseColor corLinha2   = BaseColor.WHITE;

                //Cabeçalho com Logo
                string caminhoLogo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "logo.png");

                PdfPTable headerTable = new PdfPTable(new float[] { 1, 3 });
                headerTable.WidthPercentage = 100;

                try
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(caminhoLogo);
                    logo.ScaleToFit(80f, 80f);
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
                textoCell.BackgroundColor     = corPrimaria;
                textoCell.Border              = Rectangle.NO_BORDER;
                textoCell.VerticalAlignment   = Element.ALIGN_MIDDLE;
                textoCell.Padding             = 12;
                textoCell.AddElement(new Paragraph("ASSISTEC",          fonteAssistec)  { SpacingAfter = 2 });
                textoCell.AddElement(new Paragraph("Relatório de Clientes", fonteSubtitulo));
                headerTable.AddCell(textoCell);

                doc.Add(headerTable);
                doc.Add(new Paragraph(" "));

                //Tabela de Clientes
                PdfPTable tabela = new PdfPTable(new float[] { 3, 2, 2, 2, 3, 2 });
                tabela.WidthPercentage = 100;

                void AddCabecalho(string texto)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(texto, fonteColuna));
                    cell.BackgroundColor     = corHeader;
                    cell.Border              = Rectangle.NO_BORDER;
                    cell.Padding             = 8;
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    tabela.AddCell(cell);
                }

                AddCabecalho("Nome");
                AddCabecalho("CPF");
                AddCabecalho("Telefone");
                AddCabecalho("Nasc.");
                AddCabecalho("Cidade / Estado");
                AddCabecalho("CEP");

                int linha = 0;
                while (reader.Read())
                {
                    BaseColor cor = linha % 2 == 0 ? corLinha1 : corLinha2;

                    void AddCelula(string valor)
                    {
                        PdfPCell cell = new PdfPCell(new Phrase(valor, fonteDado));
                        cell.BackgroundColor     = cor;
                        cell.Border              = Rectangle.NO_BORDER;
                        cell.Padding             = 7;
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        tabela.AddCell(cell);
                    }

                    AddCelula(reader.GetString("nome"));
                    AddCelula(reader.GetString("cpf"));
                    AddCelula(reader.IsDBNull(reader.GetOrdinal("telefone"))  ? "-" : reader.GetString("telefone"));
                    AddCelula(reader.IsDBNull(reader.GetOrdinal("datanasc"))  ? "-" : reader.GetDateTime("datanasc").ToString("dd/MM/yyyy"));
                    AddCelula($"{reader.GetString("cidade")} / {reader.GetString("estado")}");
                    AddCelula(reader.GetString("cep"));

                    linha++;
                }

                reader.Close();
                con.CloseConnection();

                // Rodapé com total de clientes
                PdfPCell rodape = new PdfPCell(new Phrase($"Total de clientes: {linha}", 
                    new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, new BaseColor(80, 80, 80))));
                rodape.Colspan             = 6;
                rodape.BackgroundColor     = new BaseColor(214, 234, 248);
                rodape.Border              = Rectangle.NO_BORDER;
                rodape.Padding             = 8;
                rodape.HorizontalAlignment = Element.ALIGN_RIGHT;
                tabela.AddCell(rodape);

                doc.Add(tabela);
                doc.Close();

                MessageBox.Show($"Relatório gerado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(caminho);
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao gerar relatório!\n" + e.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        
        public void ImprimirCliente(int id) {
            try
            {
                Cliente cliente = carregarDados(id);

                con.OpenConnection();
                sql = @"SELECT 
                            os.id_os,
                            u.nome          AS nome_tecnico,
                            e.descricao     AS descricao_equipamento,
                            os.status,
                            os.data_abertura,
                            os.data_fechamento,
                            os.valor_mao_obra,
                            os.valor_pecas,
                            os.valor_total,
                            os.problema_relatado,
                            os.diagnostico,
                            os.observacoes
                        FROM ordem_servico os
                        LEFT JOIN usuarios      u  ON u.id_usuario     = os.id_tecnico
                        LEFT JOIN equipamentos  e  ON e.id_equipamento = os.id_equipamento
                        WHERE os.id_cliente = @id";

                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                // Configurar PDF 
                string pasta = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "Relatório de Clientes",
                    DateTime.Now.ToString("dd-MM-yyyy")
                );
                Directory.CreateDirectory(pasta);

                string caminho = Path.Combine(
                    pasta,
                    "Relatorio_Cliente_" +
                    $"{cliente.nome}_{DateTime.Now:ddMMyyyy_HHmm}.pdf"
                );

                

                Document doc     = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                // Fontes 
                Font fonteTitulo    = new Font(Font.FontFamily.HELVETICA, 16, Font.BOLD,   BaseColor.WHITE);
                Font fonteSecao     = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD,   new BaseColor(30, 30, 30));
                Font fonteLabel     = new Font(Font.FontFamily.HELVETICA,  9, Font.BOLD,   new BaseColor(80, 80, 80));
                Font fonteValor     = new Font(Font.FontFamily.HELVETICA,  9, Font.NORMAL, new BaseColor(30, 30, 30));
                Font fonteOsTitulo  = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD,   BaseColor.WHITE);
                Font fonteTotal     = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD,   new BaseColor(30, 30, 30));

                BaseColor corPrimaria = new BaseColor(33, 97, 140);
                BaseColor corSecao    = new BaseColor(214, 234, 248);
                BaseColor corOsHeader = new BaseColor(52, 73, 94);
                BaseColor corLinha1   = new BaseColor(245, 248, 250);
                BaseColor corLinha2   = BaseColor.WHITE;

                //Cabeçalho com Logo
                string caminhoLogo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "img", "logopng.png");

                PdfPTable headerTable = new PdfPTable(new float[] { 1, 3 });
                headerTable.WidthPercentage = 100;

                // Célula da logo
                try
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(caminhoLogo);
                    logo.ScaleToFit(80f, 80f);
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
                    // Se não encontrar a logo, coloca célula vazia
                    PdfPCell vazia = new PdfPCell(new Phrase(""));
                    vazia.BackgroundColor = corPrimaria;
                    vazia.Border          = Rectangle.NO_BORDER;
                    headerTable.AddCell(vazia);
                }

                // Célula do texto ASSISTEC + subtítulo
                Font fonteAssistec  = new Font(Font.FontFamily.HELVETICA, 28, Font.BOLD,   BaseColor.WHITE);
                Font fonteSubtitulo = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL, new BaseColor(200, 220, 240));

                PdfPCell textoCell = new PdfPCell();
                textoCell.BackgroundColor     = corPrimaria;
                textoCell.Border              = Rectangle.NO_BORDER;
                textoCell.VerticalAlignment   = Element.ALIGN_MIDDLE;
                textoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                textoCell.Padding             = 12;
                textoCell.AddElement(new Paragraph("ASSISTEC",            fonteAssistec)  { SpacingAfter = 2 });
                textoCell.AddElement(new Paragraph("Relatório do Cliente", fonteSubtitulo));
                headerTable.AddCell(textoCell);

                doc.Add(headerTable);
                doc.Add(new Paragraph(" "));
                
               

                //Dados do Cliente
                PdfPTable secaoCliente = new PdfPTable(1);
                secaoCliente.WidthPercentage = 100;
                PdfPCell secaoCell = new PdfPCell(new Phrase("DADOS DO CLIENTE", fonteSecao));
                secaoCell.BackgroundColor = corSecao;
                secaoCell.Padding         = 8;
                secaoCell.Border          = Rectangle.NO_BORDER;
                secaoCliente.AddCell(secaoCell);
                doc.Add(secaoCliente);

                PdfPTable dadosCliente = new PdfPTable(new float[] { 2, 5 });
                dadosCliente.WidthPercentage = 100;

                void AddLinha(string label, string valor, BaseColor cor)
                {
                    PdfPCell l = new PdfPCell(new Phrase(label, fonteLabel));
                    l.BackgroundColor = cor; l.Border = Rectangle.NO_BORDER; l.Padding = 6;
                    PdfPCell v = new PdfPCell(new Phrase(valor, fonteValor));
                    v.BackgroundColor = cor; v.Border = Rectangle.NO_BORDER; v.Padding = 6;
                    dadosCliente.AddCell(l);
                    dadosCliente.AddCell(v);
                }

                AddLinha("Nome",      cliente.nome,                                      corLinha1);
                AddLinha("CPF",       cliente.cpf,                                       corLinha2);
                AddLinha("Telefone",  cliente.telefone,                                  corLinha1);
                AddLinha("Nasc.",     cliente.dataNascimento,                            corLinha2);
                AddLinha("Endereço",  $"{cliente.rua}, {cliente.numero} - {cliente.bairro}", corLinha1);
                AddLinha("Cidade",    $"{cliente.cidade} / {cliente.estado}",            corLinha2);
                AddLinha("CEP",       cliente.cep,                                       corLinha1);
                AddLinha("Compl.",    cliente.complemento,                               corLinha2);

                doc.Add(dadosCliente);
                doc.Add(new Paragraph(" "));

                //Ordens de Serviço
                PdfPTable secaoOs = new PdfPTable(1);
                secaoOs.WidthPercentage = 100;
                PdfPCell secaoOsCell = new PdfPCell(new Phrase("ORDENS DE SERVIÇO", fonteSecao));
                secaoOsCell.BackgroundColor = corSecao;
                secaoOsCell.Padding         = 8;
                secaoOsCell.Border          = Rectangle.NO_BORDER;
                secaoOs.AddCell(secaoOsCell);
                doc.Add(secaoOs);

                decimal totalMaoObra = 0, totalPecas = 0, totalGeral = 0;
                int totalOs = 0;
                bool temOs  = false;

                while (reader.Read())
                {
                    if (reader.IsDBNull(reader.GetOrdinal("id_os"))) break;

                    temOs = true;
                    decimal maoObra = reader.GetDecimal("valor_mao_obra");
                    decimal pecas   = reader.GetDecimal("valor_pecas");
                    decimal total   = reader.GetDecimal("valor_total");
                    totalMaoObra   += maoObra;
                    totalPecas     += pecas;
                    totalGeral     += total;
                    totalOs++;

                    // Header da OS
                    PdfPTable osHeader = new PdfPTable(1);
                    osHeader.WidthPercentage = 100;
                    osHeader.SpacingBefore   = 8;
                    PdfPCell osHeaderCell = new PdfPCell(new Phrase($"OS #{reader.GetInt32("id_os")}  —  {reader.GetString("status")}", fonteOsTitulo));
                    osHeaderCell.BackgroundColor    = corOsHeader;
                    osHeaderCell.Padding            = 7;
                    osHeaderCell.Border             = Rectangle.NO_BORDER;
                    osHeader.AddCell(osHeaderCell);
                    doc.Add(osHeader);

                    // Campos da OS
                    PdfPTable dadosOs = new PdfPTable(new float[] { 2, 5 });
                    dadosOs.WidthPercentage = 100;

                    void AddLinhaOs(string label, string valor, BaseColor cor)
                    {
                        PdfPCell l = new PdfPCell(new Phrase(label, fonteLabel));
                        l.BackgroundColor = cor; l.Border = Rectangle.NO_BORDER; l.Padding = 5;
                        PdfPCell v = new PdfPCell(new Phrase(valor, fonteValor));
                        v.BackgroundColor = cor; v.Border = Rectangle.NO_BORDER; v.Padding = 5;
                        dadosOs.AddCell(l);
                        dadosOs.AddCell(v);
                    }

                    AddLinhaOs("Técnico",     reader.IsDBNull(reader.GetOrdinal("nome_tecnico"))          ? "Não atribuído" : reader.GetString("nome_tecnico"),          corLinha1);
                    AddLinhaOs("Equipamento", reader.IsDBNull(reader.GetOrdinal("descricao_equipamento")) ? "Não informado"  : reader.GetString("descricao_equipamento"), corLinha2);
                    AddLinhaOs("Abertura",    reader.GetDateTime("data_abertura").ToString("dd/MM/yyyy HH:mm"),                                                           corLinha1);
                    AddLinhaOs("Fechamento",  reader.IsDBNull(reader.GetOrdinal("data_fechamento")) ? "Em aberto" : reader.GetDateTime("data_fechamento").ToString("dd/MM/yyyy HH:mm"), corLinha2);
                    AddLinhaOs("Problema",    reader.GetString("problema_relatado"),                                                                                      corLinha1);
                    AddLinhaOs("Diagnóstico", reader.IsDBNull(reader.GetOrdinal("diagnostico")) ? "Não informado" : reader.GetString("diagnostico"),                      corLinha2);
                    AddLinhaOs("Observações", reader.IsDBNull(reader.GetOrdinal("observacoes"))  ? "Nenhuma"       : reader.GetString("observacoes"),                     corLinha1);
                    AddLinhaOs("Mão de obra", $"R$ {maoObra:F2}",                                                                                                        corLinha2);
                    AddLinhaOs("Peças",       $"R$ {pecas:F2}",                                                                                                          corLinha1);
                    AddLinhaOs("Total",       $"R$ {total:F2}",                                                                                                           corLinha2);

                    doc.Add(dadosOs);
                }

                if (!temOs)
                {
                    doc.Add(new Paragraph("Nenhuma ordem de serviço encontrada.", fonteValor));
                }

                reader.Close();
                con.CloseConnection();

                //Resumo Financeiro
                if (totalOs > 0)
                {
                    doc.Add(new Paragraph(" "));

                    PdfPTable secaoResumo = new PdfPTable(1);
                    secaoResumo.WidthPercentage = 100;
                    PdfPCell secaoResumoCell = new PdfPCell(new Phrase("RESUMO FINANCEIRO", fonteSecao));
                    secaoResumoCell.BackgroundColor = corSecao;
                    secaoResumoCell.Padding         = 8;
                    secaoResumoCell.Border          = Rectangle.NO_BORDER;
                    secaoResumo.AddCell(secaoResumoCell);
                    doc.Add(secaoResumo);

                    PdfPTable resumo = new PdfPTable(new float[] { 3, 2 });
                    resumo.WidthPercentage = 100;

                    void AddResumo(string label, string valor, BaseColor cor)
                    {
                        PdfPCell l = new PdfPCell(new Phrase(label, fonteTotal));
                        l.BackgroundColor = cor; l.Border = Rectangle.NO_BORDER; l.Padding = 7;
                        PdfPCell v = new PdfPCell(new Phrase(valor, fonteTotal));
                        v.BackgroundColor = cor; v.Border = Rectangle.NO_BORDER; v.Padding = 7;
                        v.HorizontalAlignment = Element.ALIGN_RIGHT;
                        resumo.AddCell(l);
                        resumo.AddCell(v);
                    }

                    AddResumo("Total de OS",          $"{totalOs}",                corLinha1);
                    AddResumo("Total mão de obra",    $"R$ {totalMaoObra:F2}",    corLinha2);
                    AddResumo("Total peças",          $"R$ {totalPecas:F2}",      corLinha1);
                    AddResumo("Total geral",          $"R$ {totalGeral:F2}",      corLinha2);

                    doc.Add(resumo);
                }

                doc.Close();

                MessageBox.Show($"Relatório gerado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Abrir o PDF automaticamente
                System.Diagnostics.Process.Start(caminho);
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao gerar relatório!\n" + e.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
    
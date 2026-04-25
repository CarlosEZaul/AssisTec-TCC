using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using Font = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

namespace AssisTec
{
    public class Usuario:Pessoa
    {
        private string senha;
        private int nivel;
        private string status;
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;

        public string Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        public int Nivel
        {
            get { return nivel; }
            set { nivel = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        
        public Usuario carregarDados(int id)
        {
            try
            {
                Usuario usuario = new Usuario();

                con.OpenConnection();

                sql = "SELECT * FROM usuarios WHERE id_usuario  = @id_usuario";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id_usuario", id);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    usuario.id = rs.GetInt32("id_usuario");
                    usuario.nome = rs.GetString("nome");
                    usuario.cpf = rs.GetString("cpf");
                    usuario.telefone = rs.GetString("telefone");
                    usuario.status = rs.GetString("status");
                    usuario.nivel = Convert.ToInt32(rs["nivel"]);
                    usuario.senha = rs.GetString("senha");
                    usuario.cep = rs.GetString("cep");
                    usuario.rua = rs.GetString("rua");
                    usuario.numero = Convert.ToInt32(rs.GetString("numero"));
                    usuario.cidade = rs.GetString("cidade");
                    usuario.estado = rs.GetString("estado");
                    usuario.bairro = rs.GetString("bairro");
                    usuario.complemento = rs.GetString("complemento");
                    
                }

                rs.Close();
                con.CloseConnection();
                return usuario;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar usuário!" + ex, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return new Usuario();
        }
        

        private bool usuarioExiste(string cpf, int? ignorarId = null)
        {
            con.OpenConnection();
            sql = "SELECT COUNT(*) FROM usuarios WHERE cpf = @cpf";
            if (ignorarId.HasValue)
            {
                sql += " AND id_usuario <> @id"; 
            }

            using (MySqlCommand cmd = new MySqlCommand(sql, con.con))
            {
                cmd.Parameters.AddWithValue("@cpf", cpf);
                if (ignorarId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@id", ignorarId.Value);
                }

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.CloseConnection();
                return count > 0;
            }
        }
        
        public bool novoUsuario(Usuario user)
        {
          
            
            if (user == null)
            {
                MessageBox.Show("Tentativa de cadastro com campo inválido. Verifique os dados.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            try
            {
                if (usuarioExiste(user.cpf))
                {
                    MessageBox.Show("Usuário com este CPF já existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                string senhaHash = BCrypt.Net.BCrypt.HashPassword(user.Senha);
                
                con.OpenConnection();
                sql = "INSERT INTO usuarios (nome, cpf, telefone, senha, nivel, status, cep, rua, numero, cidade, bairro, estado, complemento) " +
                      "VALUES (@nome, @cpf, @telefone, @senha, @nivel, @status, @cep, @rua, @numero, @cidade, @bairro, @estado, @complemento)";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", user.nome);
                cmd.Parameters.AddWithValue("@cpf", user.cpf);
                cmd.Parameters.AddWithValue("@telefone", user.telefone);
                cmd.Parameters.AddWithValue("@senha", senhaHash);
                cmd.Parameters.AddWithValue("@nivel", user.nivel);
                cmd.Parameters.AddWithValue("@status", user.status);
                cmd.Parameters.AddWithValue("@cep", user.cep);
                cmd.Parameters.AddWithValue("@rua", user.rua);
                cmd.Parameters.AddWithValue("@numero", user.numero);
                cmd.Parameters.AddWithValue("@cidade", user.cidade);
                cmd.Parameters.AddWithValue("@bairro", user.bairro);
                cmd.Parameters.AddWithValue("@estado", user.estado);
                cmd.Parameters.AddWithValue("@complemento", user.complemento);

                cmd.ExecuteNonQuery();
                con.CloseConnection();

                MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                return true;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Erro ao cadastrar usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        
        public bool editarUsuario(Usuario user)
        {
            if (user == null)
            {
                MessageBox.Show("Dados inválidos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                if (usuarioExiste(user.cpf, user.id))
                {
                    MessageBox.Show("Usuário já existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                con.OpenConnection();

                // NÃO altera senha se for *******
                bool alterarSenha = 
                    !string.IsNullOrWhiteSpace(user.senha) && 
                    user.senha != "********";

                string sql = @"UPDATE usuarios SET 
                                nome=@nome, 
                                cpf=@cpf, 
                                telefone=@tel, 
                                nivel=@nivel, 
                                status=@status,
                                cep=@cep, 
                                rua=@rua, 
                                numero=@numero, 
                                cidade=@cidade, 
                                bairro=@bairro, 
                                estado=@estado, 
                                complemento=@complemento";

                if (alterarSenha)
                {
                    sql += ", senha=@senha";
                }

                sql += " WHERE id_usuario=@id";

                using (MySqlCommand cmd = new MySqlCommand(sql, con.con))
                {
                    cmd.Parameters.AddWithValue("@nome", user.nome);
                    cmd.Parameters.AddWithValue("@cpf", user.cpf);
                    cmd.Parameters.AddWithValue("@tel", user.telefone);
                    cmd.Parameters.AddWithValue("@nivel", user.nivel);
                    cmd.Parameters.AddWithValue("@status", user.status);
                    cmd.Parameters.AddWithValue("@cep", user.cep);
                    cmd.Parameters.AddWithValue("@rua", user.rua);
                    cmd.Parameters.AddWithValue("@numero", user.numero);
                    cmd.Parameters.AddWithValue("@cidade", user.cidade);
                    cmd.Parameters.AddWithValue("@bairro", user.bairro);
                    cmd.Parameters.AddWithValue("@estado", user.estado);
                    cmd.Parameters.AddWithValue("@complemento", user.complemento);
                    cmd.Parameters.AddWithValue("@id", user.id);

                    // Só atualiza senha se for nova
                    if (alterarSenha)
                    {
                        string senhaHash = BCrypt.Net.BCrypt.HashPassword(user.senha);
                        cmd.Parameters.AddWithValue("@senha", senhaHash);
                    }

                    cmd.ExecuteNonQuery();
                }

                con.CloseConnection();
                

                MessageBox.Show("Usuário editado com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                con.CloseConnection();
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public void deletarUsuario(int id, DataGridView dgv)
        {
            DialogResult result = MessageBox.Show("Deseja excluir usuário?", "Confirmar Exclusão", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.OpenConnection();
                    sql = "SELECT COUNT(*) FROM ordem_servico where id_tecnico = @id AND status = 'EM ANDAMENTO'";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@id", id);
                    int osEmAndamento = Convert.ToInt32(cmd.ExecuteScalar());
                    con.CloseConnection();

                    if (osEmAndamento >0)
                    {
                        MessageBox.Show("Não é possivel excluir usuário com um Ordem de Serviço em andamento", "Exclusão bloqueada", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        
                    }
                
                    con.OpenConnection();
                    sql = "DELETE FROM usuarios WHERE id_usuario = @id";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                    dgv.DataSource = atualizarDados();
                    
                
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    
                }
                
            }
        }
        
        //Atualiza os dados do datagridview
        public DataTable atualizarDados()
        {
            DataTable dt = new DataTable();
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM usuarios ORDER BY NOME ASC";
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

        public DataTable historicoOs(int id_tecnico)
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
            WHERE os.id_tecnico = @id";
            cmd = new MySqlCommand(sql, con.con);
            cmd.Parameters.AddWithValue("@id", id_tecnico);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.Fill(dt);
            con.CloseConnection();
            return dt;
        }

        public void gerarRelatorioTodosUsuarios(string nomeFiltro, bool apenasInativos, int nivelSelecionado)
        {
            try
            {
                con.OpenConnection();

                sql = "SELECT * FROM usuarios WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(nomeFiltro))
                    sql += " AND nome LIKE @nome";

                if (apenasInativos)
                    sql += " AND status = 'Desativado'";

                if (nivelSelecionado != 0)
                    sql += " AND nivel = @nivel";

                sql += " ORDER BY nome ASC";

                cmd = new MySqlCommand(sql, con.con);

                if (!string.IsNullOrWhiteSpace(nomeFiltro))
                    cmd.Parameters.AddWithValue("@nome", nomeFiltro + "%");

                if (nivelSelecionado != 0)
                    cmd.Parameters.AddWithValue("@nivel", nivelSelecionado);

                MySqlDataReader reader = cmd.ExecuteReader();

                string caminho = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                              "Relatorio_Usuarios_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".pdf");

                Document  doc    = new Document(PageSize.A4.Rotate(), 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                Font fonteAssistec  = new Font(Font.FontFamily.HELVETICA, 28, Font.BOLD,   BaseColor.WHITE);
                Font fonteSubtitulo = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL, new BaseColor(200, 220, 240));
                Font fonteColuna    = new Font(Font.FontFamily.HELVETICA,  9, Font.BOLD,   BaseColor.WHITE);
                Font fonteDado      = new Font(Font.FontFamily.HELVETICA,  9, Font.NORMAL, new BaseColor(30, 30, 30));

                BaseColor corPrimaria = new BaseColor(33, 97, 140);
                BaseColor corHeader   = new BaseColor(52, 73, 94);
                BaseColor corLinha1   = new BaseColor(245, 248, 250);
                BaseColor corLinha2   = BaseColor.WHITE;

                byte[] logoBytes = (byte[])new ImageConverter().ConvertTo(Properties.Resources.logopng, typeof(byte[]));
                

                PdfPTable headerTable = new PdfPTable(new float[] { 1, 3 });
                headerTable.WidthPercentage = 100;

                try
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoBytes);
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
                textoCell.BackgroundColor   = corPrimaria;
                textoCell.Border            = Rectangle.NO_BORDER;
                textoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                textoCell.Padding           = 12;
                textoCell.AddElement(new Paragraph("ASSISTEC",             fonteAssistec)  { SpacingAfter = 2 });
                textoCell.AddElement(new Paragraph("Relatório de Usuários", fonteSubtitulo));
                headerTable.AddCell(textoCell);

                doc.Add(headerTable);
                doc.Add(new Paragraph(" "));

                PdfPTable tabela = new PdfPTable(new float[] { 3, 2, 2, 1, 1, 3, 2 });
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
                AddCabecalho("Nível");
                AddCabecalho("Status");
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

                    string nivel = reader.GetString("nivel");
                    if      (nivel == "1") nivel = "Administrador";
                    else if (nivel == "2") nivel = "Técnico";
                    else if (nivel == "3") nivel = "Atendente";

                    AddCelula(reader.GetString("nome"));
                    AddCelula(reader.GetString("cpf"));
                    AddCelula(reader.IsDBNull(reader.GetOrdinal("telefone")) ? "-" : reader.GetString("telefone"));
                    AddCelula(nivel);
                    AddCelula(reader.GetString("status"));
                    AddCelula(reader.GetString("cidade") + " / " + reader.GetString("estado"));
                    AddCelula(reader.GetString("cep"));

                    linha++;
                }

                reader.Close();
                con.CloseConnection();

                PdfPCell rodape = new PdfPCell(new Phrase("Total de usuários: " + linha,
                    new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, new BaseColor(80, 80, 80))));
                rodape.Colspan             = 7;
                rodape.BackgroundColor     = new BaseColor(214, 234, 248);
                rodape.Border              = Rectangle.NO_BORDER;
                rodape.Padding             = 8;
                rodape.HorizontalAlignment = Element.ALIGN_RIGHT;
                tabela.AddCell(rodape);

                doc.Add(tabela);
                doc.Close();

                MessageBox.Show("Relatório gerado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(caminho);
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao gerar relatório!\n" + e.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        public void ImprimirTecnico(int id)
        {
            try
            {
                Usuario tecnico = carregarDados(id);

                con.OpenConnection();
                sql = @"SELECT 
                            os.id_os,
                            c.nome          AS nome_cliente,
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
                        LEFT JOIN clientes     c  ON c.id_cliente     = os.id_cliente
                        LEFT JOIN equipamentos e  ON e.id_equipamento = os.id_equipamento
                        WHERE os.id_tecnico = @id";

                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader reader = cmd.ExecuteReader();

                string caminho = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                                              "Relatorio_" + tecnico.nome + "_" + DateTime.Now.ToString("ddMMyyyy_HHmm") + ".pdf");

                Document  doc    = new Document(PageSize.A4, 40, 40, 40, 40);
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));
                doc.Open();

                Font fonteSecao    = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD,   new BaseColor(30, 30, 30));
                Font fonteLabel    = new Font(Font.FontFamily.HELVETICA,  9, Font.BOLD,   new BaseColor(80, 80, 80));
                Font fonteValor    = new Font(Font.FontFamily.HELVETICA,  9, Font.NORMAL, new BaseColor(30, 30, 30));
                Font fonteOsTitulo = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD,   BaseColor.WHITE);
                Font fonteTotal    = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD,   new BaseColor(30, 30, 30));

                BaseColor corPrimaria = new BaseColor(33, 97, 140);
                BaseColor corSecao    = new BaseColor(214, 234, 248);
                BaseColor corOsHeader = new BaseColor(52, 73, 94);
                BaseColor corLinha1   = new BaseColor(245, 248, 250);
                BaseColor corLinha2   = BaseColor.WHITE;

                byte[] logoBytes = (byte[])new ImageConverter().ConvertTo(Properties.Resources.logopng, typeof(byte[]));
                
                PdfPTable headerTable = new PdfPTable(new float[] { 1, 3 });
                headerTable.WidthPercentage = 100;

                try
                {
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoBytes);
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

                Font fonteAssistec  = new Font(Font.FontFamily.HELVETICA, 28, Font.BOLD,   BaseColor.WHITE);
                Font fonteSubtitulo = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL, new BaseColor(200, 220, 240));

                PdfPCell textoCell = new PdfPCell();
                textoCell.BackgroundColor     = corPrimaria;
                textoCell.Border              = Rectangle.NO_BORDER;
                textoCell.VerticalAlignment   = Element.ALIGN_MIDDLE;
                textoCell.HorizontalAlignment = Element.ALIGN_LEFT;
                textoCell.Padding             = 12;
                textoCell.AddElement(new Paragraph("ASSISTEC",             fonteAssistec)  { SpacingAfter = 2 });
                textoCell.AddElement(new Paragraph("Relatório do Técnico", fonteSubtitulo));
                headerTable.AddCell(textoCell);

                doc.Add(headerTable);
                doc.Add(new Paragraph(" "));

                PdfPTable secaoTecnico = new PdfPTable(1);
                secaoTecnico.WidthPercentage = 100;
                PdfPCell secaoCell = new PdfPCell(new Phrase("DADOS DO TÉCNICO", fonteSecao));
                secaoCell.BackgroundColor = corSecao;
                secaoCell.Padding         = 8;
                secaoCell.Border          = Rectangle.NO_BORDER;
                secaoTecnico.AddCell(secaoCell);
                doc.Add(secaoTecnico);

                PdfPTable dadosTecnico = new PdfPTable(new float[] { 2, 5 });
                dadosTecnico.WidthPercentage = 100;

                void AddLinha(string label, string valor, BaseColor cor)
                {
                    PdfPCell l = new PdfPCell(new Phrase(label, fonteLabel));
                    l.BackgroundColor = cor; l.Border = Rectangle.NO_BORDER; l.Padding = 6;
                    PdfPCell v = new PdfPCell(new Phrase(valor, fonteValor));
                    v.BackgroundColor = cor; v.Border = Rectangle.NO_BORDER; v.Padding = 6;
                    dadosTecnico.AddCell(l);
                    dadosTecnico.AddCell(v);
                }

                AddLinha("Nome",     tecnico.nome,                                          corLinha1);
                AddLinha("CPF",      tecnico.cpf,                                           corLinha2);
                AddLinha("Telefone", tecnico.telefone,                                      corLinha1);
                AddLinha("Status",   tecnico.status,                                        corLinha2);
                AddLinha("Endereço", tecnico.rua + ", " + tecnico.numero + " - " + tecnico.bairro, corLinha1);
                AddLinha("Cidade",   tecnico.cidade + " / " + tecnico.estado,              corLinha2);
                AddLinha("CEP",      tecnico.cep,                                           corLinha1);
                AddLinha("Compl.",   tecnico.complemento,                                   corLinha2);

                doc.Add(dadosTecnico);
                doc.Add(new Paragraph(" "));

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

                    PdfPTable osHeader = new PdfPTable(1);
                    osHeader.WidthPercentage = 100;
                    osHeader.SpacingBefore   = 8;
                    PdfPCell osHeaderCell = new PdfPCell(new Phrase("OS #" + reader.GetInt32("id_os") + "  —  " + reader.GetString("status"), fonteOsTitulo));
                    osHeaderCell.BackgroundColor = corOsHeader;
                    osHeaderCell.Padding         = 7;
                    osHeaderCell.Border          = Rectangle.NO_BORDER;
                    osHeader.AddCell(osHeaderCell);
                    doc.Add(osHeader);

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

                    AddLinhaOs("Cliente",     reader.IsDBNull(reader.GetOrdinal("nome_cliente"))          ? "Não informado" : reader.GetString("nome_cliente"),          corLinha1);
                    AddLinhaOs("Equipamento", reader.IsDBNull(reader.GetOrdinal("descricao_equipamento")) ? "Não informado"  : reader.GetString("descricao_equipamento"), corLinha2);
                    AddLinhaOs("Abertura",    reader.GetDateTime("data_abertura").ToString("dd/MM/yyyy HH:mm"),                                                           corLinha1);
                    AddLinhaOs("Fechamento",  reader.IsDBNull(reader.GetOrdinal("data_fechamento")) ? "Em aberto" : reader.GetDateTime("data_fechamento").ToString("dd/MM/yyyy HH:mm"), corLinha2);
                    AddLinhaOs("Problema",    reader.GetString("problema_relatado"),                                                                                      corLinha1);
                    AddLinhaOs("Diagnóstico", reader.IsDBNull(reader.GetOrdinal("diagnostico")) ? "Não informado" : reader.GetString("diagnostico"),                      corLinha2);
                    AddLinhaOs("Observações", reader.IsDBNull(reader.GetOrdinal("observacoes"))  ? "Nenhuma"       : reader.GetString("observacoes"),                     corLinha1);
                    AddLinhaOs("Mão de obra", "R$ " + maoObra.ToString("F2"),                                                                                            corLinha2);
                    AddLinhaOs("Peças",       "R$ " + pecas.ToString("F2"),                                                                                              corLinha1);
                    AddLinhaOs("Total",       "R$ " + total.ToString("F2"),                                                                                              corLinha2);

                    doc.Add(dadosOs);
                }

                if (!temOs)
                {
                    doc.Add(new Paragraph("Nenhuma ordem de serviço encontrada.", fonteValor));
                }

                reader.Close();
                con.CloseConnection();

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

                    AddResumo("Total de OS",       totalOs.ToString(),              corLinha1);
                    AddResumo("Total mão de obra", "R$ " + totalMaoObra.ToString("F2"), corLinha2);
                    AddResumo("Total peças",       "R$ " + totalPecas.ToString("F2"),   corLinha1);
                    AddResumo("Total geral",       "R$ " + totalGeral.ToString("F2"),   corLinha2);

                    doc.Add(resumo);
                }

                doc.Close();

                MessageBox.Show("Relatório gerado com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

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
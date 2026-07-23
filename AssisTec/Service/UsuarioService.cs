using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AssisTec.Dtos;
using AssisTec.Repository;
using AssisTec.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace AssisTec.Service
{
    public class UsuarioService
    {
        private readonly IUsuarioReposity repository;
        private readonly IOrdemServicoRepository ordemServicoRepository;

        public UsuarioService(IUsuarioReposity _repository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        public UsuarioService(IUsuarioReposity _repository, IOrdemServicoRepository _ordemServicoRepository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            this.ordemServicoRepository = _ordemServicoRepository ?? throw new ArgumentNullException(nameof(_ordemServicoRepository));
        }

        public List<Usuario> ObterTodos()
        {
            return repository.ObterTodosUsuarios();
        }

        public List<Usuario> FiltrarUsuarios(string busca, bool apenasInativos, int nivel)
        {
            return repository.ObterComFiltros(busca, apenasInativos, nivel);
        }

        public Usuario ObterPorId(int id)
        {
            if (id <= 0) return null;
            return repository.ObterPorId(id);
        }

        public bool AlterarStatus(int id)
        {
            return repository.AlterarStatus(id);
        }

        public bool ExisteEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return repository.EmailExiste(email);
        }

        public (bool sucesso, string mensagem) AlterarSenha(string email, string novaSenha)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(novaSenha))
                {
                    return (false, "A nova senha não pode estar em branco.");
                }

                var usuario = repository.ObterPorEmail(email);

                if (usuario == null)
                {
                    return (false, "Usuário não encontrado.");
                }

                usuario.Senha = GerarHashSHA256(novaSenha);

                bool alterado = repository.AlterarSenha(usuario);

                if (alterado)
                {
                    return (true, "Senha alterada com sucesso!");
                }

                return (false, "Não foi possível atualizar a senha no banco de dados.");
            }
            catch (Exception ex)
            {
                
                return (false, "Ocorreu um erro interno ao tentar alterar a senha. "+ex.Message);
            }
        }

        public (bool sucesso, string mensagem, Usuario usuario) RealizarLogin(string cpf, string senha)
        {
            if (string.IsNullOrWhiteSpace(cpf) || string.IsNullOrWhiteSpace(senha))
            {
                return (false, "Por favor, preencha o CPF e a senha.", null);
            }

            string cpfLimpo = cpf.Replace(".", "").Replace("-", "").Trim();
            if (cpfLimpo.Length != 11)
            {
                return (false, "O CPF digitado é inválido. Certifique-se de digitar os 11 dígitos.", null);
            }

            try
            {
                Usuario usuario = repository.ObterPorCpf(cpfLimpo);

                if (usuario == null)
                {
                    return (false, "CPF ou senha inválidos.", null);
                }

                if (!usuario.Status.Equals("Ativado", StringComparison.OrdinalIgnoreCase))
                {
                    return (false, "Este usuário está desativado. Entre em contato com o administrador.", null);
                }

                string senhaHashDigitada = GerarHashSHA256(senha);
                if (usuario.Senha != senhaHashDigitada)
                {
                    return (false, "CPF ou senha inválidos.", null);
                }

                return (true, $"Bem-vindo de volta, {usuario.Nome}!", usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno ao processar o login: " + ex.Message, ex);
            }
        }

        public (bool sucesso, string mensagem) ValidarAntesDeDesativar(int id, int idUsuarioLogado)
        {
            if (id <= 0)
            {
                return (false, "Selecione um usuário válido para realizar a exclusão.");
            }

            if (idUsuarioLogado == id)
            {
                return (false, "Você não pode desativar a sua própria conta logada no sistema.");
            }

            bool possuiOsAberta = ordemServicoRepository.ExisteOSAbertaPorTecnico(id);
            if (possuiOsAberta)
            {
                return (false, "Não é possível desativar este usuário pois ele possui Ordens de Serviço em ABERTA.");
            }

            return (true, string.Empty);
        }

        public (bool sucesso, string messagem) CadastrarUsuario(Usuario usuario)
        {
            if (usuario == null) 
                return (false, "Dados do usuário inválidos.");

            if (string.IsNullOrWhiteSpace(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Senha))
                return (false, "Campos obrigatórios não preenchidos.");

            if (!Validacao.ValidarCPF(usuario.Cpf))
                return (false, "Formato do CPF inválido!");

            if (!Validacao.ValidarTelefone(usuario.Telefone))
                return (false, "Formato do telefone inválido");

            if (!Validacao.ValidarEmail(usuario.Email))
                return (false, "Formato do Email inválido");

            if (repository.CpfExiste(usuario.Cpf))
            {
                return (false, "O CPF informado já está cadastrado no sistema.");
            }

            if (repository.EmailExiste(usuario.Email))
            {
                return (false, "E-mail já cadastrado");
            }

            usuario.Senha = GerarHashSHA256(usuario.Senha);

            bool inserirUsuario = repository.InserirUsuario(usuario);
            if (inserirUsuario)
            {
                return (true, "Usuário cadastrado com sucesso!");
            }

            return (false, "Erro interno ao tentar salvar o usuário.");
        }

        public (bool sucesso, string mensagem) EditarUsuario(Usuario usuario)
        {
            if (usuario == null || usuario.Id <= 0) 
                return (false, "Dados do usuário inválidos para edição.");

            if (string.IsNullOrWhiteSpace(usuario.Nome))
                return (false, "O nome do usuário não pode ficar vazio.");

            try
            {
                Usuario usuarioBanco = repository.ObterPorId(usuario.Id);
                if (usuarioBanco == null)
                {
                    return (false, "Usuário não localizado no banco de dados para edição.");
                }

                if (string.IsNullOrWhiteSpace(usuario.Senha))
                {
                    usuario.Senha = usuarioBanco.Senha;
                }
                else 
                {
                    usuario.Senha = GerarHashSHA256(usuario.Senha);
                }

                bool atualizou = repository.AtualizarUsuario(usuario);
                if (atualizou)
                {
                    return (true, "Usuário atualizado com sucesso!");
                }

                return (false, "Erro interno ao tentar atualizar o usuário.");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar a edição do usuário: " + ex.Message, ex);
            }
        }

        public async Task<(bool sucesso, string cidade, string rua, string bairro, string estado)> ConsultarCepAsync(string cep)
        {
            try
            {
                BuscaCEP buscador = new BuscaCEP();
                buscador.Cep = cep;

                var ds = await Task.Run(() => buscador.Consultar());

                if (ds != null && !string.IsNullOrWhiteSpace(buscador.Cidade))
                {
                    return (true, buscador.Cidade, buscador.Rua, buscador.Bairro, buscador.Estado);
                }

                return (false, null, null, null, null);
            }
            catch
            {
                return (false, null, null, null, null);
            }
        }

        private string GerarHashSHA256(string senhaTextoClaro)
        {
            if (string.IsNullOrEmpty(senhaTextoClaro)) return string.Empty;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytesOriginal = Encoding.UTF8.GetBytes(senhaTextoClaro);
                byte[] bytesHash = sha256Hash.ComputeHash(bytesOriginal);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytesHash.Length; i++)
                {
                    builder.Append(bytesHash[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public void GerarRelatorioUsuariosPdf(string nome, bool apenasInativos, int nivel, string caminhoDestino)
        {
            try
            {
                List<Usuario> usuariosFiltrados = repository.ObterComFiltros(nome, apenasInativos, nivel);

                UsuarioDTO.UsuariosRelatorioDTO relatorio = new UsuarioDTO.UsuariosRelatorioDTO
                {
                    FiltroNome = string.IsNullOrEmpty(nome) ? "Todos" : nome,
                    FiltroNivel = nivel == 0 ? "Todos" : ObterDescricaoNivel(nivel),
                    FiltroStatus = apenasInativos ? "Apenas Desativado" : "Todos (Ativado/Desativado)",
                    TotalAtivos = 0,
                    TotalInativos = 0,
                    Itens = new List<UsuarioDTO.UsuarioRelatorioDTO>()
                };

                foreach (var usuario in usuariosFiltrados)
                {
                    bool inativo = usuario.Status.Equals("Desativado", StringComparison.OrdinalIgnoreCase);

                    if (inativo)
                    {
                        relatorio.TotalInativos++;
                    }
                    else
                    {
                        relatorio.TotalAtivos++;
                    }

                    relatorio.Itens.Add(new UsuarioDTO.UsuarioRelatorioDTO
                    {
                        Id = usuario.Id,
                        Nome = usuario.Nome,
                        Cpf = usuario.Cpf,
                        Telefone = usuario.Telefone,
                        Nivel = usuario.Nivel,
                        Status = usuario.Status,
                        Cidade = usuario.Cidade,
                        Estado = usuario.Estado
                    });
                }

                ExecutarGeracaoPdfUsuarios(relatorio, caminhoDestino);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao gerar o relatório de usuários em PDF.", ex);
            }
        }

        public DataTable obterHistoricoOs(int id)
        {
            return ordemServicoRepository.ObterHistoricoUsuario(id);
        }

        public List<Usuario> obterTodosTecnicos()
        {
            try
            {
                return repository.ObterTodosTecnicosAtivados();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void ExecutarGeracaoPdfUsuarios(UsuarioDTO.UsuariosRelatorioDTO dados, string caminhoDestino)
        {
            Document doc = new Document(PageSize.A4, 36, 36, 36, 36);

            using (FileStream fs = new FileStream(caminhoDestino, FileMode.Create))
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                BaseFont bfRegular = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                BaseFont bfBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                Font fontTitulo = new Font(bfBold, 20, Font.NORMAL, new BaseColor(26, 54, 93));
                Font fontSubtitulo = new Font(bfRegular, 10, Font.NORMAL, new BaseColor(74, 85, 104));
                Font fontMeta = new Font(bfRegular, 9, Font.NORMAL, new BaseColor(113, 128, 150));
                Font fontSecao = new Font(bfBold, 12, Font.NORMAL, new BaseColor(43, 108, 176));
                Font fontBold = new Font(bfBold, 9, Font.NORMAL, new BaseColor(45, 55, 72));
                Font fontRegular = new Font(bfRegular, 9, Font.NORMAL, new BaseColor(45, 55, 72));
                Font fontHeaderTabela = new Font(bfBold, 9, Font.NORMAL, BaseColor.WHITE);

                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 60f, 40f });

                PdfPCell cellLeft = new PdfPCell();
                cellLeft.Border = PdfPCell.NO_BORDER;
                cellLeft.AddElement(new Paragraph("AssisTEC", fontTitulo));
                cellLeft.AddElement(new Paragraph("Relatório de Controle de Usuários", fontSubtitulo));
                headerTable.AddCell(cellLeft);

                PdfPCell cellRight = new PdfPCell();
                cellRight.Border = PdfPCell.NO_BORDER;
                Paragraph pMeta = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\nUsuário: Logado", fontMeta);
                pMeta.Alignment = Element.ALIGN_RIGHT;
                cellRight.AddElement(pMeta);
                headerTable.AddCell(cellRight);

                doc.Add(headerTable);

                Paragraph linhaDivisoria = new Paragraph(new Chunk(new LineSeparator(2f, 100f, new BaseColor(43, 108, 176), Element.ALIGN_CENTER, -1f)));
                linhaDivisoria.SpacingAfter = 15f;
                doc.Add(linhaDivisoria);

                doc.Add(new Paragraph("FILTROS APLICADOS", fontSecao));

                PdfPTable filterTable = new PdfPTable(4);
                filterTable.WidthPercentage = 100;
                filterTable.SetWidths(new float[] { 15f, 35f, 15f, 35f });
                filterTable.SpacingBefore = 5f;
                filterTable.SpacingAfter = 15f;

                string[,] filtros = {
                    { "Nome:", dados.FiltroNome, "Nível:", dados.FiltroNivel },
                    { "Status:", dados.FiltroStatus, "", "" }
                };

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i == 1 && j >= 2)
                        {
                            PdfPCell emptyCell = new PdfPCell(new Phrase("", fontRegular));
                            emptyCell.Border = PdfPCell.NO_BORDER;
                            filterTable.AddCell(emptyCell);
                            continue;
                        }

                        bool isLabel = j % 2 == 0;
                        PdfPCell cell = new PdfPCell(new Phrase(filtros[i, j], isLabel ? fontBold : fontRegular));
                        cell.BackgroundColor = new BaseColor(247, 250, 252);
                        cell.BorderColor = new BaseColor(237, 242, 247);
                        cell.Padding = 6;
                        filterTable.AddCell(cell);
                    }
                }
                doc.Add(filterTable);

                doc.Add(new Paragraph("RESUMO GERAL", fontSecao));

                PdfPTable summaryTable = new PdfPTable(3);
                summaryTable.WidthPercentage = 100;
                summaryTable.SetWidths(new float[] { 33.33f, 33.33f, 33.33f });
                summaryTable.SpacingBefore = 5f;
                summaryTable.SpacingAfter = 15f;

                summaryTable.AddCell(CriarCardResumo("USUÁRIOS ATIVOS", dados.TotalAtivos.ToString(), new BaseColor(56, 161, 105), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("USUÁRIOS INATIVOS", dados.TotalInativos.ToString(), new BaseColor(229, 62, 98), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("TOTAL GERAL", dados.TotalGeral.ToString(), new BaseColor(74, 85, 104), fontMeta, fontTitulo));

                doc.Add(summaryTable);

                doc.Add(new Paragraph("DETALHAMENTO DOS USUÁRIOS", fontSecao));

                PdfPTable dataTable = new PdfPTable(8);
                dataTable.WidthPercentage = 100;
                dataTable.SetWidths(new float[] { 6f, 24f, 13f, 13f, 12f, 12f, 12f, 8f });
                dataTable.SpacingBefore = 5f;

                string[] headers = { "ID", "Nome", "CPF", "Telefone", "Nível", "Cidade", "Estado", "Status" };
                foreach (var header in headers)
                {
                    PdfPCell hCell = new PdfPCell(new Phrase(header, fontHeaderTabela));
                    hCell.BackgroundColor = new BaseColor(26, 54, 93);
                    hCell.BorderColor = new BaseColor(26, 54, 93);
                    hCell.Padding = 6;
                    dataTable.AddCell(hCell);
                }

                foreach (var item in dados.Itens)
                {
                    dataTable.AddCell(new PdfPCell(new Phrase(item.Id.ToString(), fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                    dataTable.AddCell(new PdfPCell(new Phrase(item.Nome, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                    dataTable.AddCell(new PdfPCell(new Phrase(item.Cpf, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                    dataTable.AddCell(new PdfPCell(new Phrase(item.Telefone, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                    dataTable.AddCell(new PdfPCell(new Phrase(ObterDescricaoNivel(item.Nivel), fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                    dataTable.AddCell(new PdfPCell(new Phrase(item.Cidade, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                    dataTable.AddCell(new PdfPCell(new Phrase(item.Estado, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });

                    bool ativo = item.Status.Equals("Ativado", StringComparison.OrdinalIgnoreCase) || item.Status.Equals("Ativo", StringComparison.OrdinalIgnoreCase);
                    PdfPCell statusCell = new PdfPCell(new Phrase(item.Status, fontBold));
                    statusCell.BackgroundColor = ativo ? new BaseColor(198, 246, 213) : new BaseColor(254, 215, 215);
                    statusCell.Padding = 6;
                    statusCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    statusCell.BorderColor = new BaseColor(226, 232, 240);
                    dataTable.AddCell(statusCell);
                }

                doc.Add(dataTable);
                doc.Close();
            }
        }

        public void GerarRelatorioIndividualPdf(int idUsuario, string caminhoDestino)
        {
            try
            {
                Usuario usuario = repository.ObterPorId(idUsuario);
                if (usuario == null)
                {
                    throw new Exception("Usuário não encontrado para a geração do relatório.");
                }

                DataTable tabelaOS = ordemServicoRepository.ObterHistoricoUsuario(idUsuario);

                UsuarioDTO.UsuarioComOrdemServicoDTO relatorio = new UsuarioDTO.UsuarioComOrdemServicoDTO
                {
                    IdUsuario = usuario.Id,
                    Nome = usuario.Nome ?? string.Empty,
                    Cpf = usuario.Cpf ?? string.Empty,
                    Telefone = usuario.Telefone ?? string.Empty,
                    NivelDescricao = ObterDescricaoNivel(usuario.Nivel),
                    StatusUsuario = usuario.Status ?? string.Empty,
                    TotalOrdens = 0,
                    OrdensAbertas = 0,
                    OrdensFinalizadas = 0,
                    FaturamentoGerado = 0,
                    Ordens = new List<UsuarioDTO.OrdemServicoItemDTO>()
                };

                if (tabelaOS != null && tabelaOS.Rows.Count > 0)
                {
                    foreach (DataRow row in tabelaOS.Rows)
                    {
                        string statusOS = row["STATUS"] != DBNull.Value ? row["STATUS"].ToString() : "ABERTA";
                        decimal valor = row["VALOR_TOTAL"] != DBNull.Value ? Convert.ToDecimal(row["VALOR_TOTAL"]) : 0m;

                        DateTime? dataFim = null;
                        if (row["DATA_FECHAMENTO"] != DBNull.Value)
                        {
                            dataFim = Convert.ToDateTime(row["DATA_FECHAMENTO"]);
                        }

                        relatorio.TotalOrdens++;

                        if (statusOS.Equals("ABERTA", StringComparison.OrdinalIgnoreCase) || 
                            statusOS.Equals("Aberto", StringComparison.OrdinalIgnoreCase) || 
                            statusOS.Equals("Em Andamento", StringComparison.OrdinalIgnoreCase))
                        {
                            relatorio.OrdensAbertas++;
                        }
                        else if (statusOS.Equals("Finalizado", StringComparison.OrdinalIgnoreCase) || 
                                 statusOS.Equals("Entregue", StringComparison.OrdinalIgnoreCase))
                        {
                            relatorio.OrdensFinalizadas++;
                            relatorio.FaturamentoGerado += valor;
                        }

                        relatorio.Ordens.Add(new UsuarioDTO.OrdemServicoItemDTO
                        {
                            IdOrdemServico = row["ID_ORDEM"] != DBNull.Value ? Convert.ToInt32(row["ID_ORDEM"]) : 0,
                            Cliente = row["CLIENTE"] != DBNull.Value ? row["CLIENTE"].ToString() : "Sem Cliente",
                            Equipamento = row["EQUIPAMENTO"] != DBNull.Value ? row["EQUIPAMENTO"].ToString() : "Sem Equipamento",
                            DataAbertura = row["DATA_ABERTURA"] != DBNull.Value ? Convert.ToDateTime(row["DATA_ABERTURA"]) : DateTime.Now,
                            DataFechamento = dataFim,
                            ValorTotal = valor,
                            Status = statusOS
                        });
                    }
                }

                ExecutarGeracaoPdfIndividual(relatorio, caminhoDestino);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar o relatório individual do usuário: " + ex.Message, ex);
            }
        }

        private string ObterDescricaoNivel(int nivel)
        {
            switch (nivel)
            {
                case 1: return "Gerente";
                case 2: return "Técnico";
                case 3: return "Atendente";
                default: return $"Nível {nivel}";
            }
        }

        private void ExecutarGeracaoPdfIndividual(UsuarioDTO.UsuarioComOrdemServicoDTO dados, string caminhoDestino)
        {
            try
            {
                Document doc = new Document(PageSize.A4, 36, 36, 36, 36);

                using (FileStream fs = new FileStream(caminhoDestino, FileMode.Create))
                {
                    PdfWriter writer = PdfWriter.GetInstance(doc, fs);
                    doc.Open();

                    BaseFont bfRegular = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                    BaseFont bfBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                    Font fontTitulo = new Font(bfBold, 18, Font.NORMAL, new BaseColor(26, 54, 93));
                    Font fontSubtitulo = new Font(bfRegular, 10, Font.NORMAL, new BaseColor(74, 85, 104));
                    Font fontMeta = new Font(bfRegular, 9, Font.NORMAL, new BaseColor(113, 128, 150));
                    Font fontSecao = new Font(bfBold, 11, Font.NORMAL, new BaseColor(43, 108, 176));
                    Font fontBold = new Font(bfBold, 9, Font.NORMAL, new BaseColor(45, 55, 72));
                    Font fontRegular = new Font(bfRegular, 9, Font.NORMAL, new BaseColor(45, 55, 72));
                    Font fontHeaderTabela = new Font(bfBold, 9, Font.NORMAL, BaseColor.WHITE);

                    PdfPTable headerTable = new PdfPTable(2);
                    headerTable.WidthPercentage = 100;
                    headerTable.SetWidths(new float[] { 60f, 40f });

                    PdfPCell cellLeft = new PdfPCell();
                    cellLeft.Border = PdfPCell.NO_BORDER;
                    cellLeft.AddElement(new Paragraph("AssisTEC", fontTitulo));
                    cellLeft.AddElement(new Paragraph("Relatório de Produtividade do Usuário", fontSubtitulo));
                    headerTable.AddCell(cellLeft);

                    PdfPCell cellRight = new PdfPCell();
                    cellRight.Border = PdfPCell.NO_BORDER;
                    Paragraph pMeta = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\nExportado por: Sistema", fontMeta);
                    pMeta.Alignment = Element.ALIGN_RIGHT;
                    cellRight.AddElement(pMeta);
                    headerTable.AddCell(cellRight);

                    doc.Add(headerTable);

                    Paragraph linhaDivisoria = new Paragraph(new Chunk(new LineSeparator(2f, 100f, new BaseColor(43, 108, 176), Element.ALIGN_CENTER, -1f)));
                    linhaDivisoria.SpacingAfter = 12f;
                    doc.Add(linhaDivisoria);

                    doc.Add(new Paragraph("DADOS DO USUÁRIO", fontSecao));

                    PdfPTable infoTable = new PdfPTable(4);
                    infoTable.WidthPercentage = 100;
                    infoTable.SetWidths(new float[] { 15f, 35f, 15f, 35f });
                    infoTable.SpacingBefore = 4f;
                    infoTable.SpacingAfter = 15f;

                    string[,] infoCampos = {
                        { "Nome:", dados.Nome, "CPF:", dados.Cpf },
                        { "Cargo/Nível:", dados.NivelDescricao, "Telefone:", dados.Telefone },
                        { "Situação:", dados.StatusUsuario, "Código:", dados.IdUsuario.ToString() }
                    };

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            bool isLabel = j % 2 == 0;
                            PdfPCell cell = new PdfPCell(new Phrase(infoCampos[i, j], isLabel ? fontBold : fontRegular));
                            cell.BackgroundColor = new BaseColor(247, 250, 252);
                            cell.BorderColor = new BaseColor(237, 242, 247);
                            cell.Padding = 6;
                            infoTable.AddCell(cell);
                        }
                    }
                    doc.Add(infoTable);

                    doc.Add(new Paragraph("MÉTRICAS E DESEMPENHO", fontSecao));

                    PdfPTable summaryTable = new PdfPTable(4);
                    summaryTable.WidthPercentage = 100;
                    summaryTable.SetWidths(new float[] { 25f, 25f, 25f, 25f });
                    summaryTable.SpacingBefore = 4f;
                    summaryTable.SpacingAfter = 15f;

                    summaryTable.AddCell(CriarCardResumo("ORDENS ATRIBUÍDAS", dados.TotalOrdens.ToString(), new BaseColor(74, 85, 104), fontMeta, fontTitulo));
                    summaryTable.AddCell(CriarCardResumo("EM ANDAMENTO", dados.OrdensAbertas.ToString(), new BaseColor(237, 137, 54), fontMeta, fontTitulo));
                    summaryTable.AddCell(CriarCardResumo("FINALIZADAS", dados.OrdensFinalizadas.ToString(), new BaseColor(56, 161, 105), fontMeta, fontTitulo));
                    summaryTable.AddCell(CriarCardResumo("VALOR PRODUZIDO", dados.FaturamentoGerado.ToString("C2"), new BaseColor(49, 130, 206), fontMeta, fontTitulo));

                    doc.Add(summaryTable);

                    doc.Add(new Paragraph("ORDENS DE SERVIÇO SOB RESPONSABILIDADE", fontSecao));

                    PdfPTable dataTable = new PdfPTable(7);
                    dataTable.WidthPercentage = 100;
                    dataTable.SetWidths(new float[] { 8f, 24f, 24f, 11f, 11f, 12f, 10f });
                    dataTable.SpacingBefore = 4f;

                    string[] headers = { "Nº OS", "Cliente", "Equipamento", "Abertura", "Fechamento", "Valor", "Status" };
                    foreach (var header in headers)
                    {
                        PdfPCell hCell = new PdfPCell(new Phrase(header, fontHeaderTabela));
                        hCell.BackgroundColor = new BaseColor(26, 54, 93);
                        hCell.BorderColor = new BaseColor(26, 54, 93);
                        hCell.Padding = 6;
                        dataTable.AddCell(hCell);
                    }

                    foreach (var item in dados.Ordens)
                    {
                        dataTable.AddCell(new PdfPCell(new Phrase(item.IdOrdemServico.ToString(), fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Cliente, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Equipamento, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.DataAbertura.ToString("dd/MM/yyyy"), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240) });

                        string dataFechamentoText = item.DataFechamento.HasValue ? item.DataFechamento.Value.ToString("dd/MM/yyyy") : "-";
                        dataTable.AddCell(new PdfPCell(new Phrase(dataFechamentoText, fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240) });

                        dataTable.AddCell(new PdfPCell(new Phrase(item.ValorTotal.ToString("C2"), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });

                        bool finalizada = item.Status.Equals("Finalizado", StringComparison.OrdinalIgnoreCase) || item.Status.Equals("Entregue", StringComparison.OrdinalIgnoreCase);
                        PdfPCell statusCell = new PdfPCell(new Phrase(item.Status, fontBold));
                        statusCell.BackgroundColor = finalizada ? new BaseColor(198, 246, 213) : new BaseColor(254, 215, 215);
                        statusCell.Padding = 6;
                        statusCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        statusCell.BorderColor = new BaseColor(226, 232, 240);
                        dataTable.AddCell(statusCell);
                    }

                    doc.Add(dataTable);
                    doc.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar relatório: " + e.Message, e);
            }
        }

        private PdfPCell CriarCardResumo(string titulo, string valor, BaseColor corBordaTop, Font fTitulo, Font fValor)
        {
            PdfPCell cell = new PdfPCell();
            cell.BackgroundColor = new BaseColor(247, 250, 252);
            cell.BorderColor = new BaseColor(237, 242, 247);
            cell.BorderWidthTop = 3f;
            cell.BorderColorTop = corBordaTop;
            cell.Padding = 8;

            Paragraph pT = new Paragraph(titulo, fTitulo);
            pT.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(pT);

            Paragraph pV = new Paragraph(valor, fValor);
            pV.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(pV);

            return cell;
        }
    }
}
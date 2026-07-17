using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using AssisTEC.DTO;
using AssisTec.Models;
using AssisTec.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace AssisTec.Service
{
    public class ClienteService
    {
        private readonly IClienteRepository repository;
        private readonly IOrdemServicoRepository ordemServicoRepository;

        public ClienteService(IClienteRepository _repository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }
        public ClienteService(IClienteRepository _repository,  IOrdemServicoRepository _ordemServicoRepository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            this.ordemServicoRepository  = _ordemServicoRepository ?? throw new ArgumentNullException(nameof(_ordemServicoRepository));
        }
        
        

        public List<Cliente> ObterTodos()
        {
            return repository.ObterTodosClientes();
        }

        public List<Cliente> FiltrarClientes(string busca)
        {
            return repository.ObterComFiltros(busca);
        }

        public Cliente ObterPorId(int id)
        {
            if (id < 0) return null;
            return repository.ObterPorId(id);
        }

        public (bool sucesso, string mensagem) CadastrarCliente(Cliente cliente)
        {
            if (cliente == null)
                return (false, "Dados do cliente inválidos.");

            if (string.IsNullOrWhiteSpace(cliente.Nome) || string.IsNullOrWhiteSpace(cliente.Cpf))
            {
                return (false, "Campos obrigatórios não preenchidos.");
            }
                

            if (!Validacao.ValidarCPF(cliente.Cpf))
            {
                return (false, "CPF inválido");
            }

            if (!Validacao.ValidarTelefone(cliente.Telefone))
            {
                return (false, "Telefone inválido");
            }
            
            if (!cliente.DataNascimento.HasValue)
            {
                return (false, "Data de nascimento é obrigatório");
            }
            
            var (dataValida, mensagemData) = Validacao.ValidarData(cliente.DataNascimento.Value);
            if (!dataValida)
            {
                return (false, mensagemData);
            }
            
            if (repository.CpfExiste(cliente.Cpf))
            {
                return (false, "O CPF informado já está cadastrado no sistema.");
            }

            bool inserirCliente = repository.InserirCliente(cliente);
            if (inserirCliente)
            {
                return (true, "Cliente cadastrado com sucesso!");
            }

            return (false, "Erro interno ao tentar salvar o cliente.");
        }

        public (bool sucesso, string mensagem) EditarCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nome) || string.IsNullOrWhiteSpace(cliente.Cpf))
            {
                return (false, "Campos obrigatórios não preenchidos.");
            }
                

            if (!Validacao.ValidarCPF(cliente.Cpf))
            {
                return (false, "CPF inválido");
            }

            if (!Validacao.ValidarTelefone(cliente.Telefone))
            {
                return (false, "Telefone inválido");
            }
            
            if (!cliente.DataNascimento.HasValue)
            {
                return (false, "Data de nascimento é obrigatório");
            }
            
            var (dataValida, mensagemData) = Validacao.ValidarData(cliente.DataNascimento.Value);
            if (!dataValida)
            {
                return (false, mensagemData);
            }
            
            if (repository.CpfExiste(cliente.Cpf) && cliente.Cpf != cliente.Cpf)
            {
                return (false, "O CPF informado já está cadastrado no sistema.");
            }

            bool atualizou = repository.AtualizarCliente(cliente);
            if (atualizou)
            {
                return (true, "Cliente atualizado com sucesso!");
            }

            return (false, "Erro interno ao tentar atualizar o cliente.");
        }

        public (bool podeExcluir, string mensagem) ValidarExclusao(int id)
        {
            if (id <= 0)
            {
                return (false, "Selecione um cliente válido para exclusão.");
            }

            return (true, string.Empty);
        }

        public bool DeletarCliente(int id)
        {
            if (id <= 0) return false;
            return repository.ExcluirCliente(id);
        }
        
        public (bool sucesso, string mensagem, string rua, string bairro, string cidade, string estado) ConsultarCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
            {
                return (false, "O CEP não pode estar vazio.", null, null, null, null);
            }

            string cepLimpo = cep.Replace("-", "").Trim();
            if (cepLimpo.Length != 8)
            {
                return (false, "Formato de CEP inválido. Certifique-se de que possui 8 dígitos.", null, null, null, null);
            }

            try
            {
                BuscaCEP buscaCep = new BuscaCEP();
                buscaCep.Cep = cepLimpo;
                buscaCep.Consultar();

                if (string.IsNullOrWhiteSpace(buscaCep.Cidade) || 
                    string.IsNullOrWhiteSpace(buscaCep.Rua) || 
                    string.IsNullOrWhiteSpace(buscaCep.Bairro))
                {
                    return (false, "Falha ao localizar as informações do CEP informado.", null, null, null, null);
                }

                return (true, "CEP localizado com sucesso!", buscaCep.Rua, buscaCep.Bairro, buscaCep.Cidade, buscaCep.Estado);
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao consultar o CEP: {ex.Message}", null, null, null, null);
            }
        }

        public DataTable ObterHistoricoOS(int id)
        {
            return ordemServicoRepository.ObterHistoricoCliente(id);
        }

        public bool AlterarStatus(int id)
        {
            try
            {
                return repository.AlterarStatus(id);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao alterar o status do cliente.", e);
            }
        }

        public List<Cliente> ObterComFiltros(string busca, bool ApenasInativos)
        {
            try
            {
                return repository.ObterComFiltros(busca, ApenasInativos);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao obter os clientes.", e);
            }
        }
        
        public void GerarRelatorioClientesPdf(string nome, bool exibirDesativados, string caminhoDestino)
        {
            try
            {
                List<Cliente> clientesFiltrados = repository.ObterComFiltros(nome, exibirDesativados);

                ClienteDTO.ClientesRelatorioDTO relatorio = new ClienteDTO.ClientesRelatorioDTO
                {
                    FiltroNome = string.IsNullOrEmpty(nome) ? "Todos" : nome,
                    FiltroStatus = exibirDesativados ? "Todos (Ativados/Desativados)" : "Apenas Ativados",
                    TotalAtivos = 0,
                    TotalInativos = 0,
                    TotalGeral = 0,
                    Itens = new List<ClienteDTO.ClienteRelatorioDTO>()
                };

                foreach (var cliente in clientesFiltrados)
                {
                    bool inativo = cliente.Status.Equals("Desativado", StringComparison.OrdinalIgnoreCase);
                    
                    if (inativo)
                    {
                        relatorio.TotalInativos++;
                    }
                    else
                    {
                        relatorio.TotalAtivos++;
                    }
                    relatorio.TotalGeral++;

                    relatorio.Itens.Add(new ClienteDTO.ClienteRelatorioDTO
                    {
                        Id = cliente.Id,
                        Nome = cliente.Nome,
                        Cpf = cliente.Cpf,
                        Telefone = cliente.Telefone,
                        Cidade = cliente.Cidade,
                        Estado = cliente.Estado,
                        Status = cliente.Status
                    });
                }

                ExecutarGeracaoPdfClientes(relatorio, caminhoDestino);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao gerar o relatório de clientes em PDF.", ex);
            }
        }

        public DataTable ObterHistoricoOsCliente(int id)
        {
            return ordemServicoRepository.ObterHistoricoCliente(id);
        }

        private void ExecutarGeracaoPdfClientes(ClienteDTO.ClientesRelatorioDTO dados, string caminhoDestino)
        {
            Document doc = new Document(PageSize.A4, 36, 36, 36, 36);
            FileStream fs = null;
            try
            {
                fs = new FileStream(caminhoDestino, FileMode.Create);
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
                cellLeft.AddElement(new Paragraph("Relatório Geral de Clientes", fontSubtitulo));
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

                PdfPTable filterTable = new PdfPTable(2);
                filterTable.WidthPercentage = 100;
                filterTable.SetWidths(new float[] { 20f, 80f });
                filterTable.SpacingBefore = 5f;
                filterTable.SpacingAfter = 15f;

                string[,] filtros = {
                    { "Nome:", dados.FiltroNome },
                    { "Status:", dados.FiltroStatus }
                };

                for (int i = 0; i < 2; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        bool isLabel = j == 0;
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

                summaryTable.AddCell(CriarCardResumo("CLIENTES ATIVOS", dados.TotalAtivos.ToString(), new BaseColor(56, 161, 105), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("CLIENTES INATIVOS", dados.TotalInativos.ToString(), new BaseColor(229, 62, 98), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("TOTAL GERAL", dados.TotalGeral.ToString(), new BaseColor(74, 85, 104), fontMeta, fontTitulo));

                doc.Add(summaryTable);

                doc.Add(new Paragraph("DETALHAMENTO DOS CLIENTES", fontSecao));

                PdfPTable dataTable = new PdfPTable(7);
                dataTable.WidthPercentage = 100;
                dataTable.SetWidths(new float[] { 6f, 26f, 16f, 14f, 18f, 12f, 8f });
                dataTable.SpacingBefore = 5f;

                string[] headers = { "ID", "Nome", "CPF", "Telefone", "Cidade", "Estado", "Status" };
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
                    dataTable.AddCell(new PdfPCell(new Phrase(item.Cidade, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                    dataTable.AddCell(new PdfPCell(new Phrase(item.Estado, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });

                    bool ativo = item.Status.Equals("Ativado", StringComparison.OrdinalIgnoreCase);
                    PdfPCell statusCell = new PdfPCell(new Phrase(item.Status, fontBold));
                    statusCell.BackgroundColor = ativo ? new BaseColor(198, 246, 213) : new BaseColor(254, 215, 215);
                    statusCell.Padding = 6;
                    statusCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    statusCell.BorderColor = new BaseColor(226, 232, 240);
                    dataTable.AddCell(statusCell);
                }

                doc.Add(dataTable);
            }
            finally
            {
                if (doc.IsOpen()) doc.Close();
                if (fs != null) fs.Dispose();
            }
        }

        public void GerarRelatorioIndividualClientePdf(int idCliente, string caminhoDestino)
        {
            try
            {
                Cliente cliente = repository.ObterPorId(idCliente);
                if (cliente == null)
                {
                    throw new Exception("Cliente não encontrado para a geração do relatório.");
                }

                DataTable tabelaOS = ordemServicoRepository.ObterHistoricoCliente(idCliente);

                ClienteDTO.ClienteComOrdemServicoDTO relatorio = new ClienteDTO.ClienteComOrdemServicoDTO
                {
                    IdCliente = cliente.Id,
                    Nome = cliente.Nome ?? string.Empty,
                    Cpf = cliente.Cpf ?? string.Empty,
                    Telefone = cliente.Telefone ?? string.Empty,
                    StatusCliente = cliente.Status ?? string.Empty,
                    TotalOrdens = 0,
                    OrdensAbertas = 0,
                    OrdensFinalizadas = 0,
                    TotalGasto = 0m,
                    Ordens = new List<ClienteDTO.OrdemServicoItemDTO>()
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
                            relatorio.TotalGasto += valor;
                        }

                        relatorio.Ordens.Add(new ClienteDTO.OrdemServicoItemDTO
                        {
                            IdOrdemServico = row["ID_ORDEM"] != DBNull.Value ? Convert.ToInt32(row["ID_ORDEM"]) : 0,
                            Tecnico = row["TECNICO"] != DBNull.Value ? row["TECNICO"].ToString() : "Não Atribuído",
                            Equipamento = row["EQUIPAMENTO"] != DBNull.Value ? row["EQUIPAMENTO"].ToString() : "Sem Equipamento",
                            DataAbertura = row["DATA_ABERTURA"] != DBNull.Value ? Convert.ToDateTime(row["DATA_ABERTURA"]) : DateTime.Now,
                            DataFechamento = dataFim,
                            ValorTotal = valor,
                            Status = statusOS
                        });
                    }
                }

                ExecutarGeracaoPdfIndividualCliente(relatorio, caminhoDestino);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar o relatório individual do cliente. " + ex.Message);
            }
        }

        private void ExecutarGeracaoPdfIndividualCliente(ClienteDTO.ClienteComOrdemServicoDTO dados, string caminhoDestino)
        {
            Document doc = new Document(PageSize.A4, 36, 36, 36, 36);
            FileStream fs = null;
            try
            {
                fs = new FileStream(caminhoDestino, FileMode.Create);
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
                cellLeft.AddElement(new Paragraph("Histórico Financeiro e de Serviços do Cliente", fontSubtitulo));
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

                doc.Add(new Paragraph("DADOS DO CLIENTE", fontSecao));

                PdfPTable infoTable = new PdfPTable(4);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 15f, 35f, 15f, 35f });
                infoTable.SpacingBefore = 4f;
                infoTable.SpacingAfter = 15f;

                string[,] infoCampos = {
                    { "Nome:", dados.Nome, "CPF:", dados.Cpf },
                    { "Telefone:", dados.Telefone, "Código:", dados.IdCliente.ToString() },
                    { "Situação:", dados.StatusCliente, "", "" }
                };

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (i == 2 && j >= 2)
                        {
                            PdfPCell emptyCell = new PdfPCell(new Phrase("", fontRegular));
                            emptyCell.Border = PdfPCell.NO_BORDER;
                            infoTable.AddCell(emptyCell);
                            continue;
                        }

                        bool isLabel = j % 2 == 0;
                        PdfPCell cell = new PdfPCell(new Phrase(infoCampos[i, j], isLabel ? fontBold : fontRegular));
                        cell.BackgroundColor = new BaseColor(247, 250, 252);
                        cell.BorderColor = new BaseColor(237, 242, 247);
                        cell.Padding = 6;
                        infoTable.AddCell(cell);
                    }
                }
                doc.Add(infoTable);

                doc.Add(new Paragraph("MÉTRICAS DE CONSUMO", fontSecao));

                PdfPTable summaryTable = new PdfPTable(4);
                summaryTable.WidthPercentage = 100;
                summaryTable.SetWidths(new float[] { 25f, 25f, 25f, 25f });
                summaryTable.SpacingBefore = 4f;
                summaryTable.SpacingAfter = 15f;

                summaryTable.AddCell(CriarCardResumo("TOTAL ORDENS", dados.TotalOrdens.ToString(), new BaseColor(74, 85, 104), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("EM ANDAMENTO", dados.OrdensAbertas.ToString(), new BaseColor(237, 137, 54), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("FINALIZADAS", dados.OrdensFinalizadas.ToString(), new BaseColor(56, 161, 105), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("TOTAL INVESTIDO", dados.TotalGasto.ToString("C2"), new BaseColor(49, 130, 206), fontMeta, fontTitulo));

                doc.Add(summaryTable);

                doc.Add(new Paragraph("HISTÓRICO DE ORDENS DE SERVIÇO", fontSecao));

                PdfPTable dataTable = new PdfPTable(7);
                dataTable.WidthPercentage = 100;
                dataTable.SetWidths(new float[] { 8f, 24f, 24f, 11f, 11f, 12f, 10f });
                dataTable.SpacingBefore = 4f;

                string[] headers = { "Nº OS", "Responsável", "Equipamento", "Abertura", "Fechamento", "Valor", "Status" };
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
                    dataTable.AddCell(new PdfPCell(new Phrase(item.Tecnico, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
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
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao gerar relatório " + e.Message);
            }
            finally
            {
                if (doc.IsOpen()) doc.Close();
                if (fs != null) fs.Dispose();
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
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AssisTec.Dtos;
using AssisTec.Models;
using AssisTec.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace AssisTec.Service
{
    public class MovimentacaoEstoqueService
    {
        private readonly IMovimentacaoEstoqueRepository _movimentacaoEstoqueRepository;

        public MovimentacaoEstoqueService(IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository)
        {
            this._movimentacaoEstoqueRepository = movimentacaoEstoqueRepository ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueRepository));
        }

        #region Consulta

        public object ListarMovimentacaoEstoque()
        {
            try
            {
                return _movimentacaoEstoqueRepository.ListarMovimentacaoEstoque();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }

        #endregion

        #region Gerenciamento
        public bool NovaMovimentacaoEstoque(MovimentacaoEstoque movimentacao)
        {
            try
            {
                return _movimentacaoEstoqueRepository.InserirMovimentacao(movimentacao);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        

        #endregion

        #region Filtro
        public object Filtrar(DateTime? dataInicio, DateTime? dataFim, string produtoSelecionado, string tipoMovimentacao)
        {
            return _movimentacaoEstoqueRepository.Filtrar(dataInicio, dataFim, produtoSelecionado,tipoMovimentacao);
        }
        

        #endregion

        #region Relatório

         public void GerarRelatorioPdf(DateTime? dataInicio, DateTime? dataFim, string produtoSelecionado, string tipoMovimentacao, string caminhoDestino)
        {
            try
            {
                var dadosFiltrados =
                    _movimentacaoEstoqueRepository.Filtrar(dataInicio, dataFim, produtoSelecionado, tipoMovimentacao) as
                        List<dynamic>;

                if (dadosFiltrados == null)
                {
                    var listaGenerica =
                        _movimentacaoEstoqueRepository.Filtrar(dataInicio, dataFim, produtoSelecionado,
                            tipoMovimentacao) as IEnumerable;
                    dadosFiltrados = new List<dynamic>();
                    if (listaGenerica != null)
                    {
                        foreach (var item in listaGenerica)
                        {
                            dadosFiltrados.Add(item);
                        }
                    }
                }

                var relatorio = new MovimentacoesEstoqueDTO();
                relatorio.ProdutoFiltro = string.IsNullOrEmpty(produtoSelecionado) ? "Todos" : produtoSelecionado;
                relatorio.TipoFiltro = string.IsNullOrEmpty(tipoMovimentacao) ? "Todos" : tipoMovimentacao;

                if (dataInicio.HasValue && dataFim.HasValue)
                {
                    relatorio.Periodo = $"{dataInicio.Value:dd/MM/yyyy} até {dataFim.Value:dd/MM/yyyy}";
                }
                else if (dataInicio.HasValue)
                {
                    relatorio.Periodo = $"A partir de {dataInicio.Value:dd/MM/yyyy}";
                }
                else if (dataFim.HasValue)
                {
                    relatorio.Periodo = $"Até {dataFim.Value:dd/MM/yyyy}";
                }
                else
                {
                    DateTime hoje = DateTime.Today;
                    relatorio.Periodo = $"Mês Atual ({hoje:MM/yyyy})";
                }

                relatorio.Itens = new List<MovimentacaoItemDTO>();
                relatorio.TotalEntradas = 0;
                relatorio.TotalSaidas = 0;

                foreach (var item in dadosFiltrados)
                {
                    string tipo = item.TipoMovimentacao;
                    int qtd = item.Quantidade;

                    if (tipo.ToLower().Contains("entrada"))
                    {
                        relatorio.TotalEntradas += qtd;
                    }
                    else if (tipo.ToLower().Contains("saída") || tipo.ToLower().Contains("saida"))
                    {
                        relatorio.TotalSaidas += qtd;
                    }

                    relatorio.Itens.Add(new MovimentacaoItemDTO
                    {
                        IdMovimentacao = item.ID_Movimentacao,
                        Produto = item.Produto,
                        Data = item.Data,
                        Quantidade = qtd,
                        Valor = item.Valor,
                        TipoMovimentacao = tipo,
                        Descricao = item.Descricao,
                        Registrado = item.Registrado
                    });
                }

                ExecutarGeracaoPdf(relatorio, caminhoDestino);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao processar e gerar o relatório em PDF.", ex);
            }
        }

        private void ExecutarGeracaoPdf(MovimentacoesEstoqueDTO dados, string caminhoDestino)
        {
            Document doc = new Document(PageSize.A4, 36, 36, 36, 36);
            PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminhoDestino, FileMode.Create));

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
            cellLeft.AddElement(new Paragraph("Relatório de Movimentação de Estoque", fontSubtitulo));
            headerTable.AddCell(cellLeft);

            PdfPCell cellRight = new PdfPCell();
            cellRight.Border = PdfPCell.NO_BORDER;
            Paragraph pMeta = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\nUsuário: Logado", fontMeta);
            pMeta.Alignment = Element.ALIGN_RIGHT;
            cellRight.AddElement(pMeta);
            headerTable.AddCell(cellRight);

            doc.Add(headerTable);

            Paragraph linhaDivisoria =
                new Paragraph(new Chunk(new LineSeparator(2f, 100f, new BaseColor(43, 108, 176), Element.ALIGN_CENTER,
                    -1f)));
            linhaDivisoria.SpacingAfter = 15f;
            doc.Add(linhaDivisoria);

            PdfPTable filterTable = new PdfPTable(4);
            filterTable.WidthPercentage = 100;
            filterTable.SetWidths(new float[] { 18f, 32f, 18f, 32f });
            filterTable.SpacingBefore = 5f;
            filterTable.SpacingAfter = 15f;

            string[,] filtros =
            {
                { "Período:", dados.Periodo, "Produto:", dados.ProdutoFiltro },
                { "Tipo de Mov.:", dados.TipoFiltro, "Status do Estoque:", "Ativo" }
            };

            for (int i = 0; i < 2; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    bool isLabel = j % 2 == 0;
                    PdfPCell cell = new PdfPCell(new Phrase(filtros[i, j], isLabel ? fontBold : fontRegular));
                    cell.BackgroundColor = new BaseColor(247, 250, 252);
                    cell.BorderColor = new BaseColor(237, 242, 247);
                    cell.Padding = 6;
                    filterTable.AddCell(cell);
                }
            }

            doc.Add(filterTable);

            doc.Add(new Paragraph("RESUMO DO PERÍODO", fontSecao));

            PdfPTable summaryTable = new PdfPTable(3);
            summaryTable.WidthPercentage = 100;
            summaryTable.SetWidths(new float[] { 31f, 31f, 31f });
            summaryTable.SpacingBefore = 5f;
            summaryTable.SpacingAfter = 15f;

            summaryTable.AddCell(CriarCardResumo("TOTAL ENTRADAS (QTD)", dados.TotalEntradas.ToString(),
                new BaseColor(56, 161, 105), fontMeta, fontTitulo));
            summaryTable.AddCell(CriarCardResumo("TOTAL SAÍDAS (QTD)", dados.TotalSaidas.ToString(),
                new BaseColor(229, 62, 98), fontMeta, fontTitulo));
            summaryTable.AddCell(CriarCardResumo("BALANÇO LÍQUIDO",
                (dados.BalancoLiquido >= 0 ? "+" : "") + dados.BalancoLiquido, new BaseColor(49, 130, 206), fontMeta,
                fontTitulo));

            doc.Add(summaryTable);

            doc.Add(new Paragraph("DETALHAMENTO DAS MOVIMENTAÇÕES", fontSecao));

            PdfPTable dataTable = new PdfPTable(8);
            dataTable.WidthPercentage = 100;
            dataTable.SetWidths(new float[] { 6f, 20f, 10f, 8f, 10f, 12f, 18f, 16f });
            dataTable.SpacingBefore = 5f;

            string[] headers = { "ID", "Produto", "Data", "Qtd.", "Valor", "Tipo", "Descrição", "Registrado por" };
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
                dataTable.AddCell(new PdfPCell(new Phrase(item.IdMovimentacao.ToString(), fontRegular))
                    { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                dataTable.AddCell(new PdfPCell(new Phrase(item.Produto, fontRegular))
                    { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                dataTable.AddCell(new PdfPCell(new Phrase(item.Data.ToString("dd/MM/yyyy"), fontRegular))
                {
                    Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240)
                });
                dataTable.AddCell(new PdfPCell(new Phrase(item.Quantidade.ToString(), fontRegular))
                {
                    Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240)
                });
                dataTable.AddCell(new PdfPCell(new Phrase(item.Valor.ToString("C2"), fontRegular))
                {
                    Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240)
                });

                bool isEntrada = item.TipoMovimentacao != null && item.TipoMovimentacao.ToLower().Contains("entrada");
                PdfPCell typeCell = new PdfPCell(new Phrase(item.TipoMovimentacao ?? string.Empty, fontBold));
                typeCell.BackgroundColor = isEntrada ? new BaseColor(198, 246, 213) : new BaseColor(254, 215, 215);
                typeCell.Padding = 6;
                typeCell.HorizontalAlignment = Element.ALIGN_CENTER;
                typeCell.BorderColor = new BaseColor(226, 232, 240);
                dataTable.AddCell(typeCell);

                dataTable.AddCell(new PdfPCell(new Phrase(item.Descricao ?? string.Empty, fontRegular))
                    { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                dataTable.AddCell(new PdfPCell(new Phrase(item.Registrado ?? string.Empty, fontRegular))
                    { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
            }

            doc.Add(dataTable);
            doc.Close();
        }

        private PdfPCell CriarCardResumo(string titulo, string valor, BaseColor corBordaTop, Font fTitulo, Font fValor)
        {
            PdfPCell cell = new PdfPCell();
            cell.BackgroundColor = new BaseColor(247, 250, 252);
            cell.BorderColor = new BaseColor(237, 242, 247);
            cell.BorderWidthTop = 3f;
            cell.BorderColorTop = corBordaTop;
            cell.Padding = 10;

            Paragraph pT = new Paragraph(titulo, fTitulo);
            pT.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(pT);

            Paragraph pV = new Paragraph(valor, fValor);
            pV.Alignment = Element.ALIGN_CENTER;
            cell.AddElement(pV);

            return cell;
        }

        #endregion

        

        
        
        

       
    }
}
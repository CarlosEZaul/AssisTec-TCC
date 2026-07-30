using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.IO;
using AssisTEC.DTO;

public static class GeradorPdfCliente
{
    public static void GerarRelatorioGeral(ClienteDTO.ClientesRelatorioDTO dados, string caminhoDestino)
    {
        Document doc = new Document(PageSize.A4, 36, 36, 36, 36);

        try
        {
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

                PdfPTable headerTable = CriarCabecalho("Relatório Geral de Clientes", fontTitulo, fontSubtitulo, fontMeta);
                doc.Add(headerTable);

                doc.Add(CriarLinhaDivisoria());

                doc.Add(new Paragraph("FILTROS APLICADOS", fontSecao));

                PdfPTable filterTable = new PdfPTable(2);
                filterTable.WidthPercentage = 100;
                filterTable.SetWidths(new float[] { 20f, 80f });
                filterTable.SpacingBefore = 5f;
                filterTable.SpacingAfter = 15f;

                string[,] filtros = {
                    { "Nome:", dados.FiltroNome ?? string.Empty },
                    { "Status:", dados.FiltroStatus ?? string.Empty }
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

                if (dados.Itens != null)
                {
                    foreach (var item in dados.Itens)
                    {
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Id.ToString(), fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Nome ?? string.Empty, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Cpf ?? string.Empty, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Telefone ?? string.Empty, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Cidade ?? string.Empty, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Estado ?? string.Empty, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });

                        string statusStr = item.Status ?? string.Empty;
                        bool ativo = statusStr.Equals("Ativado", StringComparison.OrdinalIgnoreCase) || statusStr.Equals("Ativo", StringComparison.OrdinalIgnoreCase);
                        PdfPCell statusCell = new PdfPCell(new Phrase(statusStr, fontBold));
                        statusCell.BackgroundColor = ativo ? new BaseColor(198, 246, 213) : new BaseColor(254, 215, 215);
                        statusCell.Padding = 6;
                        statusCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        statusCell.BorderColor = new BaseColor(226, 232, 240);
                        dataTable.AddCell(statusCell);
                    }
                }

                doc.Add(dataTable);
                doc.Close();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao gerar relatório geral de clientes em PDF: " + ex.Message, ex);
        }
    }

    public static void GerarRelatorioIndividual(ClienteDTO.ClienteComOrdemServicoDTO dados, string caminhoDestino)
    {
        Document doc = new Document(PageSize.A4, 36, 36, 36, 36);

        try
        {
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

                PdfPTable headerTable = CriarCabecalho("Histórico Financeiro e de Serviços do Cliente", fontTitulo, fontSubtitulo, fontMeta);
                doc.Add(headerTable);

                doc.Add(CriarLinhaDivisoria());

                doc.Add(new Paragraph("DADOS DO CLIENTE", fontSecao));

                PdfPTable infoTable = new PdfPTable(4);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 15f, 35f, 15f, 35f });
                infoTable.SpacingBefore = 4f;
                infoTable.SpacingAfter = 15f;

                string[,] infoCampos = {
                    { "Nome:", dados.Nome ?? string.Empty, "CPF:", dados.Cpf ?? string.Empty },
                    { "Telefone:", dados.Telefone ?? string.Empty, "Código:", dados.IdCliente.ToString() },
                    { "Situação:", dados.StatusCliente ?? string.Empty, "", "" }
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

                if (dados.Ordens != null)
                {
                    foreach (var item in dados.Ordens)
                    {
                        dataTable.AddCell(new PdfPCell(new Phrase(item.IdOrdemServico.ToString(), fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Tecnico ?? string.Empty, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Equipamento ?? string.Empty, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.DataAbertura.ToString("dd/MM/yyyy"), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240) });
                        
                        string dataFechamentoText = item.DataFechamento.HasValue ? item.DataFechamento.Value.ToString("dd/MM/yyyy") : "-";
                        dataTable.AddCell(new PdfPCell(new Phrase(dataFechamentoText, fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240) });
                        
                        dataTable.AddCell(new PdfPCell(new Phrase(item.ValorTotal.ToString("C2"), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });

                        string statusOS = item.Status ?? string.Empty;
                        bool finalizada = statusOS.Equals("Finalizado", StringComparison.OrdinalIgnoreCase) || 
                                           statusOS.Equals("Entregue", StringComparison.OrdinalIgnoreCase) ||
                                           statusOS.Equals("FINALIZADA", StringComparison.OrdinalIgnoreCase);
                        PdfPCell statusCell = new PdfPCell(new Phrase(statusOS, fontBold));
                        statusCell.BackgroundColor = finalizada ? new BaseColor(198, 246, 213) : new BaseColor(254, 215, 215);
                        statusCell.Padding = 6;
                        statusCell.HorizontalAlignment = Element.ALIGN_CENTER;
                        statusCell.BorderColor = new BaseColor(226, 232, 240);
                        dataTable.AddCell(statusCell);
                    }
                }

                doc.Add(dataTable);
                doc.Close();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao gerar relatório individual do cliente em PDF: " + ex.Message, ex);
        }
    }

    private static PdfPTable CriarCabecalho(string subtitulo, Font fontTitulo, Font fontSubtitulo, Font fontMeta)
    {
        PdfPTable headerTable = new PdfPTable(2);
        headerTable.WidthPercentage = 100;
        headerTable.SetWidths(new float[] { 60f, 40f });

        PdfPCell cellLeft = new PdfPCell();
        cellLeft.Border = PdfPCell.NO_BORDER;
        cellLeft.AddElement(new Paragraph("AssisTEC", fontTitulo));
        cellLeft.AddElement(new Paragraph(subtitulo, fontSubtitulo));
        headerTable.AddCell(cellLeft);

        PdfPCell cellRight = new PdfPCell();
        cellRight.Border = PdfPCell.NO_BORDER;
        Paragraph pMeta = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\nSistema AssisTEC", fontMeta);
        pMeta.Alignment = Element.ALIGN_RIGHT;
        cellRight.AddElement(pMeta);
        headerTable.AddCell(cellRight);

        return headerTable;
    }

    private static Paragraph CriarLinhaDivisoria()
    {
        Paragraph linhaDivisoria = new Paragraph(new Chunk(new LineSeparator(2f, 100f, new BaseColor(43, 108, 176), Element.ALIGN_CENTER, -1f)));
        linhaDivisoria.SpacingAfter = 12f;
        return linhaDivisoria;
    }

    private static PdfPCell CriarCardResumo(string titulo, string valor, BaseColor corBordaTop, Font fTitulo, Font fValor)
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
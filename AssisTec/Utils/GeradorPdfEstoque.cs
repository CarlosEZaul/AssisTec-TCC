using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using System;
using System.IO;
using AssisTec.Dtos;

public static class GeradorPdfEstoque
{
    public static void GerarRelatorio(ProdutoDTO.EstoqueRelatorioDTO dados, string caminhoDestino)
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

                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 60f, 40f });

                PdfPCell cellLeft = new PdfPCell();
                cellLeft.Border = PdfPCell.NO_BORDER;
                cellLeft.AddElement(new Paragraph("AssisTEC", fontTitulo));
                cellLeft.AddElement(new Paragraph("Relatório de Controle de Estoque", fontSubtitulo));
                headerTable.AddCell(cellLeft);

                PdfPCell cellRight = new PdfPCell();
                cellRight.Border = PdfPCell.NO_BORDER;
                Paragraph pMeta = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\nSistema AssisTEC", fontMeta);
                pMeta.Alignment = Element.ALIGN_RIGHT;
                cellRight.AddElement(pMeta);
                headerTable.AddCell(cellRight);

                doc.Add(headerTable);

                Paragraph linhaDivisoria = new Paragraph(new Chunk(new LineSeparator(2f, 100f, new BaseColor(43, 108, 176), Element.ALIGN_CENTER, -1f)));
                linhaDivisoria.SpacingAfter = 15f;
                doc.Add(linhaDivisoria);

                PdfPTable filterTable = new PdfPTable(4);
                filterTable.WidthPercentage = 100;
                filterTable.SetWidths(new float[] { 18f, 32f, 18f, 32f });
                filterTable.SpacingBefore = 5f;
                filterTable.SpacingAfter = 15f;

                string[,] filtros = {
                    { "Descrição:", dados.FiltroDescricao ?? string.Empty, "Fornecedor:", dados.FiltroFornecedor ?? string.Empty },
                    { "Status/Estoque:", dados.FiltroStatus ?? string.Empty, "", "" }
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

                doc.Add(new Paragraph("RESUMO DO ESTOQUE", fontSecao));

                PdfPTable summaryTable = new PdfPTable(4);
                summaryTable.WidthPercentage = 100;
                summaryTable.SetWidths(new float[] { 25f, 25f, 25f, 25f });
                summaryTable.SpacingBefore = 5f;
                summaryTable.SpacingAfter = 15f;

                summaryTable.AddCell(CriarCardResumo("TOTAL PRODUTOS", dados.TotalCadastrado.ToString(), new BaseColor(74, 85, 104), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("ABAIXO DO MÍNIMO", dados.AbaixoMinimo.ToString(), new BaseColor(237, 137, 54), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("SEM ESTOQUE", dados.SemEstoque.ToString(), new BaseColor(229, 62, 98), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("VALOR EM ESTOQUE", dados.ValorEstoque.ToString("C2"), new BaseColor(49, 130, 206), fontMeta, fontTitulo));

                doc.Add(summaryTable);

                doc.Add(new Paragraph("DETALHAMENTO DO INVENTÁRIO", fontSecao));

                PdfPTable dataTable = new PdfPTable(9);
                dataTable.WidthPercentage = 100;
                dataTable.SetWidths(new float[] { 6f, 22f, 7f, 12f, 12f, 7f, 7f, 15f, 12f });
                dataTable.SpacingBefore = 5f;

                string[] headers = { "ID", "Descrição", "Unid.", "Preço Compra", "Preço Venda", "Qtd.", "Min.", "Fornecedor", "Status" };
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
                        dataTable.AddCell(new PdfPCell(new Phrase(item.IdProduto.ToString(), fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Descricao ?? string.Empty, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Unidade ?? string.Empty, fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.PrecoCompra.ToString("C2"), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.PrecoVenda.ToString("C2"), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Quantidade.ToString(), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.QuantidadeMinima.ToString(), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });
                        dataTable.AddCell(new PdfPCell(new Phrase(item.Fornecedor ?? string.Empty, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });

                        string statusStr = item.Status ?? string.Empty;
                        bool ativo = statusStr.Equals("Ativo", StringComparison.OrdinalIgnoreCase) || statusStr.Equals("Ativado", StringComparison.OrdinalIgnoreCase);
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
            throw new Exception("Erro ao gerar relatório de estoque em PDF: " + ex.Message, ex);
        }
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
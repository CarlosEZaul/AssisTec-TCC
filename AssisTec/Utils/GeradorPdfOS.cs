using System;
using System.IO;
using System.Linq;
using AssisTec.DTO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace AssisTec.Utils
{
    public static class GeradorPdfOS
    {
        public static string GerarRecibo(OrdemServicoRelatorioDTO dados, string caminhoDestino, string caminhoLogo = null)
        {
            Document doc = new Document(PageSize.A4, 36, 36, 36, 36);

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminhoDestino, FileMode.Create));
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

                if (!string.IsNullOrEmpty(caminhoLogo) && File.Exists(caminhoLogo))
                {
                    Image logo = Image.GetInstance(caminhoLogo);
                    logo.ScaleToFit(100f, 40f);
                    cellLeft.AddElement(logo);
                }

                cellLeft.AddElement(new Paragraph("AssisTEC", fontTitulo));
                cellLeft.AddElement(new Paragraph($"Ordem de Serviço Nº {dados.IdOS:D6}", fontSubtitulo));
                headerTable.AddCell(cellLeft);

                PdfPCell cellRight = new PdfPCell();
                cellRight.Border = PdfPCell.NO_BORDER;
                Paragraph pMeta = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\nStatus: {dados.Status}", fontMeta);
                pMeta.Alignment = Element.ALIGN_RIGHT;
                cellRight.AddElement(pMeta);
                headerTable.AddCell(cellRight);

                doc.Add(headerTable);

                Paragraph linhaDivisoria = new Paragraph(new Chunk(new LineSeparator(2f, 100f, new BaseColor(43, 108, 176), Element.ALIGN_CENTER, -1f)));
                linhaDivisoria.SpacingAfter = 12f;
                doc.Add(linhaDivisoria);

                doc.Add(new Paragraph("DADOS DO CLIENTE E EQUIPAMENTO", fontSecao));

                PdfPTable infoTable = new PdfPTable(4);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 15f, 35f, 15f, 35f });
                infoTable.SpacingBefore = 4f;
                infoTable.SpacingAfter = 15f;

                string[,] infoCampos = {
                    { "Cliente:", dados.NomeCliente, "Documento:", dados.DocumentoCliente },
                    { "Telefone:", dados.TelefoneCliente, "Endereço:", dados.EnderecoCliente },
                    { "Equipamento:", dados.Equipamento, "Marca/Modelo:", dados.MarcaModelo },
                    { "Nº Série:", dados.NumeroSerie, "Abertura:", dados.DataAbertura.ToString("dd/MM/yyyy HH:mm") }
                };

                for (int i = 0; i < 4; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        bool isLabel = j % 2 == 0;
                        PdfPCell cell = new PdfPCell(new Phrase(infoCampos[i, j] ?? "", isLabel ? fontBold : fontRegular));
                        cell.BackgroundColor = new BaseColor(247, 250, 252);
                        cell.BorderColor = new BaseColor(237, 242, 247);
                        cell.Padding = 6;
                        infoTable.AddCell(cell);
                    }
                }
                doc.Add(infoTable);

                doc.Add(new Paragraph("DIAGNÓSTICO E LAUDO TÉCNICO", fontSecao));

                PdfPTable diagTable = new PdfPTable(2);
                diagTable.WidthPercentage = 100;
                diagTable.SetWidths(new float[] { 20f, 80f });
                diagTable.SpacingBefore = 4f;
                diagTable.SpacingAfter = 15f;

                PdfPCell cDefeitoLabel = new PdfPCell(new Phrase("Defeito Relatado:", fontBold)) { BackgroundColor = new BaseColor(247, 250, 252), BorderColor = new BaseColor(237, 242, 247), Padding = 6 };
                PdfPCell cDefeitoVal = new PdfPCell(new Phrase(dados.DefeitoRelatado ?? "-", fontRegular)) { BackgroundColor = new BaseColor(247, 250, 252), BorderColor = new BaseColor(237, 242, 247), Padding = 6 };

                PdfPCell cLaudoLabel = new PdfPCell(new Phrase("Laudo Técnico:", fontBold)) { BackgroundColor = new BaseColor(247, 250, 252), BorderColor = new BaseColor(237, 242, 247), Padding = 6 };
                PdfPCell cLaudoVal = new PdfPCell(new Phrase(dados.LaudoTecnico ?? "-", fontRegular)) { BackgroundColor = new BaseColor(247, 250, 252), BorderColor = new BaseColor(237, 242, 247), Padding = 6 };

                diagTable.AddCell(cDefeitoLabel);
                diagTable.AddCell(cDefeitoVal);
                diagTable.AddCell(cLaudoLabel);
                diagTable.AddCell(cLaudoVal);
                doc.Add(diagTable);

                doc.Add(new Paragraph("RESUMO FINANCEIRO", fontSecao));

                PdfPTable summaryTable = new PdfPTable(4);
                summaryTable.WidthPercentage = 100;
                summaryTable.SetWidths(new float[] { 25f, 25f, 25f, 25f });
                summaryTable.SpacingBefore = 4f;
                summaryTable.SpacingAfter = 15f;

                summaryTable.AddCell(CriarCardResumo("TOTAL PEÇAS", dados.ValorPecas.ToString("C2"), new BaseColor(74, 85, 104), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("TOTAL MÃO DE OBRA", dados.ValorMaoObra.ToString("C2"), new BaseColor(237, 137, 54), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("VALOR TOTAL", dados.ValorTotal.ToString("C2"), new BaseColor(56, 161, 105), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("PAGAMENTO", dados.FormaPagamento ?? "Pendente", new BaseColor(49, 130, 206), fontMeta, fontTitulo));

                doc.Add(summaryTable);

                var servicos = dados.Itens?.Where(x => string.Equals(x.Tipo, "Serviço", StringComparison.OrdinalIgnoreCase)).ToList();
                var produtos = dados.Itens?.Where(x => !string.Equals(x.Tipo, "Serviço", StringComparison.OrdinalIgnoreCase)).ToList();

                doc.Add(new Paragraph("SERVIÇOS PRESTADOS", fontSecao));
                PdfPTable tblServicos = CriarTabelaServicos(servicos, fontHeaderTabela, fontRegular);
                tblServicos.SpacingBefore = 4f;
                tblServicos.SpacingAfter = 15f;
                doc.Add(tblServicos);

                doc.Add(new Paragraph("PRODUTOS E PEÇAS UTILIZADAS", fontSecao));
                PdfPTable tblProdutos = CriarTabelaItens(produtos, fontHeaderTabela, fontRegular);
                tblProdutos.SpacingBefore = 4f;
                tblProdutos.SpacingAfter = 15f;
                doc.Add(tblProdutos);

                doc.Add(new Paragraph("\n\n"));

                PdfPTable tblAssinaturas = new PdfPTable(2);
                tblAssinaturas.WidthPercentage = 100;
                tblAssinaturas.SetWidths(new float[] { 45f, 45f });

                PdfPCell cellAssinatura1 = new PdfPCell(new Paragraph("___________________________________\nAssinatura do Técnico", fontRegular));
                cellAssinatura1.Border = Rectangle.NO_BORDER;
                cellAssinatura1.HorizontalAlignment = Element.ALIGN_CENTER;

                PdfPCell cellAssinatura2 = new PdfPCell(new Paragraph("___________________________________\nAssinatura do Cliente", fontRegular));
                cellAssinatura2.Border = Rectangle.NO_BORDER;
                cellAssinatura2.HorizontalAlignment = Element.ALIGN_CENTER;

                tblAssinaturas.AddCell(cellAssinatura1);
                tblAssinaturas.AddCell(cellAssinatura2);

                doc.Add(tblAssinaturas);

                return caminhoDestino;

            }
            finally
            {
                if (doc.IsOpen())
                {
                    doc.Close();
                }
            }
        }

        private static PdfPTable CriarTabelaServicos(System.Collections.Generic.List<ItemOSRelatorioDTO> itens, Font fontHeader, Font fontBody)
        {
            PdfPTable table = new PdfPTable(2);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 75f, 25f });

            string[] headers = { "Descrição do Serviço", "Valor" };
            foreach (var header in headers)
            {
                PdfPCell hCell = new PdfPCell(new Phrase(header, fontHeader));
                hCell.BackgroundColor = new BaseColor(26, 54, 93);
                hCell.BorderColor = new BaseColor(26, 54, 93);
                hCell.Padding = 6;
                table.AddCell(hCell);
            }

            if (itens != null && itens.Count > 0)
            {
                foreach (var item in itens)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.Descricao, fontBody)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                    table.AddCell(new PdfPCell(new Phrase(item.ValorTotal.ToString("C2"), fontBody)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });
                }
            }
            else
            {
                PdfPCell emptyCell = new PdfPCell(new Phrase("Nenhum serviço registrado nesta ordem.", fontBody));
                emptyCell.Colspan = 2;
                emptyCell.Padding = 6;
                emptyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                emptyCell.BorderColor = new BaseColor(226, 232, 240);
                table.AddCell(emptyCell);
            }

            return table;
        }

        private static PdfPTable CriarTabelaItens(System.Collections.Generic.List<ItemOSRelatorioDTO> itens, Font fontHeader, Font fontBody)
        {
            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;
            table.SetWidths(new float[] { 55f, 10f, 17.5f, 17.5f });

            string[] headers = { "Descrição", "Qtd", "Unitário", "Total" };
            foreach (var header in headers)
            {
                PdfPCell hCell = new PdfPCell(new Phrase(header, fontHeader));
                hCell.BackgroundColor = new BaseColor(26, 54, 93);
                hCell.BorderColor = new BaseColor(26, 54, 93);
                hCell.Padding = 6;
                table.AddCell(hCell);
            }

            if (itens != null && itens.Count > 0)
            {
                foreach (var item in itens)
                {
                    table.AddCell(new PdfPCell(new Phrase(item.Descricao, fontBody)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                    table.AddCell(new PdfPCell(new Phrase(item.Quantidade.ToString(), fontBody)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240) });
                    table.AddCell(new PdfPCell(new Phrase(item.ValorUnitario.ToString("C2"), fontBody)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });
                    table.AddCell(new PdfPCell(new Phrase(item.ValorTotal.ToString("C2"), fontBody)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });
                }
            }
            else
            {
                PdfPCell emptyCell = new PdfPCell(new Phrase("Nenhum item registrado nesta categoria.", fontBody));
                emptyCell.Colspan = 4;
                emptyCell.Padding = 6;
                emptyCell.HorizontalAlignment = Element.ALIGN_CENTER;
                emptyCell.BorderColor = new BaseColor(226, 232, 240);
                table.AddCell(emptyCell);
            }

            return table;
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
}
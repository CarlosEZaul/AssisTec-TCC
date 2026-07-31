using System;
using System.Data;
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
        public static string ImprimirOS(OrdemServicoRelatorioDTO dados, string caminhoDestino, string caminhoLogo = null)
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

        public static string GerarRelatorioGeral(DataTable dados, RelatorioTotaisDTO totais, string caminhoDestino,
            string caminhoLogo = null)
        {
            Document doc = new Document(PageSize.A4.Rotate(), 36, 36, 36, 36);

            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(caminhoDestino, FileMode.Create));
                doc.Open();

                BaseFont bfRegular = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                BaseFont bfBold = BaseFont.CreateFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);

                Font fontTitulo = new Font(bfBold, 16, Font.NORMAL, new BaseColor(26, 54, 93));
                Font fontSubtitulo = new Font(bfRegular, 9, Font.NORMAL, new BaseColor(74, 85, 104));
                Font fontMeta = new Font(bfRegular, 8, Font.NORMAL, new BaseColor(113, 128, 150));
                Font fontSecao = new Font(bfBold, 10, Font.NORMAL, new BaseColor(43, 108, 176));
                Font fontHeaderTabela = new Font(bfBold, 8, Font.NORMAL, BaseColor.WHITE);
                Font fontRegular = new Font(bfRegular, 8, Font.NORMAL, new BaseColor(45, 55, 72));
                Font fontBold = new Font(bfBold, 8, Font.NORMAL, new BaseColor(45, 55, 72));

                PdfPTable headerTable = new PdfPTable(2);
                headerTable.WidthPercentage = 100;
                headerTable.SetWidths(new float[] { 60f, 40f });

                PdfPCell cellLeft = new PdfPCell { Border = PdfPCell.NO_BORDER };

                if (!string.IsNullOrEmpty(caminhoLogo) && File.Exists(caminhoLogo))
                {
                    Image logo = Image.GetInstance(caminhoLogo);
                    logo.ScaleToFit(90f, 35f);
                    cellLeft.AddElement(logo);
                }

                cellLeft.AddElement(new Paragraph("AssisTEC - Relatório Geral de Ordens de Serviço", fontTitulo));
                cellLeft.AddElement(new Paragraph(
                    $"Período: {totais.FiltroPeriodo ?? "Geral"} | Status: {totais.FiltroStatus ?? "Todos"}",
                    fontSubtitulo));
                headerTable.AddCell(cellLeft);

                PdfPCell cellRight = new PdfPCell { Border = PdfPCell.NO_BORDER };
                Paragraph pMeta =
                    new Paragraph(
                        $"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\nTotal de Registros: {dados?.Rows.Count ?? 0}",
                        fontMeta);
                pMeta.Alignment = Element.ALIGN_RIGHT;
                cellRight.AddElement(pMeta);
                headerTable.AddCell(cellRight);

                doc.Add(headerTable);

                Paragraph linhaDivisoria = new Paragraph(new Chunk(new LineSeparator(1.5f, 100f,
                    new BaseColor(43, 108, 176), Element.ALIGN_CENTER, -1f)));
                linhaDivisoria.SpacingAfter = 10f;
                doc.Add(linhaDivisoria);

                doc.Add(new Paragraph("RESUMO OPERACIONAL E FINANCEIRO", fontSecao));

                PdfPTable summaryTable = new PdfPTable(4);
                summaryTable.WidthPercentage = 100;
                summaryTable.SetWidths(new float[] { 25f, 25f, 25f, 25f });
                summaryTable.SpacingBefore = 4f;
                summaryTable.SpacingAfter = 12f;

                summaryTable.AddCell(CriarCardResumo("TOTAL OS", totais.TotalOS.ToString(), new BaseColor(74, 85, 104),
                    fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("EM ATENDIMENTO", totais.EmAtendimento.ToString(),
                    new BaseColor(237, 137, 54), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("RECEBIDO", $"{totais.TotalRecebido:C2} ({totais.QntRecebido})",
                    new BaseColor(56, 161, 105), fontMeta, fontTitulo));
                summaryTable.AddCell(CriarCardResumo("A RECEBER", totais.TotalAReceber.ToString("C2"),
                    new BaseColor(49, 130, 206), fontMeta, fontTitulo));

                doc.Add(summaryTable);

                doc.Add(new Paragraph("LISTAGEM DE ORDENS DE SERVIÇO", fontSecao));

                PdfPTable table = new PdfPTable(7);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 10f, 20f, 20f, 15f, 12f, 11f, 12f });
                table.SpacingBefore = 4f;

                string[] headers =
                    { "Nº OS", "Cliente", "Equipamento", "Status", "Abertura", "Valor Total", "Pagamento" };
                foreach (var header in headers)
                {
                    PdfPCell hCell = new PdfPCell(new Phrase(header, fontHeaderTabela))
                    {
                        BackgroundColor = new BaseColor(26, 54, 93),
                        BorderColor = new BaseColor(26, 54, 93),
                        Padding = 5,
                        HorizontalAlignment = Element.ALIGN_CENTER
                    };
                    table.AddCell(hCell);
                }

                if (dados != null && dados.Rows.Count > 0)
                {
                    foreach (DataRow row in dados.Rows)
                    {
                        string idVal = ObterValorPorColuna(row, "ID", "id_os", "id");
                        string idOS = "-";
                        if (int.TryParse(idVal, out int parsedId))
                        {
                            idOS = parsedId.ToString("D6");
                        }
                        else if (!string.IsNullOrWhiteSpace(idVal) && idVal != "-")
                        {
                            idOS = idVal;
                        }

                        string cliente = ObterValorPorColuna(row, "Cliente", "cliente");
                        string equipamento = ObterValorPorColuna(row, "Equipamento", "equipamento");
                        string status = ObterValorPorColuna(row, "Status", "status");

                        string dataVal = ObterValorPorColuna(row, "Data de Abertura", "DataAbertura", "data_abertura");
                        string dataAbertura = "-";
                        if (DateTime.TryParse(dataVal, out DateTime parsedData))
                        {
                            dataAbertura = parsedData.ToString("dd/MM/yyyy");
                        }

                        string valorVal = ObterValorPorColuna(row, "Valor Total", "ValorTotal", "valor_total");
                        string valorTotal = "R$ 0,00";
                        if (decimal.TryParse(valorVal.Replace("R$", "").Trim(), out decimal parsedValor))
                        {
                            valorTotal = parsedValor.ToString("C2");
                        }

                        string formaPagamento = ObterValorPorColuna(row, "Forma de Pagamento", "FormaPagamento", "forma_pagamento", "Pagamento");

                        table.AddCell(CriarCelulaTabela(idOS, fontRegular, Element.ALIGN_CENTER));
                        table.AddCell(CriarCelulaTabela(cliente, fontRegular, Element.ALIGN_LEFT));
                        table.AddCell(CriarCelulaTabela(equipamento, fontRegular, Element.ALIGN_LEFT));
                        table.AddCell(CriarCelulaTabela(status, fontBold, Element.ALIGN_CENTER));
                        table.AddCell(CriarCelulaTabela(dataAbertura, fontRegular, Element.ALIGN_CENTER));
                        table.AddCell(CriarCelulaTabela(valorTotal, fontRegular, Element.ALIGN_RIGHT));
                        table.AddCell(CriarCelulaTabela(formaPagamento, fontRegular, Element.ALIGN_CENTER));
                    }
                }
                else
                {
                    PdfPCell emptyCell =
                        new PdfPCell(new Phrase("Nenhum registro encontrado para os filtros aplicados.", fontRegular))
                        {
                            Colspan = 7,
                            Padding = 8,
                            HorizontalAlignment = Element.ALIGN_CENTER,
                            BorderColor = new BaseColor(226, 232, 240)
                        };
                    table.AddCell(emptyCell);
                }

                doc.Add(table);

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
        
        private static string ObterValorPorColuna(DataRow row, params string[] nomesTentativas)
        {
            foreach (DataColumn column in row.Table.Columns)
            {
                string colName = column.ColumnName.Trim();
                foreach (string tentativa in nomesTentativas)
                {
                    if (string.Equals(colName, tentativa, StringComparison.OrdinalIgnoreCase))
                    {
                        if (row[column] != DBNull.Value && row[column] != null)
                        {
                            string valor = row[column].ToString().Trim();
                            if (!string.IsNullOrEmpty(valor))
                                return valor;
                        }
                    }
                }
            }

            return "-";
        }

        private static PdfPCell CriarCelulaTabela(string texto, Font font, int alinhamento)
        {
            return new PdfPCell(new Phrase(texto ?? "-", font))
            {
                Padding = 5,
                HorizontalAlignment = alinhamento,
                VerticalAlignment = Element.ALIGN_MIDDLE,
                BorderColor = new BaseColor(226, 232, 240)
            };
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
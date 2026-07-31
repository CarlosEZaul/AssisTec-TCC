using System;
using System.IO;
using AssisTec.Dtos;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace AssisTec.Service
{
    public static class GeradorPdfContasReceber
    {
        public static void GerarRelatorioGeral(ContasReceberDto.ContasReceberRelatorioDTO dados, string caminhoDestino)
        {
            Document doc = new Document(PageSize.A4.Rotate(), 36, 36, 36, 36);

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

                    PdfPTable headerTable = CriarCabecalho("Relatório Geral de Contas a Receber", fontTitulo, fontSubtitulo, fontMeta);
                    doc.Add(headerTable);

                    doc.Add(CriarLinhaDivisoria());

                    doc.Add(new Paragraph("FILTROS APLICADOS", fontSecao));

                    PdfPTable filterTable = new PdfPTable(4);
                    filterTable.WidthPercentage = 100;
                    filterTable.SetWidths(new float[] { 15f, 35f, 15f, 35f });
                    filterTable.SpacingBefore = 5f;
                    filterTable.SpacingAfter = 15f;

                    string[,] filtros = {
                        { "Período:", dados.FiltroPeriodo, "Descrição:", dados.FiltroDescricao },
                        { "Status:", dados.FiltroStatus, "Forma Pagto:", dados.FiltroFormaPagamento }
                    };

                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            bool isLabel = j % 2 == 0;
                            PdfPCell cell = new PdfPCell(new Phrase(filtros[i, j] ?? string.Empty, isLabel ? fontBold : fontRegular));
                            cell.BackgroundColor = new BaseColor(247, 250, 252);
                            cell.BorderColor = new BaseColor(237, 242, 247);
                            cell.Padding = 6;
                            filterTable.AddCell(cell);
                        }
                    }
                    doc.Add(filterTable);

                    doc.Add(new Paragraph("RESUMO FINANCEIRO", fontSecao));

                    PdfPTable summaryTable = new PdfPTable(4);
                    summaryTable.WidthPercentage = 100;
                    summaryTable.SetWidths(new float[] { 25f, 25f, 25f, 25f });
                    summaryTable.SpacingBefore = 5f;
                    summaryTable.SpacingAfter = 15f;

                    summaryTable.AddCell(CriarCardResumo("TOTAL GERAL", dados.TotalGeral.ToString("C2"), new BaseColor(74, 85, 104), fontMeta, fontTitulo));
                    summaryTable.AddCell(CriarCardResumo("TOTAL RECEBIDO", dados.TotalRecebido.ToString("C2"), new BaseColor(56, 161, 105), fontMeta, fontTitulo));
                    summaryTable.AddCell(CriarCardResumo("TOTAL PENDENTE", dados.TotalPendente.ToString("C2"), new BaseColor(237, 137, 54), fontMeta, fontTitulo));
                    summaryTable.AddCell(CriarCardResumo("TOTAL ATRASADO", dados.TotalAtrasado.ToString("C2"), new BaseColor(229, 62, 98), fontMeta, fontTitulo));

                    doc.Add(summaryTable);

                    doc.Add(new Paragraph("DETALHAMENTO DAS CONTAS", fontSecao));

                    PdfPTable dataTable = new PdfPTable(8);
                    dataTable.WidthPercentage = 100;
                    dataTable.SetWidths(new float[] { 6f, 26f, 13f, 11f, 11f, 11f, 14f, 8f });
                    dataTable.SpacingBefore = 5f;

                    string[] headers = { "ID", "Descrição", "Valor", "Emissão", "Vencimento", "Pagamento", "Status", "OS" };
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
                            dataTable.AddCell(new PdfPCell(new Phrase(item.IdContaReceber.ToString(), fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                            dataTable.AddCell(new PdfPCell(new Phrase(item.Descricao ?? string.Empty, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                            dataTable.AddCell(new PdfPCell(new Phrase(item.Valor.ToString("C2"), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });
                            dataTable.AddCell(new PdfPCell(new Phrase(item.DataEmissao.ToString("dd/MM/yyyy"), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240) });
                            
                            string dataVenc = item.DataVencimento.HasValue ? item.DataVencimento.Value.ToString("dd/MM/yyyy") : "-";
                            dataTable.AddCell(new PdfPCell(new Phrase(dataVenc, fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240) });

                            string dataPag = item.DataPagamento.HasValue ? item.DataPagamento.Value.ToString("dd/MM/yyyy") : "-";
                            dataTable.AddCell(new PdfPCell(new Phrase(dataPag, fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240) });

                            string statusStr = item.Status ?? string.Empty;
                            PdfPCell statusCell = new PdfPCell(new Phrase(statusStr, fontBold));
                            statusCell.Padding = 6;
                            statusCell.HorizontalAlignment = Element.ALIGN_CENTER;
                            statusCell.BorderColor = new BaseColor(226, 232, 240);

                            if (statusStr.Equals("PAGA", StringComparison.OrdinalIgnoreCase) || statusStr.Equals("RECEBIDO", StringComparison.OrdinalIgnoreCase))
                            {
                                statusCell.BackgroundColor = new BaseColor(198, 246, 213);
                            }
                            else if (statusStr.Equals("ATRASADO", StringComparison.OrdinalIgnoreCase))
                            {
                                statusCell.BackgroundColor = new BaseColor(254, 215, 215);
                            }
                            else
                            {
                                statusCell.BackgroundColor = new BaseColor(254, 235, 200);
                            }

                            dataTable.AddCell(statusCell);
                            dataTable.AddCell(new PdfPCell(new Phrase(item.IdOrdemServico?.ToString() ?? "-", fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240) });
                        }
                    }

                    doc.Add(dataTable);
                    doc.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar o relatório geral de contas a receber: " + ex.Message, ex);
            }
        }

        public static void GerarRelatorioIndividual(ContasReceberDto dto, string caminhoDestino)
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

                    PdfPTable headerTable = CriarCabecalho("Comprovante / Detalhes da Conta", fontTitulo, fontSubtitulo, fontMeta);
                    doc.Add(headerTable);

                    doc.Add(CriarLinhaDivisoria());

                    doc.Add(new Paragraph("DADOS DA CONTA", fontSecao));

                    PdfPTable infoTable = new PdfPTable(4);
                    infoTable.WidthPercentage = 100;
                    infoTable.SetWidths(new float[] { 18f, 32f, 18f, 32f });
                    infoTable.SpacingBefore = 4f;
                    infoTable.SpacingAfter = 15f;

                    string[,] infoCampos = {
                        { "Código:", dto.IdContaReceber.ToString(), "Valor:", dto.Valor.ToString("C2") },
                        { "Descrição:", dto.Descricao, "Status:", dto.Status },
                        { "Emissão:", dto.DataEmissao.ToString("dd/MM/yyyy"), "Vencimento:", dto.DataVencimento?.ToString("dd/MM/yyyy") ?? "-" },
                        { "Pagamento:", dto.DataPagamento?.ToString("dd/MM/yyyy") ?? "-", "Forma Pagto:", dto.FormaPagamentoDescricao },
                        { "Observações:", string.IsNullOrEmpty(dto.Observacoes) ? "-" : dto.Observacoes, "", "" }
                    };

                    for (int i = 0; i < 5; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (i == 4 && j >= 2)
                            {
                                PdfPCell emptyCell = new PdfPCell(new Phrase("", fontRegular)) { Border = PdfPCell.NO_BORDER };
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

                    if (dto.IdOrdemServico.HasValue && dto.IdOrdemServico.Value > 0)
                    {
                        doc.Add(new Paragraph($"ORDEM DE SERVIÇO VINCULADA (Nº {dto.IdOrdemServico.Value})", fontSecao));

                        PdfPTable osTable = new PdfPTable(4);
                        osTable.WidthPercentage = 100;
                        osTable.SetWidths(new float[] { 18f, 32f, 18f, 32f });
                        osTable.SpacingBefore = 4f;
                        osTable.SpacingAfter = 15f;

                        string[,] osCampos = {
                            { "Cliente:", string.IsNullOrEmpty(dto.ClienteNome) ? "-" : dto.ClienteNome, "Equipamento:", string.IsNullOrEmpty(dto.Equipamento) ? "-" : dto.Equipamento },
                            { "Defeito Relatado:", string.IsNullOrEmpty(dto.DefeitoRelatado) ? "-" : dto.DefeitoRelatado, "Serviço Realizado:", string.IsNullOrEmpty(dto.ServicoRealizado) ? "-" : dto.ServicoRealizado }
                        };

                        for (int i = 0; i < 2; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                bool isLabel = j % 2 == 0;
                                PdfPCell cell = new PdfPCell(new Phrase(osCampos[i, j], isLabel ? fontBold : fontRegular));
                                cell.BackgroundColor = new BaseColor(247, 250, 252);
                                cell.BorderColor = new BaseColor(237, 242, 247);
                                cell.Padding = 6;
                                osTable.AddCell(cell);
                            }
                        }
                        doc.Add(osTable);
                    }

                    doc.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar relatório individual da conta: " + ex.Message, ex);
            }
        }

        #region Métodos Auxiliares Privados

        private static PdfPTable CriarCabecalho(string subtitulo, Font fTitulo, Font fSubtitulo, Font fMeta)
        {
            PdfPTable headerTable = new PdfPTable(2);
            headerTable.WidthPercentage = 100;
            headerTable.SetWidths(new float[] { 60f, 40f });

            PdfPCell cellLeft = new PdfPCell();
            cellLeft.Border = PdfPCell.NO_BORDER;
            cellLeft.AddElement(new Paragraph("AssisTEC", fTitulo));
            cellLeft.AddElement(new Paragraph(subtitulo, fSubtitulo));
            headerTable.AddCell(cellLeft);

            PdfPCell cellRight = new PdfPCell();
            cellRight.Border = PdfPCell.NO_BORDER;
            Paragraph pMeta = new Paragraph($"Gerado em: {DateTime.Now:dd/MM/yyyy HH:mm}\nExportado por: Sistema", fMeta);
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

        #endregion
    }
}
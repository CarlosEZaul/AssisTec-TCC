using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using AssisTec.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using Font = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

namespace AssisTec.Reports
{
    public class ContasReceberRelatorio
    {
        private readonly conexao con;

        private readonly BaseColor corPrimaria =
            new BaseColor(33, 97, 140);

        private readonly BaseColor corHeader =
            new BaseColor(52, 73, 94);

        private readonly BaseColor corLinha1 =
            new BaseColor(245, 248, 250);

        private readonly BaseColor corLinha2 =
            BaseColor.WHITE;

        private readonly BaseColor corSecao =
            new BaseColor(214, 234, 248);

        private readonly BaseColor corRecebido =
            new BaseColor(39, 174, 96);

        private readonly BaseColor corPendente =
            new BaseColor(230, 126, 34);

        private readonly BaseColor corAtrasado =
            new BaseColor(192, 57, 43);

        public ContasReceberRelatorio()
        {
            con = new conexao();
        }

        // =========================
        // MÉTODOS PÚBLICOS
        // =========================

        public (bool sucesso, string mensagem, string caminho)
            GerarRelatorioGeral(
                DataTable dados,
                decimal totalGeral,
                decimal totalRecebido,
                decimal totalPendente,
                decimal totalAtrasado)
        {
            try
            {
                if (dados == null || dados.Rows.Count == 0)
                {
                    return (
                        false,
                        "Nenhum registro encontrado.",
                        null
                    );
                }

                string caminho = CriarPdfRelatorio(
                    dados,
                    totalGeral,
                    totalRecebido,
                    totalPendente,
                    totalAtrasado
                );

                AbrirPdf(caminho);

                return (
                    true,
                    "Relatório gerado com sucesso!",
                    caminho
                );
            }
            catch (Exception ex)
            {
                return (
                    false,
                    $"Erro ao gerar relatório: {ex.Message}",
                    null
                );
            }
        }

        public (bool sucesso, string mensagem, string caminho) GerarRecibo(int idConta)
        {
            try
            {
                DataRow conta = BuscarConta(idConta);

                if (conta == null)
                {
                    return (
                        false,
                        "Conta não encontrada.",
                        null
                    );
                }

                string status = conta["status"]
                    .ToString()
                    .Trim()
                    .ToUpper();

                if (status == "ATRASADO" || status == "PENDENTE")
                {
                    return (
                        false,
                        "O recibo só pode ser gerado para contas pagas.",
                        null
                    );
                }

                string caminho = CriarPdfRecibo(conta);

                AbrirPdf(caminho);

                return (
                    true,
                    "Recibo gerado com sucesso!",
                    caminho
                );
            }
            catch (Exception ex)
            {
                return (
                    false,
                    $"Erro ao gerar recibo: {ex.Message}",
                    null
                );
            }
        }

        // =========================
        // PDF RELATÓRIO
        // =========================

        private string CriarPdfRelatorio(
            DataTable dados,
            decimal totalGeral,
            decimal totalRecebido,
            decimal totalPendente,
            decimal totalAtrasado)
        {
            string caminho = ObterCaminhoArquivo(
                "Relatorio_ContasReceber",
                "Contas a Receber"
            );

            Document doc = new Document(
                PageSize.A4.Rotate(),
                40,
                40,
                40,
                40
            );

            PdfWriter.GetInstance(
                doc,
                new FileStream(caminho, FileMode.Create)
            );

            doc.Open();

            AdicionarCabecalho(
                doc,
                "RELATÓRIO DE CONTAS A RECEBER"
            );

            AdicionarCardsTotais(
                doc,
                totalGeral,
                totalRecebido,
                totalPendente,
                totalAtrasado
            );

            AdicionarTabelaContas(
                doc,
                dados
            );

            doc.Close();

            return caminho;
        }

        // =========================
        // PDF RECIBO
        // =========================

        private string CriarPdfRecibo(
            DataRow conta)
        {
            string caminho = ObterCaminhoArquivo(
                $"Recibo_{conta["id_conta_receber"]}",
                "Recibos"
            );

            // ALTERADO:
            // Página menor e margens reduzidas
            Document doc = new Document(
                PageSize.A5,
                25,
                25,
                25,
                25
            );

            PdfWriter.GetInstance(
                doc,
                new FileStream(caminho, FileMode.Create)
            );

            doc.Open();

            AdicionarCabecalhoRecibo(
                doc,
                "RECIBO DE PAGAMENTO"
            );

            AdicionarDadosRecibo(
                doc,
                conta
            );

            doc.Close();

            return caminho;
        }

        // =========================
        // CABEÇALHO RELATÓRIO
        // =========================

        private void AdicionarCabecalho(
            Document doc,
            string titulo)
        {
            Font fonteTitulo = new Font(
                Font.FontFamily.HELVETICA,
                26,
                Font.BOLD,
                BaseColor.WHITE
            );

            Font fonteSubtitulo = new Font(
                Font.FontFamily.HELVETICA,
                11,
                Font.NORMAL,
                BaseColor.WHITE
            );

            PdfPTable tabela = new PdfPTable(
                new float[] { 1, 4 });

            tabela.WidthPercentage = 100;

            try
            {
                byte[] logoBytes =
                    (byte[])new ImageConverter()
                    .ConvertTo(
                        Properties.Resources.logopng,
                        typeof(byte[])
                    );

                iTextSharp.text.Image logo =
                    iTextSharp.text.Image
                    .GetInstance(logoBytes);

                logo.ScaleToFit(80f, 80f);

                PdfPCell logoCell =
                    new PdfPCell(logo);

                logoCell.BackgroundColor =
                    corPrimaria;

                logoCell.Border =
                    Rectangle.NO_BORDER;

                logoCell.HorizontalAlignment =
                    Element.ALIGN_CENTER;

                logoCell.VerticalAlignment =
                    Element.ALIGN_MIDDLE;

                logoCell.Padding = 8;

                tabela.AddCell(logoCell);
            }
            catch
            {
                PdfPCell vazia =
                    new PdfPCell(
                        new Phrase("")
                    );

                vazia.BackgroundColor =
                    corPrimaria;

                vazia.Border =
                    Rectangle.NO_BORDER;

                tabela.AddCell(vazia);
            }

            PdfPCell textoCell =
                new PdfPCell();

            textoCell.BackgroundColor =
                corPrimaria;

            textoCell.Border =
                Rectangle.NO_BORDER;

            textoCell.Padding = 12;

            textoCell.VerticalAlignment =
                Element.ALIGN_MIDDLE;

            Paragraph nomeSistema =
                new Paragraph(
                    "ASSISTEC",
                    fonteTitulo
                );

            nomeSistema.SpacingAfter = 2;

            textoCell.AddElement(nomeSistema);

            textoCell.AddElement(
                new Paragraph(
                    titulo,
                    fonteSubtitulo
                )
            );

            tabela.AddCell(textoCell);

            doc.Add(tabela);

            doc.Add(new Paragraph(" "));
        }

        // =========================
        // CABEÇALHO RECIBO
        // =========================

        private void AdicionarCabecalhoRecibo(
            Document doc,
            string titulo)
        {
            Font fonteTitulo = new Font(
                Font.FontFamily.HELVETICA,
                18,
                Font.BOLD,
                BaseColor.WHITE
            );

            Font fonteSubtitulo = new Font(
                Font.FontFamily.HELVETICA,
                9,
                Font.NORMAL,
                BaseColor.WHITE
            );

            PdfPTable tabela =
                new PdfPTable(
                    new float[] { 1, 3 });

            tabela.WidthPercentage = 100;

            try
            {
                byte[] logoBytes =
                    (byte[])new ImageConverter()
                    .ConvertTo(
                        Properties.Resources.logopng,
                        typeof(byte[])
                    );

                iTextSharp.text.Image logo =
                    iTextSharp.text.Image
                    .GetInstance(logoBytes);

                logo.ScaleToFit(45f, 45f);

                PdfPCell logoCell =
                    new PdfPCell(logo);

                logoCell.BackgroundColor =
                    corPrimaria;

                logoCell.Border =
                    Rectangle.NO_BORDER;

                logoCell.HorizontalAlignment =
                    Element.ALIGN_CENTER;

                logoCell.VerticalAlignment =
                    Element.ALIGN_MIDDLE;

                logoCell.Padding = 5;

                tabela.AddCell(logoCell);
            }
            catch
            {
                PdfPCell vazia =
                    new PdfPCell(
                        new Phrase("")
                    );

                vazia.BackgroundColor =
                    corPrimaria;

                vazia.Border =
                    Rectangle.NO_BORDER;

                tabela.AddCell(vazia);
            }

            PdfPCell textoCell =
                new PdfPCell();

            textoCell.BackgroundColor =
                corPrimaria;

            textoCell.Border =
                Rectangle.NO_BORDER;

            textoCell.Padding = 8;

            textoCell.VerticalAlignment =
                Element.ALIGN_MIDDLE;

            Paragraph nomeSistema =
                new Paragraph(
                    "ASSISTEC",
                    fonteTitulo
                );

            nomeSistema.SpacingAfter = 1;

            textoCell.AddElement(nomeSistema);

            textoCell.AddElement(
                new Paragraph(
                    titulo,
                    fonteSubtitulo
                )
            );

            tabela.AddCell(textoCell);

            doc.Add(tabela);

            doc.Add(new Paragraph(" "));
        }

        // =========================
        // CARDS
        // =========================

        private void AdicionarCardsTotais(
            Document doc,
            decimal totalGeral,
            decimal totalRecebido,
            decimal totalPendente,
            decimal totalAtrasado)
        {
            Font fonteLabel = new Font(
                Font.FontFamily.HELVETICA,
                8,
                Font.NORMAL,
                BaseColor.WHITE
            );

            Font fonteValor = new Font(
                Font.FontFamily.HELVETICA,
                11,
                Font.BOLD,
                BaseColor.WHITE
            );

            PdfPTable tabela =
                new PdfPTable(4);

            tabela.WidthPercentage = 100;

            tabela.SetWidths(
                new float[]
                {
                    25f,
                    25f,
                    25f,
                    25f
                });

            AdicionarCard(
                tabela,
                "Total Geral",
                totalGeral,
                corHeader,
                fonteLabel,
                fonteValor
            );

            AdicionarCard(
                tabela,
                "Recebido",
                totalRecebido,
                corRecebido,
                fonteLabel,
                fonteValor
            );

            AdicionarCard(
                tabela,
                "Pendente",
                totalPendente,
                corPendente,
                fonteLabel,
                fonteValor
            );

            AdicionarCard(
                tabela,
                "Atrasado",
                totalAtrasado,
                corAtrasado,
                fonteLabel,
                fonteValor
            );

            doc.Add(tabela);

            doc.Add(new Paragraph(" "));
        }

        private void AdicionarCard(
            PdfPTable tabela,
            string label,
            decimal valor,
            BaseColor cor,
            Font fonteLabel,
            Font fonteValor)
        {
            PdfPCell card =
                new PdfPCell();

            card.BackgroundColor = cor;

            card.Border =
                Rectangle.NO_BORDER;

            card.Padding = 8f;

            card.AddElement(
                new Paragraph(
                    label,
                    fonteLabel
                )
            );

            card.AddElement(
                new Paragraph(
                    $"R$ {valor:N2}",
                    fonteValor
                )
            );

            tabela.AddCell(card);
        }

        // =========================
        // TABELA
        // =========================

        private void AdicionarTabelaContas(
            Document doc,
            DataTable dados)
        {
            Font fonteHeader = new Font(
                Font.FontFamily.HELVETICA,
                9,
                Font.BOLD,
                BaseColor.WHITE
            );

            Font fonteDados = new Font(
                Font.FontFamily.HELVETICA,
                9,
                Font.NORMAL,
                BaseColor.BLACK
            );

            PdfPTable tabela =
                new PdfPTable(
                    new float[]
                    {
                        3,
                        1.5f,
                        2,
                        2,
                        2,
                        2,
                        2,
                        2
                    });

            tabela.WidthPercentage = 100;

            string[] colunas =
            {
                "Descrição",
                "OS",
                "Valor",
                "Emissão",
                "Vencimento",
                "Pagamento",
                "Status",
                "Forma Pgto"
            };

            foreach (string coluna in colunas)
            {
                PdfPCell cell =
                    new PdfPCell(
                        new Phrase(
                            coluna,
                            fonteHeader
                        )
                    );

                cell.BackgroundColor =
                    corHeader;

                cell.Border =
                    Rectangle.NO_BORDER;

                cell.Padding = 8;

                cell.HorizontalAlignment =
                    Element.ALIGN_CENTER;

                tabela.AddCell(cell);
            }

            int linha = 0;

            foreach (DataRow row in dados.Rows)
            {
                BaseColor cor =
                    linha % 2 == 0
                    ? corLinha1
                    : corLinha2;

                AdicionarCelula(
                    tabela,
                    row["descricao"].ToString(),
                    fonteDados,
                    cor,
                    Element.ALIGN_LEFT
                );

                AdicionarCelula(
                    tabela,
                    row["id_os_fk"] != DBNull.Value
                        ? row["id_os_fk"].ToString()
                        : "-",
                    fonteDados,
                    cor
                );

                AdicionarCelula(
                    tabela,
                    $"R$ {Convert.ToDecimal(row["valor"]):N2}",
                    fonteDados,
                    cor
                );

                AdicionarCelula(
                    tabela,
                    FormatarData(
                        row["data_emissao"]
                    ),
                    fonteDados,
                    cor
                );

                AdicionarCelula(
                    tabela,
                    FormatarData(
                        row["data_vencimento"]
                    ),
                    fonteDados,
                    cor
                );

                AdicionarCelula(
                    tabela,
                    FormatarData(
                        row["data_pagamento"]
                    ),
                    fonteDados,
                    cor
                );

                AdicionarCelula(
                    tabela,
                    row["status"].ToString(),
                    fonteDados,
                    cor
                );

                AdicionarCelula(
                    tabela,
                    row["forma_pagamento"] != DBNull.Value
                        ? row["forma_pagamento"].ToString()
                        : "-",
                    fonteDados,
                    cor
                );

                linha++;
            }

            PdfPCell rodape =
                new PdfPCell(
                    new Phrase(
                        $"Total de registros: {linha}",
                        new Font(
                            Font.FontFamily.HELVETICA,
                            9,
                            Font.BOLD
                        )
                    )
                );

            rodape.Colspan = 8;

            rodape.BackgroundColor =
                corSecao;

            rodape.Border =
                Rectangle.NO_BORDER;

            rodape.Padding = 8;

            rodape.HorizontalAlignment =
                Element.ALIGN_RIGHT;

            tabela.AddCell(rodape);

            doc.Add(tabela);
        }

        // =========================
        // RECIBO
        // =========================

        private void AdicionarDadosRecibo(
            Document doc,
            DataRow conta)
        {
            Font fonteTitulo = new Font(
                Font.FontFamily.HELVETICA,
                18,
                Font.BOLD,
                corRecebido
            );

            Font fonteSubtitulo = new Font(
                Font.FontFamily.HELVETICA,
                9,
                Font.NORMAL,
                BaseColor.DARK_GRAY
            );

            Font fonteLabel = new Font(
                Font.FontFamily.HELVETICA,
                8,
                Font.BOLD,
                BaseColor.DARK_GRAY
            );

            Font fonteValor = new Font(
                Font.FontFamily.HELVETICA,
                9,
                Font.NORMAL,
                BaseColor.BLACK
            );

            Font fonteRodape = new Font(
                Font.FontFamily.HELVETICA,
                7,
                Font.ITALIC,
                BaseColor.GRAY
            );

            decimal valor =
                Convert.ToDecimal(
                    conta["valor"]
                );

            PdfPTable card =
                new PdfPTable(1);

            card.WidthPercentage = 100;

            PdfPCell cellTitulo =
                new PdfPCell();

            cellTitulo.BackgroundColor =
                new BaseColor(245, 248, 250);

            cellTitulo.BorderColor =
                new BaseColor(220, 220, 220);

            cellTitulo.BorderWidth = 1f;

            cellTitulo.Padding = 10f;

            Paragraph titulo =
                new Paragraph(
                    "RECIBO DE PAGAMENTO",
                    fonteTitulo
                );

            titulo.Alignment =
                Element.ALIGN_CENTER;

            titulo.SpacingAfter = 4f;

            cellTitulo.AddElement(titulo);

            Paragraph subtitulo =
                new Paragraph(
                    "Comprovante de recebimento financeiro",
                    fonteSubtitulo
                );

            subtitulo.Alignment =
                Element.ALIGN_CENTER;

            subtitulo.SpacingAfter = 10f;

            cellTitulo.AddElement(subtitulo);

            Paragraph valorRecebido =
                new Paragraph(
                    $"R$ {valor:N2}",
                    new Font(
                        Font.FontFamily.HELVETICA,
                        22,
                        Font.BOLD,
                        corRecebido
                    )
                );

            valorRecebido.Alignment =
                Element.ALIGN_CENTER;

            valorRecebido.SpacingAfter = 8f;

            cellTitulo.AddElement(valorRecebido);

            card.AddCell(cellTitulo);

            doc.Add(card);

            doc.Add(new Paragraph(" "));

            PdfPTable tabela =
                new PdfPTable(
                    new float[] { 2, 5 });

            tabela.WidthPercentage = 100;

            tabela.SpacingBefore = 2f;

            AdicionarLinhaDados(
                tabela,
                "Descrição",
                conta["descricao"].ToString(),
                fonteLabel,
                fonteValor
            );

            AdicionarLinhaDados(
                tabela,
                "OS",
                conta["id_os_fk"] != DBNull.Value
                    ? conta["id_os_fk"].ToString()
                    : "-",
                fonteLabel,
                fonteValor
            );

            AdicionarLinhaDados(
                tabela,
                "Vencimento",
                FormatarData(
                    conta["data_vencimento"]
                ),
                fonteLabel,
                fonteValor
            );

            AdicionarLinhaDados(
                tabela,
                "Pagamento",
                FormatarData(
                    conta["data_pagamento"]
                ),
                fonteLabel,
                fonteValor
            );

            AdicionarLinhaDados(
                tabela,
                "Forma Pgto",
                conta["forma_pagamento"] != DBNull.Value
                    ? conta["forma_pagamento"].ToString()
                    : "-",
                fonteLabel,
                fonteValor
            );

            doc.Add(tabela);

            // =========================
            // ASSINATURA
            // =========================

            Paragraph assinatura =
                new Paragraph(
                    "\n____________________________________\n" +
                    "AssisTec - Controle de Assistência Técnica",
                    fonteRodape
                );

            assinatura.Alignment =
                Element.ALIGN_CENTER;

            assinatura.SpacingBefore = 12f;

            doc.Add(assinatura);

            // =========================
            // DATA EMISSÃO
            // =========================

            Paragraph emissao =
                new Paragraph(
                    "Emitido em " +
                    DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                    fonteRodape
                );

            emissao.Alignment =
                Element.ALIGN_RIGHT;

            emissao.SpacingBefore = 5f;

            doc.Add(emissao);
        }

        // =========================
        // BANCO
        // =========================

        private DataRow BuscarConta(int idConta)
        {
            ContasReceberRepositoy contasReceberRepositoy = new ContasReceberRepositoy();                                                         
            
            DataTable dt = contasReceberRepositoy.carregarContaReceberdoBD(idConta);

            if (dt.Rows.Count == 0)
                return null;

            return dt.Rows[0];
        }

        // =========================
        // UTILITÁRIOS
        // =========================

        private void AdicionarCelula(
            PdfPTable tabela,
            string texto,
            Font fonte,
            BaseColor cor,
            int alinhamento =
                Element.ALIGN_CENTER)
        {
            PdfPCell cell =
                new PdfPCell(
                    new Phrase(
                        texto ?? "-",
                        fonte
                    )
                );

            cell.BackgroundColor = cor;

            cell.Border =
                Rectangle.NO_BORDER;

            cell.Padding = 7;

            cell.HorizontalAlignment =
                alinhamento;

            tabela.AddCell(cell);
        }

        private void AdicionarLinhaDados(
            PdfPTable tabela,
            string label,
            string valor,
            Font fonteLabel,
            Font fonteValor)
        {
            PdfPCell cellLabel =
                new PdfPCell(
                    new Phrase(
                        label,
                        fonteLabel
                    )
                );

            cellLabel.BackgroundColor =
                corLinha1;

            cellLabel.Border =
                Rectangle.NO_BORDER;

            cellLabel.Padding = 5;

            PdfPCell cellValor =
                new PdfPCell(
                    new Phrase(
                        valor ?? "-",
                        fonteValor
                    )
                );

            cellValor.BackgroundColor =
                corLinha2;

            cellValor.Border =
                Rectangle.NO_BORDER;

            cellValor.Padding = 5;

            tabela.AddCell(cellLabel);
            tabela.AddCell(cellValor);
        }

        private string FormatarData(
            object valor)
        {
            if (valor == DBNull.Value)
                return "-";

            DateTime data;

            if (DateTime.TryParse(
                valor.ToString(),
                out data))
            {
                return data
                    .ToString("dd/MM/yyyy");
            }

            return "-";
        }

        private void AbrirPdf(
            string caminho)
        {
            if (File.Exists(caminho))
            {
                Process.Start(caminho);
            }
        }

        private string ObterCaminhoArquivo(
            string nomeBase,
            string subPasta)
        {
            string pasta =
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.Desktop
                    ),

                    "Relatórios AssisTec",

                    DateTime.Now
                        .ToString("dd-MM-yyyy"),

                    subPasta
                );

            Directory.CreateDirectory(
                pasta
            );

            return Path.Combine(
                pasta,
                $"{nomeBase}_{DateTime.Now:HHmmss}.pdf"
            );
        }
    }
}
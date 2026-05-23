using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using AssisTec.Data;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

namespace AssisTec.Reports
{
    public class UsuarioRelatorio
    {
        private readonly UsuarioRepository repository;

        public UsuarioRelatorio()
        {
            repository = new UsuarioRepository();
        }

        // =========================
        // MÉTODOS PÚBLICOS
        // =========================

        public (bool sucesso, string mensagem, string caminho) GerarRelatorioGeral(
            string nomeFiltro = null,
            bool apenasInativos = false,
            int nivelSelecionado = 0)
        {
            try
            {
                DataTable dados = repository.ObterComFiltros(
                    nomeFiltro,
                    apenasInativos,
                    nivelSelecionado
                );

                if (dados.Rows.Count == 0)
                    return (false,
                        "Nenhum usuário encontrado com os filtros aplicados.",
                        null);

                string caminho = CriarPdfRelatorioGeral(dados);

                AbrirPdf(caminho);

                return (true,
                    "Relatório gerado com sucesso!",
                    caminho);
            }
            catch (Exception ex)
            {
                return (false,
                    $"Erro ao gerar relatório: {ex.Message}",
                    null);
            }
        }

        public (bool sucesso, string mensagem, string caminho)
            GerarRelatorioTecnico(int idTecnico)
        {
            try
            {
                Usuario tecnico = repository.ObterPorId(idTecnico);

                if (tecnico == null)
                    return (false, "Técnico não encontrado.", null);

                DataTable ordensServico =
                    repository.ObterHistoricoOs(idTecnico);

                string caminho =
                    CriarPdfRelatorioTecnico(tecnico, ordensServico);

                AbrirPdf(caminho);

                return (true,
                    "Relatório gerado com sucesso!",
                    caminho);
            }
            catch (Exception ex)
            {
                return (false,
                    $"Erro ao gerar relatório: {ex.Message}",
                    null);
            }
        }

        // =========================
        // RELATÓRIO GERAL
        // =========================

        private string CriarPdfRelatorioGeral(DataTable dados)
        {
            string caminho = ObterCaminhoArquivo(
                "Relatorio_Usuarios",
                "Todos Usuarios"
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

            AdicionarCabecalho(doc, "RELATÓRIO DE USUÁRIOS");

            AdicionarTabelaUsuarios(doc, dados);

            doc.Close();

            return caminho;
        }

        // =========================
        // RELATÓRIO TÉCNICO
        // =========================

        private string CriarPdfRelatorioTecnico(
            Usuario usuario,
            DataTable ordensServico)
        {
            string nomeArquivo =
                $"Relatorio_Usuario_" +
                $"{RemoverCaracteresInvalidos(usuario.nome)}_" +
                $"{DateTime.Now:ddMMyyyy_HHmm}";

            string caminho = ObterCaminhoArquivo(
                nomeArquivo,
                "Usuarios Individuais"
            );

            Document doc = new Document(
                PageSize.A4,
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

            if (usuario.nivel == 1)
            {
                AdicionarCabecalho(doc, "RELATÓRIO DO GERENTE");

                AdicionarDadosTecnico(doc, usuario);

                AdicionarOrdensServico(doc, ordensServico);
            }

            if (usuario.nivel == 2)
            {
                AdicionarCabecalho(doc, "RELATÓRIO DO ATENDENTE");

                AdicionarDadosTecnico(doc, usuario);
            }

            if (usuario.nivel == 3)
            {
                AdicionarCabecalho(doc, "RELATÓRIO DO TÉCNICO");

                AdicionarDadosTecnico(doc, usuario);

                AdicionarOrdensServico(doc, ordensServico);
            }

            

            doc.Close();

            return caminho;
        }

        // =========================
        // CABEÇALHO
        // =========================

        private void AdicionarCabecalho(Document doc, string titulo)
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

            BaseColor corPrimaria =
                new BaseColor(33, 97, 140);

            PdfPTable tabela = new PdfPTable(
                new float[] { 1, 4 });

            tabela.WidthPercentage = 100;

            // LOGO
            try
            {
                byte[] logoBytes =
                    (byte[])new ImageConverter()
                    .ConvertTo(
                        Properties.Resources.logopng,
                        typeof(byte[])
                    );

                iTextSharp.text.Image logo =
                    iTextSharp.text.Image.GetInstance(logoBytes);

                logo.ScaleToFit(80f, 80f);

                PdfPCell logoCell = new PdfPCell(logo);

                logoCell.BackgroundColor = corPrimaria;
                logoCell.Border = Rectangle.NO_BORDER;
                logoCell.HorizontalAlignment =
                    Element.ALIGN_CENTER;
                logoCell.VerticalAlignment =
                    Element.ALIGN_MIDDLE;
                logoCell.Padding = 8;

                tabela.AddCell(logoCell);
            }
            catch
            {
                PdfPCell vazia = new PdfPCell(
                    new Phrase(""));

                vazia.BackgroundColor = corPrimaria;
                vazia.Border = Rectangle.NO_BORDER;

                tabela.AddCell(vazia);
            }

            // TEXTO
            PdfPCell textoCell = new PdfPCell();

            textoCell.BackgroundColor = corPrimaria;
            textoCell.Border = Rectangle.NO_BORDER;
            textoCell.Padding = 12;
            textoCell.VerticalAlignment =
                Element.ALIGN_MIDDLE;

            Paragraph nomeSistema =
                new Paragraph("ASSISTEC", fonteTitulo);

            nomeSistema.SpacingAfter = 2;

            textoCell.AddElement(nomeSistema);

            textoCell.AddElement(
                new Paragraph(titulo, fonteSubtitulo));

            tabela.AddCell(textoCell);

            doc.Add(tabela);

            doc.Add(new Paragraph(" "));
        }

        // =========================
        // TABELA DE USUÁRIOS
        // =========================

        private void AdicionarTabelaUsuarios(
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

            BaseColor corHeader =
                new BaseColor(52, 73, 94);

            BaseColor corLinha1 =
                new BaseColor(245, 248, 250);

            BaseColor corLinha2 =
                BaseColor.WHITE;

            PdfPTable tabela = new PdfPTable(
                new float[] { 3, 2, 2, 1, 1, 3, 2 });

            tabela.WidthPercentage = 100;

            string[] colunas =
            {
                "Nome",
                "CPF",
                "Telefone",
                "Nível",
                "Status",
                "Cidade / Estado",
                "CEP"
            };

            foreach (string coluna in colunas)
            {
                PdfPCell cell = new PdfPCell(
                    new Phrase(coluna, fonteHeader));

                cell.BackgroundColor = corHeader;
                cell.Border = Rectangle.NO_BORDER;
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
                    row["nome"].ToString(),
                    fonteDados,
                    cor);

                AdicionarCelula(
                    tabela,
                    row["cpf"].ToString(),
                    fonteDados,
                    cor);

                AdicionarCelula(
                    tabela,
                    row["telefone"].ToString(),
                    fonteDados,
                    cor);

                AdicionarCelula(
                    tabela,
                    FormatarNivel(row["nivel"].ToString()),
                    fonteDados,
                    cor);

                AdicionarCelula(
                    tabela,
                    row["status"].ToString(),
                    fonteDados,
                    cor);

                AdicionarCelula(
                    tabela,
                    row["cidade"] + " / " + row["estado"],
                    fonteDados,
                    cor);

                AdicionarCelula(
                    tabela,
                    row["cep"].ToString(),
                    fonteDados,
                    cor);

                linha++;
            }

            PdfPCell rodape = new PdfPCell(
                new Phrase(
                    $"Total de usuários: {linha}",
                    new Font(
                        Font.FontFamily.HELVETICA,
                        9,
                        Font.BOLD
                    )));

            rodape.Colspan = 7;
            rodape.BackgroundColor =
                new BaseColor(214, 234, 248);

            rodape.Border = Rectangle.NO_BORDER;
            rodape.Padding = 8;

            rodape.HorizontalAlignment =
                Element.ALIGN_RIGHT;

            tabela.AddCell(rodape);

            doc.Add(tabela);
        }

        // =========================
        // DADOS DO TÉCNICO
        // =========================

        private void AdicionarDadosTecnico(
            Document doc,
            Usuario tecnico)
        {
            Font fonteSecao = new Font(
                Font.FontFamily.HELVETICA,
                11,
                Font.BOLD);

            Font fonteLabel = new Font(
                Font.FontFamily.HELVETICA,
                9,
                Font.BOLD);

            Font fonteValor = new Font(
                Font.FontFamily.HELVETICA,
                9,
                Font.NORMAL);

            BaseColor corSecao =
                new BaseColor(214, 234, 248);

            BaseColor corLinha1 =
                new BaseColor(245, 248, 250);

            BaseColor corLinha2 =
                BaseColor.WHITE;

            PdfPTable secao = new PdfPTable(1);

            secao.WidthPercentage = 100;

            PdfPCell secaoCell = new PdfPCell(
                new Phrase(
                    "DADOS DO TÉCNICO",
                    fonteSecao));

            secaoCell.BackgroundColor = corSecao;
            secaoCell.Border = Rectangle.NO_BORDER;
            secaoCell.Padding = 8;

            secao.AddCell(secaoCell);

            doc.Add(secao);

            PdfPTable tabela =
                new PdfPTable(new float[] { 2, 5 });

            tabela.WidthPercentage = 100;

            AdicionarLinhaDados(
                tabela,
                "Nome",
                tecnico.nome,
                fonteLabel,
                fonteValor,
                corLinha1);

            AdicionarLinhaDados(
                tabela,
                "CPF",
                tecnico.cpf,
                fonteLabel,
                fonteValor,
                corLinha2);

            AdicionarLinhaDados(
                tabela,
                "Telefone",
                tecnico.telefone,
                fonteLabel,
                fonteValor,
                corLinha1);

            AdicionarLinhaDados(
                tabela,
                "Status",
                tecnico.status,
                fonteLabel,
                fonteValor,
                corLinha2);

            AdicionarLinhaDados(
                tabela,
                "Endereço",
                $"{tecnico.rua}, {tecnico.numero} - {tecnico.bairro}",
                fonteLabel,
                fonteValor,
                corLinha1);

            AdicionarLinhaDados(
                tabela,
                "Cidade",
                $"{tecnico.cidade} / {tecnico.estado}",
                fonteLabel,
                fonteValor,
                corLinha2);

            AdicionarLinhaDados(
                tabela,
                "CEP",
                tecnico.cep,
                fonteLabel,
                fonteValor,
                corLinha1);

            AdicionarLinhaDados(
                tabela,
                "Complemento",
                tecnico.complemento,
                fonteLabel,
                fonteValor,
                corLinha2);

            doc.Add(tabela);

            doc.Add(new Paragraph(" "));
        }

        // =========================
        // ORDENS DE SERVIÇO
        // =========================

        private void AdicionarOrdensServico(
            Document doc,
            DataTable ordensServico)
        {
            Font fonteSecao = new Font(
                Font.FontFamily.HELVETICA,
                11,
                Font.BOLD);

            Font fonteHeader = new Font(
                Font.FontFamily.HELVETICA,
                9,
                Font.BOLD,
                BaseColor.WHITE);

            Font fonteDados = new Font(
                Font.FontFamily.HELVETICA,
                8,
                Font.NORMAL);

            BaseColor corSecao =
                new BaseColor(214, 234, 248);

            BaseColor corHeader =
                new BaseColor(52, 73, 94);

            BaseColor corLinha1 =
                new BaseColor(245, 248, 250);

            BaseColor corLinha2 =
                BaseColor.WHITE;

            PdfPTable secao = new PdfPTable(1);

            secao.WidthPercentage = 100;

            PdfPCell secaoCell = new PdfPCell(
                new Phrase(
                    "ORDENS DE SERVIÇO",
                    fonteSecao));

            secaoCell.BackgroundColor = corSecao;
            secaoCell.Border = Rectangle.NO_BORDER;
            secaoCell.Padding = 8;

            secao.AddCell(secaoCell);

            doc.Add(secao);

            doc.Add(new Paragraph(" "));

            if (ordensServico.Rows.Count == 0)
            {
                doc.Add(new Paragraph(
                    "Nenhuma ordem de serviço encontrada.",
                    fonteDados));

                return;
            }

            PdfPTable tabela = new PdfPTable(
                new float[] { 1, 3, 2, 2, 2, 2 });

            tabela.WidthPercentage = 100;

            string[] colunas =
            {
                "ID",
                "Equipamento",
                "Defeito",
                "Status",
                "Entrada",
                "Valor"
            };

            foreach (string coluna in colunas)
            {
                PdfPCell cell = new PdfPCell(
                    new Phrase(coluna, fonteHeader));

                cell.BackgroundColor = corHeader;
                cell.Border = Rectangle.NO_BORDER;
                cell.Padding = 7;
                cell.HorizontalAlignment =
                    Element.ALIGN_CENTER;

                tabela.AddCell(cell);
            }

            decimal valorTotal = 0;

            int linha = 0;

            foreach (DataRow row in ordensServico.Rows)
            {
                BaseColor cor =
                    linha % 2 == 0
                    ? corLinha1
                    : corLinha2;

                decimal valor = 0;

                decimal.TryParse(
                    row["valor"].ToString(),
                    out valor);

                valorTotal += valor;

                AdicionarCelula(
                    tabela,
                    row["id"].ToString(),
                    fonteDados,
                    cor);

                AdicionarCelula(
                    tabela,
                    row["equipamento"].ToString(),
                    fonteDados,
                    cor);

                AdicionarCelula(
                    tabela,
                    row["defeito"].ToString(),
                    fonteDados,
                    cor);

                AdicionarCelula(
                    tabela,
                    row["status"].ToString(),
                    fonteDados,
                    cor);

                AdicionarCelula(
                    tabela,
                    Convert.ToDateTime(row["data_entrada"])
                        .ToString("dd/MM/yyyy"),
                    fonteDados,
                    cor);

                AdicionarCelula(
                    tabela,
                    valor.ToString("C2"),
                    fonteDados,
                    cor);

                linha++;
            }

            PdfPCell rodape = new PdfPCell(
                new Phrase(
                    $"Total de OS: {linha}   |   Valor Total: {valorTotal:C2}",
                    new Font(
                        Font.FontFamily.HELVETICA,
                        9,
                        Font.BOLD)));

            rodape.Colspan = 6;
            rodape.BackgroundColor = corSecao;
            rodape.Border = Rectangle.NO_BORDER;
            rodape.Padding = 8;
            rodape.HorizontalAlignment =
                Element.ALIGN_RIGHT;

            tabela.AddCell(rodape);

            doc.Add(tabela);
        }

        // =========================
        // UTILITÁRIOS
        // =========================

        private void AdicionarCelula(
            PdfPTable tabela,
            string texto,
            Font fonte,
            BaseColor cor)
        {
            PdfPCell cell = new PdfPCell(
                new Phrase(texto ?? "-", fonte));

            cell.BackgroundColor = cor;
            cell.Border = Rectangle.NO_BORDER;
            cell.Padding = 7;
            cell.HorizontalAlignment =
                Element.ALIGN_CENTER;

            tabela.AddCell(cell);
        }

        private void AdicionarLinhaDados(
            PdfPTable tabela,
            string label,
            string valor,
            Font fonteLabel,
            Font fonteValor,
            BaseColor cor)
        {
            PdfPCell cellLabel = new PdfPCell(
                new Phrase(label, fonteLabel));

            cellLabel.BackgroundColor = cor;
            cellLabel.Border = Rectangle.NO_BORDER;
            cellLabel.Padding = 6;

            PdfPCell cellValor = new PdfPCell(
                new Phrase(valor ?? "-", fonteValor));

            cellValor.BackgroundColor = cor;
            cellValor.Border = Rectangle.NO_BORDER;
            cellValor.Padding = 6;

            tabela.AddCell(cellLabel);
            tabela.AddCell(cellValor);
        }

        private void AbrirPdf(string caminho)
        {
            if (File.Exists(caminho))
            {
                Process.Start(caminho);
            }
        }

        private string FormatarNivel(string nivel)
        {
            switch (nivel)
            {
                case "1":
                    return "Administrador";

                case "2":
                    return "Técnico";

                case "3":
                    return "Atendente";

                default:
                    return nivel;
            }
        }

        private string ObterCaminhoArquivo(
            string nomeBase,
            string subPasta)
        {
            string pasta = Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.Desktop),

                "Relatórios AssisTec",
                DateTime.Now.ToString("dd-MM-yyyy"),
                subPasta
            );

            Directory.CreateDirectory(pasta);

            return Path.Combine(
                pasta,
                $"{nomeBase}.pdf");
        }

        private string RemoverCaracteresInvalidos(string texto)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                texto = texto.Replace(c.ToString(), "");
            }

            return texto;
        }
    }
}
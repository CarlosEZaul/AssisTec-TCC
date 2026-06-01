using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using AssisTec.Repository;
using AssisTec.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Font = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

namespace AssisTec.Reports
{
    public class UsuarioRelatorio
    {
        private readonly IUsuarioReposity repository;

        public UsuarioRelatorio(IUsuarioReposity _repository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        public (bool sucesso, string mensagem, string caminho) GerarRelatorioGeral(
            string nomeFiltro = null,
            bool apenasInativos = false,
            int nivelSelecionado = 0)
        {
            try
            {
                List<Usuario> listaUsuarios = repository.ObterComFiltros(
                    nomeFiltro,
                    apenasInativos,
                    nivelSelecionado
                );

                if (listaUsuarios == null || listaUsuarios.Count == 0)
                    return (false, "Nenhum usuário encontrado com os filtros aplicados.", null);

                string caminho = CriarPdfRelatorioGeral(listaUsuarios);
                AbrirPdf(caminho);

                return (true, "Relatório gerado com sucesso!", caminho);
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao gerar relatório: {ex.Message}", null);
            }
        }

        public (bool sucesso, string mensagem, string caminho) GerarRelatorioTecnico(int idTecnico)
        {
            try
            {
                Usuario tecnico = repository.ObterPorId(idTecnico);

                if (tecnico == null)
                    return (false, "Técnico não encontrado.", null);

                //DataTable ordensServico = repository.ObterHistoricoOs(idTecnico);

                //string caminho = CriarPdfRelatorioTecnico(tecnico, ordensServico);
                //AbrirPdf(caminho);

                return (true, "Relatório gerado com sucesso!", "caminho");
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao gerar relatório: {ex.Message}", null);
            }
        }

        private string CriarPdfRelatorioGeral(List<Usuario> dados)
        {
            string caminho = ObterCaminhoArquivo("Relatorio_Usuarios", "Todos Usuarios");

            Document doc = new Document(PageSize.A4.Rotate(), 40, 40, 40, 40);
            PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));

            doc.Open();
            AdicionarCabecalho(doc, "RELATÓRIO DE USUÁRIOS");
            AdicionarTabelaUsuarios(doc, dados);
            doc.Close();

            return caminho;
        }

        private string CriarPdfRelatorioTecnico(Usuario usuario, DataTable ordensServico)
        {
            string nomeArquivo = $"Relatorio_Usuario_{RemoverCaracteresInvalidos(usuario.Nome)}_{DateTime.Now:ddMMyyyy_HHmm}";
            string caminho = ObterCaminhoArquivo(nomeArquivo, "Usuarios Individuais");

            Document doc = new Document(PageSize.A4, 40, 40, 40, 40);
            PdfWriter.GetInstance(doc, new FileStream(caminho, FileMode.Create));

            doc.Open();

            if (usuario.Nivel == 1)
            {
                AdicionarCabecalho(doc, "RELATÓRIO DO GERENTE");
                AdicionarDadosTecnico(doc, usuario);
                AdicionarOrdensServico(doc, ordensServico);
            }
            else if (usuario.Nivel == 2)
            {
                AdicionarCabecalho(doc, "RELATÓRIO DO ATENDENTE");
                AdicionarDadosTecnico(doc, usuario);
            }
            else if (usuario.Nivel == 3)
            {
                AdicionarCabecalho(doc, "RELATÓRIO DO TÉCNICO");
                AdicionarDadosTecnico(doc, usuario);
                AdicionarOrdensServico(doc, ordensServico);
            }

            doc.Close();
            return caminho;
        }

        private void AdicionarCabecalho(Document doc, string titulo)
        {
            Font fonteTitulo = new Font(Font.FontFamily.HELVETICA, 26, Font.BOLD, BaseColor.WHITE);
            Font fonteSubtitulo = new Font(Font.FontFamily.HELVETICA, 11, Font.NORMAL, BaseColor.WHITE);
            BaseColor corPrimaria = new BaseColor(33, 97, 140);

            PdfPTable tabela = new PdfPTable(new float[] { 1, 4 }) { WidthPercentage = 100 };

            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    Properties.Resources.logopng.Save(ms, ImageFormat.Png);
                    byte[] logoBytes = ms.ToArray();
                    iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoBytes);
                    logo.ScaleToFit(80f, 80f);

                    PdfPCell logoCell = new PdfPCell(logo)
                    {
                        BackgroundColor = corPrimaria,
                        Border = Rectangle.NO_BORDER,
                        HorizontalAlignment = Element.ALIGN_CENTER,
                        VerticalAlignment = Element.ALIGN_MIDDLE,
                        Padding = 8
                    };
                    tabela.AddCell(logoCell);
                }
            }
            catch
            {
                PdfPCell vazia = new PdfPCell(new Phrase(""))
                {
                    BackgroundColor = corPrimaria,
                    Border = Rectangle.NO_BORDER
                };
                tabela.AddCell(vazia);
            }

            PdfPCell textoCell = new PdfPCell()
            {
                BackgroundColor = corPrimaria,
                Border = Rectangle.NO_BORDER,
                Padding = 12,
                VerticalAlignment = Element.ALIGN_MIDDLE
            };

            Paragraph nomeSistema = new Paragraph("ASSISTEC", fonteTitulo);
            nomeSistema.SpacingAfter = 2;
            textoCell.AddElement(nomeSistema);
            textoCell.AddElement(new Paragraph(titulo, fonteSubtitulo));

            tabela.AddCell(textoCell);
            doc.Add(tabela);
            doc.Add(new Paragraph(" "));
        }

        private void AdicionarTabelaUsuarios(Document doc, List<Usuario> dados)
        {
            Font fonteHeader = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.WHITE);
            Font fonteDados = new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL, BaseColor.BLACK);

            BaseColor corHeader = new BaseColor(52, 73, 94);
            BaseColor corLinha1 = new BaseColor(245, 248, 250);
            BaseColor corLinha2 = BaseColor.WHITE;

            PdfPTable tabela = new PdfPTable(new float[] { 3, 2, 2, 1, 1, 3, 2 }) { WidthPercentage = 100 };
            string[] colunas = { "Nome", "CPF", "Telefone", "Nível", "Status", "Cidade / Estado", "CEP" };

            foreach (string coluna in colunas)
            {
                PdfPCell cell = new PdfPCell(new Phrase(coluna, fonteHeader))
                {
                    BackgroundColor = corHeader,
                    Border = Rectangle.NO_BORDER,
                    Padding = 8,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                tabela.AddCell(cell);
            }

            int linha = 0;
            foreach (Usuario user in dados)
            {
                BaseColor cor = (linha % 2 == 0) ? corLinha1 : corLinha2;

                AdicionarCelula(tabela, user.Nome, fonteDados, cor);
                AdicionarCelula(tabela, user.Cpf, fonteDados, cor);
                AdicionarCelula(tabela, user.Telefone, fonteDados, cor);
                AdicionarCelula(tabela, FormatarNivel(user.Nivel.ToString()), fonteDados, cor);
                AdicionarCelula(tabela, user.Status, fonteDados, cor);
                AdicionarCelula(tabela, $"{user.Cidade} / {user.Estado}", fonteDados, cor);
                AdicionarCelula(tabela, user.Cep, fonteDados, cor);

                linha++;
            }

            PdfPCell rodape = new PdfPCell(new Phrase($"Total de usuários: {linha}", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD)))
            {
                Colspan = 7,
                BackgroundColor = new BaseColor(214, 234, 248),
                Border = Rectangle.NO_BORDER,
                Padding = 8,
                HorizontalAlignment = Element.ALIGN_RIGHT
            };

            tabela.AddCell(rodape);
            doc.Add(tabela);
        }

        private void AdicionarDadosTecnico(Document doc, Usuario tecnico)
        {
            Font fonteSecao = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD);
            Font fonteLabel = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD);
            Font fonteValor = new Font(Font.FontFamily.HELVETICA, 9, Font.NORMAL);

            BaseColor corSecao = new BaseColor(214, 234, 248);
            BaseColor corLinha1 = new BaseColor(245, 248, 250);
            BaseColor corLinha2 = BaseColor.WHITE;

            PdfPTable secao = new PdfPTable(1) { WidthPercentage = 100 };
            PdfPCell secaoCell = new PdfPCell(new Phrase("DADOS DO USUÁRIO", fonteSecao))
            {
                BackgroundColor = corSecao,
                Border = Rectangle.NO_BORDER,
                Padding = 8
            };
            secao.AddCell(secaoCell);
            doc.Add(secao);

            PdfPTable tabela = new PdfPTable(new float[] { 2, 5 }) { WidthPercentage = 100 };

            AdicionarLinhaDados(tabela, "Nome", tecnico.Nome, fonteLabel, fonteValor, corLinha1);
            AdicionarLinhaDados(tabela, "CPF", tecnico.Cpf, fonteLabel, fonteValor, corLinha2);
            AdicionarLinhaDados(tabela, "Telefone", tecnico.Telefone, fonteLabel, fonteValor, corLinha1);
            AdicionarLinhaDados(tabela, "Status", tecnico.Status, fonteLabel, fonteValor, corLinha2);
            AdicionarLinhaDados(tabela, "Endereço", $"{tecnico.Rua}, {tecnico.Numero} - {tecnico.Bairro}", fonteLabel, fonteValor, corLinha1);
            AdicionarLinhaDados(tabela, "Cidade", $"{tecnico.Cidade} / {tecnico.Estado}", fonteLabel, fonteValor, corLinha2);
            AdicionarLinhaDados(tabela, "CEP", tecnico.Cep, fonteLabel, fonteValor, corLinha1);
            AdicionarLinhaDados(tabela, "Complemento", tecnico.Complemento, fonteLabel, fonteValor, corLinha2);

            doc.Add(tabela);
            doc.Add(new Paragraph(" "));
        }

        private void AdicionarOrdensServico(Document doc, DataTable ordensServico)
        {
            Font fonteSecao = new Font(Font.FontFamily.HELVETICA, 11, Font.BOLD);
            Font fonteHeader = new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD, BaseColor.WHITE);
            Font fonteDados = new Font(Font.FontFamily.HELVETICA, 8, Font.NORMAL);

            BaseColor corSecao = new BaseColor(214, 234, 248);
            BaseColor corHeader = new BaseColor(52, 73, 94);
            BaseColor corLinha1 = new BaseColor(245, 248, 250);
            BaseColor corLinha2 = BaseColor.WHITE;

            PdfPTable secao = new PdfPTable(1) { WidthPercentage = 100 };
            PdfPCell secaoCell = new PdfPCell(new Phrase("HISTÓRICO DE ORDENS DE SERVIÇO", fonteSecao))
            {
                BackgroundColor = corSecao,
                Border = Rectangle.NO_BORDER,
                Padding = 8
            };
            secao.AddCell(secaoCell);
            doc.Add(secao);
            doc.Add(new Paragraph(" "));

            if (ordensServico == null || ordensServico.Rows.Count == 0)
            {
                doc.Add(new Paragraph("Nenhuma ordem de serviço vinculada a este perfil.", fonteDados));
                return;
            }

            PdfPTable tabela = new PdfPTable(new float[] { 1, 3, 2, 2, 2, 2 }) { WidthPercentage = 100 };
            string[] colunas = { "ID", "Equipamento", "Defeito", "Status", "Entrada", "Valor" };

            foreach (string coluna in colunas)
            {
                PdfPCell cell = new PdfPCell(new Phrase(coluna, fonteHeader))
                {
                    BackgroundColor = corHeader,
                    Border = Rectangle.NO_BORDER,
                    Padding = 7,
                    HorizontalAlignment = Element.ALIGN_CENTER
                };
                tabela.AddCell(cell);
            }

            decimal valorTotal = 0;
            int linha = 0;

            foreach (DataRow row in ordensServico.Rows)
            {
                BaseColor cor = (linha % 2 == 0) ? corLinha1 : corLinha2;
                decimal.TryParse(row["valor"].ToString(), out decimal valor);
                valorTotal += valor;

                AdicionarCelula(tabela, row["id"].ToString(), fonteDados, cor);
                AdicionarCelula(tabela, row["equipamento"].ToString(), fonteDados, cor);
                AdicionarCelula(tabela, row["defeito"].ToString(), fonteDados, cor);
                AdicionarCelula(tabela, row["status"].ToString(), fonteDados, cor);
                
                string dataFormatada = DateTime.TryParse(row["data_entrada"].ToString(), out DateTime dt) 
                    ? dt.ToString("dd/MM/yyyy") 
                    : row["data_entrada"].ToString();
                
                AdicionarCelula(tabela, dataFormatada, fonteDados, cor);
                AdicionarCelula(tabela, valor.ToString("C2"), fonteDados, cor);

                linha++;
            }

            PdfPCell rodape = new PdfPCell(new Phrase($"Total de OS: {linha}   |   Valor Total: {valorTotal:C2}", new Font(Font.FontFamily.HELVETICA, 9, Font.BOLD)))
            {
                Colspan = 6,
                BackgroundColor = corSecao,
                Border = Rectangle.NO_BORDER,
                Padding = 8,
                HorizontalAlignment = Element.ALIGN_RIGHT
            };

            tabela.AddCell(rodape);
            doc.Add(tabela);
        }

        private void AdicionarCelula(PdfPTable tabela, string texto, Font fonte, BaseColor cor)
        {
            PdfPCell cell = new PdfPCell(new Phrase(texto ?? "-", fonte))
            {
                BackgroundColor = cor,
                Border = Rectangle.NO_BORDER,
                Padding = 7,
                HorizontalAlignment = Element.ALIGN_CENTER
            };
            tabela.AddCell(cell);
        }

        private void AdicionarLinhaDados(PdfPTable tabela, string label, string valor, Font fonteLabel, Font fonteValor, BaseColor cor)
        {
            PdfPCell cellLabel = new PdfPCell(new Phrase(label, fonteLabel))
            {
                BackgroundColor = cor,
                Border = Rectangle.NO_BORDER,
                Padding = 6
            };

            PdfPCell cellValor = new PdfPCell(new Phrase(valor ?? "-", fonteValor))
            {
                BackgroundColor = cor,
                Border = Rectangle.NO_BORDER,
                Padding = 6
            };

            tabela.AddCell(cellLabel);
            tabela.AddCell(cellValor);
        }

        private void AbrirPdf(string caminho)
        {
            if (File.Exists(caminho))
            {
                Process.Start(new ProcessStartInfo(caminho) { UseShellExecute = true });
            }
        }

        private string FormatarNivel(string nivel)
        {
            switch (nivel)
            {
                case "1": return "Gerente";
                case "2": return "Atendente";
                case "3": return "Técnico";
                default: return nivel;
            }
        }

        private string ObterCaminhoArquivo(string nomeBase, string subPasta)
        {
            string pasta = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "Relatórios AssisTec",
                DateTime.Now.ToString("dd-MM-yyyy"),
                subPasta
            );

            Directory.CreateDirectory(pasta);
            return Path.Combine(pasta, $"{nomeBase}.pdf");
        }

        private string RemoverCaracteresInvalidos(string texto)
        {
            if (string.IsNullOrEmpty(texto)) return "Sem_Nome";
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                texto = texto.Replace(c.ToString(), "");
            }
            return texto;
        }
    }
}
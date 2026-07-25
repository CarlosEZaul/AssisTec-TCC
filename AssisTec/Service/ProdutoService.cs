using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using AssisTec.Dtos;
using AssisTec.Models;
using AssisTec.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace AssisTec.Service
{
    public class ProdutoService 
    {
        private readonly IProdutoRepository repository;
        
        public ProdutoService(IProdutoRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<Produto> ObterProdutos()
        {
            return repository.ObterProdutos();
        }

        public Produto ObterProdutoPorId(int id)
        {
            if(id < 0) return null;
            return repository.ObterProdutoPorId(id);
        }
        

        public bool Salvar(Produto produto)
        {
            ValidarCampos(produto);

            var Inserir = repository.InserirProduto(produto);

            if (Inserir)
            {
                return true;
            }
            else
            {
                throw new Exception("Produto não foi salvo");
                return false;
            }
            
        }

        public bool atualizarProduto(Produto produto)
        {
            ValidarCampos(produto);
            
            var atualizar = repository.AtualizarProduto(produto);

            if (atualizar)
            {
                return true;
            }
            else
            {
                throw new Exception("Falha ao  atualizar Produto");
            }
        }

        public bool excluirProduto(Produto produto)
        {
            if(produto.idProduto < 0)
            {
                throw new ArgumentNullException("Produto nulo");
            }
            var excluir = repository.ExcluirProduto(produto.idProduto);

            if (excluir)
            {
                return true;
            }
            else
            {
                throw new Exception("Falha ao deletar Produto");
            }
            
            
        }

        public bool alterarStatus(int id)
        {
            try
            {
                return repository.alterarStatus(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao alterar Status", ex);
            }
        }

        public bool darEntradaProduto(int id, int quantidade)
        {
            try
            {
                return repository.darEntradaProduto(id, quantidade);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao dar entrada no produto", e);
            }
        }

        public bool darSaidaProduto(int id, int quantidade)
        {
            try
            {
                return repository.darSaidaProduto(id, quantidade);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao dar saida no produto", e);
            }
        }

        public (int totalCadastrado, int abaixoMinimo, int semEstoque, decimal valorEstoque) obterTotais()
        {
            return repository.obterTotais(new Produto());
        }

        public (DataTable dados, int totalCadastrado, int abaixoMinimo, int semEstoque, decimal valorEstoque) Filtrar(string descricao, bool abaixoMinimo, bool semEstoque, bool desativados)
        {
            var filtro = new Produto()
            {
                filtroDescricao = descricao?.Trim(),
                filtroAbaixoMinimo = abaixoMinimo,
                filtroSemEstoque = semEstoque,
                filtroProdutosDesativados = desativados
            };
    
            var dados = repository.Filtrar(filtro);
            var totais = repository.obterTotais(filtro);
    
            return (dados, totais.totalCadastrado, totais.abaixoMinimo, totais.semEstoque, totais.valorEstoque);
        }

        public object obterDescricaoProdutos()
        {
            try
            {
                return repository.ObterDescricaoProdutos();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        

        private bool ValidarCampos(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException("produto nulo");
            }
            if (string.IsNullOrWhiteSpace(produto.descricao))
            {
                throw new ArgumentNullException("Descrição nula");
            }
            if (produto.preco_compra <0)
            {
                throw new ArgumentNullException("Preço inválido");
            }

            if (produto.preco_venda < 0)
            {
                throw new ArgumentNullException("Preço inválido");
            }
            if (produto.quantidade <0)
            {
                throw new ArgumentNullException("Quantidade não pode ser menor que 0");
            }

            if (string.IsNullOrWhiteSpace(produto.unidade))
            {
                throw new ArgumentNullException("Unidade nula");
            }
            if (produto.quantidade_minima <0)
            {
                throw new ArgumentNullException("Quantidade minima não pode ser menor que 0");
            }

            return true;

        }

        public DataTable ProdutosAbaixoMinimo()
        {
            try
            {
                return repository.ProdutosAbaixoMinimo();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao dar abaixo do produto", e);
            }
        }
        
        public void GerarRelatorioEstoquePdf(Produto filtro, string caminhoDestino)
        {
            try
            {
                DataTable tabelaProdutos = repository.Filtrar(filtro);
                var totais = repository.obterTotais(filtro);

                List<string> filtrosAtivos = new List<string>();
                
                if (filtro.filtroAbaixoMinimo)
                {
                    filtrosAtivos.Add("Abaixo do Mínimo");
                }
                if (filtro.filtroSemEstoque)
                {
                    filtrosAtivos.Add("Sem Estoque");
                }
                
                string statusDescricao = filtro.filtroProdutosDesativados ? "Inativos / Todos" : "Ativos";

                ProdutoDTO.EstoqueRelatorioDTO relatorio = new ProdutoDTO.EstoqueRelatorioDTO
                {
                    FiltroDescricao = string.IsNullOrEmpty(filtro.filtroDescricao) ? "Todos" : filtro.filtroDescricao,
                    FiltroStatus = filtrosAtivos.Count > 0 
                        ? $"{statusDescricao} ({string.Join(", ", filtrosAtivos)})" 
                        : statusDescricao,
                    TotalCadastrado = totais.totalCadastrado,
                    AbaixoMinimo = totais.abaixoMinimo,
                    SemEstoque = totais.semEstoque,
                    ValorEstoque = totais.valorEstoque,
                    Itens = new List<ProdutoDTO.ProdutoRelatorioDTO>()
                };

                foreach (DataRow row in tabelaProdutos.Rows)
                {
                    relatorio.Itens.Add(new ProdutoDTO.ProdutoRelatorioDTO
                    {
                        IdProduto = Convert.ToInt32(row["ID_PRODUTO"]),
                        Descricao = row["DESCRIÇÃO"].ToString(),
                        Unidade = row["UNIDADE"].ToString(),
                        PrecoVenda = Convert.ToDecimal(row["PREÇO_VENDA"]),
                        PrecoCompra = Convert.ToDecimal(row["PREÇO_COMPRA"]),
                        Quantidade = Convert.ToInt32(row["QUANTIDADE"]),
                        QuantidadeMinima = Convert.ToInt32(row["QUANTIDADE_MINIMA"]),
                        Status = row["STATUS"].ToString()
                    });
                }

                ExecutarGeracaoPdfEstoque(relatorio, caminhoDestino);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao gerar o relatório de estoque em PDF.", ex);
            }
        }

        private void ExecutarGeracaoPdfEstoque(ProdutoDTO.EstoqueRelatorioDTO dados, string caminhoDestino)
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
            cellLeft.AddElement(new Paragraph("Relatório de Controle de Estoque", fontSubtitulo));
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

            PdfPTable filterTable = new PdfPTable(4);
            filterTable.WidthPercentage = 100;
            filterTable.SetWidths(new float[] { 18f, 32f, 18f, 32f });
            filterTable.SpacingBefore = 5f;
            filterTable.SpacingAfter = 15f;

            string[,] filtros = {
                { "Descrição:", dados.FiltroDescricao, "Status/Estoque:", dados.FiltroStatus }
            };

            for (int j = 0; j < 4; j++)
            {
                bool isLabel = j % 2 == 0;
                PdfPCell cell = new PdfPCell(new Phrase(filtros[0, j], isLabel ? fontBold : fontRegular));
                cell.BackgroundColor = new BaseColor(247, 250, 252);
                cell.BorderColor = new BaseColor(237, 242, 247);
                cell.Padding = 6;
                filterTable.AddCell(cell);
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

            PdfPTable dataTable = new PdfPTable(8);
            dataTable.WidthPercentage = 100;
            dataTable.SetWidths(new float[] { 8f, 32f, 8f, 13f, 13f, 9f, 9f, 8f });
            dataTable.SpacingBefore = 5f;

            string[] headers = { "ID", "Descrição", "Unid.", "Preço Compra", "Preço Venda", "Qtd.", "Min.", "Status" };
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
                dataTable.AddCell(new PdfPCell(new Phrase(item.IdProduto.ToString(), fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                dataTable.AddCell(new PdfPCell(new Phrase(item.Descricao, fontRegular)) { Padding = 6, BorderColor = new BaseColor(226, 232, 240) });
                dataTable.AddCell(new PdfPCell(new Phrase(item.Unidade, fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_CENTER, BorderColor = new BaseColor(226, 232, 240) });
                dataTable.AddCell(new PdfPCell(new Phrase(item.PrecoCompra.ToString("C2"), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });
                dataTable.AddCell(new PdfPCell(new Phrase(item.PrecoVenda.ToString("C2"), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });
                dataTable.AddCell(new PdfPCell(new Phrase(item.Quantidade.ToString(), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });
                dataTable.AddCell(new PdfPCell(new Phrase(item.QuantidadeMinima.ToString(), fontRegular)) { Padding = 6, HorizontalAlignment = Element.ALIGN_RIGHT, BorderColor = new BaseColor(226, 232, 240) });

                bool ativo = item.Status.Equals("Ativo", StringComparison.OrdinalIgnoreCase);
                PdfPCell statusCell = new PdfPCell(new Phrase(item.Status, fontBold));
                statusCell.BackgroundColor = ativo ? new BaseColor(198, 246, 213) : new BaseColor(254, 215, 215);
                statusCell.Padding = 6;
                statusCell.HorizontalAlignment = Element.ALIGN_CENTER;
                statusCell.BorderColor = new BaseColor(226, 232, 240);
                dataTable.AddCell(statusCell);
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
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
        private IItemOSRepository itemOSRepository;
        
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

            if (tabelaProdutos != null)
            {
                foreach (DataRow row in tabelaProdutos.Rows)
                {
                    relatorio.Itens.Add(new ProdutoDTO.ProdutoRelatorioDTO
                    {
                        IdProduto = row["ID_PRODUTO"] != DBNull.Value ? Convert.ToInt32(row["ID_PRODUTO"]) : 0,
                        Descricao = ObterValorColuna(row, tabelaProdutos, "DESCRIÇÃO", "DESC_PRODUTO"),
                        Unidade = ObterValorColuna(row, tabelaProdutos, "UNIDADE", "UN_MEDIDA"),
                        PrecoVenda = ObterDecimalColuna(row, tabelaProdutos, "PREÇO_VENDA", "PRECO_VENDA"),
                        PrecoCompra = ObterDecimalColuna(row, tabelaProdutos, "PREÇO_COMPRA", "PRECO_COMPRA"),
                        Quantidade = ObterIntColuna(row, tabelaProdutos, "QUANTIDADE", "QTD_ESTOQUE"),
                        QuantidadeMinima = ObterIntColuna(row, tabelaProdutos, "QUANTIDADE_MINIMA", "QTD_MINIMA"),
                        Status = ObterValorColuna(row, tabelaProdutos, "STATUS", "SITUACAO")
                    });
                }
            }

            GeradorPdfEstoque.GerarRelatorio(relatorio, caminhoDestino);
        }
        catch (Exception ex)
        {
            string mensagemDetalhada = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            throw new Exception("Falha ao gerar o relatório de estoque em PDF: " + mensagemDetalhada, ex);
        }
    }

    private string ObterValorColuna(DataRow row, DataTable table, string colPrincipal, string colAlternative)
    {
        if (table.Columns.Contains(colPrincipal) && row[colPrincipal] != DBNull.Value)
            return row[colPrincipal].ToString();

        if (table.Columns.Contains(colAlternative) && row[colAlternative] != DBNull.Value)
            return row[colAlternative].ToString();

        return string.Empty;
    }

    private decimal ObterDecimalColuna(DataRow row, DataTable table, string colPrincipal, string colAlternative)
    {
        if (table.Columns.Contains(colPrincipal) && row[colPrincipal] != DBNull.Value)
            return Convert.ToDecimal(row[colPrincipal]);

        if (table.Columns.Contains(colAlternative) && row[colAlternative] != DBNull.Value)
            return Convert.ToDecimal(row[colAlternative]);

        return 0m;
    }

    private int ObterIntColuna(DataRow row, DataTable table, string colPrincipal, string colAlternative)
    {
        if (table.Columns.Contains(colPrincipal) && row[colPrincipal] != DBNull.Value)
            return Convert.ToInt32(row[colPrincipal]);

        if (table.Columns.Contains(colAlternative) && row[colAlternative] != DBNull.Value)
            return Convert.ToInt32(row[colAlternative]);

        return 0;
    }
    }
}
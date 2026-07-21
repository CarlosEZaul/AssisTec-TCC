using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;

namespace AssisTec.Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext context;
        public ProdutoRepository(AppDbContext _context)
        {
            this.context = _context;
        }

        public bool InserirProduto(Produto produto)
        {
            try
            {
                context.Produtos.Add(produto);
                return context.SaveChanges() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception(
                    "Erro ao inserir produto no banco: " + (dbEx.InnerException?.Message ?? dbEx.Message), dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao inserir produto.", ex);
            }
        }

        

        public bool AtualizarProduto(Produto produto)
        {
            try
            {
                var local = context.Produtos.Local.FirstOrDefault( p => p.idProduto == produto.idProduto );
                if (local != null)
                {
                    context.Entry(local).State = EntityState.Detached;
                }
                context.Entry(produto).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public bool ExcluirProduto(int id)
        {
            try
            {
                var produto = context.Produtos.FirstOrDefault(p => p.idProduto == id);
                if (produto != null)
                {
                    context.Produtos.Remove(produto);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao excluir produto no banco de dados.", ex);
            }
        }

        public bool darEntradaProduto(int id, int quantidade)
        {
            try
            {
                var produto = context.Produtos.FirstOrDefault(p => p.idProduto == id);
    
                if (produto != null)
                {
                    produto.quantidade += quantidade;
                    context.SaveChanges();
                    return true;
                }
    
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao dar entrada no produto. Detalhes: " + e.Message, e);
            }
        }

        public bool darSaidaProduto(int id, int quantidade)
        {
            try
            {
                var produto = context.Produtos.FirstOrDefault(p => p.idProduto == id);
    
                if (produto != null)
                {
                    produto.quantidade -= quantidade;
                    context.SaveChanges();
                    return true;
                }
    
                return false;
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao dar entrada no produto. Detalhes: " + e.Message, e);
            }
        }

        public bool alterarStatus(int id)
        {
            var produto = context.Produtos.FirstOrDefault(p => p.idProduto == id);
            if (produto == null)
            {
                return false;
            }

            if (produto.status == "Ativado")
            {
                produto.status = "Desativado";
            }
            else
            {
                produto.status = "Ativado";
            }
            
            context.SaveChanges();
            return true;
        }

        public Produto ObterProdutoPorId(int id)
        {
            return context.Produtos.FirstOrDefault(p => p.idProduto == id);
        }

        public IEnumerable<Produto> ObterProdutos()
        {
            return context.Produtos.ToList();
        }

        public object ObterDescricaoProdutos()
        {
            try
            {
                return context.Produtos.Select(p => new
                {
                    Produto = Convert.ToString(p.idProduto) + " - " + p.descricao,
                }).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao obter produtos do DB.", e);
            }
        }

        public DataTable Filtrar(Produto filtro)
        {
            var resultado = AplicarFiltro(filtro).ToList();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID_PRODUTO", typeof(int));
            dataTable.Columns.Add("DESCRIÇÃO", typeof(string));
            dataTable.Columns.Add("UNIDADE", typeof(string));
            dataTable.Columns.Add("PREÇO_VENDA", typeof(decimal));
            dataTable.Columns.Add("PREÇO_COMPRA", typeof(decimal));
            dataTable.Columns.Add("QUANTIDADE", typeof(int));
            dataTable.Columns.Add("QUANTIDADE_MINIMA", typeof(int));
            dataTable.Columns.Add("STATUS", typeof(string));

            foreach (var produto in resultado)
            {
                dataTable.Rows.Add(
                    produto.idProduto,
                    produto.descricao,
                    produto.unidade,
                    produto.preco_venda,
                    produto.preco_compra,
                    produto.quantidade,
                    produto.quantidade_minima,
                    produto.status
                );
            }
            return dataTable;
        }

        public IQueryable<Produto> AplicarFiltro(Produto filtro)
        {
            var query = context.Produtos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.filtroDescricao))
            {
                query = query.Where(p => p.descricao.Contains(filtro.filtroDescricao));
            }

            if (filtro.filtroAbaixoMinimo)
            {
                query = query.Where(p => p.quantidade <= p.quantidade_minima);
            }

            if (filtro.filtroSemEstoque)
            {
                query = query.Where(p => p.quantidade <= 0);
            }

            if (filtro.filtroProdutosDesativados)
            {
                query = query.Where(p => p.status == "Desativado");
            }

            return query;
        }

        public DataTable ProdutosAbaixoMinimo()
        {
            var produtos = context.Produtos.Where(p => p.quantidade < p.quantidade_minima).OrderBy(p => p.quantidade);
            return MontarDataTableAbaixoMinimo(produtos);
            
        }

        private DataTable MontarDataTableAbaixoMinimo(IQueryable<Produto> query)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID_PRODUTO", typeof(int));
            dataTable.Columns.Add("Descrção", typeof(string));
            dataTable.Columns.Add("Quantidade", typeof(string));
            dataTable.Columns.Add("Quantidade mínima", typeof(int));

            var dadosProjetados = query.Select(p => new
                {
                    p.idProduto,
                    p.descricao,
                    p.quantidade,
                    p.quantidade_minima,
                }
            ).ToList();

            foreach (var produto in dadosProjetados)
            {
                dataTable.Rows.Add(
                    produto.idProduto,
                    produto.descricao,
                    produto.quantidade,
                    produto.quantidade_minima
                );
            }
            
            return dataTable;
        }

        public (int totalCadastrado, int abaixoMinimo, int semEstoque, decimal valorEstoque) obterTotais(Produto produto)
        {
            var dados = AplicarFiltro(produto).ToList();
    
            int totalCadastrado = dados.Count;
            int abaixoMinimo = dados.Count(p => p.quantidade < p.quantidade_minima);
            int semEstoque = dados.Count(p => p.quantidade <= 0);
            decimal valorEstoque = dados.Sum(p => p.quantidade * p.preco_compra);

            return (totalCadastrado, abaixoMinimo, semEstoque, valorEstoque);
        }
    }
}
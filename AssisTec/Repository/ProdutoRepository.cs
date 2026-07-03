using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;

namespace AssisTec.Repository
{
    public class ProdutoRepository
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

        public Produto ObterProdutoPorId(int id)
        {
            return context.Produtos.FirstOrDefault(p => p.idProduto == id);
        }

        public IEnumerable<Produto> ObterProdutos()
        {
            return context.Produtos.ToList();
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
            dataTable.Columns.Add("QUANTIDADE_MINIMA", typeof(string));

            foreach (var produto in resultado)
            {
                dataTable.Rows.Add(
                    produto.idProduto,
                    produto.descricao,
                    produto.unidade,
                    produto.preco_venda,
                    produto.preco_compra,
                    produto.quantidade,
                    produto.quantidade_minima
                );
            }
            return dataTable;
            
        }

        IQueryable<Produto> AplicarFiltro(Produto filtro)
        {
            var query = context.Produtos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.filtroDescricao))
            {
                query = query.Where(p=> p.descricao.Contains(filtro.filtroDescricao));
            }

            if (!filtro.filtroAbaixoMinimo == false)
            {
                query = query.Where(p => p.quantidade < p.quantidade_minima);
            }
            return query;
        }

        (int totalCadastrado, int abaixoMinimo, int semEstoque, decimal valorEstoque) obterTotais(Produto produto)
        {
            var dados = AplicarFiltro(produto).ToList();
            int totalCadastrado = dados.Count;
            int abaixoMinimo = dados.Count(p => p.quantidade < p.quantidade_minima & p.quantidade > 0);
            int semEstoque = dados.Count(p => p.quantidade <= 0);
            decimal valorEstoque = dados.Sum(p=> p.quantidade * p.preco_compra);
            return (totalCadastrado, abaixoMinimo, semEstoque, valorEstoque);
        }
    }
}
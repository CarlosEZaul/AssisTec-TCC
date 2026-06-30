using System;
using System.Collections.Generic;
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

        public List<Produto> ObterProdutos()
        {
            return context.Produtos.ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;

namespace AssisTec.Repository
{
    public class MovimentacaoEstoqueRepository : IMovimentacaoEstoqueRepository
    {
        private readonly AppDbContext context;
        public MovimentacaoEstoqueRepository(AppDbContext context)
        {
            this.context = context;
        }

        public bool InserirMovimentacao(MovimentacaoEstoque movimentacaoEstoque)
        {
            try
            {
                context.movimentacaoEstoque.Add(movimentacaoEstoque);
                return context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao inserir movimentação do estoque no banco de dados", e);
            }
        }

        public IEnumerable<MovimentacaoEstoque> ListarMovimentacaoEstoque()
        {
            try
            {
                return context.movimentacaoEstoque.ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao carregar movimentação do estoque", e);
            }
        }
        
    }
}
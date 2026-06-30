using System;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;

namespace AssisTec.Repository
{
    public class MovimentacaoEstoqueRepository
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
    }
}
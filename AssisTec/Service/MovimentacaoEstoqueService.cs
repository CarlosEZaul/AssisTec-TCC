using System;
using AssisTec.Models;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class MovimentacaoEstoqueService
    {
        private readonly IMovimentacaoEstoqueRepository _movimentacaoEstoqueRepository;

        public MovimentacaoEstoqueService(IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository)
        {
            this._movimentacaoEstoqueRepository = movimentacaoEstoqueRepository ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueRepository));
        }

        public bool NovaMovimentacaoEstoque(MovimentacaoEstoque movimentacao)
        {
            try
            {
                return _movimentacaoEstoqueRepository.InserirMovimentacao(movimentacao);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
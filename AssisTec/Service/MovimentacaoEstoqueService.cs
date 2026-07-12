using System;
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
    }
}
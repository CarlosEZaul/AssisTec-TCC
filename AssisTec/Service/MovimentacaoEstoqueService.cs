using System;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<MovimentacaoEstoque> ListarMovimentacaoEstoque()
        {
            try
            {
                return _movimentacaoEstoqueRepository.ListarMovimentacaoEstoque();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            
        }
    }
}
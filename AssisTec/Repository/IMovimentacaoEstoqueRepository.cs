using System;
using System.Collections.Generic;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IMovimentacaoEstoqueRepository
    {
        bool InserirMovimentacao(MovimentacaoEstoque movimentacaoEstoque);
        IEnumerable<MovimentacaoEstoque> ListarMovimentacaoEstoque();
        
    }
}
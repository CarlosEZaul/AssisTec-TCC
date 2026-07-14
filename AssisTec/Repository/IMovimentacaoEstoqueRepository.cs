using System;
using System.Collections.Generic;
using System.Data;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IMovimentacaoEstoqueRepository
    {
        bool InserirMovimentacao(MovimentacaoEstoque movimentacaoEstoque);
        object ListarMovimentacaoEstoque();
        object Filtrar(DateTime? dataInicio, DateTime? dataFim, string produtoSelecionado, string tipoMovimentacao);

    }
}
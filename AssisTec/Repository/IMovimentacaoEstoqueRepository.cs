using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IMovimentacaoEstoqueRepository
    {
        bool InserirMovimentacao(MovimentacaoEstoque movimentacaoEstoque);
    }
}
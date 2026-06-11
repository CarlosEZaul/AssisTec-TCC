using System.Collections.Generic;
using System.Data;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IContaReceberRepository
    {
        bool Inserir(ContasReceber conta);
        bool InserirComVinculoOS(ContasReceber conta, int idOs);
        IEnumerable<ContasReceber> ObterTodos();
        ContasReceber ObterPorId(int id);
        bool Atualizar(ContasReceber conta);
        bool Excluir(int id);
        bool MarcarComoAtrasado(int id);
        DataTable Filtrar(ContasReceber filtro);
        (decimal TotalGeral, decimal TotalRecebido, decimal TotalPendente, decimal TotalAtrasado) ObterTotais(ContasReceber filtro);
    }
}
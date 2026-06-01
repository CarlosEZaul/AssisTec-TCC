using System.Collections.Generic;
using System.Data;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IContaReceberRepository
    {
        
        bool InserirContasReceber(ContasReceber conta);
        bool InserirContasReceberOS(ContasReceber conta, int idOs);
        List<ContasReceber> ObterTodasContasReceber();
        ContasReceber ObterPorId(int id);
        bool AtualizarContasReceber(ContasReceber conta);
        bool ExcluirContasReceber(int id);
        bool AtualizarParaAtrasado(int id);
        DataTable FiltrarContasReceber(ContasReceber filtro);
        (decimal totalGeral, decimal totalRecebido, decimal totalPendente, decimal totalAtrasado) AtualizarTotais(ContasReceber filtro);
    }
}
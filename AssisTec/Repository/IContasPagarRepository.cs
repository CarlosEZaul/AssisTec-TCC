using System.Collections.Generic;
using System.Data;
using AssisTec.Dtos;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IContasPagarRepository
    {
        bool Inserir(ContasPagar conta);
        IEnumerable<ContasPagar> ObterTodos();
        ContasPagar ObterPorId(int id);
        bool Atualizar(ContasPagar conta);
        bool Excluir(int id);
        bool MarcarComoAtrasado(int id);
        DataTable Filtrar(ContasPagar filtro);
        (decimal TotalGeral, decimal TotalPagar, decimal TotalPendente, decimal TotalAtrasado) ObterTotais(ContasPagar filtro);
    }
}
using System.Collections.Generic;
using System.Data;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IPagamentoRepository
    {
        DataTable carregarFormasPamento();
        List<Pagamento> ObterTodas();
        Pagamento ObterPorId(int id);
        bool Inserir(Pagamento pagamento);
        bool Atualizar(Pagamento pagamento);
        bool Excluir(int id);
    }
}
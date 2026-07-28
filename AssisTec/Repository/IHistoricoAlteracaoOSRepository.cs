using System.Collections.Generic;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IHistoricoAlteracaoOSRepository
    {
        bool RegistrarHistorico(HistoricoAlteracaoOS historico);
        IEnumerable<dynamic> ObterPorOrdemServico(int idOS);
    }
}
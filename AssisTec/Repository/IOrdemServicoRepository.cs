using System.Collections.Generic;
using System.Data;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IOrdemServicoRepository
    {
        IEnumerable<dynamic> ObterTodasOSAtuais();
        OrdemServico ObterPorId(int idOrdemServico);
        bool SalvarOrdemServico(OrdemServico ordemServico);
        bool SalvarAlteracoesOS(OrdemServico ordemServico);
        DataTable ObterHistoricoUsuario(int idUsuario);
        DataTable ObterHistoricoCliente(int idCliente);
        int ObterQntOsAbertas();
        DataTable OrdensRecentes();
        bool ExisteOSAbertaPorTecnico(int idTecnico);
        bool ExisteOSAbertaPorCliente(int idCliente);
        bool CancelarOrdemServico(int idOS);
        bool ReabrirOrdemServico(int idOS);

    }
}
using System.Data;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IOrdemServicoRepository
    {
        bool SalvarOrdemServico(OrdemServico ordemServico);
        DataTable ObterHistoricoUsuario(int idUsuario);
        DataTable ObterHistoricoCliente(int idCliente);
        int ObterOsAbertas();
        DataTable OrdensRecentes();
        bool ExisteOSAbertaPorTecnico(int idTecnico);
        bool ExisteOSAbertaPorCliente(int idCliente);

    }
}
using System.Collections.Generic;
using System.Data;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IOrdemServicoRepository
    {
        IEnumerable<dynamic> ObterTodasOSAtuais();
        bool SalvarOrdemServico(OrdemServico ordemServico);
        DataTable ObterHistoricoUsuario(int idUsuario);
        DataTable ObterHistoricoCliente(int idCliente);
        int ObterQntOsAbertas();
        DataTable OrdensRecentes();
        bool ExisteOSAbertaPorTecnico(int idTecnico);
        bool ExisteOSAbertaPorCliente(int idCliente);

    }
}
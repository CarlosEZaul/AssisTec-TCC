using System.Data;

namespace AssisTec.Repository
{
    public interface IOrdemServicoRepository
    {
        DataTable ObterHistoricoUsuario(int idUsuario);
        DataTable ObterHistoricoCliente(int idCliente);
        
    }
}
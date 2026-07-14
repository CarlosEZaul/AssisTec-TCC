using System.Data;

namespace AssisTec.Repository
{
    public interface IOrdemServicoRepository
    {
        DataTable ObterPorUsuario(int idUsuario);
    }
}
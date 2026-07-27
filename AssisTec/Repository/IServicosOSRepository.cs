using System.Collections.Generic;
using System.Data;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IServicosOSRepository
    {
        bool SalvarAcaoOS(ServicosOS servicos);
        ServicosOS ObterAcaoOSPorID(int idAcao);
        bool ExcluirAcaoOS(int idAcao);
        List<ServicosOS> ListarAcaoOSPorOS(int idOS);
    }
}
using System.Linq;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IEquipamentoRepository
    {
        Equipamento ObterEquipamentoPorId(int id);
        bool SalvarEquipamento(Equipamento equipamento);
    }
}
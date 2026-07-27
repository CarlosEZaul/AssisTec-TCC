using System.Collections.Generic;
using System.Data;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IItemOSRepository
    {
        bool SalvarItemOS(ItemOS item);
        ItemOS ObterPorId(int idItem);
        bool Remover(int idItem);
        List<ItemOS> ObterPorOrdemServico(int idOS);
    }
}
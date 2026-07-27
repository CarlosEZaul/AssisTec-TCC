using System;
using AssisTec.Models;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class EquipamentoService
    {
        private readonly IEquipamentoRepository repository;
        
        public  EquipamentoService(IEquipamentoRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        

        public bool Salvar(Equipamento equipamento)
        {
            try
            {
                return repository.SalvarEquipamento(equipamento);
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao salvar equipamento" + e.Message);
            }
        }
    }
}
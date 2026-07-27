using System;
using System.Linq;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public class EquipamentoRepository : IEquipamentoRepository
    {
        private readonly AppDbContext _context;
        
        public EquipamentoRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public Equipamento ObterEquipamentoPorId(int id)
        {
            try
            {
                return _context.Equipamentos.Find(id);
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao carregar equipamento do BD. " + e.Message);
            }
        }

        public bool SalvarEquipamento(Equipamento equipamento)
        {
            try
            {
                _context.Equipamentos.Add(equipamento);
                return _context.SaveChanges() >0;
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao salvar equipamento no BD. " + e.Message);
            }
        }

        public bool AtualzarEquipamento(Equipamento equipamento)
        {
            try
            {
                _context.Equipamentos.Update(equipamento);
                return _context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;
using Exception = AssisTec.AtendeClienteService.Exception;

namespace AssisTec.Repository
{
    public class ItemOSRepository: IItemOSRepository
    {
        private readonly AppDbContext _context;

        public ItemOSRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public ItemOS ObterPorId(int idItem)
        {
            try
            {
                return _context.ItemOS
                    .Include(x => x.Produto)
                    .FirstOrDefault(x => x.Id == idItem);
            }
            catch (Exception e)
            {
                throw new ArgumentNullException(e.Message);
            }
        }
        
        public bool SalvarItemOS(ItemOS item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            try
            {
                _context.ItemOS.Add(item);
                return _context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Remover(int idItem)
        {
            try
            {
                var item = _context.ItemOS.Find(idItem);
                
                if (item == null)
                    return false;

                _context.ItemOS.Remove(item);
                return _context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                throw new ArgumentNullException(e.Message);
            }
        }
        public IEnumerable<dynamic> ObterPorOrdemServico(int idOS)
        {
            try
            {
                return _context.ItemOS
                    .AsNoTracking()
                    .Include(x => x.Produto)
                    .Where(x => x.id_OS == idOS)
                    .Select(x => new
                    {
                        id_produto = x.id_produto,
                        Produto = x.Produto != null ? x.Produto.descricao : "Não informado",
                        Quantidade = x.Quantidade,
                        ValorUnitario = x.ValorUnitario,
                        ValorTotal = x.ValorTotal
                    })
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao buscar os itens da OS no banco de dados.", e);
            }
        }
    }
}
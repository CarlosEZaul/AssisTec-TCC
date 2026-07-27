using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;
using Exception = AssisTec.AtendeClienteService.Exception;

namespace AssisTec.Repository
{
    public class ItemOSRepository: IitemOSRepository
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
        public DataTable ObterPorOrdemServico(int idOS)
        {
            try
            {
                var dt = new DataTable("ItensOS");
                dt.Columns.Add("Id", typeof(int));
                dt.Columns.Add("idProduto", typeof(int));
                dt.Columns.Add("Produto", typeof(string));
                dt.Columns.Add("Quantidade", typeof(int));
                dt.Columns.Add("ValorUnitario", typeof(decimal));
                dt.Columns.Add("ValorTotal", typeof(decimal));
        
                var itens = _context.ItemOS
                    .AsNoTracking()
                    .Include(x => x.Produto)
                    .Where(x => x.id_OS == idOS)
                    .ToList();
        
                foreach (var item in itens)
                {
                    dt.Rows.Add(
                        item.Id,
                        item.id_produto,
                        item.Produto != null ? item.Produto.descricao : "Sem Produto",
                        item.Quantidade,
                        item.ValorUnitario,
                        item.ValorTotal
                    );
                }

                return dt;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao buscar os itens da OS no banco de dados.", e);
            }
        }
    }
}
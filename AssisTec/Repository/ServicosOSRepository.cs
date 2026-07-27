using System;
using System.Collections.Generic;
using System.Linq;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;

namespace AssisTec.Repository
{
    public class ServicosOSRepository : IServicosOSRepository
    {
        private readonly AppDbContext _context;

        public ServicosOSRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool SalvarAcaoOS(ServicosOS servicos)
        {
            if (servicos.idServico > 0)
            {
                _context.ServicosOS.Update(servicos);
            }
            else
            {
                _context.ServicosOS.Add(servicos);
            }

            return _context.SaveChanges() > 0;
        }

        public ServicosOS ObterAcaoOSPorID(int idAcao)
        {
            return _context.ServicosOS
                .AsNoTracking()
                .FirstOrDefault(a => a.idServico == idAcao);
        }

        public bool ExcluirAcaoOS(int idAcao)
        {
            var acao = _context.ServicosOS.Find(idAcao);
            if (acao == null) return false;

            _context.ServicosOS.Remove(acao);
            return _context.SaveChanges() > 0;
        }

        public List<ServicosOS> ListarAcaoOSPorOS(int idOS)
        {
            return _context.ServicosOS
                .AsNoTracking()
                .Where(a => a.id_OS == idOS)
                .Select(a => new ServicosOS
                {
                    id_OS = a.id_OS,
                    descricao = a.descricao,
                    valor_cobrado = a.valor_cobrado,
                    OrdemServico = a.OrdemServico
                })
                .ToList();
        }
    }
}
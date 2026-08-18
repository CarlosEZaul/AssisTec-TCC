using System;
using System.Collections.Generic;
using System.Linq;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;

namespace AssisTec.Repository
{
    public class HistoricoAlteracaoOSRepository : IHistoricoAlteracaoOSRepository
    {
        private readonly AppDbContext _context;

        public HistoricoAlteracaoOSRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Consulta

        public IEnumerable<dynamic> ObterPorOrdemServico(int idOS)
        {
            try
            {
                return _context.HistoricoAlteracaoOS
                    .AsNoTracking()
                    .Include(h => h.usuario)
                    .Where(h => h.idOS == idOS)
                    .OrderByDescending(h => h.dataAlteracao)
                    .Select(h => new
                    {
                        ID = h.id,
                        Usuario = h.usuario != null ? h.usuario.Nome : "Não informado",
                        Tipo = h.tipo,
                        Descricao = h.descricao,
                        DataAlteracao = h.dataAlteracao
                    })
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao consultar o histórico da Ordem de Serviço.", e);
            }
        }

        #endregion

        #region Gerenciamento

        public bool RegistrarHistorico(HistoricoAlteracaoOS historico)
        {
            try
            {
                _context.HistoricoAlteracaoOS.Add(historico);
                return _context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        

      
    }
}
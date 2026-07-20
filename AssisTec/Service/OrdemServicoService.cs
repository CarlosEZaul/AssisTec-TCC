using System;
using System.Data;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class OrdemServicoService
    {
        private readonly IOrdemServicoRepository ordemServicoRepository;

        public OrdemServicoService(IOrdemServicoRepository _ordemServicoRepository)
        {
            ordemServicoRepository = _ordemServicoRepository ??  throw new ArgumentNullException(nameof(_ordemServicoRepository));
        }

        public DataTable obterHistoricoOsTecnico(int id)
        {
            return ordemServicoRepository.ObterHistoricoUsuario(id);
        }

        public int obterOsAbertas()
        {
            try
            {
                return ordemServicoRepository.ObterOsAbertas();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using AssisTec.Models;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class ServicoOSService
    {
        private readonly IServicosOSRepository _servicoOSRepository;
        private readonly IOrdemServicoRepository _ordemServicoRepository;

        public ServicoOSService(IServicosOSRepository servicoOSRepository, IOrdemServicoRepository ordemServicoRepository)
        {
            _servicoOSRepository = servicoOSRepository ?? throw new ArgumentNullException(nameof(servicoOSRepository));
            _ordemServicoRepository = ordemServicoRepository ?? throw new ArgumentNullException(nameof(ordemServicoRepository));
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AssisTec.Models;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class OrdemServicoService
    {
        private readonly IOrdemServicoRepository ordemServicoRepository;
        private readonly IEquipamentoRepository equipamentoRepository;

        public OrdemServicoService(IOrdemServicoRepository _ordemServicoRepository,  IEquipamentoRepository _equipamentoRepository)
        {
            ordemServicoRepository = _ordemServicoRepository ??  throw new ArgumentNullException(nameof(_ordemServicoRepository));
            equipamentoRepository = _equipamentoRepository ??  throw new ArgumentNullException(nameof(_equipamentoRepository));
        }

        public bool SalvarOS(OrdemServico ordemServico, Equipamento equipamento)
        {
            try
            {
                ValidarEntidades(ordemServico, equipamento);

                bool equipamentoSalvo = equipamentoRepository.SalvarEquipamento(equipamento);
                if (!equipamentoSalvo || equipamento.Id_equipamento <= 0)
                {
                    throw new InvalidOperationException("Não foi possível cadastrar o equipamento no sistema.");
                }

                ordemServico.id_equipamento = equipamento.Id_equipamento;
                ordemServico.data_abertura = DateTime.Now;
                ordemServico.status = "ABERTA";

                return ordemServicoRepository.SalvarOrdemServico(ordemServico);
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao salvar OS "+e.Message);
            }
            
        }

        private void ValidarEntidades(OrdemServico os, Equipamento eq)
        {
            if (!os.id_cliente.HasValue || os.id_cliente.Value <= 0)
                throw new ArgumentException("Selecione um cliente válido.");

            if (!os.id_tecnico.HasValue || os.id_tecnico.Value <= 0)
                throw new ArgumentException("Selecione um técnico responsável.");

            if (string.IsNullOrWhiteSpace(os.problema_relatado))
                throw new ArgumentException("O problema relatado deve ser preenchido.");

            if (string.IsNullOrWhiteSpace(eq.Descricao))
                throw new ArgumentException("A descrição do equipamento é obrigatória.");

            if (string.IsNullOrWhiteSpace(eq.Marca))
                throw new ArgumentException("A marca do equipamento é obrigatória.");

            if (string.IsNullOrWhiteSpace(eq.Modelo))
                throw new ArgumentException("O modelo do equipamento é obrigatório.");

            if (string.IsNullOrWhiteSpace(eq.Numero_Serie))
                throw new ArgumentException("O número de série é obrigatório.");

            if (string.IsNullOrWhiteSpace(eq.estado_entrada))
                throw new ArgumentException("O estado de entrada do equipamento é obrigatório.");
        }

        public DataTable obterHistoricoOsTecnico(int id)
        {
            return ordemServicoRepository.ObterHistoricoUsuario(id);
        }

        public int ObterQntOsAbertas()
        {
            try
            {
                return ordemServicoRepository.ObterQntOsAbertas();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }

        public DataTable OrdensRecentes()
        {
            try
            {
                return ordemServicoRepository.OrdensRecentes();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IEnumerable<dynamic> ObterTodasOSAtuais()
        {
            try
            {
                return ordemServicoRepository.ObterTodasOSAtuais();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
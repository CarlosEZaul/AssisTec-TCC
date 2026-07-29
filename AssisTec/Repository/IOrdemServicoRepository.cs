using System.Collections.Generic;
using System.Data;
using System.Linq;
using AssisTec.DTO;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IOrdemServicoRepository
    {
        DataTable ObterTodasOSAtuais();
        OrdemServico ObterPorId(int idOrdemServico);
        int ObterQntOsAbertas();
        bool ExisteOSAbertaPorTecnico(int idTecnico);
        bool ExisteOSAbertaPorCliente(int idCliente);
        List<ItemOSRelatorioDTO> ObterItensPorOSId(int idOS);
        DataTable ObterHistoricoUsuario(int idUsuario);
        DataTable ObterHistoricoCliente(int idCliente);
        DataTable OrdensRecentes();
        bool SalvarOrdemServico(OrdemServico ordemServico);
        bool SalvarAlteracoesOS(OrdemServico ordemServico);
        bool ReabrirOrdemServico(int idOS);
        bool CancelarOrdemServico(int idOS);
        DataTable Filtrar(OrdemServico filtro);

        (int TotalOS, int EmAtendimento, int ParaRetirada, decimal TotalAReceber, decimal TotalRecebido, int
            QntRecebido, decimal TotalCancelado, int QntCancelado) ObterTotais(OrdemServico filtro);
        IQueryable<OrdemServico> AplicarFiltros(OrdemServico filtro);


    }
}
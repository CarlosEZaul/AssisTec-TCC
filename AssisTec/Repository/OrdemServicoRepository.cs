using System;
using System.Data;
using System.Linq;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public class OrdemServicoRepository : IOrdemServicoRepository
    {
        private readonly AppDbContext context;
        private IOrdemServicoRepository _ordemServicoRepositoryImplementation;

        public OrdemServicoRepository(AppDbContext context)
        {
            this.context = context;
        }

        public bool SalvarOrdemServico(OrdemServico ordemServico)
        {
            try
            {
                context.OrdemServicos.Add(ordemServico);
                return context.SaveChanges()>0;
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao salvar ordemServico no BD" + e.Message);
            }
        }

        public DataTable ObterHistoricoUsuario(int idUsuario)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID_ORDEM", typeof(int));
                dataTable.Columns.Add("ID_CLIENTE", typeof(int));
                dataTable.Columns.Add("CLIENTE", typeof(string));
                dataTable.Columns.Add("CLIENTE_EXIBICAO", typeof(string));
                dataTable.Columns.Add("ID_TECNICO", typeof(int));
                dataTable.Columns.Add("Técnico Responsável", typeof(string));
                dataTable.Columns.Add("TECNICO_EXIBICAO", typeof(string));
                dataTable.Columns.Add("EQUIPAMENTO", typeof(string));
                dataTable.Columns.Add("DATA_ABERTURA", typeof(DateTime));
                dataTable.Columns.Add("DATA_FECHAMENTO", typeof(object));
                dataTable.Columns.Add("VALOR_TOTAL", typeof(decimal));
                dataTable.Columns.Add("STATUS", typeof(string));

                var ordens = context.OrdemServicos
                    .Where(os => os.id_tecnico == idUsuario)
                    .Select(os => new
                    {
                        os.id_os,
                        ClienteId = os.Cliente != null ? os.Cliente.Id : 0,
                        ClienteNome = os.Cliente != null ? os.Cliente.Nome : "Sem Cliente",
                        TecnicoId = os.Tecnico != null ? os.Tecnico.Id : 0,
                        TecnicoNome = os.Tecnico != null ? os.Tecnico.Nome : "Sem Tecnico",
                        EquipamentoDescricao = os.Equipamento != null ? os.Equipamento.Descricao : "Sem Equipamento",
                        os.data_abertura,
                        os.data_fechamento,
                        os.valor_total,
                        os.status
                    })
                    .ToList();

                foreach (var os in ordens)
                {
                    dataTable.Rows.Add(
                        os.id_os,
                        os.ClienteId,
                        os.ClienteNome,
                        $"ID: {os.ClienteId} - Nome: {os.ClienteNome}",
                        os.TecnicoId,
                        os.TecnicoNome,
                        $"ID: {os.TecnicoId} - Nome: {os.TecnicoNome}",
                        os.EquipamentoDescricao,
                        os.data_abertura,
                        (object)os.data_fechamento ?? DBNull.Value,
                        os.valor_total,
                        os.status ?? "ABERTA"
                    );
                }

                return dataTable;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public DataTable ObterHistoricoCliente(int idCliente)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID_ORDEM", typeof(int));
                dataTable.Columns.Add("ID_CLIENTE", typeof(int));
                dataTable.Columns.Add("CLIENTE", typeof(string));
                dataTable.Columns.Add("CLIENTE_EXIBICAO", typeof(string));
                dataTable.Columns.Add("ID_TECNICO", typeof(int));
                dataTable.Columns.Add("Técnico Responsável", typeof(string));
                dataTable.Columns.Add("TECNICO_EXIBICAO", typeof(string));
                dataTable.Columns.Add("EQUIPAMENTO", typeof(string));
                dataTable.Columns.Add("DATA_ABERTURA", typeof(DateTime));
                dataTable.Columns.Add("DATA_FECHAMENTO", typeof(object));
                dataTable.Columns.Add("VALOR_TOTAL", typeof(decimal));
                dataTable.Columns.Add("STATUS", typeof(string));

                var ordens = context.OrdemServicos
                    .Where(os => os.id_cliente == idCliente)
                    .Select(os => new
                    {
                        os.id_os,
                        ClienteId = os.Cliente != null ? os.Cliente.Id : 0,
                        ClienteNome = os.Cliente != null ? os.Cliente.Nome : "Sem Cliente",
                        TecnicoId = os.Tecnico != null ? os.Tecnico.Id : 0,
                        TecnicoNome = os.Tecnico != null ? os.Tecnico.Nome : "Sem Tecnico",
                        EquipamentoDescricao = os.Equipamento != null ? os.Equipamento.Descricao : "Sem Equipamento",
                        os.data_abertura,
                        os.data_fechamento,
                        os.valor_total,
                        os.status
                    })
                    .ToList();

                foreach (var os in ordens)
                {
                    dataTable.Rows.Add(
                        os.id_os,
                        os.ClienteId,
                        os.ClienteNome,
                        $"ID: {os.ClienteId} - Nome: {os.ClienteNome}",
                        os.TecnicoId,
                        os.TecnicoNome,
                        $"ID: {os.TecnicoId} - Nome: {os.TecnicoNome}",
                        os.EquipamentoDescricao,
                        os.data_abertura,
                        (object)os.data_fechamento ?? DBNull.Value,
                        os.valor_total,
                        os.status ?? "ABERTA"
                    );
                }

                return dataTable;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message, e);
            }
        }

        public int ObterOsAbertas()
        {
            return context.OrdemServicos.Count(os => os.status == "ABERTA");
        }

        public DataTable OrdensRecentes()
        {
            var ordens = context.OrdemServicos
                .OrderByDescending(os => os.data_abertura)
                .Take(15);

            return MontarDataTableOrdemServico(ordens);
        }

        public bool ExisteOSAbertaPorTecnico(int idTecnico)
        {
            return context.OrdemServicos.Any(os => os.id_tecnico == idTecnico && os.status == "ABERTA");
        }

        public bool ExisteOSAbertaPorCliente(int idCliente)
        {
            return context.OrdemServicos.Any(os => os.id_cliente == idCliente && os.status == "ABERTA");
        }

        private DataTable MontarDataTableOrdemServico(IQueryable<OrdemServico> query)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("ID_OS", typeof(int));
            dataTable.Columns.Add("Cliente", typeof(string));
            dataTable.Columns.Add("Técnico", typeof(string));
            dataTable.Columns.Add("Equipamento", typeof(string));
            dataTable.Columns.Add("Valor Total", typeof(decimal));
            dataTable.Columns.Add("Status", typeof(string));

            var dadosProjetados = query
                .Select(os => new
                {
                    os.id_os,
                    ClienteNome = os.Cliente != null ? os.Cliente.Nome : "Sem Cliente",
                    TecnicoNome = os.Tecnico != null ? os.Tecnico.Nome : "Sem Tecnico",
                    EquipamentoDescricao = os.Equipamento != null ? os.Equipamento.Descricao : "Sem Equipamento",
                    os.valor_total,
                    os.status
                })
                .ToList();

            foreach (var os in dadosProjetados)
            {
                dataTable.Rows.Add(
                    os.id_os,
                    os.ClienteNome,
                    os.TecnicoNome,
                    os.EquipamentoDescricao,
                    os.valor_total,
                    os.status ?? "ABERTA"
                );
            }

            return dataTable;
        }
    }
}
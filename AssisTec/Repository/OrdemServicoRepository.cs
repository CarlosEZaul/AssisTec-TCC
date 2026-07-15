using System;
using System.Data;
using System.Linq;

namespace AssisTec.Repository
{
    public class OrdemServicoRepository : IOrdemServicoRepository
    {
        private readonly AppDbContext context;
        
        public OrdemServicoRepository(AppDbContext context)
        {
            this.context = context;
        }
        public DataTable ObterHistoricoUsuario(int idUsuario)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID_ORDEM", typeof(int));
                dataTable.Columns.Add("CLIENTE", typeof(string));
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
                        ClienteNome = os.Cliente != null ? os.Cliente.Nome : "Sem Cliente",
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
                        os.ClienteNome,
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
                dataTable.Columns.Add("CLIENTE", typeof(string));
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
                        ClienteNome = os.Cliente != null ? os.Cliente.Nome : "Sem Cliente",
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
                        os.ClienteNome,
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
    }
}
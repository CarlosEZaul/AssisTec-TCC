using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;

namespace AssisTec.Repository
{
    public class OrdemServicoRepository : IOrdemServicoRepository
    {
        private readonly AppDbContext context;

        public OrdemServicoRepository(AppDbContext context)
        {
            this.context = context;
        }

        #region Consultas e Leitura

        public IEnumerable<dynamic> ObterTodasOSAtuais()
        {
            try
            {
                DateTime dataAtual = DateTime.Now;
                int mesAtual = dataAtual.Month;
                int anoAtual = dataAtual.Year;

                return context.OrdemServicos
                    .Where(os => 
                        ((os.status == "CONCLUIDA" || os.status == "CANCELADA") 
                         && os.data_abertura.Month == mesAtual 
                         && os.data_abertura.Year == anoAtual)
                        ||
                        (os.status != "CONCLUIDA" && os.status != "CANCELADA")
                    )
                    .Select(os => new
                    {
                        ID = os.id_os,
                        Tecnico = os.Tecnico != null ? os.Tecnico.Nome : "Não informado",
                        Cliente = os.Cliente != null ? os.Cliente.Nome : "Não informado",
                        Equipamento = os.Equipamento != null ? os.Equipamento.Descricao : "Não informado",
                        Status = os.status,
                        DataAbertura = os.data_abertura,
                        UltimaAtulizacao = os.data_atualizacao,
                        DataConclusao = os.data_fechamento,
                        ValorTotal = os.valor_total
                    })
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao consultar a lista de Ordens de Serviço.", e);
            }
        }

        public OrdemServico ObterPorId(int idOrdemServico)
        {
            try
            {
                return context.OrdemServicos
                    .Include(os => os.Cliente)
                    .Include(os => os.Tecnico)
                    .Include(os => os.Equipamento)
                    .FirstOrDefault(os => os.id_os == idOrdemServico);
            }
            catch (Exception ex)
            {
                throw new ArgumentNullException("Erro ao obter OS do BD.");
            }
        }

        public int ObterQntOsAbertas()
        {
            return context.OrdemServicos.Count(os => os.status == "ABERTA");
        }

        public bool ExisteOSAbertaPorTecnico(int idTecnico)
        {
            return context.OrdemServicos.Any(os => os.id_tecnico == idTecnico && os.status == "ABERTA");
        }

        public bool ExisteOSAbertaPorCliente(int idCliente)
        {
            return context.OrdemServicos.Any(os => os.id_cliente == idCliente && os.status == "ABERTA");
        }

        #endregion

        #region Relatórios e DataTables

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

        public DataTable OrdensRecentes()
        {
            var ordens = context.OrdemServicos
                .OrderByDescending(os => os.data_abertura)
                .Take(15);

            return MontarDataTableOrdemServico(ordens);
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

        #endregion

        #region Persistência e Alterações de Estado

        public bool SalvarOrdemServico(OrdemServico ordemServico)
        {
            try
            {
                context.OrdemServicos.Add(ordemServico);
                return context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao salvar ordemServico no BD" + e.Message);
            }
        }

        public bool SalvarAlteracoesOS(OrdemServico ordemServico)
        {
            try
            {
                var local = context.OrdemServicos.Local.FirstOrDefault(o => o.id_os == ordemServico.id_os);
                if (local != null)
                {
                    context.Entry(local).State = EntityState.Detached;
                }
                context.Entry(ordemServico).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao salvar ordemServico no BD" + e.Message);
            }
        }

        public bool ReabrirOrdemServico(int idOS)
        {
            if (idOS <= 0) return false;

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var ordemServico = context.OrdemServicos
                        .FirstOrDefault(x => x.id_os == idOS);

                    if (ordemServico == null) return false;

                    ordemServico.status = "ABERTA";

                    var itensOS = context.ItemOS
                        .Include(x => x.Produto)
                        .Where(x => x.id_OS == idOS)
                        .ToList();

                    foreach (var item in itensOS)
                    {
                        if (item.Produto != null)
                        {
                            if (item.Produto.quantidade < item.Quantidade)
                            {
                                throw new InvalidOperationException($"Estoque insuficiente para o produto '{item.Produto.descricao}'. Disponível: {item.Produto.quantidade}, Necessário: {item.Quantidade}");
                            }

                            item.Produto.quantidade -= item.Quantidade;

                            var movimentacao = new MovimentacaoEstoque
                            {
                                idProduto = item.id_produto,
                                quantidade = item.Quantidade,
                                valor = item.ValorUnitario,
                                tipoMovimentacao = "Saída",
                                descricao = $"Saída de estoque por reabertura da OS #{idOS}",
                                data = DateTime.Now
                            };

                            context.movimentacaoEstoque.Add(movimentacao);
                        }
                    }

                    context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao reabrir a Ordem de Serviço no banco de dados.", ex);
                }
            }
        }

        public bool CancelarOrdemServico(int idOS)
        {
            if (idOS <= 0) return false;

            using (var transaction = context.Database.BeginTransaction())
            {
                try
                {
                    var ordemServico = context.OrdemServicos
                        .FirstOrDefault(x => x.id_os == idOS);

                    if (ordemServico == null) return false;

                    ordemServico.status = "CANCELADA";

                    var itensOS = context.ItemOS
                        .Include(x => x.Produto)
                        .Where(x => x.id_OS == idOS)
                        .ToList();

                    foreach (var item in itensOS)
                    {
                        if (item.Produto != null)
                        {
                            item.Produto.quantidade += item.Quantidade;

                            var movimentacao = new MovimentacaoEstoque
                            {
                                idProduto = item.id_produto,
                                quantidade = item.Quantidade,
                                valor = item.ValorUnitario,
                                tipoMovimentacao = "Entrada",
                                descricao = $"Devolução de estoque por cancelamento da OS #{idOS}",
                                data = DateTime.Now
                            };

                            context.movimentacaoEstoque.Add(movimentacao);
                        }
                    }

                    context.SaveChanges();
                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao cancelar a Ordem de Serviço no banco de dados.", ex);
                }
            }
        }

        #endregion
    }
}
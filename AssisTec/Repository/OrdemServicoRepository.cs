using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using AssisTec.DTO;
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

        public DataTable ObterTodasOSAtuais()
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
            var fimMes = inicioMes.AddMonths(1);

            var query = context.OrdemServicos
                .Include(o => o.Cliente)
                .Include(o => o.Tecnico)
                .Include(o => o.Equipamento)
                .Where(o => (o.data_abertura >= inicioMes && o.data_abertura < fimMes) || o.status == "ABERTA");

            var ordens = query.ToList();
            var osIds = ordens.Select(o => o.id_os).ToList();

            var contasMap = context.ContasReceber
                .Include(c => c.Pagamento)
                .Where(c => c.id_os_fk.HasValue && osIds.Contains(c.id_os_fk.Value))
                .ToList()
                .GroupBy(c => c.id_os_fk.Value)
                .ToDictionary(
                    g => g.Key, 
                    g => g.FirstOrDefault()?.Pagamento?.Descricao ?? "Não registrado"
                );

            DataTable tabela = new DataTable();
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Cliente", typeof(string));
            tabela.Columns.Add("Técnico", typeof(string));
            tabela.Columns.Add("Equipamento", typeof(string));
            tabela.Columns.Add("Status", typeof(string));
            tabela.Columns.Add("Data de Abertura", typeof(DateTime));
            tabela.Columns.Add("Ultima Atualização", typeof(DateTime));
            tabela.Columns.Add("Data de Conclusão", typeof(DateTime));
            tabela.Columns.Add("Valor Total", typeof(decimal));
            tabela.Columns.Add("Forma de Pagamento", typeof(string));

            foreach (var o in ordens)
            {
                string pagamento = contasMap.ContainsKey(o.id_os) ? contasMap[o.id_os] : "-";

                tabela.Rows.Add(
                    o.id_os,
                    o.Cliente != null ? o.Cliente.Nome : string.Empty,
                    o.Tecnico != null ? o.Tecnico.Nome : string.Empty,
                    o.Equipamento != null ? o.Equipamento.Descricao : string.Empty,
                    o.status,
                    o.data_abertura,
                    o.data_atualizacao,
                    o.data_fechamento,
                    o.valor_total,
                    pagamento
                );
            }

            return tabela;
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
        
        public List<ItemOSRelatorioDTO> ObterItensPorOSId(int idOS)
        {
            var pecas = context.ItemOS
                .Where(i => i.id_OS == idOS)
                .Select(i => new ItemOSRelatorioDTO
                {
                    Descricao = i.Produto != null ? i.Produto.descricao : "Peça",
                    Quantidade = i.Quantidade,
                    ValorUnitario = i.ValorUnitario,
                    ValorTotal = i.Quantidade * i.ValorUnitario,
                    Tipo = "PECA"
                })
                .ToList();

            var resultado = new List<ItemOSRelatorioDTO>();
            resultado.AddRange(pecas);

            return resultado;
        }

        public List<ServicoOSRelatorioDTO> ObterServicosPorOSId(int idOS)
        {
            return context.ServicosOS
                .AsNoTracking()
                .Where(i => i.id_OS == idOS)
                .Select(i => new ServicoOSRelatorioDTO
                {
                    Descricao = i.descricao,
                    ValorCobrado = i.valor_cobrado,
                    Tipo = "SERVICO"
                })
                .ToList();
        }

        #endregion

        #region Relatórios e DataTables

        public DataTable ObterHistoricoUsuario(int idUsuario)
        {
            try
            {
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add("ID_ORDEM", typeof(int));
                dataTable.Columns.Add("CLIENTE", typeof(string));
                dataTable.Columns.Add("Técnico Responsável", typeof(string));
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
                        os.ClienteNome,
                        os.TecnicoNome,
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
                dataTable.Columns.Add("Técnico Responsável", typeof(string));
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
                        os.ClienteNome,
                        os.TecnicoNome,
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

        #region Filtros e Totais Dashboard

        public (DataTable Dados, int TotalOS, int EmAtendimento, int ParaRetirada, decimal TotalAReceber, decimal TotalRecebido, int QntRecebido, decimal TotalCancelado, int QntCancelado) Filtrar(string dataInicio, string dataFim, string busca, int indexStatus, string status)
        {
            OrdemServico filtro = new OrdemServico
            {
                filtroDataInicio = dataInicio,
                filtroDataConclusao = dataFim,
                filtroBusca = busca,
                filtroStatus = (indexStatus > 0 && status != "TODOS") ? status : null
            };

            DataTable tabela = FiltrarHistorico(null, null, dataInicio, dataFim, busca, status);
    
            var totais = ObterTotais(filtro);

            return (
                tabela, 
                totais.TotalOS, 
                totais.EmAtendimento, 
                totais.ParaRetirada, 
                totais.TotalAReceber, 
                totais.TotalRecebido, 
                totais.QntRecebido, 
                totais.TotalCancelado, 
                totais.QntCancelado
            );
        }
        

        public (int TotalOS, int EmAtendimento, int ParaRetirada, decimal TotalAReceber, decimal TotalRecebido, int QntRecebido, decimal TotalCancelado, int QntCancelado) ObterTotais(OrdemServico filtro)
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
            var fimMes = inicioMes.AddMonths(1);

            IQueryable<OrdemServico> query;

            if (filtro != null && (!string.IsNullOrWhiteSpace(filtro.filtroDataInicio) || 
                                   !string.IsNullOrWhiteSpace(filtro.filtroDataConclusao) || 
                                   !string.IsNullOrWhiteSpace(filtro.filtroBusca) || 
                                   !string.IsNullOrWhiteSpace(filtro.filtroStatus)))
            {
                query = AplicarFiltros(filtro);
            }
            else
            {
                query = context.OrdemServicos.Where(o => o.data_abertura >= inicioMes && o.data_abertura < fimMes);
            }

            var dados = query.Select(o => new 
            { 
                Status = o.status != null ? o.status.Trim().ToUpper() : string.Empty, 
                ValorTotal = o.valor_total, 
                MaoDeObra = o.valor_mao_obra, 
                Pecas = o.valor_pecas 
            }).ToList();

            int totalOS = dados.Count;

            int emAtendimento = dados.Count(o => o.Status == "ABERTA");

            int paraRetirada = dados.Count(o => o.Status == "AGUARDANDO_RETIRADA" || 
                                                o.Status == "AGUARDANDO RETIRADA" ||
                                                o.Status == "PARA RETIRADA" || 
                                                o.Status == "CONCLUIDA" ||
                                                o.Status == "CONCLUÍDA");

            decimal totalAReceber = dados.Where(o => o.Status == "ABERTA" || 
                                                     o.Status == "AGUARDANDO_RETIRADA" || 
                                                     o.Status == "AGUARDANDO RETIRADA" || 
                                                     o.Status == "PARA RETIRADA")
                                         .Sum(o => o.ValorTotal > 0 ? o.ValorTotal : (o.MaoDeObra + o.Pecas));

            var osFinalizadas = dados.Where(o => o.Status == "FINALIZADA").ToList();
            decimal totalRecebido = osFinalizadas.Sum(o => o.ValorTotal > 0 ? o.ValorTotal : (o.MaoDeObra + o.Pecas));
            int qntRecebido = osFinalizadas.Count;

            var osCanceladas = dados.Where(o => o.Status == "CANCELADA").ToList();
            decimal totalCancelado = osCanceladas.Sum(o => o.ValorTotal > 0 ? o.ValorTotal : (o.MaoDeObra + o.Pecas));
            int qntCancelado = osCanceladas.Count;

            return (totalOS, emAtendimento, paraRetirada, totalAReceber, totalRecebido, qntRecebido, totalCancelado, qntCancelado);
        }

        public IQueryable<OrdemServico> AplicarFiltros(OrdemServico filtro)
        {
            var query = context.OrdemServicos
                .Include(o => o.Cliente)
                .Include(o => o.Tecnico)
                .Include(o => o.Equipamento)
                .AsQueryable();

            if (filtro == null) return query;

            if (!string.IsNullOrWhiteSpace(filtro.filtroStatus))
            {
                query = query.Where(o => o.status == filtro.filtroStatus);
            }

            if (!string.IsNullOrWhiteSpace(filtro.filtroBusca))
            {
                string termo = filtro.filtroBusca.Trim().ToLower();
                bool ehNumero = int.TryParse(termo, out int idBusca);

                query = query.Where(o =>
                    (ehNumero && o.id_os == idBusca) ||
                    (o.Cliente != null && o.Cliente.Nome.ToLower().Contains(termo)) ||
                    (o.Tecnico != null && o.Tecnico.Nome.ToLower().Contains(termo)) ||
                    (o.Equipamento != null && o.Equipamento.Descricao.ToLower().Contains(termo))
                );
            }

            if (DateTime.TryParseExact(filtro.filtroDataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtInicio))
            {
                query = query.Where(o => o.data_abertura >= dtInicio.Date);
            }

            if (DateTime.TryParseExact(filtro.filtroDataConclusao, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtFim))
            {
                query = query.Where(o => o.data_fechamento < dtFim.Date.AddDays(1));
            }

            return query;
        }
        
        public DataTable FiltrarHistorico(int? idCliente, int? idTecnico, string dataInicio, string dataFim, string busca, string status)
        {
            var query = context.OrdemServicos
                .Include(o => o.Cliente)
                .Include(o => o.Tecnico)
                .Include(o => o.Equipamento)
                .AsQueryable();

            if (idCliente.HasValue && idCliente.Value > 0)
            {
                query = query.Where(o => o.id_cliente == idCliente.Value);
            }

            if (idTecnico.HasValue && idTecnico.Value > 0)
            {
                query = query.Where(o => o.id_tecnico == idTecnico.Value);
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "TODOS" && status != "Todos")
            {
                query = query.Where(o => o.status == status);
            }

            if (!string.IsNullOrWhiteSpace(busca))
            {
                string termo = busca.Trim().ToLower();
                bool ehNumero = int.TryParse(termo, out int idBusca);

                query = query.Where(o =>
                    (ehNumero && o.id_os == idBusca) ||
                    (o.Cliente != null && o.Cliente.Nome.ToLower().Contains(termo)) ||
                    (o.Tecnico != null && o.Tecnico.Nome.ToLower().Contains(termo)) ||
                    (o.Equipamento != null && o.Equipamento.Descricao.ToLower().Contains(termo))
                );
            }

            if (DateTime.TryParseExact(dataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtInicio))
            {
                query = query.Where(o => o.data_abertura >= dtInicio.Date);
            }

            if (DateTime.TryParseExact(dataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtFim))
            {
                query = query.Where(o => o.data_fechamento < dtFim.Date.AddDays(1));
            }

            var ordens = query.ToList();
            var osIds = ordens.Select(o => o.id_os).ToList();

            var contasMap = context.ContasReceber
                .Include(c => c.Pagamento)
                .Where(c => c.id_os_fk.HasValue && osIds.Contains(c.id_os_fk.Value))
                .ToList()
                .GroupBy(c => c.id_os_fk.Value)
                .ToDictionary(
                    g => g.Key,
                    g => g.FirstOrDefault()?.Pagamento?.Descricao ?? "Não registrado"
                );

            DataTable tabela = new DataTable();
            tabela.Columns.Add("ID", typeof(int));
            tabela.Columns.Add("Cliente", typeof(string));
            tabela.Columns.Add("Técnico", typeof(string));
            tabela.Columns.Add("Equipamento", typeof(string));
            tabela.Columns.Add("Status", typeof(string));
            tabela.Columns.Add("Data de Abertura", typeof(DateTime));
            tabela.Columns.Add("Ultima Atualização", typeof(DateTime));
            tabela.Columns.Add("Data de Conclusão", typeof(DateTime));
            tabela.Columns.Add("Valor Total", typeof(decimal));
            tabela.Columns.Add("Forma de Pagamento", typeof(string));

            foreach (var o in ordens)
            {
                string descPagamento = contasMap.ContainsKey(o.id_os) ? contasMap[o.id_os] : "-";

                tabela.Rows.Add(
                    o.id_os,
                    o.Cliente != null ? o.Cliente.Nome : string.Empty,
                    o.Tecnico != null ? o.Tecnico.Nome : string.Empty,
                    o.Equipamento != null ? o.Equipamento.Descricao : string.Empty,
                    o.status,
                    o.data_abertura,
                    o.data_atualizacao,
                    o.data_fechamento,
                    o.valor_total,
                    descPagamento
                );
            }

            return tabela;
        }

        #endregion
    }
}
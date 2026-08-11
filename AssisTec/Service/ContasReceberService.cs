using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Repository;
using AssisTec.Dtos;

namespace AssisTec.Service
{
    public class ContasReceberService
    {
        private readonly IContaReceberRepository _repository;
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IOrdemServicoRepository _ordemServicoRepository;

        public ContasReceberService(
            IContaReceberRepository repository, 
            IPagamentoRepository pagamentoRepository,
            IOrdemServicoRepository ordemServicoRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _pagamentoRepository = pagamentoRepository ?? throw new ArgumentNullException(nameof(pagamentoRepository));
            _ordemServicoRepository = ordemServicoRepository;
        }

        public void ProcessarContasAtrasadas()
        {
            var contasDto = _repository.ObterTodos();
            var dataAtual = DateTime.Today;

            foreach (var contaDto in contasDto)
            {
                if (contaDto.Status == "PENDENTE" && contaDto.DataVencimento.HasValue && contaDto.DataVencimento.Value.Date < dataAtual)
                {
                    var conta = _repository.ObterPorId(contaDto.IdContaReceber);
                    if (conta != null)
                    {
                        conta.status = "ATRASADO";
                        _repository.Atualizar(conta);
                    }
                }
            }
        }

        public void Excluir(int id)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");

            if (!_repository.Excluir(id))
                throw new InvalidOperationException("A conta informada não foi localizada para exclusão.");
        }

        public IEnumerable<ContasReceberDto> CarregarTodas()
        {
            var contasDto = _repository.ObterTodos().ToList();
            var dataAtual = DateTime.Today;

            foreach (var contaDto in contasDto)
            {
                if (contaDto.Status == "PENDENTE" && contaDto.DataVencimento.HasValue && contaDto.DataVencimento.Value.Date < dataAtual)
                {
                    contaDto.Status = "ATRASADO";

                    var contaDb = _repository.ObterPorId(contaDto.IdContaReceber);
                    if (contaDb != null)
                    {
                        contaDb.status = "ATRASADO";
                        _repository.Atualizar(contaDb);
                    }
                }
            }

            return contasDto;
        }

        public DataTable CarregarFormasPagamento(bool incluirOpcaoTodas = false)
        {
            var dt = _pagamentoRepository.carregarFormasPamento();

            if (incluirOpcaoTodas)
            {
                DataRow dr = dt.NewRow();
                dr["id_forma_pagamento"] = 0;
                dr["exibicao"] = "Todas as formas de pagamento";
                dt.Rows.InsertAt(dr, 0);
            }
            return dt;
        }
        
        public ContasReceber ObterPorId(int id)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");

            return _repository.ObterPorId(id)
                ?? throw new InvalidOperationException("Conta a receber não localizada.");
        }

        public void Salvar(ContasReceber conta, bool ehInsercao)
        {
            ValidarCampos(conta);

            if (ehInsercao)
            {
                if (!_repository.Inserir(conta))
                    throw new InvalidOperationException("Erro ao inserir conta.");
            }
            else
            {
                if (conta.id_conta_receber <= 0) throw new ArgumentException("ID inválido.");
                if (!_repository.Atualizar(conta))
                    throw new InvalidOperationException("Erro ao atualizar conta.");
            }
        }

        private void ValidarCampos(ContasReceber conta)
        {
            if (string.IsNullOrWhiteSpace(conta.descricao)) 
                throw new ArgumentException("Descrição obrigatória.");
            if (conta.valor <= 0) 
                throw new ArgumentException("Valor deve ser maior que zero.");
            if (string.IsNullOrWhiteSpace(conta.status)) 
                throw new ArgumentException("Status obrigatório.");
            if (conta.data_emissao == DateTime.MinValue) 
                throw new ArgumentException("Data de emissão inválida.");
            if (conta.data_vencimento == DateTime.MinValue) 
                throw new ArgumentException("Data de vencimento inválida.");
        }

        private bool ValidarData(string data)
            => !string.IsNullOrWhiteSpace(data?.Replace("/", "").Trim())
               && DateTime.TryParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

        public void ValidarPagamento(DataGridViewRow row)
        {
            if (row == null) throw new InvalidOperationException("Nenhuma conta selecionada.");
            if (row.Cells["Status"].Value?.ToString() == "PAGA")
                throw new InvalidOperationException("Registro de pagamento apenas para contas não pagas.");
        }

        public (DataTable Dados, decimal TotalGeral, decimal TotalRecebido, decimal TotalPendente, decimal TotalAtrasado) Filtrar(
            string dataInicio, string dataFim, string descricao, int statusIndex, string statusText, object idFormaPagamento)
        {
            var filtro = new ContasReceber
            {
                filtroDataInicio = ValidarData(dataInicio) ? dataInicio : null,
                filtroDataFim    = ValidarData(dataFim)    ? dataFim    : null,
                filtroDescricao  = descricao?.Trim(),
                filtroStatus     = statusIndex > 0 ? statusText : null,
                filtroIdFormaPagamento = int.TryParse(idFormaPagamento?.ToString(), out int id) && id > 0 ? id : (int?)null
            };

            var dados  = _repository.Filtrar(filtro);
            var totais = _repository.ObterTotais(filtro);

            if (dados != null)
            {
                string[] colunasParaRemover = { "ClienteNome", "Equipamento", "DefeitoRelatado", "ServicoRealizado" };

                foreach (string nomeColuna in colunasParaRemover)
                {
                    if (dados.Columns.Contains(nomeColuna))
                    {
                        dados.Columns.Remove(nomeColuna);
                    }
                }
            }

            return (dados, totais.TotalGeral, totais.TotalRecebido, totais.TotalPendente, totais.TotalAtrasado);
        }

        public (decimal TotalGeral, decimal TotalRecebido, decimal TotalPendente, decimal TotalAtrasado) ObterTotaisPadrao()
        {
            return _repository.ObterTotais(new ContasReceber());
        }

        public void GerarRelatorioFiltradoPdf(string dataInicio, string dataFim, string descricao, int statusIndex, string statusText, object idFormaPagamento, string nomeFormaPagamento, string caminhoDestino)
        {
            try
            {
                var resultadoFiltro = Filtrar(dataInicio, dataFim, descricao, statusIndex, statusText, idFormaPagamento);

                string periodo = "Todos";
                if (!string.IsNullOrEmpty(dataInicio) && !string.IsNullOrEmpty(dataFim))
                {
                    periodo = $"{dataInicio} até {dataFim}";
                }
                else if (!string.IsNullOrEmpty(dataInicio))
                {
                    periodo = $"A partir de {dataInicio}";
                }
                else if (!string.IsNullOrEmpty(dataFim))
                {
                    periodo = $"Até {dataFim}";
                }

                ContasReceberDto.ContasReceberRelatorioDTO dtoRelatorio = new ContasReceberDto.ContasReceberRelatorioDTO
                {
                    FiltroPeriodo = periodo,
                    FiltroDescricao = string.IsNullOrEmpty(descricao) ? "Todas" : descricao,
                    FiltroStatus = statusIndex > 0 ? statusText : "Todos",
                    FiltroFormaPagamento = string.IsNullOrEmpty(nomeFormaPagamento) ? "Todas" : nomeFormaPagamento,
                    TotalGeral = resultadoFiltro.TotalGeral,
                    TotalRecebido = resultadoFiltro.TotalRecebido,
                    TotalPendente = resultadoFiltro.TotalPendente,
                    TotalAtrasado = resultadoFiltro.TotalAtrasado,
                    Itens = new List<ContasReceberDto>()
                };

                DataTable tabelaDados = resultadoFiltro.Dados;
                if (tabelaDados != null)
                {
                    foreach (DataRow row in tabelaDados.Rows)
                    {
                        dtoRelatorio.Itens.Add(MapearDataRowParaDto(row, tabelaDados));
                    }
                }

                GeradorPdfContasReceber.GerarRelatorioGeral(dtoRelatorio, caminhoDestino);
            }
            catch (Exception ex)
            {
                string mensagemDetalhada = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception("Falha ao gerar o relatório de contas a receber em PDF: " + mensagemDetalhada, ex);
            }
        }

        public void GerarRelatorioIndividualPdf(int idContaReceber, string caminhoDestino)
        {
            try
            {
                ContasReceber contaDb = ObterPorId(idContaReceber);

                ContasReceberDto dto = new ContasReceberDto
                {
                    IdContaReceber = contaDb.id_conta_receber,
                    Descricao = contaDb.descricao ?? string.Empty,
                    Valor = contaDb.valor,
                    DataEmissao = contaDb.data_emissao,
                    DataVencimento = contaDb.data_vencimento,
                    DataPagamento = contaDb.data_pagamento,
                    Status = contaDb.status ?? string.Empty,
                    Observacoes = contaDb.observacoes ?? string.Empty,
                    IdOrdemServico = contaDb.id_os_fk,
                    FormaPagamentoDescricao = contaDb.Pagamento?.Descricao ?? "Não informada"
                };

                if (dto.IdOrdemServico.HasValue && dto.IdOrdemServico.Value > 0 && _ordemServicoRepository != null)
                {
                    var os = _ordemServicoRepository.ObterPorId(dto.IdOrdemServico.Value);

                    if (os != null)
                    {
                        dto.ClienteNome = os.Cliente?.Nome ?? string.Empty;
                        dto.Equipamento = os.Equipamento?.Descricao ?? string.Empty;
                        dto.DefeitoRelatado = os.problema_relatado ?? string.Empty;
                        dto.ServicoRealizado = os.diagnostico ?? string.Empty;
                    }
                }

                GeradorPdfContasReceber.GerarRelatorioIndividual(dto, caminhoDestino);
            }
            catch (Exception ex)
            {
                string mensagemDetalhada = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception("Falha ao gerar o relatório individual da conta: " + mensagemDetalhada, ex);
            }
        }

        private ContasReceberDto MapearDataRowParaDto(DataRow row, DataTable dt)
        {
            return new ContasReceberDto
            {
                IdContaReceber = ObterIntColuna(row, dt, "ID_CONTA_RECEBER", "ID"),
                Descricao = ObterValorColuna(row, dt, "DESCRICAO", "DESC_CONTA"),
                Valor = ObterDecimalColuna(row, dt, "VALOR", "VALOR_TOTAL"),
                DataEmissao = ObterDateTimeColuna(row, dt, "DATA_EMISSAO", "EMISSAO") ?? DateTime.Now,
                DataVencimento = ObterDateTimeColuna(row, dt, "DATA_VENCIMENTO", "VENCIMENTO"),
                DataPagamento = ObterDateTimeColuna(row, dt, "DATA_PAGAMENTO", "PAGAMENTO"),
                Status = ObterValorColuna(row, dt, "STATUS", "SITUACAO"),
                Observacoes = ObterValorColuna(row, dt, "OBSERVACOES", "OBS"),
                IdOrdemServico = ObterIntNulavelColuna(row, dt, "ID_ORDEM_SERVICO", "ID_ORDEM"),
                FormaPagamentoDescricao = ObterValorColuna(row, dt, "FORMA_PAGAMENTO", "FORMA_PAGAMENTO_DESCRICAO")
            };
        }

        private string ObterValorColuna(DataRow row, DataTable table, string colPrincipal, string colAlternative)
        {
            if (table.Columns.Contains(colPrincipal) && row[colPrincipal] != DBNull.Value)
                return row[colPrincipal].ToString();

            if (table.Columns.Contains(colAlternative) && row[colAlternative] != DBNull.Value)
                return row[colAlternative].ToString();

            return string.Empty;
        }

        private decimal ObterDecimalColuna(DataRow row, DataTable table, string colPrincipal, string colAlternative)
        {
            if (table.Columns.Contains(colPrincipal) && row[colPrincipal] != DBNull.Value)
                return Convert.ToDecimal(row[colPrincipal]);

            if (table.Columns.Contains(colAlternative) && row[colAlternative] != DBNull.Value)
                return Convert.ToDecimal(row[colAlternative]);

            return 0m;
        }

        private int ObterIntColuna(DataRow row, DataTable table, string colPrincipal, string colAlternative)
        {
            if (table.Columns.Contains(colPrincipal) && row[colPrincipal] != DBNull.Value)
                return Convert.ToInt32(row[colPrincipal]);

            if (table.Columns.Contains(colAlternative) && row[colAlternative] != DBNull.Value)
                return Convert.ToInt32(row[colAlternative]);

            return 0;
        }

        private int? ObterIntNulavelColuna(DataRow row, DataTable table, string colPrincipal, string colAlternative)
        {
            if (table.Columns.Contains(colPrincipal) && row[colPrincipal] != DBNull.Value)
                return Convert.ToInt32(row[colPrincipal]);

            if (table.Columns.Contains(colAlternative) && row[colAlternative] != DBNull.Value)
                return Convert.ToInt32(row[colAlternative]);

            return null;
        }

        private DateTime? ObterDateTimeColuna(DataRow row, DataTable table, string colPrincipal, string colAlternative)
        {
            if (table.Columns.Contains(colPrincipal) && row[colPrincipal] != DBNull.Value)
                return Convert.ToDateTime(row[colPrincipal]);

            if (table.Columns.Contains(colAlternative) && row[colAlternative] != DBNull.Value)
                return Convert.ToDateTime(row[colAlternative]);

            return null;
        }
    }
}
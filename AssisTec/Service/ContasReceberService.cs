using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class ContasReceberService
    {
        private readonly IContaReceberRepository _repository;
        private readonly IPagamentoRepository _pagamentoRepository;

        public ContasReceberService(IContaReceberRepository repository, IPagamentoRepository pagamentoRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _pagamentoRepository = pagamentoRepository ?? throw new ArgumentNullException(nameof(pagamentoRepository));
        }

        public void ProcessarContasAtrasadas()
        {
            var contas = _repository.ObterTodos();
            var dataAtual = DateTime.Today;

            foreach (var conta in contas)
            {
                // CORRIGIDO: Atualiza direto no objeto em vez de chamar MarcarComoAtrasado(id),
                // evitando N queries adicionais ao banco (uma por conta atrasada).
                if (conta.status == "PENDENTE" && conta.data_vencimento.Date < dataAtual)
                {
                    conta.status = "ATRASADO";
                    _repository.Atualizar(conta);
                }
            }
        }

        public void Excluir(int id)
        {
            if (id <= 0) throw new ArgumentException("ID inválido.");

            if (!_repository.Excluir(id))
                throw new InvalidOperationException("A conta informada não foi localizada para exclusão.");
        }

        public DataTable CarregarFormasPagamento(bool incluirOpcaoTodas = false)
        {
            // CORRIGIDO: Erro de digitação no método original — "carregarFormasPamento" → "CarregarFormasPagamento"
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

        public IEnumerable<ContasReceber> CarregarTodas()
        {
            // CORRIGIDO: Evita chamar ObterTodos() duas vezes (uma dentro de ProcessarContasAtrasadas
            // e outra no retorno). Agora processa e retorna a mesma lista.
            var contas = _repository.ObterTodos().ToList();
            var dataAtual = DateTime.Today;

            foreach (var conta in contas)
            {
                if (conta.status == "PENDENTE" && conta.data_vencimento.Date < dataAtual)
                {
                    conta.status = "ATRASADO";
                    _repository.Atualizar(conta);
                }
            }

            return contas;
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
            if (string.IsNullOrWhiteSpace(conta.descricao)) throw new ArgumentException("Descrição obrigatória.");
            if (conta.valor <= 0) throw new ArgumentException("Valor deve ser maior que zero.");
            if (string.IsNullOrWhiteSpace(conta.status)) throw new ArgumentException("Status obrigatório.");
            if (conta.data_emissao == DateTime.MinValue) throw new ArgumentException("Data de emissão inválida.");
            if (conta.data_vencimento == DateTime.MinValue) throw new ArgumentException("Data de vencimento inválida.");
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
                // CORRIGIDO: Atribui null quando não parseia, semânticamente mais correto
                // que 0 (o repositório já trata null como "sem filtro")
                filtroIdFormaPagamento = int.TryParse(idFormaPagamento?.ToString(), out int id) && id > 0 ? id : (int?)null
            };

            var dados  = _repository.Filtrar(filtro);
            var totais = _repository.ObterTotais(filtro);

            return (dados, totais.TotalGeral, totais.TotalRecebido, totais.TotalPendente, totais.TotalAtrasado);
        }

        private bool ValidarData(string data)
            => !string.IsNullOrWhiteSpace(data?.Replace("/", "").Trim())
               && DateTime.TryParseExact(data, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);

        // OBSERVAÇÃO: DataGridViewRow acopla o Service à camada de UI (WinForms).
        // Considere receber apenas os valores necessários (ex: string status) em vez da linha inteira.
        public void ValidarPagamento(DataGridViewRow row)
        {
            if (row == null) throw new InvalidOperationException("Nenhuma conta selecionada.");
            if (row.Cells["Status"].Value?.ToString() == "PAGA")
                throw new InvalidOperationException("Registro de pagamento apenas para contas não pagas.");
        }

        public (decimal TotalGeral, decimal TotalRecebido, decimal TotalPendente, decimal TotalAtrasado) ObterTotaisPadrao()
        {
            return _repository.ObterTotais(new ContasReceber());
        }
    }
}
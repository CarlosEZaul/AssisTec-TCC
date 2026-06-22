using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using AssisTec.Dtos;
using AssisTec.Models;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class ContasPagarService
    {
        private readonly IContasPagarRepository _repository;
        private readonly IPagamentoRepository _pagamento;

        public ContasPagarService(IContasPagarRepository repository, IPagamentoRepository pagamento)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _pagamento = pagamento ?? throw new ArgumentNullException(nameof(pagamento));
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
            if(id<=0) throw new ArgumentException("ID inválido");
            if (!_repository.Excluir(id))
                throw new InvalidOperationException("Falha ao excluir");

        }

        public ContasPagar ObterPorId(int id)
        {
            if(id<=0) throw new ArgumentNullException(nameof(id));
            return _repository.ObterPorId(id) ?? throw new InvalidOperationException("Nenhuma conta existe");
        }

        public IEnumerable<ContasPagarDto> ObterTodasContas()
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

            return  contasDto;
        }

        public void Salvar(ContasPagar contasPagar, bool ehInsercao)
        {
            ValidarCampos(contasPagar);
            
            if (ehInsercao)
            {
                if (!_repository.Inserir(contasPagar))
                    throw new InvalidOperationException("Erro ao inserir conta.");
            }
            else
            {
                if (contasPagar.id_conta_pagar <= 0) throw new ArgumentException("ID inválido.");
                if (!_repository.Atualizar(contasPagar))
                    throw new InvalidOperationException("Erro ao atualizar conta.");
            }
        }
        
        private void ValidarCampos(ContasPagar conta)
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
        
        public (DataTable Dados, decimal TotalGeral, decimal TotalPagar, decimal TotalPendente, decimal TotalAtrasado) Filtrar(
            string dataInicio, string dataFim, string descricao, int statusIndex, string statusText, object idFormaPagamento)
        {
            var filtro = new ContasPagar
            {
                filtroDataInicio = ValidarData(dataInicio) ? dataInicio : null,
                filtroDataFim    = ValidarData(dataFim)    ? dataFim    : null,
                filtroDescricao  = descricao?.Trim(),
                filtroStatus     = statusIndex > 0 ? statusText : null,
                filtroIdFormaPagamento = int.TryParse(idFormaPagamento?.ToString(), out int id) && id > 0 ? id : (int?)null
            };

            var dados  = _repository.Filtrar(filtro);
            var totais = _repository.ObterTotais(filtro);

            return (dados, totais.TotalGeral, totais.TotalPagar, totais.TotalPendente, totais.TotalAtrasado);
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

        public (decimal TotalGeral, decimal TotalPagar, decimal TotalPendente, decimal TotalAtrasado) ObterTotaisPadrao()
        {
            return _repository.ObterTotais(new ContasPagar());
        }
        
    }
}
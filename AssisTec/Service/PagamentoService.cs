using System;
using System.Data;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class PagamentoService
    {
        private readonly IContaReceberRepository _contasReceberRepository;
        private readonly IContasPagarRepository _contasPagarRepository;
        private readonly IPagamentoRepository _pagamentoRepository;

        public PagamentoService(IContaReceberRepository contasReceberRepository, IContasPagarRepository contasPagarRepository, IPagamentoRepository pagamentoRepository)
        {
            _contasReceberRepository = contasReceberRepository ?? throw new ArgumentNullException(nameof(contasReceberRepository));
            _contasPagarRepository = contasPagarRepository ?? throw new ArgumentNullException(nameof(contasPagarRepository));
            _pagamentoRepository = pagamentoRepository ?? throw new ArgumentNullException(nameof(pagamentoRepository));
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

        public void RegistrarPagamentoEntrada(int idConta, int idFormaPagamento, DateTime dataPagamento)
        {
            if (idConta <= 0)
                throw new ArgumentException("ID da conta inválido para registrar pagamento.");

            if (idFormaPagamento <= 0)
                throw new ArgumentException("Selecione uma forma de pagamento válida.");

            try
            {
                var conta = _contasReceberRepository.ObterPorId(idConta);
                if (conta == null)
                {
                    throw new Exception("A conta a receber informada não foi localizada no sistema.");
                }

                if (conta.status == "PAGA")
                {
                    throw new InvalidOperationException("Esta conta já foi baixada e está paga.");
                }

                conta.status = "PAGA";
                conta.id_forma_pagamento_fk = idFormaPagamento;
                conta.data_pagamento = dataPagamento;

                bool atualizou = _contasReceberRepository.Atualizar(conta);
                if (!atualizou)
                {
                    throw new Exception("Não foi possível persistir a baixa do pagamento no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Falha na camada de negócio ao registrar o pagamento: " + ex.Message, ex);
            }
        }

        public void RegistrarPagamentoSaida(int idConta, int idFormaPagamento, DateTime dataPagamento)
        {
            if (idConta <= 0)
                throw new ArgumentException("ID da conta inválido para registrar pagamento.");

            if (idFormaPagamento <= 0)
                throw new ArgumentException("Selecione uma forma de pagamento válida.");

            try
            {
                var conta = _contasPagarRepository.ObterPorId(idConta);
                if (conta == null)
                {
                    throw new Exception("A conta a pagar informada não foi localizada no sistema.");
                }

                if (conta.status == "PAGA")
                {
                    throw new InvalidOperationException("Esta conta já foi baixada e está paga.");
                }

                conta.status = "PAGA";
                conta.id_forma_pagamento_fk = idFormaPagamento;
                conta.data_pagamento = dataPagamento;

                bool atualizou = _contasPagarRepository.Atualizar(conta);
                if (!atualizou)
                {
                    throw new Exception("Não foi possível persistir a baixa do pagamento no banco de dados.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Falha na camada de negócio ao registrar o pagamento: " + ex.Message, ex);
            }
        }
    }
}
using System;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class PagamentoService
    {
        private readonly IContaReceberRepository _contasRepository;

        public PagamentoService()
        {
            var context = new AppDbContext();
            _contasRepository = new ContasReceberRepository(context);
        }

        public void RegistrarPagamentoEntrada(int idConta, int idFormaPagamento, DateTime dataPagamento)
        {
            if (idConta <= 0)
                throw new ArgumentException("ID da conta inválido para registrar pagamento.");

            if (idFormaPagamento <= 0)
                throw new ArgumentException("Selecione uma forma de pagamento válida.");

            try
            {
                var conta = _contasRepository.ObterPorId(idConta);
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

                bool atualizou = _contasRepository.AtualizarContasReceber(conta);
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
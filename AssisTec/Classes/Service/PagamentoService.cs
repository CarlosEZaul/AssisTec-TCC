using AssisTec.Data;

namespace AssisTec.Business
{
    public class PagamentoService
    {
        PagamentoRepository  pagamentoRepository = new PagamentoRepository();

        public (bool sucesso, string mensagem) RegistrarPagamentoEntrada(Pagamento pagamento, ContasReceber contasReceber)
        {
            bool sucesso = pagamentoRepository.registrarPagamentoEntrada(pagamento, contasReceber);

            if (sucesso)
            {
                return (true, "Pagamento registrado com sucesso");
            }
            else
            {
                return (false,"Falha ao registrar o Pagamento");
            }
        }
    }
}
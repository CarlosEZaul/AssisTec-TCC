using System;
using AssisTec.Data;

namespace AssisTec.Business
{
    public class ContasReceberService
    {
        private readonly ContasReceberRepositoy repository = new ContasReceberRepositoy();

        public (bool sucesso, string mensagem) CadastrarContasReceber(ContasReceber contasReceber)
        {
            if (contasReceber.status == "PAGA" && string.IsNullOrWhiteSpace(contasReceber.dataPagamento.Replace("/","").Trim()))
            {
                return (false, "Uma forma de pagamento deve ser selecionada");
            }

            if (Convert.ToDateTime(contasReceber.dataVencimento).Date < DateTime.Now)
            {
                return (false, "A data de venciamento não pode ser menor que a data de hoje");
            }
            
            bool sucesso = repository.SalvarEntrada(contasReceber);

            if (sucesso)
            {
                return (true, "Entrada realizada com sucesso");
            }
            else
            {
                return (false, "Erro ao cadastrar entrada");
            }
                
            
        }

        public (bool sucesso, string mensagem) EditarContasReceber(ContasReceber contasReceber)
        {
            if (contasReceber.status == "PAGA" && string.IsNullOrWhiteSpace(contasReceber.dataPagamento.Replace("/","").Trim()))
            {
                return (false, "Uma forma de pagamento deve ser selecionada");
            }

            if (Convert.ToDateTime(contasReceber.dataVencimento).Date < DateTime.Now)
            {
                return (false, "A data de venciamento não pode ser menor que a data de hoje");
            }
            
            bool sucesso = repository.editarContaReceber(contasReceber);

            if (sucesso)
            {
                return (true,"Conta editada com sucesso");
            }
                
            else
            {
                return (false, "Erro ao editar");
            }
        }

        public bool DeletarContasReceber(int id)
        {
            return repository.deletarContaReceber(id);
        }

        public bool AlterarParaAtrasado()
        {
            return repository.alterarParaAtrasado();
        }

        
    }
}
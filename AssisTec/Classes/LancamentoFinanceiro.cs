namespace AssisTec
{
    public class LancamentoFinanceiro
    {
        private int id_lancamento{get;set;}
        private OrdemDeServico ordemDeServico{get;set;}
        
        private string dataEmissao{get;set;}
        private string dataPagamento{get;set;}
        private string tipo{get;set;}
        private double valor{get;set;}
    }
}
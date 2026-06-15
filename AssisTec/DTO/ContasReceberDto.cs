using System;

namespace AssisTec.Dtos
{
    public class ContasReceberDto
    {
        public int IdContaReceber { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string Status { get; set; }
        public string Observacoes { get; set; }
        public int? IdOrdemServico { get; set; }
        public string FormaPagamentoDescricao { get; set; }
    }
}
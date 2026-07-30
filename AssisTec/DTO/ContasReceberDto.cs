using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Dtos
{
    public class ContasReceberDto
    {
        public int IdContaReceber { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime DataEmissao { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? DataVencimento { get; set; }
        [Column(TypeName = "timestamp")]
        public DateTime? DataPagamento { get; set; }
        public string Status { get; set; }
        public string Observacoes { get; set; }
        public int? IdOrdemServico { get; set; }
        public string FormaPagamentoDescricao { get; set; }
    }
}
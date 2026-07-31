using System;
using System.Collections.Generic;

namespace AssisTec.Dtos
{
    public class ContasPagarDto
    {
        public int IdContaPagar { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataEmissao { get; set; }
        public DateTime? DataVencimento { get; set; }
        public DateTime? DataPagamento { get; set; }
        public string Status { get; set; }
        public string Observacoes { get; set; }
        public string FormaPagamentoDescricao { get; set; }

        public class ContasPagarRelatorioDTO
        {
            public string FiltroPeriodo { get; set; }
            public string FiltroDescricao { get; set; }
            public string FiltroStatus { get; set; }
            public string FiltroFormaPagamento { get; set; }
            public decimal TotalGeral { get; set; }
            public decimal TotalPagar { get; set; }
            public decimal TotalPendente { get; set; }
            public decimal TotalAtrasado { get; set; }
            public List<ContasPagarDto> Itens { get; set; } = new List<ContasPagarDto>();
        }
    }
}
using System;
using System.Collections.Generic;

namespace AssisTec.Dtos
{
    public class MovimentacaoItemDTO
    {
        public int IdMovimentacao { get; set; }
        public string Produto { get; set; }
        public DateTime Data { get; set; }
        public int Quantidade { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimentacao { get; set; }
        public string Descricao { get; set; }
        public string Registrado { get; set; }
    }
    public class MovimentacoesEstoqueDTO
    {
        public string Periodo { get; set; }
        public string ProdutoFiltro { get; set; }
        public string TipoFiltro { get; set; }
        public int TotalEntradas { get; set; }
        public int TotalSaidas { get; set; }
        public int BalancoLiquido => TotalEntradas - TotalSaidas;
        public List<MovimentacaoItemDTO> Itens { get; set; }
    }
}
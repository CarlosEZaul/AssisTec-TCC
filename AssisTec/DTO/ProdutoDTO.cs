using System.Collections.Generic;

namespace AssisTec.Dtos
{
    public class ProdutoDTO
    {
        public class ProdutoRelatorioDTO
        {
            public int IdProduto { get; set; }
            public string Descricao { get; set; }
            public string Unidade { get; set; }
            public decimal PrecoVenda { get; set; }
            public decimal PrecoCompra { get; set; }
            public int Quantidade { get; set; }
            public int QuantidadeMinima { get; set; }
            public string Status { get; set; }
        }

        public class EstoqueRelatorioDTO
        {
            public string FiltroDescricao { get; set; }
            public string FiltroStatus { get; set; }
            public int TotalCadastrado { get; set; }
            public int AbaixoMinimo { get; set; }
            public int SemEstoque { get; set; }
            public decimal ValorEstoque { get; set; }
            public List<ProdutoRelatorioDTO> Itens { get; set; }
        }
    }
}
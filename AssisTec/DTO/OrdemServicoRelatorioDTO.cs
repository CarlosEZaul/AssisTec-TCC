using System;
using System.Collections.Generic;

namespace AssisTec.DTO
{
    public class ItemOSRelatorioDTO
    {
        public string Descricao { get; set; }
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
        public decimal ValorTotal { get; set; }
        public string Tipo { get; set; }
    }

    public class OrdemServicoRelatorioDTO
    {
        public int IdOS { get; set; }
        public DateTime DataAbertura { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string Status { get; set; }

        public string NomeCliente { get; set; }
        public string DocumentoCliente { get; set; }
        public string TelefoneCliente { get; set; }
        public string EnderecoCliente { get; set; }

        public string Equipamento { get; set; }
        public string MarcaModelo { get; set; }
        public string NumeroSerie { get; set; }
        public string DefeitoRelatado { get; set; }
        public string LaudoTecnico { get; set; }

        public decimal ValorPecas { get; set; }
        public decimal ValorMaoObra { get; set; }
        public decimal ValorTotal { get; set; }
        public string FormaPagamento { get; set; }

        public List<ItemOSRelatorioDTO> Itens { get; set; } = new List<ItemOSRelatorioDTO>();
    }
}
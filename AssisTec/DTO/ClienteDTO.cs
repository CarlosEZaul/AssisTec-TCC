using System;
using System.Collections.Generic;

namespace AssisTEC.DTO
{
    public class ClienteDTO
    {
        public class ClientesRelatorioDTO
        {
            public string FiltroNome { get; set; }
            public string FiltroStatus { get; set; }
            public int TotalAtivos { get; set; }
            public int TotalInativos { get; set; }
            public int TotalGeral { get; set; }
            public List<ClienteRelatorioDTO> Itens { get; set; }
        }

        public class ClienteRelatorioDTO
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Cpf { get; set; }
            public string Telefone { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
            public string Status { get; set; }
        }

        public class ClienteComOrdemServicoDTO
        {
            public int IdCliente { get; set; }
            public string Nome { get; set; }
            public string Cpf { get; set; }
            public string Telefone { get; set; }
            public string StatusCliente { get; set; }
            public int TotalOrdens { get; set; }
            public int OrdensAbertas { get; set; }
            public int OrdensFinalizadas { get; set; }
            public decimal TotalGasto { get; set; }
            public List<OrdemServicoItemDTO> Ordens { get; set; }
        }

        public class OrdemServicoItemDTO
        {
            public int IdOrdemServico { get; set; }
            public string Tecnico { get; set; }
            public string Equipamento { get; set; }
            public DateTime DataAbertura { get; set; }
            public DateTime? DataFechamento { get; set; }
            public decimal ValorTotal { get; set; }
            public string Status { get; set; }
        }
    }
}
using System;
using System.Collections.Generic;

namespace AssisTec.Dtos
{
    public class UsuarioDTO
    {
        public class UsuarioRelatorioDTO
        {
            public int Id { get; set; }
            public string Nome { get; set; }
            public string Cpf { get; set; }
            public string Telefone { get; set; }
            public int Nivel { get; set; }
            public string Status { get; set; }
            public string Cidade { get; set; }
            public string Estado { get; set; }
        }

        public class UsuariosRelatorioDTO
        {
            public string FiltroNome { get; set; }
            public string FiltroNivel { get; set; }
            public string FiltroStatus { get; set; }
            public int TotalAtivos { get; set; }
            public int TotalInativos { get; set; }
            public int TotalGeral => TotalAtivos + TotalInativos;
            public List<UsuarioRelatorioDTO> Itens { get; set; }
        }
        
        public class OrdemServicoItemDTO
        {
            public int IdOrdemServico { get; set; }
            public string Cliente { get; set; }
            public string Equipamento { get; set; }
            public DateTime DataAbertura { get; set; }
            public DateTime? DataFechamento { get; set; }
            public decimal ValorTotal { get; set; }
            public string Status { get; set; }
        }

        public class UsuarioComOrdemServicoDTO
        {
            public int IdUsuario { get; set; }
            public string Nome { get; set; }
            public string Cpf { get; set; }
            public string Telefone { get; set; }
            public string NivelDescricao { get; set; }
            public string StatusUsuario { get; set; }
        
            public int TotalOrdens { get; set; }
            public int OrdensAbertas { get; set; }
            public int OrdensFinalizadas { get; set; }
            public decimal FaturamentoGerado { get; set; }

            public List<OrdemServicoItemDTO> Ordens { get; set; }
        }
    }
}
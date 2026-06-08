using System;
using System.ComponentModel.DataAnnotations;

namespace AssisTec.Models
{
    public class OrdemServico
    {
        public int id_os { get; set; }

        public int? id_tecnico { get; set; }
        public virtual Usuario Tecnico { get; set; }

        public int? id_cliente { get; set; }
        public virtual Cliente Cliente { get; set; }

        public int? id_equipamento { get; set; }
        public virtual Equipamento Equipamento { get; set; }

        [Required]
        [StringLength(30)]
        public string status { get; set; } = "ABERTA";

        [Required]
        public DateTime data_abertura { get; set; } = DateTime.Now;

        public DateTime? data_atualizacao { get; set; }

        public DateTime? data_fechamento { get; set; }

        [Required]
        public decimal valor_mao_obra { get; set; } = 0.00m;

        [Required]
        public decimal valor_pecas { get; set; } = 0.00m;

        [Required]
        public decimal valor_total { get; set; } = 0.00m;

        [StringLength(500)]
        public string problema_relatado { get; set; } = string.Empty;

        [StringLength(500)]
        public string diagnostico { get; set; } = string.Empty;

        [StringLength(500)]
        public string observacoes { get; set; } = string.Empty;
    }
}
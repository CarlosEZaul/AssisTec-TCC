using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "timestamp")]
        public DateTime data_abertura { get; set; } = DateTime.Now;
        
        [Column(TypeName = "timestamp")]
        public DateTime? data_atualizacao { get; set; }

        [Column(TypeName = "timestamp")]
        public DateTime? data_fechamento { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal valor_mao_obra { get; set; } = 0.00m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal valor_pecas { get; set; } = 0.00m;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal valor_total { 
            get 
            { 
                return valor_mao_obra + valor_pecas; 
            }
            set { } 
        }

        [StringLength(500)]
        public string problema_relatado { get; set; } = string.Empty;

        [StringLength(500)]
        public string diagnostico { get; set; } = string.Empty;

        [StringLength(500)]
        public string observacoes { get; set; } = string.Empty;
        
        [NotMapped]
        [Browsable(false)]
        public decimal valor_pagamento { get; set; }
            
        
        [NotMapped]
        [Browsable(false)]
        public string filtroDataInicio { get; set; } = string.Empty;

        [NotMapped]
        [Browsable(false)]
        public string filtroDataConclusao { get; set; } = string.Empty;

        [NotMapped]
        [Browsable(false)]
        public string filtroBusca { get; set; } = string.Empty;

        [NotMapped]
        [Browsable(false)]
        public string filtroStatus { get; set; } = string.Empty;
    }
}
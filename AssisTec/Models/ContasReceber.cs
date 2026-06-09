using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    public class ContasReceber
    {
        public int id_conta_receber { get; set; }
        
        [Required]
        [StringLength(100)]
        public string descricao { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal valor { get; set; }
        
        [Required]
        public DateTime data_emissao { get; set; }
        public DateTime? data_pagamento { get; set; }
        public DateTime data_vencimento { get; set; }
        
        [Required]
        [StringLength(30)]
        public string status { get; set; }
        
        [StringLength(100)]
        public string observacoes { get; set; }

        public int? id_os_fk { get; set; }
        public virtual OrdemServico OrdemServico { get; set; }

        public int? id_forma_pagamento_fk { get; set; }
        public virtual Pagamento Pagamento { get; set; }

        [NotMapped] [Browsable(false)] 
        public string filtroDataInicio { get; set; } = string.Empty;
        [NotMapped] [Browsable(false)]
        public string filtroDataFim { get; set; } = string.Empty;
        [NotMapped] [Browsable(false)] 
        public string filtroDescricao { get; set; } = string.Empty;
        [NotMapped] [Browsable(false)] 
        public string filtroStatus { get; set; } = string.Empty;
        [NotMapped] [Browsable(false)]
        public int? filtroIdFormaPagamento { get; set; }
    }
}
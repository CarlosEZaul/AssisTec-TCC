using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    [Table("contas_receber")]
    public class ContasReceber
    {
        [Key]
        public int id_conta_receber { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(100)")]
        [StringLength(100)]
        public string descricao { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal valor { get; set; }
        
        [Required]
        [Column(TypeName = "datetime(6)")]
        public DateTime data_emissao { get; set; }

        [Column(TypeName = "datetime(6)")]
        public DateTime? data_pagamento { get; set; }

        [Required]
        [Column(TypeName = "datetime(6)")]
        public DateTime data_vencimento { get; set; }
        
        [Required]
        [Column(TypeName = "varchar(30)")]
        [StringLength(30)]
        public string status { get; set; }
        
        [Column(TypeName = "varchar(100)")]
        [StringLength(100)]
        public string observacoes { get; set; }

        public int? id_os_fk { get; set; }
        [ForeignKey("id_os_fk")]
        public virtual OrdemServico OrdemServico { get; set; }

        public int? id_forma_pagamento_fk { get; set; }
        [ForeignKey("id_forma_pagamento_fk")]
        public virtual Pagamento Pagamento { get; set; }

        [NotMapped]
        [Browsable(false)]
        public string filtroDataInicio { get; set; } = string.Empty;

        [NotMapped]
        [Browsable(false)]
        public string filtroDataFim { get; set; } = string.Empty;

        [NotMapped]
        [Browsable(false)]
        public string filtroDescricao { get; set; } = string.Empty;

        [NotMapped]
        [Browsable(false)]
        public string filtroStatus { get; set; } = string.Empty;

        [NotMapped]
        [Browsable(false)]
        public int? filtroIdFormaPagamento { get; set; }
    }
}
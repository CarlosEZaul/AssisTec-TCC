using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    public class LancamentoFinanceiro
    {
        [Required]
        [StringLength(100)]
        public string descricao { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor deve ser maior que zero.")]
        public decimal valor { get; set; }

        [Required]
        public DateTime data_emissao { get; set; }

        public DateTime? data_pagamento { get; set; }

        public DateTime data_vencimento { get; set; }

        [Required]
        [StringLength(30)]
        public string status { get; set; } = string.Empty;

        [Column("id_forma_pagamento_fk")]
        public int? id_forma_pagamento_fk { get; set; }

        [ForeignKey("id_forma_pagamento_fk")]
        public Pagamento Pagamento { get; set; }

        public string observacoes { get; set; } = string.Empty;
    }
}
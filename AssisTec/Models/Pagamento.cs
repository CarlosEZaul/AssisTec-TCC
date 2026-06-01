using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    [Table("forma_pagamento")]
    public class Pagamento
    {
        [Key]
        [Required]
        [Column("id_forma_pagamento")]
        public int Idforma_pagamento { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;
    }
}
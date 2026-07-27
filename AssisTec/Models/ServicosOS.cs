using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    public class ServicosOS
    {
        [Key]
        public int idServico { get; set; }

        public int? id_OS { get; set; }
        public virtual OrdemServico OrdemServico { get; set; }

        [Required]
        [StringLength(150)]
        public string descricao { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal valor_cobrado { get; set; }
    }
}
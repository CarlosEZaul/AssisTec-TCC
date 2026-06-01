using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    [Table ("Equipamento")]
    public class Equipamento
    {
        [Key]
        [Required]
        [Column("id_equipamento")]
        public int Id_equipamento { get; set; }

        [Required]
        [StringLength(150)]
        public string Descricao { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Marca { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [StringLength (50)]
        public string Numero_Serie { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string estado_entrada{get;set;} = string.Empty;
        
        [StringLength(150)]
        public string acessorios { get; set; } = string.Empty;

        
        [Column (TypeName = "text")]
        public string Observacoes { get; set; } = string.Empty;

    }
}
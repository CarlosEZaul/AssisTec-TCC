using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    public class MovimentacaoEstoque
    {
        public int idMovimentacao { get; set; }
        
        [NotMapped]
        public virtual Produto produto { get; set; }
        
        public int? idProduto { get; set; }
        
        [Required]
        [Column(TypeName = "datetime(6)")]
        public DateTime data { get; set; }
        
        [Required]
        public int quantidade { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal valor { get; set; }
        
        [Required]
        [StringLength(100)]
        public string descricao { get; set; }
        
        [Required]
        [StringLength(10)]
        public string tipoMovimentacao { get; set; }
    }
}
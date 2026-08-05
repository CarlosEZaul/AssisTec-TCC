using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    public class MovimentacaoEstoque
    {
        public int idMovimentacao { get; set; }
        
       
        public virtual Produto produto { get; set; }
        public virtual Usuario usuario { get; set; }
        
        public int? idProduto { get; set; }
        
        [Required]
        [Column(TypeName = "timestamp")]
        public DateTime data { get; set; }
        
        [Required]
        public int quantidade { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal valor { get; set; }
        
        [Required]
        [StringLength(100)]
        public string descricao { get; set; }
        
        
       
        public int idUsuario { get; set; }
        
        [Required]
        [StringLength(10)]
        public string tipoMovimentacao { get; set; }
        
        [NotMapped]
        [Browsable(false)]
        public string filtroDescricao { get; set; } = string.Empty;
        
        [NotMapped]
        [Browsable(false)]
        public string filtroDataInicio { get; set; } = string.Empty;
        
        [NotMapped]
        [Browsable(false)]
        public string filtroDataFim { get; set; } = string.Empty;
        
        
    }
}
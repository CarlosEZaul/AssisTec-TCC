using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    public class Produto
    {
        
        public int idProduto { get; set; }
        
        [Required]
        [StringLength(100)]
        public string descricao { get; set; }
        
        [Required]
        [StringLength(2)]
        public string unidade { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal preco_venda { get; set; }
        
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal preco_compra { get; set; }
        
        [Required]
        public int quantidade { get; set; }
        
        [Required]
        public int quantidade_minima { get; set; }
        
        [NotMapped]
        [Browsable(false)]
        public string filtroDescricao { get; set; }
        
        [NotMapped]
        [Browsable(false)]
        public bool filtroAbaixoMinimo { get; set; }
        
        
        
    }
}
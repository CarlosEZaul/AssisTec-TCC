using System.ComponentModel.DataAnnotations.Schema;
using AssisTec.Models;

public class ItemOS
{
    public int Id { get; set; }
    public int Quantidade { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorUnitario { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal ValorTotal 
    { 
        get => Quantidade * ValorUnitario; 
        private set { } 
    }

    public int? id_OS { get; set; }
    public virtual OrdemServico OrdemServico { get; set; }

    public int? id_produto { get; set; }
    public virtual Produto Produto { get; set; }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    public class HistoricoAlteracaoOS
    {
        public int id{get;set;}
        public Usuario usuario{get;set;}
        public int idUsuario{get;set;}
        
        public OrdemServico ordemServico{get;set;}
        public int idOS{get;set;}
        
        [Required]
        [StringLength(100)]
        public string descricao{get;set;}
        
        [Required]
        [StringLength(100)]
        public string tipo{get;set;}
        
        [Required]
        [Column(TypeName = "datetime(6)")]
        public DateTime dataAlteracao{get;set;}
    }
}
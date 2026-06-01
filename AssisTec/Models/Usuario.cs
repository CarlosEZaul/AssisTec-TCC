using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        [Column("id_usuario")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nome")]
        public string Nome { get; set; }= string.Empty;

        [Required]
        [StringLength(14)]
        [Column("cpf")]
        public string Cpf { get; set; }= string.Empty;

        [Required]
        [StringLength(255)]
        [Column("senha")]
        public string Senha { get; set; }= string.Empty;

        [StringLength(20)]
        [Column("telefone")]
        public string Telefone { get; set; }= string.Empty;

        [Required]
        [Column("nivel")]
        public int Nivel { get; set; } 

        [Required]
        [StringLength(20)]
        [Column("status")]
        public string Status { get; set; }= string.Empty;

        [Required]
        [StringLength(9)]
        [Column("cep")]
        public string Cep { get; set; }= string.Empty;

        [Required]
        [StringLength(100)]
        [Column("rua")]
        public string Rua { get; set; }

        [Required]
        [StringLength(10)]
        [Column("numero")]
        public string Numero { get; set; }= string.Empty;

        [Required]
        [StringLength(60)]
        [Column("cidade")]
        public string Cidade { get; set; }= string.Empty;

        [Required]
        [StringLength(60)]
        [Column("bairro")]
        public string Bairro { get; set; }= string.Empty;

        [Required]
        [StringLength(100)]
        [Column("estado")]
        public string Estado { get; set; }= string.Empty;

        [StringLength(100)]
        [Column("complemento")]
        public string Complemento { get; set; }= string.Empty;

        
        
    }
}
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AssisTec.Models
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        [Column("id_cliente")]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Column("nome")]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(14)]
        [Column("cpf")]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Column("telefone")]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        [Column("data_nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required]
        [StringLength(9)]
        [Column("cep")]
        public string Cep { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("rua")]
        public string Rua { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        [Column("numero")]
        public string Numero { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        [Column("cidade")]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [Column("estado")]
        public string Estado { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        [Column("bairro")]
        public string Bairro { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("complemento")]
        public string Complemento { get; set; } = string.Empty;
    }
}
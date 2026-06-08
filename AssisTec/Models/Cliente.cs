using System;
using System.ComponentModel.DataAnnotations;

namespace AssisTec.Models
{
    public class Cliente
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; }

        [Required]
        [StringLength(14)]
        public string Cpf { get; set; }

        [StringLength(20)]
        public string Telefone { get; set; }

        public DateTime? DataNascimento { get; set; }

        [Required]
        [StringLength(9)]
        public string Cep { get; set; }

        [Required]
        [StringLength(100)]
        public string Rua { get; set; }

        [Required]
        [StringLength(10)]
        public string Numero { get; set; }

        [Required]
        [StringLength(60)]
        public string Cidade { get; set; }

        [Required]
        [StringLength(100)]
        public string Estado { get; set; }

        [Required]
        [StringLength(60)]
        public string Bairro { get; set; }

        [StringLength(100)]
        public string Complemento { get; set; }
    }
}
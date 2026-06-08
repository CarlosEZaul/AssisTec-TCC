using System;
using System.ComponentModel.DataAnnotations;

namespace AssisTec.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(14)]
        public string Cpf { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Senha { get; set; } = string.Empty;

        [StringLength(20)]
        public string Telefone { get; set; } = string.Empty;

        [Required]
        public int Nivel { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; } = string.Empty;

        [Required]
        [StringLength(9)]
        public string Cep { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Rua { get; set; } = string.Empty;

        [Required]
        [StringLength(10)]
        public string Numero { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        public string Cidade { get; set; } = string.Empty;

        [Required]
        [StringLength(60)]
        public string Bairro { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Estado { get; set; } = string.Empty;

        [StringLength(100)]
        public string Complemento { get; set; } = string.Empty;
    }
}
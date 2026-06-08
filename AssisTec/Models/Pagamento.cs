using System;
using System.ComponentModel.DataAnnotations;

namespace AssisTec.Models
{
    public class Pagamento
    {
        public int Idforma_pagamento { get; set; }

        [Required]
        [StringLength(100)]
        public string Descricao { get; set; } = string.Empty;
    }
}
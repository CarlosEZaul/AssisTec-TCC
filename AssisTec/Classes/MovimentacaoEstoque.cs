using System;

namespace AssisTec
{
    public class MovimentacaoEstoque
    {
        private int idMovimentacao { get; set; }
        private Produto produto { get; set; }
        private string data{get;set;}
        private string tipo{get;set;}
        private int quantidade { get; set; }
        private string descricao{get;set;}
    }
}
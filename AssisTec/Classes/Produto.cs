namespace AssisTec
{
    public class Produto
    {
        private int Id;
        private string Descricao;
        private string Unidade;
        private double PrecoCompra;
        private double PrecoVenda;
        private double Estoque;
        private double EstoqueMinimo;
        private string Fornecedor;

        public int id { get; set; }
        public string descricao { get; set; }
        public string unidade { get; set; }
        public double precoCompra { get; set; }
        public double precoVenda { get; set; }
        public double estoque { get; set; }
        public double estoqueMinimo { get; set; }
        public string fornecedor { get; set; }
    }
    
    
}
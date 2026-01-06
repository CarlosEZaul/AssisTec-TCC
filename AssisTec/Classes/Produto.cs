namespace AssisTec
{
    public class Produto
    {
        private int Id;
        private string Descricao;
        private string Unidade;
        private double PrecoCompra;
        private double PrecoVenda;
        private int Estoque;
        private int EstoqueMinimo;

        public int id { get; set; }
        public string descricao { get; set; }
        public string unidade { get; set; }
        public double precoCompra { get; set; }
        public double precoVenda { get; set; }
        public int estoque { get; set; }
        public int estoqueMinimo { get; set; }
    }
    
    
}
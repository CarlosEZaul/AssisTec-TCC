namespace AssisTec
{
    public class Usuario:Pessoa
    {
        private string Senha;
        private string Status;
        private int Nivel;
        public string cep { get; set; }
        public string rua { get; set; }
        public string numero { get; set; }
        public string cidade { get; set; }
        public string bairro { get; set; }
        public string estado { get; set; }
        public string complemento { get; set; }
        public string status
        {
            get { return Status; }
            set { Status = value; }
        }

        public string senha
        {
            get { return Senha; }
            set { Senha = value; }
        }

        public int nivel
        {
            get { return Nivel; }
            set { Nivel = value; }
        }
        
    }
    
}
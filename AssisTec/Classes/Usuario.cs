namespace AssisTec
{
    public class Usuario
    {
        
        
        private int Id;
        private string Nome;
        private string Cpf;
        private string Telefone;
        private string Senha;
        private int Nivel;
        private string Status;

       
        public int id
        {
            get { return Id;}
            set { Id = value;}
        }

        public string nome
        {
            get { return Nome;}
            set { Nome = value;}
        }

        public string cpf
        {
            get { return Cpf;}
            set { Cpf = value;}
        }

        public string telefone
        {
            get { return Telefone;}
            set { Telefone = value;}
        }
        public string senha
        {
            get { return Senha;}
            set { Senha = value;}
        }

        public int nivel
        {
            get { return Nivel;}
            set { Nivel = value;}
        }

        public string status
        {
            get { return Status;}
            set { Status = value;}
        }

        
    }
    
}
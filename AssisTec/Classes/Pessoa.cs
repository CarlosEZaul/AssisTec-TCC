using System.Data;
using System;
using System.Windows.Forms;
using Mysqlx;

namespace AssisTec
{
    public class Pessoa
    {
        private int Id;
        private string Nome;
        private string CPF;
        private string RG;
        private string Telefone;
        private string DataNascimento;
        private string CEP;
        private string Estado;
        private string Cidade;
        private string Bairro;
        private string Rua;
        private int Numero;
        private string Complemento;
        
        public string dataNascimentoFormatada
        {
            get
            {
                if (string.IsNullOrWhiteSpace(DataNascimento))
                {
                    MessageBox.Show("Data de nascimento vazia!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                try
                {
                    DateTime dataConvertida = DateTime.ParseExact(DataNascimento, "dd/MM/yyyy", null);

                    if (dataConvertida > DateTime.Today)
                    {
                        MessageBox.Show("Data de nascimento não pode ser no futuro!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }

                    return dataConvertida.ToString("yyyy-MM-dd");
                }
                catch
                {
                    MessageBox.Show("Data de nascimento inválida!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
            }
        }


        public int id
        {
            get{return Id;} set{Id = value;} 
            
        }

        public string nome
        {
            get { return Nome; }
            set { Nome = value; }
        }

        public string cpf
        {
            get {return CPF;} 
            set{CPF = value;}
        }

        public string rg
        {
            get { return RG; }
            set { RG = value; }
        }

        public string telefone
        {
            get {return Telefone;}
            set{Telefone = value;}
        }

        public string dataNascimento
        {
            get => DataNascimento;
            set => DataNascimento = value;
        }

        public string cep
        {
            get {return CEP;} 
            set{CEP = value;}
        }

        public string estado
        {
            get {return Estado;} 
            set{Estado = value;}
        }

        public string cidade
        {
            get {return Cidade;} 
            set{Cidade = value;}
        }

        public string bairro
        {
            get {return Bairro;} 
            set{Bairro = value;}
        }

        public string rua
        {
            get {return Rua;} 
            set{Rua = value;}
        }

        public int numero
        {
            get {return Numero;} 
            set{Numero = value;}
        }

        public string complemento
        {
            get {return Complemento;} 
            set{Complemento = value;}
        }
        
        
        
    }
    
    
}
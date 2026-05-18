using System.Data;
using System;
using System.Linq;
using System.Windows.Forms;
using Mysqlx;

namespace AssisTec
{
    public class Pessoa
    {
        private int Id;
        private string Nome;
        private string CPF;
       
        private string Telefone;
        private string DataNascimento;
        private string CEP;
        private string Estado;
        private string Cidade;
        private string Bairro;
        private string Rua;
        private int Numero;
        private string Complemento;
        
        public static bool ValidarTelefone(string telefone)
        {
            if (string.IsNullOrWhiteSpace(telefone))
                return false;

           
            telefone = new string(telefone.Where(char.IsDigit).ToArray());
            
            if (telefone.Length < 10 || telefone.Length > 11)
                return false;
            
            if (telefone.Distinct().Count() == 1)
                return false;
            
            int ddd = int.Parse(telefone.Substring(0, 2));
            if (ddd < 11 || ddd > 99)
                return false;
            
            if (telefone.Length == 11 && telefone[2] != '9')
                return false;

            return true;
        }
        
        public bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove qualquer caractere que não seja número (pontos, traços, etc.)
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // O CPF deve ter exatamente 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Evita CPFs com todos os números iguais (ex: 111.111.111-11)
            if (cpf.Distinct().Count() == 1)
                return false;

            // Validação do primeiro dígito verificador
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf = cpf.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf = tempCpf + digito;

            // Validação do segundo dígito verificador
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito = digito + resto.ToString();

            // Retorna true se os dígitos calculados forem iguais aos do CPF informado
            return cpf.EndsWith(digito);
        }
        
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
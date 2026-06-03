using System;
using System.Linq;

namespace AssisTec
{
    public static class Validacao
    {
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

        public static bool ValidarCPF(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf))
                return false;

            // Remove qualquer caractere que não seja número
            cpf = new string(cpf.Where(char.IsDigit).ToArray());

            // O CPF deve ter exatamente 11 dígitos
            if (cpf.Length != 11)
                return false;

            // Evita CPFs com todos os números iguais
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

        public static (bool sucesso, string mensagem) ValidarData(DateTime data)
        {
            if (data > DateTime.Today)
            {
                return (false, "A data não pode ser uma data futura");
            }

            int idadeMaxima = 120;
            if (data < DateTime.Today.AddYears(-idadeMaxima))
            {
                return (false, "A data limite é 120 anos");
            }

            if (data.Year < 1900)
            {
                return (false, "O ano deve ser maior que 1900");
            }
            return (true, string.Empty);
        }
    }
}
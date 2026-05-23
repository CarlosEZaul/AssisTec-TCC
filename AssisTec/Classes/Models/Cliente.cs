using System;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace AssisTec
{
    public class Cliente : Pessoa
    {
        private conexao con = new conexao();
        private MySqlCommand cmd;
        private string sql;
        
        public bool ValidarDados(out string erro)
        {
            if (!ValidarCPF(cpf))
            {
                erro = "CPF inválido";
                return false;
            }
        
            if (!ValidarTelefone(telefone))
            {
                erro = "Telefone inválido";
                return false;
            }
        
            erro = null;
            return true;
        }
        
        
    }
}
    
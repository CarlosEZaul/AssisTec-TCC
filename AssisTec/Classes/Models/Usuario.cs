using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using AssisTec.UserControls.SubUserControl_do_Gerenciador_de_Usuarios;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MySql.Data.MySqlClient;
using Font = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

namespace AssisTec
{
    public class Usuario:Pessoa
    {
        public string senha { get; set; }
        public int nivel { get; set;}
        public string status { get; set;}
        
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;
        
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
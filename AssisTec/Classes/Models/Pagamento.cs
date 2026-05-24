using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec
{
    public class Pagamento
    {
        conexao con = new conexao();
        string sql;
        private MySqlCommand cmd;
        
        public int id_pagamento{get;set;}
        public string forma_pagamento{get;set;}
        
        public string data_pagamento{get;set;}

        
        
        public string DataFormatada(string DataFornecida, bool obrigatoria = true)
        {
            if (string.IsNullOrWhiteSpace(DataFornecida))
            {
                if (obrigatoria)
                {
                    MessageBox.Show("Data vazia!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return null;
            }

            try
            {
                DateTime dataConvertida = DateTime.ParseExact(DataFornecida, "dd/MM/yyyy", null);
                return dataConvertida.ToString("yyyy-MM-dd");
            }
            catch
            {
                if (obrigatoria)
                {
                    MessageBox.Show("Data inválida!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return null;
            }
        }
    }
}
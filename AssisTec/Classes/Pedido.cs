using System;
using System.Windows.Forms;

namespace AssisTec
{
    public class Pedido
    {
        private int Id_pedido;
        private int Id_cliente;
        private int Id_equipamento;
        private int Id_tecnico;
        
        private string Problema_relatado;
        private string Diagnostico;
        private string Status;

        private string Data_abertura;
        private string Data_atualizacao;
        private string Data_fechamento;

        private decimal Valor_mao_obra;
        private decimal Valor_peças;
        private decimal Valor_total;

        private string Observacoes;
        
        public string dataFormatada(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
            {
                MessageBox.Show("Data vazia!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }

            try
            {
                DateTime dataConvertida = DateTime.ParseExact(data, "dd/MM/yyyy", null);
                return dataConvertida.ToString("yyyy-MM-dd");
            }
            catch
            {
                MessageBox.Show("Data inválida!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        

        public int id_pedido
        {
            get { return Id_pedido; }
            set { Id_pedido = value; }
        }

        public int id_cliente
        {
            get { return Id_cliente; }
            set { Id_cliente = value; }
        }

        public int id_equipamento
        {
            get { return Id_equipamento; }
            set { Id_equipamento = value; }
        }

        public int id_tecnico
        {
            get { return Id_tecnico; }
            set { Id_tecnico = value; }
        }
        
        public string problema_relatado
        {
            get { return Problema_relatado; }
            set { Problema_relatado = value; }
        }

        public string diagnostico
        {
            get { return Diagnostico; }
            set { Diagnostico = value; }
        }

        public string status
        {
            get { return Status; }
            set { Status = value; }
        }

        public string data_abertura
        {
            get { return dataFormatada(Data_abertura); }
            set { Data_abertura = value; }
        }

        public string data_atualizacao
        {
            get { return dataFormatada(Data_atualizacao); }
            set { Data_atualizacao = value; }
        }

        public string data_fechamento
        {
            get { return dataFormatada(Data_fechamento); }
            set { Data_fechamento = value; }
        }
        

        public decimal valor_mao_obra
        {
            get { return Valor_mao_obra; }
            set { Valor_mao_obra = value; }
        }

        public decimal valor_peças
        {
            get { return Valor_peças; }
            set { Valor_peças = value; }
        }

        public decimal valor_total
        {
            get { return Valor_total; }
            set { Valor_total = value; }
        }
        

        public string observacoes
        {
            get { return Observacoes; }
            set { Observacoes = value; }
        }
    }
}

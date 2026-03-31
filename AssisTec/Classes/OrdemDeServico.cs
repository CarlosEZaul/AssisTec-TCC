using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec
{
    public class OrdemDeServico
    {
        private int Id_pedido;
        private Cliente cliente;
        private Equipamento equipamento;
        private Usuario tecnico;
        
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


        private conexao con;
        private MySqlCommand cmd;
        private string sql;
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

        public Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        public Equipamento Equipamento
        {
            get { return Equipamento; }
            set { Equipamento = value; }
        }

        public Usuario Tecnico
        {
            get { return Tecnico; }
            set { Tecnico = value; }
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

        public void salvarOS(OrdemDeServico ordemDeServico)
        {
            try
            {
                con.OpenConnection();
                sql = "insert into equipamentos (descricao, marca, modelo, numero_serie, acessorios, estado_entrada, observacoes) values (@descricao, @marca, @modelo, @numero_serie, @acessorios, @estado_entrada, @observacoes)";
                cmd = new MySqlCommand(sql, con.con);
                
                cmd.Parameters.AddWithValue("@descricao", equipamento.Descricao);
                cmd.Parameters.AddWithValue("@marca", equipamento.Marca);
                cmd.Parameters.AddWithValue("@modelo", equipamento.Modelo);
                cmd.Parameters.AddWithValue("@numero_serie", equipamento.NumeroSerie);
                cmd.Parameters.AddWithValue("@estado_entrada", equipamento.EstadoEntrada);
                cmd.Parameters.AddWithValue("@acessorios", equipamento.Acessorio);
                cmd.Parameters.AddWithValue("@observacoes",equipamento.Observacoes);
                cmd.ExecuteNonQuery();

                int idEquipamento = (int)cmd.LastInsertedId;
                var dataAtual = DateTime.Now;
                con.CloseConnection();
                
                con.OpenConnection();
                sql = "insert into ordem_servico (id_tecnico, id_cliente , id_equipamento , problema_relatado, data_abertura) values (@id_tecnico,@id_cliente, @id_equipamento, @problema_relatado, @data_abertura)";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id_cliente", ordemDeServico.cliente.id);
                cmd.Parameters.AddWithValue("@id_tecnico", ordemDeServico.tecnico.id);
                cmd.Parameters.AddWithValue("@id_equipamento", idEquipamento);
                cmd.Parameters.AddWithValue("@problema_relatado", ordemDeServico.problema_relatado);
                cmd.Parameters.AddWithValue("@data_abertura", dataAtual);
                
                
                cmd.ExecuteNonQuery();
                con.CloseConnection();
                MessageBox.Show("Pedido Cadastrado!", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
    }
}

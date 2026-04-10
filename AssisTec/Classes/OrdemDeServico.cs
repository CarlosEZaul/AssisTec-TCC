using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec
{
    public class OrdemDeServico
    {
        // ================= ATRIBUTOS =================
        private int id_pedido;

        private Cliente cliente;
        private Equipamento equipamento;
        private Usuario tecnico;

        private string problemaRelatado;
        private string diagnostico;
        private string status;

        private DateTime dataAbertura;
        private DateTime dataAtualizacao;
        private DateTime dataFechamento;

        private decimal valorMaoObra;
        private decimal valorPecas;
        private decimal valorTotal;

        private string observacoes;

        private conexao con;

        // ================= CONSTRUTOR =================
        public OrdemDeServico()
        {
            cliente = new Cliente();
            equipamento = new Equipamento();
            tecnico = new Usuario();
            con = new conexao();

            dataAbertura = DateTime.Now;
        }

        // ================= PROPRIEDADES =================
        public int IdPedido
        {
            get { return id_pedido; }
            set { id_pedido = value; }
        }

        public Cliente Cliente
        {
            get { return cliente; }
            set { cliente = value; }
        }

        public Equipamento Equipamento
        {
            get { return equipamento; }
            set { equipamento = value; }
        }

        public Usuario Tecnico
        {
            get { return tecnico; }
            set { tecnico = value; }
        }

        public string ProblemaRelatado
        {
            get { return problemaRelatado; }
            set { problemaRelatado = value; }
        }

        public string Diagnostico
        {
            get { return diagnostico; }
            set { diagnostico = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }

        public DateTime DataAbertura
        {
            get { return dataAbertura; }
            set { dataAbertura = value; }
        }

        public DateTime DataAtualizacao
        {
            get { return dataAtualizacao; }
            set { dataAtualizacao = value; }
        }

        public DateTime DataFechamento
        {
            get { return dataFechamento; }
            set { dataFechamento = value; }
        }

        public decimal ValorMaoObra
        {
            get { return valorMaoObra; }
            set { valorMaoObra = value; }
        }

        public decimal ValorPecas
        {
            get { return valorPecas; }
            set { valorPecas = value; }
        }

        public decimal ValorTotal
        {
            get { return valorTotal; }
            set { valorTotal = value; }
        }

        public string Observacoes
        {
            get { return observacoes; }
            set { observacoes = value; }
        }

        // ================= MÉTODO SALVAR =================
        public void salvarOS()
        {
            try
            {
                con.OpenConnection();

                // Inserir equipamento
                string sqlEquip = @"INSERT INTO equipamentos 
                (descricao, marca, modelo, numero_serie, acessorios, estado_entrada, observacoes) 
                VALUES (@descricao, @marca, @modelo, @numero_serie, @acessorios, @estado_entrada, @observacoes)";

                MySqlCommand cmd = new MySqlCommand(sqlEquip, con.con);

                cmd.Parameters.AddWithValue("@descricao", equipamento.Descricao);
                cmd.Parameters.AddWithValue("@marca", equipamento.Marca);
                cmd.Parameters.AddWithValue("@modelo", equipamento.Modelo);
                cmd.Parameters.AddWithValue("@numero_serie", equipamento.NumeroSerie);
                cmd.Parameters.AddWithValue("@acessorios", equipamento.Acessorio);
                cmd.Parameters.AddWithValue("@estado_entrada", equipamento.EstadoEntrada);
                cmd.Parameters.AddWithValue("@observacoes", equipamento.Observacoes);

                cmd.ExecuteNonQuery();

                int idEquipamento = (int)cmd.LastInsertedId;

                con.CloseConnection();
                con.OpenConnection();

                // Inserir ordem de serviço
                string sqlOS = @"INSERT INTO ordem_servico 
                (id_tecnico, id_cliente, id_equipamento, problema_relatado, data_abertura) 
                VALUES (@id_tecnico, @id_cliente, @id_equipamento, @problema_relatado, @data_abertura)";

                cmd = new MySqlCommand(sqlOS, con.con);

                cmd.Parameters.AddWithValue("@id_cliente", cliente.id);
                cmd.Parameters.AddWithValue("@id_tecnico", tecnico.id);
                cmd.Parameters.AddWithValue("@id_equipamento", idEquipamento);
                cmd.Parameters.AddWithValue("@problema_relatado", problemaRelatado);
                cmd.Parameters.AddWithValue("@data_abertura", dataAbertura);

                cmd.ExecuteNonQuery();

                MessageBox.Show("Ordem de Serviço cadastrada com sucesso!", "Sucesso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erro",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                con.CloseConnection();
            }
        }
    }
}
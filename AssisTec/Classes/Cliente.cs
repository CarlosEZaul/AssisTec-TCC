using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec
{
    public class Cliente : Pessoa
    {
        private conexao con = new conexao();
        private MySqlCommand cmd;
        private string sql;

        public Cliente carregarDados(int id)
        {
            try
            {
                Cliente cliente = new Cliente();

                con.OpenConnection();

                sql = "SELECT * FROM clientes WHERE id_cliente = @id_cliente";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id_cliente", id);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    cliente.id = rs.GetInt32("id_cliente");
                    cliente.nome = rs.GetString("nome");
                    cliente.cpf = rs.GetString("cpf");
                    cliente.telefone = rs.GetString("telefone");
                    cliente.dataNascimento = rs.GetDateTime("datanasc").ToString("dd/MM/yyyy");
                    cliente.cep = rs.GetString("cep");
                    cliente.rua = rs.GetString("rua");
                    cliente.numero = Convert.ToInt32(rs.GetString("numero"));
                    cliente.cidade = rs.GetString("cidade");
                    cliente.estado = rs.GetString("estado");
                    cliente.bairro = rs.GetString("bairro");
                    cliente.complemento = rs.GetString("complemento");
                    
                }

                rs.Close();
                con.CloseConnection();
                return cliente;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar cliente!" + ex, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return new Cliente();
        }

        public void novoCliente(Cliente cliente)
        {

            if (cliente == null)
            {
                MessageBox.Show("Cadastro com um campo inválido!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

            try
            {

                con.OpenConnection();
                sql = "SELECT COUNT(*) FROM clientes WHERE cpf=@cpf";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@cpf", cliente.cpf);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                if (count > 0)
                {
                    MessageBox.Show("Cliente já cadastrado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.CloseConnection();
                }
                else
                {

                    con.OpenConnection();
                    sql =
                        "insert into clientes (nome, cpf, telefone, datanasc, cep, rua, numero, cidade, estado, bairro, complemento) values (@nome, @cpf, @telefone, @datanasc, @cep, @rua, @numero, @cidade, @estado, @bairro, @complemento)";
                    cmd = new MySqlCommand(sql, con.con);

                    cmd.Parameters.AddWithValue("@nome", cliente.nome);
                    cmd.Parameters.AddWithValue("@cpf", cliente.cpf);
                    cmd.Parameters.AddWithValue("@telefone", cliente.telefone);
                    cmd.Parameters.AddWithValue("@datanasc", cliente.dataNascimentoFormatada);
                    cmd.Parameters.AddWithValue("@cep", cliente.cep);
                    cmd.Parameters.AddWithValue("@rua", cliente.rua);
                    cmd.Parameters.AddWithValue("@numero", cliente.numero);
                    cmd.Parameters.AddWithValue("@cidade", cliente.cidade);
                    cmd.Parameters.AddWithValue("@estado", cliente.estado);
                    cmd.Parameters.AddWithValue("@bairro", cliente.bairro);
                    cmd.Parameters.AddWithValue("@complemento", cliente.complemento);

                    cmd.ExecuteNonQuery();
                    con.CloseConnection();


                    MessageBox.Show("Cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao cadastrar cliente\n" + ex.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }



        }

        public void editarCliente(Cliente cliente)
        {

            if (cliente == null)
            {
                MessageBox.Show("Tentativa de cadastro com campo inválido!", "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            try
            {
                con.OpenConnection();
                sql =
                    "update clientes set nome=@nome, cpf=@cpf, telefone=@telefone, datanasc=@datanasc, cep=@cep, rua=@rua, numero=@numero, cidade=@cidade, estado=@estado, bairro=@bairro, complemento=@complemento where id_cliente=@id_cliente";

                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id_cliente", cliente.id);
                cmd.Parameters.AddWithValue("@nome", cliente.nome);
                cmd.Parameters.AddWithValue("@cpf", cliente.cpf);
                cmd.Parameters.AddWithValue("@telefone", cliente.telefone);
                cmd.Parameters.AddWithValue("@datanasc", cliente.dataNascimentoFormatada);
                cmd.Parameters.AddWithValue("@cep", cliente.cep);
                cmd.Parameters.AddWithValue("@rua", cliente.rua);
                cmd.Parameters.AddWithValue("@numero", cliente.numero);
                cmd.Parameters.AddWithValue("@cidade", cliente.cidade);
                cmd.Parameters.AddWithValue("@estado", cliente.estado);
                cmd.Parameters.AddWithValue("@bairro", cliente.bairro);
                cmd.Parameters.AddWithValue("@complemento", cliente.complemento);

                cmd.ExecuteNonQuery();
                con.CloseConnection();



                MessageBox.Show("Cliente editado com sucesso!", "Sucesso", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception exception)
            {
                MessageBox.Show("Erro ao editar!\n" + exception.Message, "Erro", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public void deletarCLiente(int id)
        {
            DialogResult result = MessageBox.Show("Deseja excluir cliente?", "Confirmar Exclusão", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                
            if (result == DialogResult.Yes)
            {
                try
                {
                    con.OpenConnection();
                    sql = "DELETE FROM clientes WHERE id_cliente = @id";
                    cmd = new MySqlCommand(sql, con.con);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                    con.CloseConnection();
                    
                    
                    MessageBox.Show("Cliente excluído com sucesso!", "Sucesso", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    atualizarDados();
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Erro ao excluir cliente!\n" + exception.Message, "Erro", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        public DataTable atualizarDados()
        {
            DataTable dt =  new DataTable();
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM clientes ORDER BY NOME ASC";
                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                da.Fill(dt);
                con.CloseConnection();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return dt;
        }

        public void gerarRelatorioCliente(int id)
        {
            try
            {
                Cliente cliente = carregarDados(id);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            
        }
    }
}
    
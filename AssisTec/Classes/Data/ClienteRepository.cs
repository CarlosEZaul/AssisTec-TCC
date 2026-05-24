using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace AssisTec.Data
{
    public class ClienteRepository
    {
        conexao con = new conexao();
        MySqlCommand cmd;
        private string sql;
        public Cliente ObterPorId(int id)
        {
            try
            {
                con.OpenConnection();

                sql = "SELECT * FROM clientes WHERE id_cliente = @id_cliente";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id_cliente", id);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    return new Cliente()
                    {
                        id = rs.GetInt32("id_cliente"),
                        nome = rs.GetString("nome"),
                        cpf = rs.GetString("cpf"),
                        telefone = rs.GetString("telefone"),
                        dataNascimento = rs.GetDateTime("datanasc").ToString("dd/MM/yyyy"),
                        cep = rs.GetString("cep"),
                        rua = rs.GetString("rua"),
                        numero = Convert.ToInt32(rs.GetString("numero")),
                        cidade = rs.GetString("cidade"),
                        estado = rs.GetString("estado"),
                        bairro = rs.GetString("bairro"),
                        complemento = rs.GetString("complemento"),
                    };
                }
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar cliente!" + ex, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            finally
            {
                con.CloseConnection();
            }
        }
        
        public DataTable ObterTodosClientes()
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
        
        public bool CpfExiste(string cpf, int? ignorarId = null)
        {
            try
            {
                con.OpenConnection();
                
                string sql = "SELECT COUNT(*) FROM usuarios WHERE cpf = @cpf";
                if (ignorarId.HasValue)
                    sql += " AND id_usuario <> @id";

                using (MySqlCommand cmd = new MySqlCommand(sql, con.con))
                {
                    cmd.Parameters.AddWithValue("@cpf", cpf);
                    if (ignorarId.HasValue)
                        cmd.Parameters.AddWithValue("@id", ignorarId.Value);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            finally
            {
                con.CloseConnection();
            }
        }
        
        public DataTable buscarClientes(string nome)
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM clientes WHERE nome LIKE @nome ORDER BY NOME ASC";  
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", nome + "%");
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                con.CloseConnection();
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show("Falha ao buscar cliente", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }
        
        public bool novoCliente(Cliente cliente)
        {
            try
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
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                con.CloseConnection();
            }
    
        }
        
        public bool editarCliente(Cliente cliente)
        {
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

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                con.CloseConnection();
            }
            
        }
        
        public bool DeletarCliente(int id)
        {
            try
            {
                con.OpenConnection();
                sql = "DELETE FROM clientes WHERE id_cliente = @id";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();


                return true;
            }
            catch (Exception exception)
            {

                return false;
            }
            finally
            {
                con.CloseConnection();
            }
            
        }
        
        public int ContarOsEmAndamento(int idCliente)
        {
            try
            {
                con.OpenConnection();
                
                string sql = "SELECT COUNT(*) FROM ordem_servico WHERE id_cliente = @id AND status = 'EM ANDAMENTO'";
                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                
                cmd.Parameters.AddWithValue("@id", idCliente);
                return Convert.ToInt32(cmd.ExecuteScalar());
                
            }
            finally
            {
                con.CloseConnection();
            }
        }
    }
}
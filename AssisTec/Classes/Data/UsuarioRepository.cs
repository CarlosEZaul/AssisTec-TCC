using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace AssisTec.Data
{
    public class UsuarioRepository
    {
        private readonly conexao con = new conexao();

        public Usuario ObterPorId(int id)
        {
            try
            {
                con.OpenConnection();
                
                string sql = "SELECT * FROM usuarios WHERE id_usuario = @id";
                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", id);
                MySqlDataReader rs = cmd.ExecuteReader();
                if (rs.Read())
                {
                    return new Usuario
                    {
                        id = rs.GetInt32("id_usuario"),
                        nome = rs.GetString("nome"),
                        cpf = rs.GetString("cpf"),
                        telefone = rs.GetString("telefone"),
                        status = rs.GetString("status"),
                        nivel = rs.GetInt32("nivel"),
                        senha = rs.GetString("senha"),
                        cep = rs.GetString("cep"),
                        rua = rs.GetString("rua"),
                        numero = rs.GetInt32("numero"),
                        cidade = rs.GetString("cidade"),
                        estado = rs.GetString("estado"),
                        bairro = rs.GetString("bairro"), 
                        complemento = rs.GetString("complemento")
                    };
                }
                return null;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public bool CpfExiste(string cpf, int? ignorarId = null)
        {
            try
            {
                con.OpenConnection();
                
                string sql = "SELECT COUNT(*) FROM usuarios WHERE cpf = @cpf";
                if (ignorarId.HasValue)
                    sql += " AND id_usuario <> @id";

                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                    
                cmd.Parameters.AddWithValue("@cpf", cpf);
                if (ignorarId.HasValue)
                    cmd.Parameters.AddWithValue("@id", ignorarId.Value);

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
                
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public bool ExisteGerenteAtivo()
        {
            try
            {
                con.OpenConnection();
                
                string sql = "SELECT COUNT(*) FROM usuarios WHERE nivel = 1 AND status = 'Ativo'";
                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                
                int total = Convert.ToInt32(cmd.ExecuteScalar());
                if (total > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public bool Inserir(Usuario usuario)
        {
            try
            {
                con.OpenConnection();
                
                string sql = @"INSERT INTO usuarios 
                    (nome, cpf, telefone, senha, nivel, status, cep, rua, numero, cidade, bairro, estado, complemento) 
                    VALUES (@nome, @cpf, @telefone, @senha, @nivel, @status, @cep, @rua, @numero, @cidade, @bairro, @estado, @complemento)";
                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                
                cmd.Parameters.AddWithValue("@nome", usuario.nome);
                cmd.Parameters.AddWithValue("@cpf", usuario.cpf);
                cmd.Parameters.AddWithValue("@telefone", usuario.telefone);
                cmd.Parameters.AddWithValue("@senha", usuario.senha);
                cmd.Parameters.AddWithValue("@nivel", usuario.nivel);
                cmd.Parameters.AddWithValue("@status", usuario.status);
                cmd.Parameters.AddWithValue("@cep", usuario.cep);
                cmd.Parameters.AddWithValue("@rua", usuario.rua);
                cmd.Parameters.AddWithValue("@numero", usuario.numero);
                cmd.Parameters.AddWithValue("@cidade", usuario.cidade);
                cmd.Parameters.AddWithValue("@bairro", usuario.bairro);
                cmd.Parameters.AddWithValue("@estado", usuario.estado);
                cmd.Parameters.AddWithValue("@complemento", usuario.complemento);

                cmd.ExecuteNonQuery();
                return true;
                
            }
            catch
            {
                return false;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public bool EditarUsuario(Usuario usuario)
        {
            try
            {
                con.OpenConnection();
                
                bool alterarSenha = !string.IsNullOrWhiteSpace(usuario.senha) && usuario.senha != "********";

                string sql = @"UPDATE usuarios SET 
                    nome=@nome, cpf=@cpf, telefone=@tel, nivel=@nivel, status=@status,
                    cep=@cep, rua=@rua, numero=@numero, cidade=@cidade, 
                    bairro=@bairro, estado=@estado, complemento=@complemento";

                if (alterarSenha)
                    sql += ", senha=@senha";

                sql += " WHERE id_usuario=@id";

                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", usuario.nome);
                cmd.Parameters.AddWithValue("@cpf", usuario.cpf);
                cmd.Parameters.AddWithValue("@tel", usuario.telefone);
                cmd.Parameters.AddWithValue("@nivel", usuario.nivel);
                cmd.Parameters.AddWithValue("@status", usuario.status);
                cmd.Parameters.AddWithValue("@cep", usuario.cep);
                cmd.Parameters.AddWithValue("@rua", usuario.rua);
                cmd.Parameters.AddWithValue("@numero", usuario.numero);
                cmd.Parameters.AddWithValue("@cidade", usuario.cidade);
                cmd.Parameters.AddWithValue("@bairro", usuario.bairro);
                cmd.Parameters.AddWithValue("@estado", usuario.estado);
                cmd.Parameters.AddWithValue("@complemento", usuario.complemento);
                cmd.Parameters.AddWithValue("@id", usuario.id);

                if (alterarSenha)
                    cmd.Parameters.AddWithValue("@senha", usuario.senha);

                cmd.ExecuteNonQuery();
                return true;
                
            }
            catch
            {
                return false;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public bool Deletar(int id)
        {
            try
            {
                con.OpenConnection();
                
                string sql = "DELETE FROM usuarios WHERE id_usuario = @id";
                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
                return true;
                
            }
            catch
            {
                return false;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public int ContarOsEmAndamento(int idTecnico)
        {
            try
            {
                con.OpenConnection();
                
                string sql = "SELECT COUNT(*) FROM ordem_servico WHERE id_tecnico = @id AND status = 'EM ANDAMENTO'";
                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                
                cmd.Parameters.AddWithValue("@id", idTecnico);
                return Convert.ToInt32(cmd.ExecuteScalar());
                
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public DataTable ObterTodos()
        {
            try
            {
                con.OpenConnection();
                
                string sql = "SELECT * FROM usuarios ORDER BY nome ASC";
                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
                
            }
            finally
            {
                con.CloseConnection();
            }
        }
        
        public Usuario ObterPorCpf(string cpf)
        {
            try
            {
                con.OpenConnection();
            
                string cpfLimpo = cpf.Replace(".", "").Replace("-", "").Replace(",", "").Trim();
            
                string sql = @"SELECT * FROM usuarios 
                          WHERE REPLACE(REPLACE(REPLACE(cpf, '.', ''), '-', ''), ',', '') = @cpf 
                          AND status = 'Ativo'";

                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                
                    cmd.Parameters.AddWithValue("@cpf", cpfLimpo);

                    MySqlDataReader reader = cmd.ExecuteReader();
                    
                    if (reader.Read())
                    {
                        return new Usuario
                        {
                            id = reader.GetInt32("id_usuario"),
                            nome = reader.GetString("nome"),
                            cpf = reader.GetString("cpf"),
                            telefone = reader.GetString("telefone"),
                            status = reader.GetString("status"),
                            nivel = reader.GetInt32("nivel"),
                            senha = reader.GetString("senha"),
                            cep = reader.GetString("cep"),
                            rua = reader.GetString("rua"),
                            numero = reader.GetInt32("numero"),
                            cidade = reader.GetString("cidade"),
                            estado = reader.GetString("estado"),
                            bairro = reader.GetString("bairro"),
                            complemento = reader.GetString("complemento")
                        };
                    }
                    
                return null;
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public DataTable ObterComFiltros(string nome, bool apenasInativos, int nivel)
        {
            try
            {
                con.OpenConnection();

                string sql = "SELECT * FROM usuarios WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(nome))
                    sql += " AND nome LIKE @nome";

                if (apenasInativos)
                    sql += " AND status = 'Desativado'";

                if (nivel > 0)
                    sql += " AND nivel = @nivel";

                sql += " ORDER BY nome ASC";

                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                
                if (!string.IsNullOrWhiteSpace(nome))
                    cmd.Parameters.AddWithValue("@nome", nome.Trim() + "%");

                if (nivel > 0)
                    cmd.Parameters.AddWithValue("@nivel", nivel);

                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
                
            }
            finally
            {
                con.CloseConnection();
            }
        }

        public DataTable ObterHistoricoOs(int idTecnico)
        {
            try
            {
                con.OpenConnection();
                
                string sql = @"SELECT 
                    os.id_os, c.nome AS nome_cliente, u.nome AS nome_tecnico,
                    e.descricao AS descricao_equipamento, os.status, os.data_abertura,
                    os.data_fechamento, os.valor_mao_obra, os.valor_pecas, os.valor_total,
                    os.problema_relatado, os.diagnostico, os.observacoes
                FROM ordem_servico os
                LEFT JOIN clientes c ON c.id_cliente = os.id_cliente
                LEFT JOIN usuarios u ON u.id_usuario = os.id_tecnico
                LEFT JOIN equipamentos e ON e.id_equipamento = os.id_equipamento
                WHERE os.id_tecnico = @id";

                MySqlCommand cmd = new MySqlCommand(sql, con.con);
                
                cmd.Parameters.AddWithValue("@id", idTecnico);
                
                DataTable dt = new DataTable();
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                da.Fill(dt);
                return dt;
                
            }
            finally
            {
                con.CloseConnection();
            }
        }
    }
}
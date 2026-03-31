using System;
using System.Data;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
namespace AssisTec
{
    public class Usuario:Pessoa
    {
        private string senha;
        private int nivel;
        private string status;
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;

        public string Senha
        {
            get { return senha; }
            set { senha = value; }
        }

        public int Nivel
        {
            get { return nivel; }
            set { nivel = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        
        public Usuario carregarDados(int id)
        {
            try
            {
                Usuario usuario = new Usuario();

                con.OpenConnection();

                sql = "SELECT * FROM usuarios WHERE id_usuario  = @id_usuario";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@id_usuario", id);

                MySqlDataReader rs = cmd.ExecuteReader();

                if (rs.Read())
                {
                    usuario.id = rs.GetInt32("id_usuario");
                    usuario.nome = rs.GetString("nome");
                    usuario.cpf = rs.GetString("cpf");
                    usuario.telefone = rs.GetString("telefone");
                    usuario.status = rs.GetString("status");
                    usuario.nivel = Convert.ToInt32(rs["nivel"]);
                    usuario.senha = rs.GetString("senha");
                    usuario.cep = rs.GetString("cep");
                    usuario.rua = rs.GetString("rua");
                    usuario.numero = Convert.ToInt32(rs.GetString("numero"));
                    usuario.cidade = rs.GetString("cidade");
                    usuario.estado = rs.GetString("estado");
                    usuario.bairro = rs.GetString("bairro");
                    usuario.complemento = rs.GetString("complemento");
                    
                }

                rs.Close();
                con.CloseConnection();
                return usuario;


            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar usuário!" + ex, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return new Usuario();
        }
        

        private bool usuarioExiste(string cpf, int? ignorarId = null)
        {
            con.OpenConnection();
            sql = "SELECT COUNT(*) FROM usuarios WHERE cpf = @cpf";
            if (ignorarId.HasValue)
            {
                sql += " AND id_usuario <> @id"; 
            }

            using (MySqlCommand cmd = new MySqlCommand(sql, con.con))
            {
                cmd.Parameters.AddWithValue("@cpf", cpf);
                if (ignorarId.HasValue)
                {
                    cmd.Parameters.AddWithValue("@id", ignorarId.Value);
                }

                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.CloseConnection();
                return count > 0;
            }
        }
        
        public bool novoUsuario(Usuario user)
        {
          
            
            if (user == null)
            {
                MessageBox.Show("Tentativa de cadastro com campo inválido. Verifique os dados.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            try
            {
                if (usuarioExiste(user.cpf))
                {
                    MessageBox.Show("Usuário com este CPF já existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                string senhaHash = BCrypt.Net.BCrypt.HashPassword(user.Senha);
                
                con.OpenConnection();
                sql = "INSERT INTO usuarios (nome, cpf, telefone, senha, nivel, status, cep, rua, numero, cidade, bairro, estado, complemento) " +
                      "VALUES (@nome, @cpf, @telefone, @senha, @nivel, @status, @cep, @rua, @numero, @cidade, @bairro, @estado, @complemento)";
                cmd = new MySqlCommand(sql, con.con);
                cmd.Parameters.AddWithValue("@nome", user.nome);
                cmd.Parameters.AddWithValue("@cpf", user.cpf);
                cmd.Parameters.AddWithValue("@telefone", user.telefone);
                cmd.Parameters.AddWithValue("@senha", senhaHash);
                cmd.Parameters.AddWithValue("@nivel", user.nivel);
                cmd.Parameters.AddWithValue("@status", user.status);
                cmd.Parameters.AddWithValue("@cep", user.cep);
                cmd.Parameters.AddWithValue("@rua", user.rua);
                cmd.Parameters.AddWithValue("@numero", user.numero);
                cmd.Parameters.AddWithValue("@cidade", user.cidade);
                cmd.Parameters.AddWithValue("@bairro", user.bairro);
                cmd.Parameters.AddWithValue("@estado", user.estado);
                cmd.Parameters.AddWithValue("@complemento", user.complemento);

                cmd.ExecuteNonQuery();
                con.CloseConnection();

                MessageBox.Show("Usuário cadastrado com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                return true;
            }
            catch (Exception ex)
            {
                
                MessageBox.Show("Erro ao cadastrar usuário: " + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return true;
            }
        }
        
        public bool editarUsuario(Usuario user)
        {
            if (user == null)
            {
                MessageBox.Show("Dados inválidos", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            try
            {
                if (usuarioExiste(user.cpf, user.id))
                {
                    MessageBox.Show("Usuário já existe", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                con.OpenConnection();

                // NÃO altera senha se for *******
                bool alterarSenha = 
                    !string.IsNullOrWhiteSpace(user.senha) && 
                    user.senha != "********";

                string sql = @"UPDATE usuarios SET 
                                nome=@nome, 
                                cpf=@cpf, 
                                telefone=@tel, 
                                nivel=@nivel, 
                                status=@status,
                                cep=@cep, 
                                rua=@rua, 
                                numero=@numero, 
                                cidade=@cidade, 
                                bairro=@bairro, 
                                estado=@estado, 
                                complemento=@complemento";

                if (alterarSenha)
                {
                    sql += ", senha=@senha";
                }

                sql += " WHERE id_usuario=@id";

                using (MySqlCommand cmd = new MySqlCommand(sql, con.con))
                {
                    cmd.Parameters.AddWithValue("@nome", user.nome);
                    cmd.Parameters.AddWithValue("@cpf", user.cpf);
                    cmd.Parameters.AddWithValue("@tel", user.telefone);
                    cmd.Parameters.AddWithValue("@nivel", user.nivel);
                    cmd.Parameters.AddWithValue("@status", user.status);
                    cmd.Parameters.AddWithValue("@cep", user.cep);
                    cmd.Parameters.AddWithValue("@rua", user.rua);
                    cmd.Parameters.AddWithValue("@numero", user.numero);
                    cmd.Parameters.AddWithValue("@cidade", user.cidade);
                    cmd.Parameters.AddWithValue("@bairro", user.bairro);
                    cmd.Parameters.AddWithValue("@estado", user.estado);
                    cmd.Parameters.AddWithValue("@complemento", user.complemento);
                    cmd.Parameters.AddWithValue("@id", user.id);

                    // Só atualiza senha se for nova
                    if (alterarSenha)
                    {
                        string senhaHash = BCrypt.Net.BCrypt.HashPassword(user.senha);
                        cmd.Parameters.AddWithValue("@senha", senhaHash);
                    }

                    cmd.ExecuteNonQuery();
                }

                con.CloseConnection();

                MessageBox.Show("Usuário editado com sucesso", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                con.CloseConnection();
                MessageBox.Show(ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        
        //Atualiza os dados do datagridview
        public void atualizarDados(DataGridView dgvUsuarios)
        {
            try
            {
                con.OpenConnection();
                sql = "SELECT * FROM usuarios ORDER BY NOME ASC";
                cmd = new MySqlCommand(sql, con.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd;
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvUsuarios.DataSource = dt;
                con.CloseConnection();
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    
}
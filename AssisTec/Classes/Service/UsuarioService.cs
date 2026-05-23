using System;
using AssisTec.Data;

namespace AssisTec.Business
{
    public class UsuarioService
    {
        private readonly UsuarioRepository repository = new UsuarioRepository();

        public (bool sucesso, string mensagem) CadastrarUsuario(Usuario usuario)
        {
            if (!usuario.ValidarDados(out string erro))
                return (false, erro);
        
            // Verificar duplicidade
            if (repository.CpfExiste(usuario.cpf))
                return (false, "CPF já cadastrado");

            if (repository.CpfExiste(usuario.cpf))
                return (false, "Usuário com este CPF já existe");

            

            
            usuario.senha = BCrypt.Net.BCrypt.HashPassword(usuario.senha);
            
            bool sucesso = repository.Inserir(usuario);
            
            if (sucesso)
            {
                return (true, "Usuário cadastrado com sucesso!");
            }
            else
            {
                return (false, "Erro ao cadastrar usuário");
            }
        }

        public (bool sucesso, string mensagem) EditarUsuario(Usuario usuario)
        {
            if (usuario == null)
                return (false, "Dados inválidos");
            
            if (!usuario.ValidarDados(out string erro))
                return (false, erro);
            
            if (repository.CpfExiste(usuario.cpf, usuario.id))
                return (false, "CPF já cadastrado para outro usuário");

            // Se a senha for alterada
            if (!string.IsNullOrWhiteSpace(usuario.senha) && usuario.senha != "********")
                usuario.senha = BCrypt.Net.BCrypt.HashPassword(usuario.senha);

            bool sucesso = repository.EditarUsuario(usuario);
            return sucesso 
                ? (true, "Usuário editado com sucesso") 
                : (false, "Erro ao editar usuário");
        }

        public (bool podeExcluir, string mensagem) ValidarExclusao(int idUsuario)
        {
            int osEmAndamento = repository.ContarOsEmAndamento(idUsuario);
            
            if (osEmAndamento > 0)
                return (false, "Não é possível excluir usuário com Ordem de Serviço em andamento");

            return (true, null);
        }

        public bool DeletarUsuario(int id)
        {
            return repository.Deletar(id);
        }

        public bool VerificarPrimeiroAcesso()
        {
            return !repository.ExisteGerenteAtivo();
        }
        
        public (bool sucesso, string mensagem, Usuario usuario) RealizarLogin(string cpf, string senha)
        {
            if (string.IsNullOrWhiteSpace(cpf) || string.IsNullOrWhiteSpace(senha))
                return (false, "Preencha todos os campos", null);

            Usuario usuario = repository.ObterPorCpf(cpf);

            if (usuario == null)
                return (false, "CPF ou senha inválidos", null);

            bool senhaValida = BCrypt.Net.BCrypt.Verify(senha, usuario.senha);

            if (!senhaValida)
                return (false, "CPF ou senha inválidos", null);

            return (true, $"Bem-vindo, {usuario.nome}!", usuario);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AssisTec.Repository;
using AssisTec.Models;
using AssisTec.Reports;

namespace AssisTec.Service
{
    public class UsuarioService
    {
        private readonly IUsuarioReposity repository;
        private readonly UsuarioRelatorio relatorio;

        public UsuarioService(IUsuarioReposity _repository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            this.relatorio = new UsuarioRelatorio(repository);
        }

        public List<Usuario> ObterTodos()
        {
            return repository.ObterTodosUsuarios();
        }

        public List<Usuario> FiltrarUsuarios(string busca, bool apenasInativos, int nivel)
        {
            return repository.ObterComFiltros(busca, apenasInativos, nivel);
        }

        public Usuario ObterPorId(int id)
        {
            if (id <= 0) return null;
            return repository.ObterPorId(id);
        }

        public (bool sucesso, string mensagem, Usuario usuario) RealizarLogin(string cpf, string senha)
        {
            if (string.IsNullOrWhiteSpace(cpf) || string.IsNullOrWhiteSpace(senha))
            {
                return (false, "Por favor, preencha o CPF e a senha.", null);
            }

            string cpfLimpo = cpf.Replace(".", "").Replace("-", "").Trim();
            if (cpfLimpo.Length != 11)
            {
                return (false, "O CPF digitado é inválido. Certifique-se de digitar os 11 dígitos.", null);
            }

            try
            {
                Usuario usuario = repository.ObterPorCpf(cpfLimpo);

                if (usuario == null)
                {
                    return (false, "Usuário não encontrado no sistema.", null);
                }

                if (usuario.Status != "Ativo")
                {
                    return (false, "Este usuário está desativado. Entre em contato com o administrador.", null);
                }

                string senhaHashDigitada = GerarHashSHA256(senha);
                if (usuario.Senha != senhaHashDigitada)
                {
                    return (false, "Senha incorreta.", null);
                }

                return (true, $"Bem-vindo de volta, {usuario.Nome}!", usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno ao processar o login: " + ex.Message, ex);
            }
        }

        public (bool sucesso, string mensagem) ValidarAntesDeExcluir(int id, int idUsuarioLogado)
        {
            if (id <= 0)
            {
                return (false, "Selecione um usuário válido para realizar a exclusão.");
            }

            if (idUsuarioLogado == id)
            {
                return (true, "Deseja excluir a sua própria conta do sistema?");
            }

            return (false, string.Empty);
        }

        public (bool sucesso, string mensagem) ConfirmarExclusao(int id)
        {
            bool deletou = repository.ExcluirUsuario(id);
            if (deletou)
            {
                return (true, "Usuário deletado com sucesso.");
            }

            return (false, "Não foi possível concluir a exclusão do usuário selecionado.");
        }

        public (bool sucesso, string messagem) CadastrarUsuario(Usuario usuario)
        {
            if (usuario == null) 
                return (false, "Dados do usuário inválidos.");
            
            if (string.IsNullOrWhiteSpace(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Senha))
                return (false, "Campos obrigatórios não preenchidos.");

            if (!Validacao.ValidarCPF(usuario.Cpf))
                return (false, "Formato do CPF inválido!");
            
            if (!Validacao.ValidarTelefone(usuario.Telefone))
                return (false, "Formato do telefone inválido");

            if (repository.CpfExiste(usuario.Cpf))
            {
                return (false, "O CPF informado já está cadastrado no sistema.");
            }

            usuario.Senha = GerarHashSHA256(usuario.Senha);

            bool inserirUsuario = repository.InserirUsuario(usuario);
            if (inserirUsuario)
            {
                return (true, "Usuário cadastrado com sucesso!");
            }
            
            return (false, "Erro interno ao tentar salvar o usuário.");
        }

        public (bool sucesso, string mensagem) EditarUsuario(Usuario usuario)
        {
            if (usuario == null || usuario.Id <= 0) 
                return (false, "Dados do usuário inválidos para edição.");

            if (string.IsNullOrWhiteSpace(usuario.Nome))
                return (false, "O nome do usuário não pode ficar vazio.");

            try
            {
                Usuario usuarioBanco = repository.ObterPorId(usuario.Id);
                if (usuarioBanco == null)
                {
                    return (false, "Usuário não localizado no banco de dados para edição.");
                }

                if (string.IsNullOrWhiteSpace(usuario.Senha))
                {
                    usuario.Senha = usuarioBanco.Senha;
                }
                else 
                {
                    usuario.Senha = GerarHashSHA256(usuario.Senha);
                }

                bool atualizou = repository.AtualizarUsuario(usuario);
                if (atualizou)
                {
                    return (true, "Usuário atualizado com sucesso!");
                }
        
                return (false, "Erro interno ao tentar atualizar o usuário.");
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar a edição do usuário: " + ex.Message, ex);
            }
        }

        public async Task<(bool sucesso, string cidade, string rua, string bairro, string estado)> ConsultarCepAsync(string cep)
        {
            try
            {
                BuscaCEP buscador = new BuscaCEP();
                buscador.Cep = cep;

                var ds = await Task.Run(() => buscador.Consultar());

                if (ds != null && !string.IsNullOrWhiteSpace(buscador.Cidade))
                {
                    return (true, buscador.Cidade, buscador.Rua, buscador.Bairro, buscador.Estado);
                }

                return (false, null, null, null, null);
            }
            catch
            {
                return (false, null, null, null, null);
            }
        }

        public void ExecutarRelatorioGeral(string busca, bool apenasInativos, int nivel)
        {
            relatorio.GerarRelatorioGeral(busca, apenasInativos, nivel);
        }

        public (bool sucesso, string mensagem) ExecutarRelatorioTecnico(int id)
        {
            if (id <= 0)
            {
                return (false, "Selecione um técnico válido para gerar o relatório.");
            }

            var resultado = relatorio.GerarRelatorioTecnico(id);
            return (resultado.Item1, resultado.Item2);
        }

        private string GerarHashSHA256(string senhaTextoClaro)
        {
            if (string.IsNullOrEmpty(senhaTextoClaro)) return string.Empty;

            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytesOriginal = Encoding.UTF8.GetBytes(senhaTextoClaro);
                byte[] bytesHash = sha256Hash.ComputeHash(bytesOriginal);

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytesHash.Length; i++)
                {
                    builder.Append(bytesHash[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
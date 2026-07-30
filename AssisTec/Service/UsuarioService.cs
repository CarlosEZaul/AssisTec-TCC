using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AssisTec.Dtos;
using AssisTec.Repository;
using AssisTec.Models;
using AssisTec.Utils;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace AssisTec.Service
{
    public class UsuarioService
    {
        private readonly IUsuarioReposity repository;
        private readonly IOrdemServicoRepository ordemServicoRepository;

        public UsuarioService(IUsuarioReposity _repository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        public UsuarioService(IUsuarioReposity _repository, IOrdemServicoRepository _ordemServicoRepository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            this.ordemServicoRepository = _ordemServicoRepository ?? throw new ArgumentNullException(nameof(_ordemServicoRepository));
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

        public bool AlterarStatus(int id)
        {
            return repository.AlterarStatus(id);
        }

        public bool ExisteEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            return repository.EmailExiste(email);
        }

        public (bool sucesso, string mensagem) AlterarSenha(string email, string novaSenha)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(novaSenha))
                {
                    return (false, "A nova senha não pode estar em branco.");
                }

                var usuario = repository.ObterPorEmail(email);

                if (usuario == null)
                {
                    return (false, "Usuário não encontrado.");
                }

                usuario.Senha = GerarHashSHA256(novaSenha);

                bool alterado = repository.AlterarSenha(usuario);

                if (alterado)
                {
                    return (true, "Senha alterada com sucesso!");
                }

                return (false, "Não foi possível atualizar a senha no banco de dados.");
            }
            catch (Exception ex)
            {
                
                return (false, "Ocorreu um erro interno ao tentar alterar a senha. "+ex.Message);
            }
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
                    return (false, "CPF ou senha inválidos.", null);
                }

                if (!usuario.Status.Equals("Ativado", StringComparison.OrdinalIgnoreCase))
                {
                    return (false, "Este usuário está desativado. Entre em contato com o administrador.", null);
                }

                string senhaHashDigitada = GerarHashSHA256(senha);
                if (usuario.Senha != senhaHashDigitada)
                {
                    return (false, "CPF ou senha inválidos.", null);
                }

                return (true, $"Bem-vindo de volta, {usuario.Nome}!", usuario);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro interno ao processar o login: " + ex.Message, ex);
            }
        }

        public (bool sucesso, string mensagem) ValidarAntesDeDesativar(int id, int idUsuarioLogado)
        {
            if (id <= 0)
            {
                return (false, "Selecione um usuário válido para realizar a exclusão.");
            }

            if (idUsuarioLogado == id)
            {
                return (false, "Você não pode desativar a sua própria conta logada no sistema.");
            }

            bool possuiOsAberta = ordemServicoRepository.ExisteOSAbertaPorTecnico(id);
            if (possuiOsAberta)
            {
                return (false, "Não é possível desativar este usuário pois ele possui Ordens de Serviço em ABERTA.");
            }

            return (true, string.Empty);
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

            if (!Validacao.ValidarEmail(usuario.Email))
                return (false, "Formato do Email inválido");

            if (repository.CpfExiste(usuario.Cpf))
            {
                return (false, "O CPF informado já está cadastrado no sistema.");
            }

            if (repository.EmailExiste(usuario.Email))
            {
                return (false, "E-mail já cadastrado");
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

        public void GerarRelatorioUsuariosPdf(string nome, bool apenasInativos, int nivel, string caminhoDestino)
        {
            try
            {
                List<Usuario> usuariosFiltrados = repository.ObterComFiltros(nome, apenasInativos, nivel);

                UsuarioDTO.UsuariosRelatorioDTO relatorio = new UsuarioDTO.UsuariosRelatorioDTO
                {
                    FiltroNome = string.IsNullOrEmpty(nome) ? "Todos" : nome,
                    FiltroNivel = nivel == 0 ? "Todos" : ObterDescricaoNivel(nivel),
                    FiltroStatus = apenasInativos ? "Apenas Desativado" : "Todos (Ativado/Desativado)",
                    TotalAtivos = 0,
                    TotalInativos = 0,
                    Itens = new List<UsuarioDTO.UsuarioRelatorioDTO>()
                };

                if (usuariosFiltrados != null)
                {
                    foreach (var usuario in usuariosFiltrados)
                    {
                        string statusAtual = usuario.Status ?? string.Empty;
                        bool inativo = statusAtual.Equals("Desativado", StringComparison.OrdinalIgnoreCase) || 
                                       statusAtual.Equals("Inativo", StringComparison.OrdinalIgnoreCase);

                        if (inativo)
                        {
                            relatorio.TotalInativos++;
                        }
                        else
                        {
                            relatorio.TotalAtivos++;
                        }

                        relatorio.Itens.Add(new UsuarioDTO.UsuarioRelatorioDTO
                        {
                            Id = usuario.Id,
                            Nome = usuario.Nome ?? string.Empty,
                            Cpf = usuario.Cpf ?? string.Empty,
                            Telefone = usuario.Telefone ?? string.Empty,
                            Nivel = usuario.Nivel,
                            Status = statusAtual,
                            Cidade = usuario.Cidade ?? string.Empty,
                            Estado = usuario.Estado ?? string.Empty
                        });
                    }
                }

                GeradorPdfUsuario.GerarRelatorioGeral(relatorio, caminhoDestino);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao gerar o relatório de usuários em PDF: ", ex);
            }
        }

        public DataTable obterHistoricoOs(int id)
        {
            return ordemServicoRepository.ObterHistoricoUsuario(id);
        }

        public List<Usuario> obterTodosTecnicos()
        {
            try
            {
                return repository.ObterTodosTecnicosAtivados();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        public DataTable ObterHistoricoOs(int id)
        {
            return ordemServicoRepository.ObterHistoricoUsuario(id);
        }

        public List<Usuario> ObterTodosTecnicos()
        {
            try
            {
                return repository.ObterTodosTecnicosAtivados();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public void GerarRelatorioIndividualPdf(int idUsuario, string caminhoDestino)
        {
            try
            {
                Usuario usuario = repository.ObterPorId(idUsuario);
                if (usuario == null)
                {
                    throw new Exception("Usuário não encontrado para a geração do relatório.");
                }

                DataTable tabelaOS = ordemServicoRepository.ObterHistoricoUsuario(idUsuario);

                UsuarioDTO.UsuarioComOrdemServicoDTO relatorio = new UsuarioDTO.UsuarioComOrdemServicoDTO
                {
                    IdUsuario = usuario.Id,
                    Nome = usuario.Nome ?? string.Empty,
                    Cpf = usuario.Cpf ?? string.Empty,
                    Telefone = usuario.Telefone ?? string.Empty,
                    NivelDescricao = ObterDescricaoNivel(usuario.Nivel),
                    StatusUsuario = usuario.Status ?? string.Empty,
                    TotalOrdens = 0,
                    OrdensAbertas = 0,
                    OrdensFinalizadas = 0,
                    FaturamentoGerado = 0,
                    Ordens = new List<UsuarioDTO.OrdemServicoItemDTO>()
                };

                if (tabelaOS != null && tabelaOS.Rows.Count > 0)
                {
                    foreach (DataRow row in tabelaOS.Rows)
                    {
                        string statusOS = row["STATUS"] != DBNull.Value ? row["STATUS"].ToString() : "ABERTA";
                        decimal valor = row["VALOR_TOTAL"] != DBNull.Value ? Convert.ToDecimal(row["VALOR_TOTAL"]) : 0m;

                        DateTime? dataFim = null;
                        if (row["DATA_FECHAMENTO"] != DBNull.Value)
                        {
                            dataFim = Convert.ToDateTime(row["DATA_FECHAMENTO"]);
                        }

                        relatorio.TotalOrdens++;

                        if (statusOS.Equals("ABERTA", StringComparison.OrdinalIgnoreCase) || 
                            statusOS.Equals("Aberto", StringComparison.OrdinalIgnoreCase) || 
                            statusOS.Equals("Em Andamento", StringComparison.OrdinalIgnoreCase))
                        {
                            relatorio.OrdensAbertas++;
                        }
                        else if (statusOS.Equals("Finalizado", StringComparison.OrdinalIgnoreCase) || 
                                 statusOS.Equals("FINALIZADA", StringComparison.OrdinalIgnoreCase) || 
                                 statusOS.Equals("Entregue", StringComparison.OrdinalIgnoreCase))
                        {
                            relatorio.OrdensFinalizadas++;
                            relatorio.FaturamentoGerado += valor;
                        }

                        relatorio.Ordens.Add(new UsuarioDTO.OrdemServicoItemDTO
                        {
                            IdOrdemServico = row["ID_ORDEM"] != DBNull.Value ? Convert.ToInt32(row["ID_ORDEM"]) : 0,
                            Cliente = row["CLIENTE"] != DBNull.Value ? row["CLIENTE"].ToString() : "Sem Cliente",
                            Equipamento = row["EQUIPAMENTO"] != DBNull.Value ? row["EQUIPAMENTO"].ToString() : "Sem Equipamento",
                            DataAbertura = row["DATA_ABERTURA"] != DBNull.Value ? Convert.ToDateTime(row["DATA_ABERTURA"]) : DateTime.Now,
                            DataFechamento = dataFim,
                            ValorTotal = valor,
                            Status = statusOS
                        });
                    }
                }

                GeradorPdfUsuario.GerarRelatorioIndividual(relatorio, caminhoDestino);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro na camada de serviço ao gerar relatório individual do usuário: " + ex.Message);
            }
        }

        private string ObterDescricaoNivel(int nivel)
        {
            switch (nivel)
            {
                case 1: return "Gerente";
                case 2: return "Técnico";
                case 3: return "Atendente";
                default: return $"Nível {nivel}";
            }
        }
    }
}
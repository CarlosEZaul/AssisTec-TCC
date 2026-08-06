using System;
using System.Collections.Generic;
using System.Linq;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;

namespace AssisTec.Repository
{
    public class UsuarioRepository:IUsuarioReposity
    {
        private readonly AppDbContext context;

        public UsuarioRepository(AppDbContext _context)
        {
            this.context = _context;
        }

        public bool InserirUsuario(Usuario usuario)
        {
            try
            {
                context.Usuarios.Add(usuario);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha interna no repositório ao inserir usuário: " + ex.Message, ex);
            }
        }

        public List<Usuario> ObterTodosUsuarios()
        {
            try
            {
                return context.Usuarios.Where(u => u.Status=="Ativado").ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter todos os usuários.", ex);
            }
        }

        public List<Usuario> ObterTodosTecnicosAtivados()
        {
            try
            {
                return context.Usuarios
                    .Where(u => u.Status == "Ativado" && (u.Nivel == 1 || u.Nivel == 3))
                    .ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao consultar técnicos no BD.");
            }
        }

        public bool EhGerente(int id)
        {
            return context.Usuarios
                .AsNoTracking()
                .Any(u => u.Id == id && u.Nivel == 1);
        }

        public int ObterQuantidadeGerentesAtivos()
        {
            return context.Usuarios
                .AsNoTracking()
                .Count(u => u.Nivel == 1 && u.Status == "Ativado");
        }

        public Usuario ObterPorId(int id)
        {
            try
            {
                return context.Usuarios.AsNoTracking().FirstOrDefault(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter usuário por ID: " + id + ".", ex);
            }
        }

        public Usuario ObterPorCpf(string cpf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cpf)) return null;

                string cpfDigitadoLimpo = cpf.Replace(".", "").Replace("-", "").Replace(",", "").Trim();

                if (cpfDigitadoLimpo.Length != 11) return null;

                return context.Usuarios
                    .AsNoTracking()
                    .ToList()
                    .FirstOrDefault(u =>
                        u.Cpf.Replace(".", "").Replace("-", "").Replace(",", "").Trim() == cpfDigitadoLimpo &&
                        u.Status.Trim().ToLower() == "ativado"
                    );
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter usuário por CPF.", ex);
            }
        }

        public Usuario ObterPorEmail(string email)
        {
            return context.Usuarios.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        public bool AtualizarUsuario(Usuario usuario)
        {
            try
            {
                var local = context.Usuarios.Local.FirstOrDefault(u => u.Id == usuario.Id);

                if (local != null)
                {
                    context.Entry(local).State = EntityState.Detached;
                }

                context.Entry(usuario).State = EntityState.Modified;

                return context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExcluirUsuario(int id)
        {
            try
            {
                var usuario = context.Usuarios.FirstOrDefault(u => u.Id == id);
                if (usuario != null)
                {
                    context.Usuarios.Remove(usuario);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao excluir usuário com ID: " + id + ".", ex);
            }
        }

        public bool AlterarStatus(int id)
        {
            try
            {
                var usuario = context.Usuarios.FirstOrDefault(u => u.Id == id);
                if (usuario == null)
                {
                    return false;
                }

                if (usuario.Status == "Ativado")
                {
                    usuario.Status = "Desativado";
                }
                else
                {
                    usuario.Status = "Ativado";
                }

                context.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CpfExiste(string cpf)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cpf)) return false;
               
                string cpfDigitadoLimpo = cpf.Replace(".", "").Replace("-", "").Replace(",", "").Trim();

                if (cpfDigitadoLimpo.Length != 11) return false;

                return context.Usuarios
                    .AsNoTracking()
                    .ToList()
                    .Any(u => u.Cpf.Replace(".", "").Replace("-", "").Replace(",", "").Trim() == cpfDigitadoLimpo);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao verificar existência do CPF no banco de dados.", ex);
            }
        }

        public bool EmailExiste(string email)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email)) return false;

                string emailTratado = email.Trim().ToLower();

                return context.Usuarios
                    .AsNoTracking()
                    .Any(u => u.Email.ToLower() == emailTratado);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao verificar existência do E-mail no banco de dados.", ex);
            }
        }

        public bool ExisteGerenteAtivo()
        {
            try
            {
                return context.Usuarios.Any(u => u.Nivel == 1 && u.Status.Trim().ToLower() == "ativado");
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao verificar existência de gerente ativo no MySQL.", ex);
            }
        }

        public bool AlterarSenha(Usuario usuario)
        {
            try
            {
                var user = context.Usuarios.Find(usuario.Id);

                if (user == null)
                {
                    return false;
                }

                user.Senha = usuario.Senha;
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao alterar senha no repositório: {ex.Message}");
                return false;
            }
        }

        public List<Usuario> ObterComFiltros(string nome, bool exibirDesativados, int nivel)
        {
            try
            {
                IQueryable<Usuario> query = context.Usuarios;

                if (!string.IsNullOrEmpty(nome))
                {
                    query = query.Where(u => u.Nome.StartsWith(nome));
                }

                if (!exibirDesativados)
                {
                    query = query.Where(u => u.Status == "Ativado");
                }

                if (nivel > 0)
                {
                    query = query.Where(u => u.Nivel == nivel);
                }

                return query.OrderBy(u => u.Nome).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter usuários com filtros.", ex);
            }
        }
    }
}
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
                return context.Usuarios.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter todos os usuários.", ex);
            }
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
                        u.Status.Trim().ToLower() == "ativo"
                    );
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter usuário por CPF.", ex);
            }
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

        public bool ExisteGerenteAtivo()
        {
            try
            {
                return context.Usuarios.Any(u => u.Nivel == 1 && u.Status.Trim().ToLower() == "ativo");
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao verificar existência de gerente ativo no MySQL.", ex);
            }
        }

        public List<Usuario> ObterComFiltros(string nome, bool apenasInativos, int nivel)
        {
            try
            {
                IQueryable<Usuario> query = context.Usuarios;

                if (!string.IsNullOrEmpty(nome))
                {
                    query = query.Where(u => u.Nome.StartsWith(nome));
                }
                if (apenasInativos)
                {
                    query = query.Where(u => u.Status == "Inativo");
                }
                else
                {
                    query = query.Where(u => u.Status == "Ativo");
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
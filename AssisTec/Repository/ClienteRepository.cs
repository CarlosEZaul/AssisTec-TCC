using System;
using System.Collections.Generic;
using System.Linq;
using AssisTec.Models;
using AssisTec.Repository;
using Microsoft.EntityFrameworkCore;

namespace AssisTec.Repository
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext context;

        public ClienteRepository(AppDbContext _context)
        {
            context = _context;
        }

        public bool InserirCliente(Cliente cliente)
        {
            try
            {
                context.Clientes.Add(cliente);
                return context.SaveChanges() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Erro ao persistir cliente no banco: " + (dbEx.InnerException?.Message ?? dbEx.Message), dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha inesperada ao inserir cliente.", ex);
            }
        }

        public List<Cliente> ObterTodosClientes()
        {
            try
            {
                return context.Clientes.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter todos os clientes.", ex);
            }
        }

        public Cliente ObterPorId(int id)
        {
            try
            {
                return context.Clientes.AsNoTracking().FirstOrDefault(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao obter cliente por ID: {id}.", ex);
            }
        }

        public bool CorrigirAtualizarCliente(Cliente cliente)
        {
            try
            {
                var local = context.Clientes.Local.FirstOrDefault(c => c.Id == cliente.Id);
                if (local != null)
                {
                    context.Entry(local).State = EntityState.Detached;
                }

                context.Entry(cliente).State = EntityState.Modified;
                return context.SaveChanges() > 0;
            }
            catch (DbUpdateException dbEx)
            {
                throw new Exception("Erro ao atualizar cliente no banco: " + (dbEx.InnerException?.Message ?? dbEx.Message), dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao atualizar cliente. " + ex.Message, ex);
            }
        }

        public bool ExcluirCliente(int id)
        {
            try
            {
                var cliente = context.Clientes.FirstOrDefault(c => c.Id == id);
                if (cliente != null)
                {
                    context.Clientes.Remove(cliente);
                    return context.SaveChanges() > 0;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao excluir cliente com ID: {id}.", ex);
            }
        }

        public Cliente ObterPorCpf(string cpf)
        {
            try
            {
                return context.Clientes.AsNoTracking().FirstOrDefault(c => c.Cpf == cpf);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter cliente por CPF.", ex);
            }
        }

        public bool CpfExiste(string cpf)
        {
            try
            {
                return context.Clientes.Any(c => c.Cpf == cpf);
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao verificar existência do CPF.", ex);
            }
        }

        public List<Cliente> ObterComFiltros(string busca)
        {
            try
            {
                var query = context.Clientes.AsNoTracking().AsQueryable();

                if (!string.IsNullOrWhiteSpace(busca))
                {
                    string buscaLimpa = busca.Replace(".", "").Replace("-", "").Trim();
                    query = query.Where(c => c.Nome.Contains(busca) || c.Cpf.Contains(buscaLimpa));
                }

                return query.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao filtrar clientes.", ex);
            }
        }
    }
}
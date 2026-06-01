using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public class PagamentoRepository : IPagamentoRepository
    {
        private readonly AppDbContext context;

        public PagamentoRepository(AppDbContext _context)
        {
            this.context = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        public DataTable carregarFormasPamento()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id_forma_pagamento", typeof(int));
            dt.Columns.Add("exibicao", typeof(string));
            try
            {
                var formas = context.Pagamentos
                                    .OrderBy(f => f.Descricao)
                                    .Select(f => new
                                    {
                                        f.Idforma_pagamento,
                                        f.Descricao
                                    })
                                    .ToList();
                foreach (var forma in formas)
                {
                    dt.Rows.Add(forma.Idforma_pagamento, forma.Descricao.ToUpper());
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Falha crítica ao carregar formas de pagamento para a interface.", ex);
            }
            return dt;
        }

        public List<Pagamento> ObterTodas()
        {
            try
            {
                return context.Pagamentos.OrderBy(f => f.Descricao).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter formas de pagamento.", ex);
            }
        }

        public Pagamento ObterPorId(int id)
        {
            try
            {
                return context.Pagamentos.FirstOrDefault(f => f.Idforma_pagamento == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao obter forma de pagamento ID: {id}.", ex);
            }
        }

        public bool Inserir(Pagamento pagamento)
        {
            try
            {
                context.Pagamentos.Add(pagamento);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao inserir forma de pagamento.", ex);
            }
        }

        public bool Atualizar(Pagamento pagamento)
        {
            try
            {
                context.Pagamentos.Update(pagamento);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao atualizar forma de pagamento.", ex);
            }
        }

        public bool Excluir(int id)
        {
            try
            {
                var pagamento = context.Pagamentos.FirstOrDefault(f => f.Idforma_pagamento == id);
                if (pagamento != null)
                {
                    context.Pagamentos.Remove(pagamento);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao excluir forma de pagamento ID: {id}.", ex);
            }
        }
    }
}
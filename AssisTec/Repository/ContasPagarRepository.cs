using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using AssisTec.Dtos;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;

namespace AssisTec.Repository
{
    public class ContasPagarRepository : IContasPagarRepository
    {
        private readonly AppDbContext _context;
        public ContasPagarRepository(AppDbContext context)
        {
            _context = context ?? throw  new System.ArgumentNullException(nameof(context));
        }

        public bool Inserir(ContasPagar conta)
        {
            _context.Add(conta);
            return _context.SaveChanges() > 0;
        }

        public IEnumerable<ContasPagarDto> ObterTodos()
        {
            return _context.Contas_Pagar
                .Include(c => c.Pagamento)
                .AsEnumerable() 
                .Select(c => new ContasPagarDto
                {
                    IdContaReceber = c.id_conta_pagar,
                    Descricao = c.descricao,
                    Valor = c.valor,
                    DataEmissao = c.data_emissao,
                    DataVencimento = c.data_vencimento,
                    DataPagamento = c.data_pagamento,
                    Status = c.status,
                    Observacoes = c.observacoes,
                    FormaPagamentoDescricao = c.Pagamento != null ? c.Pagamento.Descricao : "---"
                })
                .ToList();
        }

        public ContasPagar ObterPorId(int id)
        {
            return _context.Contas_Pagar.Find(id);
        }

        public bool Atualizar(ContasPagar conta)
        {
            _context.Update(conta);
            return _context.SaveChanges() > 0;
        }

        public bool Excluir(int idConta)
        {
            var conta = _context.Contas_Pagar.Find(idConta);
            if (conta == null) return false;
            _context.Remove(conta);
            return _context.SaveChanges() > 0;
        }

        public bool MarcarComoAtrasado(int id)
        {
            var conta = _context.Contas_Pagar.Find(id);
            if (conta == null) return false;
            if(conta.status == "PENDENTE") return true;
            conta.status = "ATRASADO";
            return _context.SaveChanges() > 0;
        }

        public DataTable Filtrar(ContasPagar filtro)
        {
            var resultado = AplicarFiltros(filtro).Include(c => c.Pagamento).ToList();
            DataTable dt = new DataTable();
            dt.Columns.Add("ID_CONTA", typeof(int));
            dt.Columns.Add("Descricao", typeof(string));
            dt.Columns.Add("Valor", typeof(decimal));
            dt.Columns.Add("DataEmissao", typeof(DateTime));
            dt.Columns.Add("DataPagamento", typeof(object));
            dt.Columns.Add("DataVencimento", typeof(DateTime));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Observacoes", typeof(string));
            dt.Columns.Add("FormaPagamento", typeof(string));

            foreach (var item in resultado)
            {
                dt.Rows.Add(
                    item.id_conta_pagar,
                    item.descricao,
                    item.valor,
                    item.data_emissao,
                    (object)item.data_pagamento ?? DBNull.Value,
                    item.status,
                    item.data_vencimento,
                    item.Pagamento?.Descricao ?? "NÃO DEFINIDA"
                );
            }
            return dt;
        }

        public (decimal TotalGeral, decimal TotalPagar, decimal TotalPendente, decimal TotalAtrasado) ObterTotais(ContasPagar filtro)
        {
            var dados = AplicarFiltros(filtro).Select(c=> new{c.status, c.valor}).ToList();
            return (
                dados.Sum(c => c.valor),
                dados.Where(c => c.status == "PAGA").Sum(c => c.valor),
                dados.Where(c => c.status == "PENDENTE").Sum(c => c.valor),
                dados.Where(c => c.status == "ATRASADO").Sum(c => c.valor)
            );
        }

        public IQueryable<ContasPagar> AplicarFiltros(ContasPagar filtro)
        {
            var query = _context.Contas_Pagar.AsQueryable();
            
            if (!string.IsNullOrWhiteSpace(filtro.filtroDescricao))
                query = query.Where(c => c.descricao.Contains(filtro.filtroDescricao));

            if (!string.IsNullOrWhiteSpace(filtro.filtroStatus))
                query = query.Where(c => c.status == filtro.filtroStatus);

            if (filtro.filtroIdFormaPagamento.HasValue && filtro.filtroIdFormaPagamento.Value > 0)
                query = query.Where(c => c.id_forma_pagamento_fk == filtro.filtroIdFormaPagamento.Value);

            if (DateTime.TryParseExact(filtro.filtroDataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtInicio))
                query = query.Where(c => c.data_vencimento >= dtInicio.Date);

            if (DateTime.TryParseExact(filtro.filtroDataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtFim))
                query = query.Where(c => c.data_vencimento < dtFim.Date.AddDays(1));

            return query;
        }

        

        
    }
}
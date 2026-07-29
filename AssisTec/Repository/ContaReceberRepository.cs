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
    public class ContasReceberRepository : IContaReceberRepository
    {
        private readonly AppDbContext _context;

        public ContasReceberRepository(AppDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool Inserir(ContasReceber conta)
        {
            _context.ContasReceber.Add(conta);
            return _context.SaveChanges() > 0;
        }

        

        public IEnumerable<ContasReceberDto> ObterTodos()
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
            var fimMes = inicioMes.AddMonths(1);

            return _context.ContasReceber
                .Where(c => (c.data_vencimento >= inicioMes && c.data_vencimento < fimMes) || c.status == "ATRASADO")
                .Select(c => new ContasReceberDto
                {
                    IdContaReceber = c.id_conta_receber,
                    Descricao = c.descricao,
                    Valor = c.valor,
                    DataEmissao = c.data_emissao,
                    DataVencimento = c.data_vencimento,
                    DataPagamento = c.data_pagamento,
                    Status = c.status,
                    Observacoes = c.observacoes,
                    IdOrdemServico = c.id_os_fk,
                    FormaPagamentoDescricao = c.Pagamento != null ? c.Pagamento.Descricao : "---"
                })
                .ToList();
        }
        
        public (decimal TotalGeral, decimal TotalRecebido, decimal TotalPendente, decimal TotalAtrasado) ObterTotais(ContasReceber filtro)
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
            var fimMes = inicioMes.AddMonths(1);

            var query = _context.ContasReceber.AsQueryable();

            if (filtro != null && (!string.IsNullOrWhiteSpace(filtro.filtroDataInicio) || !string.IsNullOrWhiteSpace(filtro.filtroDescricao) || !string.IsNullOrWhiteSpace(filtro.filtroStatus)))
            {
                query = AplicarFiltros(filtro);
            }
            else
            {
                query = query.Where(c => (c.data_vencimento >= inicioMes && c.data_vencimento < fimMes) 
                                         || (c.data_vencimento < inicioMes && c.status != "PAGA"));
            }

            var dados = query.Select(c => new { c.status, c.valor, c.data_vencimento }).ToList();

            return (
                dados.Sum(c => c.valor),
                dados.Where(c => c.status == "PAGA").Sum(c => c.valor),
                dados.Where(c => c.status == "PENDENTE" && c.data_vencimento >= inicioMes).Sum(c => c.valor),
                dados.Where(c => c.status == "ATRASADO" || (c.data_vencimento < inicioMes && c.status != "PAGA")).Sum(c => c.valor)
            );
        }

        public ContasReceber ObterPorId(int id) => _context.ContasReceber.Find(id);

        public bool Atualizar(ContasReceber conta)
        {
            _context.ContasReceber.Update(conta);
            return _context.SaveChanges() > 0;
        }

        public bool Excluir(int id)
        {
            var conta = _context.ContasReceber.Find(id);
            if (conta == null) return false;

            _context.ContasReceber.Remove(conta);
            return _context.SaveChanges() > 0;
        }

        public bool MarcarComoAtrasado(int id)
        {
            var conta = _context.ContasReceber.Find(id);
            if (conta == null) return false;

            if (conta.status == "ATRASADO") return true;

            conta.status = "ATRASADO";
            return _context.SaveChanges() > 0;
        }

        public DataTable Filtrar(ContasReceber filtro)
        {
            var resultado = AplicarFiltros(filtro)
                .Include(c => c.Pagamento)
                .ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("ID_CONTA", typeof(int));
            dt.Columns.Add("Descricao", typeof(string));
            dt.Columns.Add("Valor", typeof(decimal));
            dt.Columns.Add("DataEmissao", typeof(DateTime));
            dt.Columns.Add("DataPagamento", typeof(object));
            dt.Columns.Add("DataVencimento", typeof(DateTime));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Observacoes", typeof(string));
            dt.Columns.Add("IdOS", typeof(int));
            dt.Columns.Add("FormaPagamento", typeof(string));

            foreach (var item in resultado)
            {
                dt.Rows.Add(
                    item.id_conta_receber,
                    item.descricao,
                    item.valor,
                    item.data_emissao,
                    (object)item.data_pagamento ?? DBNull.Value,
                    item.data_vencimento,
                    item.status,
                    item.observacoes,
                    (object)item.id_os_fk ?? DBNull.Value,
                    item.Pagamento?.Descricao ?? "NÃO DEFINIDA"
                );
            }

            return dt;
        }

        public IQueryable<ContasReceber> AplicarFiltros(ContasReceber filtro)
        {
            var query = _context.ContasReceber.AsQueryable();

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
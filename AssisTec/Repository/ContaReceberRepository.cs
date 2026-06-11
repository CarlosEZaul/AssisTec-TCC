using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
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

        public bool InserirComVinculoOS(ContasReceber conta, int idOs)
        {
            if (!_context.OrdemServicos.Any(o => o.id_os == idOs)) return false;

            conta.id_os_fk = idOs;
            _context.ContasReceber.Add(conta);
            return _context.SaveChanges() > 0;
        }

        // CORRIGIDO: Inclui navegações para evitar dados incompletos
        public IEnumerable<ContasReceber> ObterTodos() =>
            _context.ContasReceber
                .Include(c => c.Pagamento)
                .Include(c => c.OrdemServico)
                .ToList();

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

            // CORRIGIDO: Evita SaveChanges desnecessário se já está ATRASADO
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
            // CORRIGIDO: IdOS como int? em vez de string para manter consistência com o model
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
                    // CORRIGIDO: Mantém DBNull quando nulo, em vez de "N/A" (incompatível com typeof(int))
                    (object)item.id_os_fk ?? DBNull.Value,
                    item.Pagamento?.Descricao ?? "NÃO DEFINIDA"
                );
            }

            return dt;
        }

        public (decimal TotalGeral, decimal TotalRecebido, decimal TotalPendente, decimal TotalAtrasado) ObterTotais(ContasReceber filtro)
        {
            var dados = AplicarFiltros(filtro).Select(c => new { c.status, c.valor }).ToList();

            return (
                dados.Sum(c => c.valor),
                dados.Where(c => c.status == "PAGA").Sum(c => c.valor),
                dados.Where(c => c.status == "PENDENTE").Sum(c => c.valor),
                dados.Where(c => c.status == "ATRASADO").Sum(c => c.valor)
            );
        }

        private IQueryable<ContasReceber> AplicarFiltros(ContasReceber filtro)
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

            // CORRIGIDO: Inclui registros do próprio dia final (até 23:59:59)
            if (DateTime.TryParseExact(filtro.filtroDataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtFim))
                query = query.Where(c => c.data_vencimento < dtFim.Date.AddDays(1));

            return query;
        }
    }
}
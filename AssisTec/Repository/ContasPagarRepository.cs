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
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        #region Consulta

        public IEnumerable<ContasPagarDto> ObterTodos()
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
            var fimMes = inicioMes.AddMonths(1);

            return _context.Contas_Pagar
                .Where(c => (c.data_vencimento >= inicioMes && c.data_vencimento < fimMes)
                            || (c.data_vencimento < inicioMes && c.status != "PAGA"))
                .Select(c => new ContasPagarDto
                {
                    IdContaPagar = c.id_conta_pagar,
                    Descricao = c.descricao,
                    Valor = c.valor,
                    DataEmissao = c.data_emissao,
                    DataPagamento = c.data_pagamento,
                    DataVencimento = c.data_vencimento,
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

        #endregion

        #region Gerenciamento

        public bool Inserir(ContasPagar conta)
        {
            _context.Add(conta);
            return _context.SaveChanges() > 0;
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
            if (conta.status == "ATRASADO") return true;

            conta.status = "ATRASADO";
            return _context.SaveChanges() > 0;
        }

        #endregion

        #region Filtro

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
                    item.data_vencimento,
                    item.status,
                    item.observacoes,
                    item.Pagamento?.Descricao ?? "NÃO DEFINIDA"
                );
            }
            return dt;
        }

        public (decimal TotalGeral, decimal TotalPagar, decimal TotalPendente, decimal TotalAtrasado) ObterTotais(ContasPagar filtro)
        {
            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);

            var dados = AplicarFiltros(filtro)
                .Select(c => new { c.status, c.valor, c.data_vencimento })
                .ToList();

            return (
                dados.Sum(c => c.valor),
                dados.Where(c => c.status == "PAGA").Sum(c => c.valor),
                dados.Where(c => c.status == "PENDENTE" && c.data_vencimento >= inicioMes).Sum(c => c.valor),
                dados.Where(c => c.status == "ATRASADO" || (c.data_vencimento < inicioMes && c.status != "PAGA")).Sum(c => c.valor)
            );
        }

        public IQueryable<ContasPagar> AplicarFiltros(ContasPagar filtro)
        {
            var query = _context.Contas_Pagar.AsQueryable();

            var hoje = DateTime.Today;
            var inicioMes = new DateTime(hoje.Year, hoje.Month, 1);
            var fimMes = inicioMes.AddMonths(1);

            bool possuiFiltroDataInicio = DateTime.TryParseExact(filtro?.filtroDataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtInicio);
            bool possuiFiltroDataFim = DateTime.TryParseExact(filtro?.filtroDataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtFim);

            if (possuiFiltroDataInicio)
                query = query.Where(c => c.data_vencimento >= dtInicio.Date);

            if (possuiFiltroDataFim)
                query = query.Where(c => c.data_vencimento < dtFim.Date.AddDays(1));

            if (!possuiFiltroDataInicio && !possuiFiltroDataFim)
            {
                query = query.Where(c => (c.data_vencimento >= inicioMes && c.data_vencimento < fimMes)
                                      || (c.data_vencimento < inicioMes && c.status != "PAGA"));
            }

            if (!string.IsNullOrWhiteSpace(filtro?.filtroDescricao))
                query = query.Where(c => c.descricao.Contains(filtro.filtroDescricao));

            if (!string.IsNullOrWhiteSpace(filtro?.filtroStatus))
                query = query.Where(c => c.status == filtro.filtroStatus);

            if (filtro?.filtroIdFormaPagamento.HasValue == true && filtro.filtroIdFormaPagamento.Value > 0)
                query = query.Where(c => c.id_forma_pagamento_fk == filtro.filtroIdFormaPagamento.Value);

            return query;
        }

        #endregion

        

        

        

        

        
    }
}
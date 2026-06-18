using System;
using System.Collections.Generic;
using System.Data;
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
                    )
            }
        }

        public (decimal TotalGeral, decimal TotalPagar, decimal TotalPendente, decimal TotalAtrasado) ObterTotais(ContasPagar filtro)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<ContasPagar> AplicarFiltros(ContasPagar filtro)
        {
            throw new NotImplementedException();
        }

        

        
    }
}
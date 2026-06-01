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
        private readonly AppDbContext context;

        public ContasReceberRepository(AppDbContext _context)
        {
            this.context = _context ?? throw new ArgumentNullException(nameof(_context));
        }

        public bool InserirContasReceber(ContasReceber conta)
        {
            try
            {
                context.ContasReceber.Add(conta);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao inserir conta a receber.", ex);
            }
        }

        public bool InserirContasReceberOS(ContasReceber conta, int idOs)
        {
            try
            {
                var os = context.OrdemServicos.FirstOrDefault(o => o.id_os == idOs);
                if (os != null)
                {
                    conta.id_os_fk = idOs;
                    context.ContasReceber.Add(conta);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao inserir conta a receber com OS.", ex);
            }
        }

        public List<ContasReceber> ObterTodasContasReceber()
        {
            try
            {
                return context.ContasReceber.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao obter todas as contas a receber.", ex);
            }
        }

        public ContasReceber ObterPorId(int id)
        {
            try
            {
                return context.ContasReceber.FirstOrDefault(c => c.id_conta_receber == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao obter conta a receber por ID: {id}.", ex);
            }
        }

        public bool AtualizarContasReceber(ContasReceber conta)
        {
            try
            {
                context.ContasReceber.Update(conta);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao atualizar conta a receber.", ex);
            }
        }

        public bool ExcluirContasReceber(int id)
        {
            try
            {
                var conta = context.ContasReceber.FirstOrDefault(c => c.id_conta_receber == id);
                if (conta != null)
                {
                    context.ContasReceber.Remove(conta);
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao excluir conta a receber com ID: {id}.", ex);
            }
        }

        public bool AtualizarParaAtrasado(int id)
        {
            try
            {
                var conta = context.ContasReceber.FirstOrDefault(c => c.id_conta_receber == id);
                if (conta != null)
                {
                    conta.status = "ATRASADO";
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Falha ao atualizar conta a receber para atrasado. ID: {id}", ex);
            }
        }

        public DataTable FiltrarContasReceber(ContasReceber filtro)
        {
            try
            {
                var query = AplicarFiltros(filtro);

                var resultado = query
                    .Include(c => c.Pagamento)
                    .Select(c => new
                    {
                        c.id_conta_receber,
                        c.id_os_fk,
                        c.descricao,
                        c.valor,
                        c.data_emissao,
                        c.data_pagamento,
                        c.data_vencimento,
                        c.status,
                        FormaPagamento = c.Pagamento != null ? c.Pagamento.Descricao : "NÃO DEFINIDA",
                        c.observacoes
                    })
                    .ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("ID_CONTA", typeof(int));
                dt.Columns.Add("ID_OS", typeof(string));
                dt.Columns.Add("Descrição", typeof(string));
                dt.Columns.Add("Valor", typeof(decimal));
                dt.Columns.Add("Data de Emissão", typeof(DateTime));
                dt.Columns.Add("Data de Pagamento", typeof(object));
                dt.Columns.Add("Data de Vencimento", typeof(DateTime));
                dt.Columns.Add("Status", typeof(string));
                dt.Columns.Add("Forma de Pagamento", typeof(string));
                dt.Columns.Add("Observações", typeof(string));

                foreach (var item in resultado)
                {
                    dt.Rows.Add(
                        item.id_conta_receber,
                        item.id_os_fk.HasValue ? item.id_os_fk.Value.ToString() : "N/A",
                        item.descricao,
                        item.valor,
                        item.data_emissao,
                        item.data_pagamento.HasValue ? (object)item.data_pagamento.Value : DBNull.Value,
                        item.data_vencimento,
                        item.status,
                        item.FormaPagamento,
                        item.observacoes
                    );
                }

                return dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao filtrar contas a receber no banco.", ex);
            }
        }

        public (decimal totalGeral, decimal totalRecebido, decimal totalPendente, decimal totalAtrasado) AtualizarTotais(ContasReceber filtro)
        {
            try
            {
                var query = AplicarFiltros(filtro);

                var dadosTotais = query
                    .Select(c => new { c.status, c.valor })
                    .ToList();

                decimal totalGeral = dadosTotais.Sum(c => c.valor);
                decimal totalRecebido = dadosTotais.Where(c => c.status == "PAGA").Sum(c => c.valor);
                decimal totalPendente = dadosTotais.Where(c => c.status == "PENDENTE").Sum(c => c.valor);
                decimal totalAtrasado = dadosTotais.Where(c => c.status == "ATRASADO").Sum(c => c.valor);

                return (totalGeral, totalRecebido, totalPendente, totalAtrasado);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao calcular totais das contas a receber.", ex);
            }
        }

        private IQueryable<ContasReceber> AplicarFiltros(ContasReceber filtro)
        {
            var query = context.ContasReceber.AsQueryable();

            if (!string.IsNullOrEmpty(filtro.filtroDescricao))
            {
                query = query.Where(c => c.descricao.Contains(filtro.filtroDescricao));
            }

            if (!string.IsNullOrEmpty(filtro.filtroStatus))
            {
                query = query.Where(c => c.status == filtro.filtroStatus);
            }

            if (filtro.filtroIdFormaPagamento.HasValue && filtro.filtroIdFormaPagamento.Value > 0)
            {
                query = query.Where(c => c.id_forma_pagamento_fk == filtro.filtroIdFormaPagamento.Value);
            }

            if (!string.IsNullOrEmpty(filtro.filtroDataInicio))
            {
                if (DateTime.TryParseExact(filtro.filtroDataInicio, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtInicio))
                {
                    query = query.Where(c => c.data_vencimento >= dtInicio.Date);
                }
            }

            if (!string.IsNullOrEmpty(filtro.filtroDataFim))
            {
                if (DateTime.TryParseExact(filtro.filtroDataFim, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dtFim))
                {
                    query = query.Where(c => c.data_vencimento <= dtFim.Date);
                }
            }

            return query;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using AssisTec.Models;
using Microsoft.EntityFrameworkCore;

namespace AssisTec.Repository
{
    public class MovimentacaoEstoqueRepository : IMovimentacaoEstoqueRepository
    {
        private readonly AppDbContext context;
        public MovimentacaoEstoqueRepository(AppDbContext context)
        {
            this.context = context;
        }

        #region Consulta
        public object ListarMovimentacaoEstoque()
        {
            try
            {
                DateTime hoje = DateTime.Today;
                DateTime primeiroDiaDoMes = new DateTime(hoje.Year, hoje.Month, 1);
                DateTime ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddTicks(-1);

                return context.movimentacaoEstoque
                    .Where(m => m.data >= primeiroDiaDoMes && m.data <= ultimoDiaDoMes)
                    .Select(m => new
                    {
                        ID_Movimentacao = m.idMovimentacao,
                        Produto = Convert.ToString(m.idProduto) + " - " + m.produto.descricao,
                        Data = m.data,
                        Quantidade = m.quantidade,
                        Valor = m.valor,
                        Descricao = m.descricao,
                        TipoMovimentacao = m.tipoMovimentacao,
                        Registrado = Convert.ToString(m.usuario.Id) + " - " + m.usuario.Nome
                    }).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao carregar movimentação do estoque", e);
            }
        }
        

        #endregion

        #region Gerenciamento

        public bool InserirMovimentacao(MovimentacaoEstoque movimentacaoEstoque)
        {
            try
            {
                context.movimentacaoEstoque.Add(movimentacaoEstoque);
                return context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao inserir movimentação do estoque no banco de dados", e);
            }
        }

        #endregion

        #region Filtro
        public object Filtrar(DateTime? dataInicio, DateTime? dataFim, string produtoSelecionado, string tipoMovimentacao)
        {
            try
            {
                var query = context.movimentacaoEstoque.AsQueryable();

                if (dataInicio.HasValue || dataFim.HasValue)
                {
                    if (dataInicio.HasValue)
                    {
                        query = query.Where(m => m.data >= dataInicio.Value);
                    }

                    if (dataFim.HasValue)
                    {
                        DateTime dataLimite = dataFim.Value.Date.AddDays(1).AddTicks(-1);
                        query = query.Where(m => m.data <= dataLimite);
                    }
                }
                else
                {
                    DateTime hoje = DateTime.Today;
                    DateTime primeiroDiaDoMes = new DateTime(hoje.Year, hoje.Month, 1);
                    DateTime ultimoDiaDoMes = primeiroDiaDoMes.AddMonths(1).AddTicks(-1);

                    query = query.Where(m => m.data >= primeiroDiaDoMes && m.data <= ultimoDiaDoMes);
                }

                if (!string.IsNullOrEmpty(produtoSelecionado) && produtoSelecionado != "Todos")
                {
                    string[] partes = produtoSelecionado.Split('-');
                    if (partes.Length > 0 && int.TryParse(partes[0].Trim(), out int idProduto))
                    {
                        query = query.Where(m => m.idProduto == idProduto);
                    }
                }

                if (!string.IsNullOrEmpty(tipoMovimentacao) && tipoMovimentacao != "Todos")
                {
                    query = query.Where(m => m.tipoMovimentacao == tipoMovimentacao);
                }

                return query.Select(m => new
                {
                    ID_Movimentacao = m.idMovimentacao,
                    Produto = Convert.ToString(m.idProduto) + " - " + m.produto.descricao,
                    Data = m.data,
                    Quantidade = m.quantidade,
                    Valor = m.valor,
                    Descricao = m.descricao,
                    TipoMovimentacao = m.tipoMovimentacao,
                    Registrado = Convert.ToString(m.idUsuario) + " - " + m.usuario.Nome
                }).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao filtrar as movimentações do estoque.", e);
            }
        }
        

        #endregion

       

        

        
    }
}
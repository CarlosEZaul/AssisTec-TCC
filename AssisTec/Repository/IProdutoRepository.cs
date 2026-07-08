using System.Collections.Generic;
using System.Data;
using System.Linq;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IProdutoRepository
    {
        bool InserirProduto(Produto produto);
        bool AtualizarProduto(Produto produto);
        bool ExcluirProduto(int id);
        Produto ObterProdutoPorId(int id);
        IEnumerable<Produto> ObterProdutos();
        DataTable Filtrar(Produto produto);
        (int totalCadastrado, int abaixoMinimo, int semEstoque, decimal valorEstoque) obterTotais(Produto produto);
        IQueryable<Produto> AplicarFiltro(Produto produto);
        
        
    }
}
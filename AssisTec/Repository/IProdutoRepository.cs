using System.Collections.Generic;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IProdutoRepository
    {
        bool InsertProduto(Produto produto);
        bool AtualizarProduto(Produto produto);
        bool ExcluirProduto(int id);
        Produto ObterProdutoPorId(int id);
        List<Produto> ObterProdutos();
        
    }
}
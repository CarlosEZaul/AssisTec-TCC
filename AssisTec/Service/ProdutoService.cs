using System;
using System.Collections.Generic;
using System.Data;
using AssisTec.Models;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class ProdutoService 
    {
        private readonly IProdutoRepository repository;
        
        public ProdutoService(IProdutoRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public IEnumerable<Produto> ObterProdutos()
        {
            return repository.ObterProdutos();
        }

        public Produto ObterProdutoPorId(int id)
        {
            if(id < 0) return null;
            return repository.ObterProdutoPorId(id);
        }

        public bool Salvar(Produto produto)
        {
            ValidarCampos(produto);
            
            
            if (produto.quantidade_minima < produto.quantidade)
            {
                throw new ArgumentNullException("Quantidade minima não pode ser menor que a atual");
            }

            var Inserir = repository.InserirProduto(produto);

            if (Inserir)
            {
                return true;
            }
            else
            {
                throw new Exception("Produto não foi salvo");
                return false;
            }
            
        }

        public bool atualizarProduto(Produto produto)
        {
            ValidarCampos(produto);
            
            var atualizar = repository.AtualizarProduto(produto);

            if (atualizar)
            {
                return true;
            }
            else
            {
                throw new Exception("Falha ao  atualizar Produto");
            }
        }

        public bool excluirProduto(Produto produto)
        {
            if(produto.idProduto < 0)
            {
                throw new ArgumentNullException("Produto nulo");
            }
            var excluir = repository.ExcluirProduto(produto.idProduto);

            if (excluir)
            {
                return true;
            }
            else
            {
                throw new Exception("Falha ao deletar Produto");
            }
            
            
        }

        public (int totalCadastrado, int abaixoMinimo, int semEstoque, decimal valorEstoque) obterTotais()
        {
            return repository.obterTotais(new Produto());
        }

        public (DataTable dados, int totalCadastrado, int abaixoMinimo, int semEstoque, decimal valorEstoque) Filtrar(string descricao)
        {
            var filtro = new Produto()
            {
                filtroDescricao = descricao?.Trim(),
            };
            
            var dados = repository.Filtrar(filtro);
            var totais = repository.obterTotais(filtro);
            
            return (dados, totais.totalCadastrado,totais.abaixoMinimo, totais.semEstoque, totais.valorEstoque);
        }
        

        private bool ValidarCampos(Produto produto)
        {
            if (produto == null)
            {
                throw new ArgumentNullException("produto nulo");
            }
            if (string.IsNullOrWhiteSpace(produto.descricao))
            {
                throw new ArgumentNullException("Descrição nula");
            }
            if (produto.preco_compra <0)
            {
                throw new ArgumentNullException("Preço inválido");
            }

            if (produto.preco_venda < 0)
            {
                throw new ArgumentNullException("Preço inválido");
            }
            if (produto.quantidade <0)
            {
                throw new ArgumentNullException("Quantidade não pode ser menor que 0");
            }

            if (string.IsNullOrWhiteSpace(produto.unidade))
            {
                throw new ArgumentNullException("Unidade nula");
            }
            if (produto.quantidade_minima <0)
            {
                throw new ArgumentNullException("Quantidade minima não pode ser menor que 0");
            }

            return true;

        }
    }
}
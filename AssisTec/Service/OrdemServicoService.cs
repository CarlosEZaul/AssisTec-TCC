using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Transactions;
using AssisTec.Models;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class OrdemServicoService
    {
        private readonly IOrdemServicoRepository _ordemServicoRepository;
        private readonly IEquipamentoRepository _equipamentoRepository;
        private readonly IUsuarioReposity _usuarioRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IitemOSRepository _itemOSRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMovimentacaoEstoqueRepository _movimentacaoEstoqueRepository;

        public OrdemServicoService(
            IOrdemServicoRepository ordemServicoRepository,
            IEquipamentoRepository equipamentoRepository,
            IUsuarioReposity usuarioRepository,
            IClienteRepository clienteRepository,
            IitemOSRepository itemOSRepository,
            IProdutoRepository produtoRepository,
            IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository)
        {
            _ordemServicoRepository = ordemServicoRepository ?? throw new ArgumentNullException(nameof(ordemServicoRepository));
            _equipamentoRepository = equipamentoRepository ?? throw new ArgumentNullException(nameof(equipamentoRepository));
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
            _itemOSRepository = itemOSRepository ?? throw new ArgumentNullException(nameof(itemOSRepository));
            _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));
            _movimentacaoEstoqueRepository = movimentacaoEstoqueRepository ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueRepository));
        }

        public List<Cliente> ObterClientes()
        {
            return _clienteRepository.ObterTodosClientes();
        }

        public List<Usuario> ObterTecnicosAtivados()
        {
            return _usuarioRepository.ObterTodosTecnicosAtivados();
        }

        public OrdemServico ObterPorId(int id)
        {
            return _ordemServicoRepository.ObterPorId(id);
        }

        public List<Produto> ObterProdutos()
        {
            return _produtoRepository.ObterProdutos().Where(p => p.status == "Ativado").ToList();
        }

        public Produto ObterProdutoPorId(int id)
        {
            return _produtoRepository.ObterProdutoPorId(id);
        }

        public bool SalvarOS(OrdemServico ordemServico, Equipamento equipamento)
        {
            ValidarEntidades(ordemServico, equipamento);

            using (var scope = new TransactionScope())
            {
                bool equipamentoSalvo = _equipamentoRepository.SalvarEquipamento(equipamento);
                if (!equipamentoSalvo || equipamento.Id_equipamento <= 0)
                {
                    throw new InvalidOperationException("Não foi possível cadastrar o equipamento no sistema.");
                }

                ordemServico.id_equipamento = equipamento.Id_equipamento;
                ordemServico.data_abertura = DateTime.Now;
                ordemServico.status = "ABERTA";

                bool osSalva = _ordemServicoRepository.SalvarOrdemServico(ordemServico);

                if (osSalva)
                {
                    scope.Complete();
                    return true;
                }

                return false;
            }
        }

        public bool SalvarAlteracoesOS(OrdemServico os)
        {
            return _ordemServicoRepository.SalvarAlteracoesOS(os);
        }

        public bool AdicionarOuAtualizarItemOS(int idOS, int idProduto, int quantidadeAdicionar)
        {
            if (idOS <= 0)
                throw new ArgumentException("Ordem de Serviço inválida.");

            if (idProduto <= 0)
                throw new ArgumentException("Selecione um produto válido.");

            if (quantidadeAdicionar <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            var produto = _produtoRepository.ObterProdutoPorId(idProduto);
            if (produto == null)
                throw new InvalidOperationException("Produto não encontrado.");

            if (produto.quantidade < quantidadeAdicionar)
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {produto.quantidade}");

            DataTable dtItens = _itemOSRepository.ObterPorOrdemServico(idOS);
            DataRow itemExistente = null;

            if (dtItens != null)
            {
                foreach (DataRow row in dtItens.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;
                    if (dtItens.Columns.Contains("idProduto") && Convert.ToInt32(row["idProduto"]) == idProduto)
                    {
                        itemExistente = row;
                        break;
                    }
                }
            }

            using (var scope = new TransactionScope())
            {
                if (itemExistente != null)
                {
                    int idItemExistente = Convert.ToInt32(itemExistente["Id"]);
                    int quantidadeAtual = Convert.ToInt32(itemExistente["Quantidade"]);
                    int novaQuantidade = quantidadeAtual + quantidadeAdicionar;

                    if (!RemoverItemDirect(idItemExistente))
                        return false;

                    var novoItem = new ItemOS
                    {
                        id_OS = idOS,
                        id_produto = idProduto,
                        Quantidade = novaQuantidade,
                        ValorUnitario = produto.preco_venda
                    };

                    if (!_itemOSRepository.SalvarItemOS(novoItem))
                        return false;

                    produto.quantidade -= quantidadeAdicionar;
                    _produtoRepository.AtualizarProduto(produto);

                    RegistrarMovimentacao(idProduto, quantidadeAdicionar, produto.preco_venda * quantidadeAdicionar,
                        $"Saída de estoque (Adição) na OS #{idOS}", "Saída");
                }
                else
                {
                    var novoItem = new ItemOS
                    {
                        id_OS = idOS,
                        id_produto = idProduto,
                        Quantidade = quantidadeAdicionar,
                        ValorUnitario = produto.preco_venda
                    };

                    if (!_itemOSRepository.SalvarItemOS(novoItem))
                        return false;

                    produto.quantidade -= quantidadeAdicionar;
                    _produtoRepository.AtualizarProduto(produto);

                    RegistrarMovimentacao(idProduto, quantidadeAdicionar, produto.preco_venda * quantidadeAdicionar,
                        $"Saída de estoque por inclusão na OS #{idOS}", "Saída");
                }

                RecalcularEAtualizarValorPecas(idOS);

                scope.Complete();
                return true;
            }
        }

        public bool ReduzirOuRemoverItemOS(int idItem, int quantidadeRemover)
        {
            if (idItem <= 0)
                throw new ArgumentException("Item inválido.");

            if (quantidadeRemover <= 0)
                throw new ArgumentException("A quantidade a ser removida deve ser maior que zero.");

            var item = _itemOSRepository.ObterPorId(idItem);
            if (item == null)
                throw new InvalidOperationException("Item da OS não encontrado.");

            var produto = _produtoRepository.ObterProdutoPorId(item.id_produto.GetValueOrDefault());
            if (produto == null)
                throw new InvalidOperationException("Produto associado ao item não foi encontrado.");

            int idOS = item.id_OS.GetValueOrDefault();

            using (var scope = new TransactionScope())
            {
                if (quantidadeRemover >= item.Quantidade)
                {
                    if (!_itemOSRepository.Remover(idItem))
                        return false;

                    produto.quantidade += item.Quantidade;
                    _produtoRepository.AtualizarProduto(produto);

                    RegistrarMovimentacao(produto.idProduto, item.Quantidade, item.ValorUnitario * item.Quantidade,
                        $"Entrada em estoque por remoção do item da OS #{item.id_OS}", "Entrada");
                }
                else
                {
                    int novaQuantidade = item.Quantidade - quantidadeRemover;

                    if (!_itemOSRepository.Remover(idItem))
                        return false;

                    var itemAtualizado = new ItemOS
                    {
                        id_OS = item.id_OS,
                        id_produto = item.id_produto,
                        Quantidade = novaQuantidade,
                        ValorUnitario = item.ValorUnitario
                    };

                    if (!_itemOSRepository.SalvarItemOS(itemAtualizado))
                        return false;

                    produto.quantidade += quantidadeRemover;
                    _produtoRepository.AtualizarProduto(produto);

                    RegistrarMovimentacao(produto.idProduto, quantidadeRemover, item.ValorUnitario * quantidadeRemover,
                        $"Entrada em estoque por redução do item da OS #{item.id_OS}", "Entrada");
                }

                RecalcularEAtualizarValorPecas(idOS);

                scope.Complete();
                return true;
            }
        }

        private void RecalcularEAtualizarValorPecas(int idOS)
        {
            var os = _ordemServicoRepository.ObterPorId(idOS);
            if (os == null) return;

            DataTable dtItens = _itemOSRepository.ObterPorOrdemServico(idOS);
            decimal totalPecas = 0;

            if (dtItens != null)
            {
                foreach (DataRow row in dtItens.Rows)
                {
                    if (row.RowState == DataRowState.Deleted) continue;

                    if (dtItens.Columns.Contains("ValorTotal") && row["ValorTotal"] != DBNull.Value)
                    {
                        if (decimal.TryParse(row["ValorTotal"].ToString(), out decimal val))
                        {
                            totalPecas += val;
                        }
                    }
                }
            }

            os.valor_pecas = totalPecas;
            os.data_atualizacao = DateTime.Now;

            _ordemServicoRepository.SalvarAlteracoesOS(os);
        }

        private bool RemoverItemDirect(int idItem)
        {
            return _itemOSRepository.Remover(idItem);
        }

        private void RegistrarMovimentacao(int idProduto, int quantidade, decimal valorTotal, string descricao, string tipo)
        {
            var movimentacao = new MovimentacaoEstoque
            {
                idProduto = idProduto,
                data = DateTime.Now,
                quantidade = quantidade,
                valor = valorTotal,
                descricao = descricao,
                tipoMovimentacao = tipo
            };

            _movimentacaoEstoqueRepository.InserirMovimentacao(movimentacao);
        }

        public bool RemoverItemOS(int idItem)
        {
            var item = _itemOSRepository.ObterPorId(idItem);
            if (item == null)
                throw new InvalidOperationException("Item da OS não encontrado.");

            return ReduzirOuRemoverItemOS(idItem, item.Quantidade);
        }

        public DataTable ObterItensDaOS(int idOS)
        {
            return _itemOSRepository.ObterPorOrdemServico(idOS);
        }

        private void ValidarEntidades(OrdemServico os, Equipamento eq)
        {
            if (os == null)
                throw new ArgumentNullException(nameof(os));

            if (eq == null)
                throw new ArgumentNullException(nameof(eq));

            if (!os.id_cliente.HasValue || os.id_cliente.Value <= 0)
                throw new ArgumentException("Selecione um cliente válido.");

            if (!os.id_tecnico.HasValue || os.id_tecnico.Value <= 0)
                throw new ArgumentException("Selecione um técnico responsável.");

            if (string.IsNullOrWhiteSpace(os.problema_relatado))
                throw new ArgumentException("O problema relatado deve ser preenchido.");

            if (string.IsNullOrWhiteSpace(eq.Descricao))
                throw new ArgumentException("A descrição do equipamento é obrigatória.");

            if (string.IsNullOrWhiteSpace(eq.Marca))
                throw new ArgumentException("A marca do equipamento é obrigatória.");

            if (string.IsNullOrWhiteSpace(eq.Modelo))
                throw new ArgumentException("O modelo do equipamento é obrigatório.");

            if (string.IsNullOrWhiteSpace(eq.Numero_Serie))
                throw new ArgumentException("O número de série é obrigatório.");

            if (string.IsNullOrWhiteSpace(eq.estado_entrada))
                throw new ArgumentException("O estado de entrada do equipamento é obrigatório.");
        }

        public DataTable ObterHistoricoOsTecnico(int id)
        {
            return _ordemServicoRepository.ObterHistoricoUsuario(id);
        }

        public int ObterQntOsAbertas()
        {
            return _ordemServicoRepository.ObterQntOsAbertas();
        }

        public DataTable OrdensRecentes()
        {
            return _ordemServicoRepository.OrdensRecentes();
        }

        public IEnumerable<dynamic> ObterTodasOSAtuais()
        {
            return _ordemServicoRepository.ObterTodasOSAtuais();
        }
    }
}
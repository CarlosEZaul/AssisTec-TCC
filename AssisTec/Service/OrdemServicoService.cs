using System;
using System.Collections;
using System.Collections.Generic;
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
        private readonly IItemOSRepository _itemOSRepository;
        private readonly IServicosOSRepository _servicosOSRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMovimentacaoEstoqueRepository _movimentacaoEstoqueRepository;
        private readonly IHistoricoAlteracaoOSRepository  _historicoAlteracaoOSRepository;

        public OrdemServicoService(
            IOrdemServicoRepository ordemServicoRepository,
            IEquipamentoRepository equipamentoRepository,
            IUsuarioReposity usuarioRepository,
            IClienteRepository clienteRepository,
            IItemOSRepository itemOSRepository,
            IServicosOSRepository servicosOSRepository,
            IProdutoRepository produtoRepository,
            IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository,
            IHistoricoAlteracaoOSRepository historicoAlteracaoOSRepository
            )
        {
            _ordemServicoRepository = ordemServicoRepository ?? throw new ArgumentNullException(nameof(ordemServicoRepository));
            _equipamentoRepository = equipamentoRepository ?? throw new ArgumentNullException(nameof(equipamentoRepository));
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
            _itemOSRepository = itemOSRepository ?? throw new ArgumentNullException(nameof(itemOSRepository));
            _servicosOSRepository =  servicosOSRepository ?? throw new ArgumentNullException(nameof(servicosOSRepository));
            _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));
            _movimentacaoEstoqueRepository = movimentacaoEstoqueRepository ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueRepository));
            _historicoAlteracaoOSRepository = historicoAlteracaoOSRepository ?? throw new ArgumentNullException(nameof(historicoAlteracaoOSRepository));
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

        #region Equipamento

        public Equipamento ObterEquipamentoPorId(int id)
        {
            try
            {
                return _equipamentoRepository.ObterEquipamentoPorId(id);
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao carregar equipamento: " + e.Message);
            }
        }

        public bool AtualizarEquipamento(Equipamento equipamento)
        {
            try
            {
                ValidarEquipamento(equipamento);
                return _equipamentoRepository.AtualzarEquipamento(equipamento);
            }
            catch (Exception e)
            {
                throw new Exception("Falha ao atualizar equipamento: " + e.Message);
            }
        }

        #endregion

        #region Produtos
        public List<Produto> ObterProdutos()
        {
            return _produtoRepository.ObterProdutos().Where(p => p.status == "Ativado").ToList();
        }

        public Produto ObterProdutoPorId(int id)
        {
            return _produtoRepository.ObterProdutoPorId(id);
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

            IEnumerable<dynamic> itens = _itemOSRepository.ObterPorOrdemServico(idOS);
            ItemOS itemExistente = itens?.FirstOrDefault(x => x.id_produto == idProduto);

            using (var scope = new TransactionScope())
            {
                if (itemExistente != null)
                {
                    int idItemExistente = itemExistente.Id;
                    int quantidadeAtual = itemExistente.Quantidade;
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

            IEnumerable<dynamic> itens = _itemOSRepository.ObterPorOrdemServico(idOS);
            decimal totalPecas = itens != null ? itens.Sum(i => (decimal)i.ValorTotal) : 0;

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
        

        #endregion

        #region Historico de Alterações

        public bool RegistrarHistoricoOS(HistoricoAlteracaoOS historicoAlteracaoOs)
        {
            if (historicoAlteracaoOs.idOS <= 0)
                throw new ArgumentException("Ordem de Serviço inválida.");

            if (historicoAlteracaoOs.idUsuario <= 0)
                throw new ArgumentException("Usuário inválido.");

            if (string.IsNullOrWhiteSpace(historicoAlteracaoOs.tipo))
                throw new ArgumentException("O tipo de alteração é obrigatório.");

            if (string.IsNullOrWhiteSpace(historicoAlteracaoOs.descricao))
                throw new ArgumentException("A descrição é obrigatória.");

            return _historicoAlteracaoOSRepository.RegistrarHistorico(historicoAlteracaoOs);
        }
        
        public IEnumerable ObterPorOrdemServico(int idOS)
        {
            if (idOS <= 0)
                throw new ArgumentException("Ordem de Serviço inválida.");

            return _historicoAlteracaoOSRepository.ObterPorOrdemServico(idOS);
        }

        #endregion

        

        public bool SalvarOS(OrdemServico ordemServico, Equipamento equipamento)
        {
            ValidarOS(ordemServico);
            ValidarEquipamento(equipamento);

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

        

        public bool SalvarServicoOS(ServicosOS servico)
        {
            ValidarServicoOS(servico);

            var os = _ordemServicoRepository.ObterPorId(servico.id_OS.GetValueOrDefault());
            if (os == null)
                throw new InvalidOperationException("A Ordem de Serviço informada não foi encontrada.");

            bool salvou = _servicosOSRepository.SalvarAcaoOS(servico);
            if (!salvou)
                return false;

            RecalcularEAtualizarValorMaoObra(servico.id_OS.GetValueOrDefault());
            return true;
        }

        public ServicosOS ObterServicoOSPorID(int idAcao)
        {
            if (idAcao <= 0)
                throw new ArgumentException("ID da Ação inválido.");

            return _servicosOSRepository.ObterAcaoOSPorID(idAcao);
        }

        public bool ExcluirServicoOS(int idAcao)
        {
            if (idAcao <= 0)
                throw new ArgumentException("ID da Ação inválido.");

            var acao = _servicosOSRepository.ObterAcaoOSPorID(idAcao);
            if (acao == null)
                throw new InvalidOperationException("Ação não encontrada.");

            int idOS = acao.id_OS.GetValueOrDefault();

            bool excluiu = _servicosOSRepository.ExcluirAcaoOS(idAcao);
            if (!excluiu)
                return false;

            RecalcularEAtualizarValorMaoObra(idOS);
            return true;
        }

        public List<ServicosOS> ListarAcaoOSPorOS(int idOS)
        {
            if (idOS <= 0)
                throw new ArgumentException("Ordem de Serviço inválida.");

            return _servicosOSRepository.ListarAcaoOSPorOS(idOS);
        }

        private void RecalcularEAtualizarValorMaoObra(int idOS)
        {
            var os = _ordemServicoRepository.ObterPorId(idOS);
            if (os == null) return;

            var acoes = _servicosOSRepository.ListarAcaoOSPorOS(idOS);
            decimal totalMaoObra = acoes != null ? acoes.Sum(a => a.valor_cobrado) : 0;

            os.valor_mao_obra = totalMaoObra;
            os.data_atualizacao = DateTime.Now;

            _ordemServicoRepository.SalvarAlteracoesOS(os);
        }

        private void ValidarServicoOS(ServicosOS servico)
        {
            if (servico == null)
                throw new ArgumentNullException(nameof(servico));

            if (servico.id_OS <= 0)
                throw new ArgumentException("Selecione uma Ordem de Serviço válida.");

            if (string.IsNullOrWhiteSpace(servico.descricao))
                throw new ArgumentException("A descrição da ação é obrigatória.");

            if (servico.descricao.Length > 150)
                throw new ArgumentException("A descrição não pode ter mais de 150 caracteres.");

            if (servico.valor_cobrado < 0)
                throw new ArgumentException("O valor cobrado não pode ser negativo.");
        }

        public IEnumerable<dynamic> ObterItensDaOS(int idOS)
        {
            return _itemOSRepository.ObterPorOrdemServico(idOS);
        }

        private void ValidarOS(OrdemServico os)
        {
            if (os == null)
                throw new ArgumentNullException(nameof(os));

            if (!os.id_cliente.HasValue || os.id_cliente.Value <= 0)
                throw new ArgumentException("Selecione um cliente válido.");

            if (!os.id_tecnico.HasValue || os.id_tecnico.Value <= 0)
                throw new ArgumentException("Selecione um técnico responsável.");

            if (string.IsNullOrWhiteSpace(os.problema_relatado))
                throw new ArgumentException("O problema relatado deve ser preenchido.");
        }

        private void ValidarEquipamento(Equipamento eq)
        {
            if (eq == null)
                throw new ArgumentNullException(nameof(eq));
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

        public System.Data.DataTable ObterHistoricoOsTecnico(int id)
        {
            return _ordemServicoRepository.ObterHistoricoUsuario(id);
        }

        public int ObterQntOsAbertas()
        {
            return _ordemServicoRepository.ObterQntOsAbertas();
        }

        public System.Data.DataTable OrdensRecentes()
        {
            return _ordemServicoRepository.OrdensRecentes();
        }

        public IEnumerable<dynamic> ObterTodasOSAtuais()
        {
            return _ordemServicoRepository.ObterTodasOSAtuais();
        }
    }
}
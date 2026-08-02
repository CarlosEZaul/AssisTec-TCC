using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Transactions;
using AssisTec.DTO;
using AssisTec.Models;
using AssisTec.Repository;
using AssisTec.Utils;
using Microsoft.EntityFrameworkCore;

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
        private readonly IHistoricoAlteracaoOSRepository _historicoAlteracaoOSRepository;
        private readonly IPagamentoRepository _pagamentoRepository;
        private readonly IContaReceberRepository _contaReceberRepository;

        public OrdemServicoService(
            IOrdemServicoRepository ordemServicoRepository,
            IEquipamentoRepository equipamentoRepository,
            IUsuarioReposity usuarioRepository,
            IClienteRepository clienteRepository,
            IItemOSRepository itemOSRepository,
            IServicosOSRepository servicosOSRepository,
            IProdutoRepository produtoRepository,
            IMovimentacaoEstoqueRepository movimentacaoEstoqueRepository,
            IHistoricoAlteracaoOSRepository historicoAlteracaoOSRepository,
            IContaReceberRepository contaReceberRepository,
            IPagamentoRepository pagamentoRepository
            )
        {
            _ordemServicoRepository = ordemServicoRepository ?? throw new ArgumentNullException(nameof(ordemServicoRepository));
            _equipamentoRepository = equipamentoRepository ?? throw new ArgumentNullException(nameof(equipamentoRepository));
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
            _itemOSRepository = itemOSRepository ?? throw new ArgumentNullException(nameof(itemOSRepository));
            _servicosOSRepository = servicosOSRepository ?? throw new ArgumentNullException(nameof(servicosOSRepository));
            _produtoRepository = produtoRepository ?? throw new ArgumentNullException(nameof(produtoRepository));
            _movimentacaoEstoqueRepository = movimentacaoEstoqueRepository ?? throw new ArgumentNullException(nameof(movimentacaoEstoqueRepository));
            _historicoAlteracaoOSRepository = historicoAlteracaoOSRepository ?? throw new ArgumentNullException(nameof(historicoAlteracaoOSRepository));
            _pagamentoRepository = pagamentoRepository ?? throw new ArgumentNullException(nameof(pagamentoRepository));
            _contaReceberRepository = contaReceberRepository ?? throw new ArgumentNullException(nameof(contaReceberRepository));
        }

        #region Consultas e Leitura

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

        public int ObterQntOsAbertas()
        {
            return _ordemServicoRepository.ObterQntOsAbertas();
        }

        

        #endregion

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

        #region Produtos e Itens da OS

        public List<Produto> ObterProdutos()
        {
            return _produtoRepository.ObterProdutos().Where(p => p.status == "Ativado").ToList();
        }

        List<ItemOSRelatorioDTO> ObterItensPorOSId(int idOS)
        {
            return _ordemServicoRepository.ObterItensPorOSId(idOS);
        }

        List<ServicoOSRelatorioDTO> ObterServicosPorOSId(int idOS)
        {
            return _ordemServicoRepository.ObterServicosPorOSId(idOS);
        }

        public Produto ObterProdutoPorId(int id)
        {
            return _produtoRepository.ObterProdutoPorId(id);
        }

        public IEnumerable<dynamic> ObterItensDaOS(int idOS)
        {
            return _itemOSRepository.ObterPorOrdemServico(idOS);
        }

        public bool AdicionarOuAtualizarItemOS(OrdemServico os, int idProduto, int quantidadeAdicionar)
        {
            if (os.id_os <= 0)
                throw new ArgumentException("Ordem de Serviço inválida.");
            
            if (os.status != "ABERTA")
                throw new InvalidOperationException("Ordem de Serviço não está aberta para alterações");

            if (idProduto <= 0)
                throw new ArgumentException("Selecione um produto válido.");

            if (quantidadeAdicionar <= 0)
                throw new ArgumentException("A quantidade deve ser maior que zero.");

            var produto = _produtoRepository.ObterProdutoPorId(idProduto);
            if (produto == null)
                throw new InvalidOperationException("Produto não encontrado.");

            if (produto.quantidade < quantidadeAdicionar)
                throw new InvalidOperationException($"Estoque insuficiente. Disponível: {produto.quantidade}");

            IEnumerable<dynamic> itens = _itemOSRepository.ObterPorOrdemServico(os.id_os);
            var itemExistenteDynamic = itens?.FirstOrDefault(x => x.id_produto == idProduto);

            using (var scope = new TransactionScope())
            {
                if (itemExistenteDynamic != null)
                {
                    int idItemExistente = itemExistenteDynamic.Id;
                    int quantidadeAtual = itemExistenteDynamic.Quantidade;
                    int novaQuantidade = quantidadeAtual + quantidadeAdicionar;

                    if (!RemoverItemDirect(idItemExistente))
                        return false;

                    var novoItem = new ItemOS
                    {
                        id_OS = os.id_os,
                        id_produto = idProduto,
                        Quantidade = novaQuantidade,
                        ValorUnitario = produto.preco_venda
                    };

                    if (!_itemOSRepository.SalvarItemOS(novoItem))
                        return false;

                    produto.quantidade -= quantidadeAdicionar;
                    _produtoRepository.AtualizarProduto(produto);

                    RegistrarMovimentacao(idProduto, quantidadeAdicionar, produto.preco_venda * quantidadeAdicionar,
                        $"Saída de estoque (Adição) na OS #{os.id_os}", "Saída");
                }
                else
                {
                    var novoItem = new ItemOS
                    {
                        id_OS = os.id_os,
                        id_produto = idProduto,
                        Quantidade = quantidadeAdicionar,
                        ValorUnitario = produto.preco_venda
                    };

                    if (!_itemOSRepository.SalvarItemOS(novoItem))
                        return false;

                    produto.quantidade -= quantidadeAdicionar;
                    _produtoRepository.AtualizarProduto(produto);

                    RegistrarMovimentacao(idProduto, quantidadeAdicionar, produto.preco_venda * quantidadeAdicionar,
                        $"Saída de estoque por inclusão na OS #{os.id_os}", "Saída");
                }

                RecalcularEAtualizarValorPecas(os.id_os);

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

            OrdemServico ordemServico = ObterPorId(item.id_OS.GetValueOrDefault());
            if (ordemServico == null)
                throw new InvalidOperationException("OS não encontrada.");

            if (ordemServico.status != "ABERTA")
                throw new InvalidOperationException("Ordem de Serviço não está aberta para alterações");

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

                RecalcularEAtualizarValorPecas(ordemServico.id_os);

                scope.Complete();
                return true;
            }
        }

        public bool RemoverItemOS(int idItem)
        {
            var item = _itemOSRepository.ObterPorId(idItem);
            if (item == null)
                throw new InvalidOperationException("Item da OS não encontrado.");

            return ReduzirOuRemoverItemOS(idItem, item.Quantidade);
        }

        private bool RemoverItemDirect(int idItem)
        {
            return _itemOSRepository.Remover(idItem);
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

        #endregion

        #region Serviços e Mão de Obra

        public bool SalvarServicoOS(ServicosOS servico)
        {
            ValidarServicoOS(servico);

            var os = _ordemServicoRepository.ObterPorId(servico.id_OS.GetValueOrDefault());
            if (os == null)
                throw new InvalidOperationException("A Ordem de Serviço informada não foi encontrada.");

            if (os.status != "ABERTA")
            {
                throw new InvalidOperationException("Ordem de Serviço não está aberta para alterações.");
            }

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

            var os = _ordemServicoRepository.ObterPorId(acao.id_OS.GetValueOrDefault());
            int idOS = acao.id_OS.GetValueOrDefault();

            if (os.status != "ABERTA")
            {
                throw new InvalidOperationException("Ordem de Serviço não está aberta para alterações");
            }

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

        #endregion

        #region Histórico de Alterações

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

        #region Relatórios e DataTables

        public DataTable ObterHistoricoOsTecnico(int id)
        {
            return _ordemServicoRepository.ObterHistoricoUsuario(id);
        }

        public DataTable OrdensRecentes()
        {
            return _ordemServicoRepository.OrdensRecentes();
        }

        #endregion

        #region Fluxo da OS e Alterações de Estado

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
        public bool DefinirParaRetirada(int idOS, int idUsuario)
        {
            if (idOS <= 0)
                throw new ArgumentException("Ordem de Serviço inválida.");

            var os = _ordemServicoRepository.ObterPorId(idOS);
            if (os == null)
            {
                throw new ArgumentException("Ordem de Serviço não encontrada.");
            }

            if (os.status == "CANCELADA")
            {
                throw new InvalidOperationException("Não é possível alterar o status de uma Ordem de Serviço cancelada por aqui.");
            }

            if (os.status == "AGUARDANDO_RETIRADA")
            {
                throw new InvalidOperationException("A Ordem de Serviço já está aguardando retirada.");
            }

            os.status = "AGUARDANDO_RETIRADA";
            os.data_atualizacao = DateTime.Now;

            bool sucesso = _ordemServicoRepository.SalvarAlteracoesOS(os);

            if (sucesso)
            {
                var historico = new HistoricoAlteracaoOS
                {
                    idOS = idOS,
                    idUsuario = idUsuario,
                    tipo = "ALTERACAO_STATUS",
                    descricao = $"Status da Ordem de Serviço #{idOS} alterado para AGUARDANDO_RETIRADA.",
                    dataAlteracao = DateTime.Now
                };

                _historicoAlteracaoOSRepository.RegistrarHistorico(historico);
            }

            return sucesso;
        }

        public bool CancelarOrdemServico(int idOS, int idUsuario)
        {
            if (idOS <= 0)
                throw new ArgumentException("Ordem de Serviço inválida.");

            var os = _ordemServicoRepository.ObterPorId(idOS);
            if (os.status == "CANCELADA")
            {
                throw new ArgumentException("A Ordem de Serviço já foi cancelada.");
            }

            bool sucesso = _ordemServicoRepository.CancelarOrdemServico(idOS);

            if (sucesso)
            {
                var historico = new HistoricoAlteracaoOS
                {
                    idOS = idOS,
                    idUsuario = idUsuario,
                    tipo = "CANCELAMENTO_OS",
                    descricao = $"Ordem de Serviço #{idOS} foi cancelada.",
                    dataAlteracao = DateTime.Now
                };

                _historicoAlteracaoOSRepository.RegistrarHistorico(historico);
            }

            return sucesso;
        }

        public bool ReabrirOrdemServico(int idOS, int idUsuario)
        {
            if (idOS <= 0)
                throw new ArgumentException("Ordem de Serviço inválida.");

            var os = _ordemServicoRepository.ObterPorId(idOS);
            if (os == null)
            {
                throw new ArgumentException("Ordem de Serviço não encontrada.");
            }

            if (os.status != "CANCELADA" && os.status != "AGUARDANDO_RETIRADA")
            {
                throw new InvalidOperationException("Apenas Ordens de Serviço canceladas ou aguardando retirada podem ser reabertas.");
            }

            bool sucesso = false;

            if (os.status == "CANCELADA")
            {
                sucesso = _ordemServicoRepository.ReabrirOrdemServico(idOS);
            }
            else if (os.status == "AGUARDANDO_RETIRADA")
            {
                os.status = "ABERTA";
                os.data_atualizacao = DateTime.Now;
                sucesso = _ordemServicoRepository.SalvarAlteracoesOS(os);
            }

            if (sucesso)
            {
                var historico = new HistoricoAlteracaoOS
                {
                    idOS = idOS,
                    idUsuario = idUsuario,
                    tipo = "REABERTURA_OS",
                    descricao = $"Ordem de Serviço #{idOS} foi reaberta.",
                    dataAlteracao = DateTime.Now
                };

                _historicoAlteracaoOSRepository.RegistrarHistorico(historico);
            }

            return sucesso;
        }

        #endregion

        #region Pagamento

        public DataTable CarregarFormasPagamento(bool incluirOpcaoTodas = false)
        {
            var dt = _pagamentoRepository.carregarFormasPamento();

            if (incluirOpcaoTodas)
            {
                DataRow dr = dt.NewRow();
                dr["id_forma_pagamento"] = 0;
                dr["exibicao"] = "Todas as formas de pagamento";
                dt.Rows.InsertAt(dr, 0);
            }
            return dt;
        }
        
        public bool RegistrarPagamento(int idOS, int idUsuario, int formaPagamento)
        {
            try
            {
                if (idOS <= 0)
                    throw new ArgumentException("Ordem de Serviço inválida.");

                var os = _ordemServicoRepository.ObterPorId(idOS);
                if (os == null)
                    throw new ArgumentException("Ordem de Serviço não encontrada.");

                if (os.status == "CANCELADA")
                    throw new InvalidOperationException("Não é possível registrar pagamento para uma OS cancelada.");

                if (os.status == "FINALIZADA")
                    throw new InvalidOperationException("Esta Ordem de Serviço já foi finalizada.");

                

                os.status = "FINALIZADA";
                os.data_atualizacao = DateTime.Now;
                os.data_fechamento = DateTime.Now;

                var contaReceber = new ContasReceber
                {
                    id_os_fk = idOS,
                    descricao = $"Recebimento referente à OS #{idOS}",
                    valor = os.valor_total,
                    data_vencimento = DateTime.Now.Date,
                    data_emissao = DateTime.Now.Date,
                    data_pagamento = DateTime.Now.Date,
                    status = "PAGA",
                    id_forma_pagamento_fk = formaPagamento,
                    observacoes = ""
                };

                bool contaSalva = _contaReceberRepository.Inserir(contaReceber);
                if (!contaSalva) return false;

                bool osAtualizada = _ordemServicoRepository.SalvarAlteracoesOS(os);

                if (osAtualizada)
                {
                    var historico = new HistoricoAlteracaoOS
                    {
                        idOS = idOS,
                        idUsuario = idUsuario,
                        tipo = "PAGAMENTO_REGISTRADO",
                        descricao = $"Pagamento de {contaReceber.valor:C2} registrado e conta baixada em Contas a Receber. OS #{idOS} finalizada.",
                        dataAlteracao = DateTime.Now
                        
                    };

                    _historicoAlteracaoOSRepository.RegistrarHistorico(historico);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao registrar pagamento: " + ex.Message, ex);
            }
        }

        #endregion

        #region RelatórioPDF
        public string GerarRelatorioGeralHistoricoPdf(
            DataTable dtDados, RelatorioTotaisDTO totais, string dataInicio, string dataFim, string status, string caminhoDestino, string caminhoLogo = null)
        {
            string filtroPeriodo = "Geral";
            if (!string.IsNullOrWhiteSpace(dataInicio) && !string.IsNullOrWhiteSpace(dataFim))
                filtroPeriodo = $"{dataInicio} até {dataFim}";
            else if (!string.IsNullOrWhiteSpace(dataInicio))
                filtroPeriodo = $"A partir de {dataInicio}";
            else if (!string.IsNullOrWhiteSpace(dataFim))
                filtroPeriodo = $"Até {dataFim}";

            totais.FiltroPeriodo = filtroPeriodo;
            totais.FiltroStatus = string.IsNullOrWhiteSpace(status) ? "Todos" : status;

            string diretorio = Path.GetDirectoryName(caminhoDestino);
            if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

            return GeradorPdfOS.GerarRelatorioGeral(dtDados, totais, caminhoDestino, caminhoLogo);
        }


        public OrdemServicoRelatorioDTO ImprimirOS(int idOS)
        {
            if (idOS <= 0)
                throw new ArgumentException("Identificador da Ordem de Serviço inválido.");

            using (var context = new AppDbContext())
            {
                var os = context.OrdemServicos
                    .Include(o => o.Cliente)
                    .Include(o => o.Equipamento)
                    .FirstOrDefault(o => o.id_os == idOS);

                if (os == null)
                    throw new InvalidOperationException($"Ordem de Serviço #{idOS} não encontrada.");

                var conta = _contaReceberRepository.ObterPorOSId(idOS);
                var itens = ObterItensPorOSId(idOS) ?? new List<ItemOSRelatorioDTO>();
                var servicos = ObterServicosPorOSId(idOS);

                if (servicos != null)
                {
                    foreach (var s in servicos)
                    {
                        itens.Add(new ItemOSRelatorioDTO
                        {
                            Descricao = s.Descricao,
                            Quantidade = 1,
                            ValorUnitario = s.ValorCobrado,
                            ValorTotal = s.ValorCobrado,
                            Tipo = "Serviço"
                        });
                    }
                }

                string formaPagamentoTexto = "Não registrado";
                if (conta != null && conta.Pagamento != null)
                {
                    formaPagamentoTexto = conta.Pagamento.Descricao;
                }

                var relatorio = new OrdemServicoRelatorioDTO
                {
                    IdOS = os.id_os,
                    DataAbertura = os.data_abertura,
                    DataAtualizacao = os.data_atualizacao,
                    Status = os.status,

                    NomeCliente = os.Cliente != null ? os.Cliente.Nome : "Cliente não informado",
                    DocumentoCliente = os.Cliente != null ? os.Cliente.Cpf : "-",
                    TelefoneCliente = os.Cliente != null ? os.Cliente.Telefone : "-",
                    EnderecoCliente = os.Cliente != null ? $"{os.Cliente.Rua}, {os.Cliente.Numero} - {os.Cliente.Bairro}" : "-",

                    Equipamento = os.Equipamento != null ? os.Equipamento.Descricao : "Não informado",
                    MarcaModelo = os.Equipamento != null ? $"{os.Equipamento.Marca} {os.Equipamento.Modelo}".Trim() : "-",
                    NumeroSerie = os.Equipamento != null ? os.Equipamento.Numero_Serie : "-",
                    DefeitoRelatado = os.problema_relatado,
                    LaudoTecnico = os.diagnostico,

                    ValorPecas = os.valor_pecas,
                    ValorMaoObra = os.valor_mao_obra,
                    ValorTotal = os.valor_total,
                    FormaPagamento = formaPagamentoTexto,

                    Itens = itens
                };

                return relatorio;
            }
        }

        public string ExportarReciboPdf(int idOS, string caminhoDestino, string caminhoLogo = null)
        {
            var dadosRelatorio = ImprimirOS(idOS);

            string diretorio = Path.GetDirectoryName(caminhoDestino);
            if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

            return GeradorPdfOS.ImprimirOS(dadosRelatorio, caminhoDestino, caminhoLogo);
        }

        public string GerarRelatorioGeralPdf(DataTable dtDados, DateTime? dataInicio, DateTime? dataFim, string status, string caminhoDestino, string caminhoLogo = null)
        {
            RelatorioTotaisDTO totais = CalcularTotaisEInformacoes(dtDados, dataInicio, dataFim, status);

            string diretorio = Path.GetDirectoryName(caminhoDestino);
            if (!string.IsNullOrEmpty(diretorio) && !Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }

            return GeradorPdfOS.GerarRelatorioGeral(dtDados, totais, caminhoDestino, caminhoLogo);
        }
        private string GetValorColuna(DataRow row, params string[] nomesColunas)
        {
            foreach (string nome in nomesColunas)
            {
                if (row.Table.Columns.Contains(nome) && row[nome] != DBNull.Value)
                {
                    return row[nome].ToString();
                }
            }
            return "";
        }

        private RelatorioTotaisDTO CalcularTotaisEInformacoes(DataTable dtDados, DateTime? dataInicio, DateTime? dataFim, string status)
        {
            RelatorioTotaisDTO totais = new RelatorioTotaisDTO();

            string filtroPeriodo = "Geral";
            if (dataInicio.HasValue && dataFim.HasValue)
            {
                filtroPeriodo = $"{dataInicio.Value:dd/MM/yyyy} até {dataFim.Value:dd/MM/yyyy}";
            }
            else if (dataInicio.HasValue)
            {
                filtroPeriodo = $"A partir de {dataInicio.Value:dd/MM/yyyy}";
            }
            else if (dataFim.HasValue)
            {
                filtroPeriodo = $"Até {dataFim.Value:dd/MM/yyyy}";
            }

            totais.FiltroPeriodo = filtroPeriodo;
            totais.FiltroStatus = string.IsNullOrWhiteSpace(status) ? "Todos" : status;

            if (dtDados == null || dtDados.Rows.Count == 0)
            {
                return totais;
            }

            totais.TotalOS = dtDados.Rows.Count;

            foreach (DataRow row in dtDados.Rows)
            {
                string statusRow = GetValorColuna(row, "status", "Status");
                
                decimal valorTotal = 0m;
                string valorStr = GetValorColuna(row, "valor_total", "Valor Total", "valorTotal", "total");
                
                if (!string.IsNullOrWhiteSpace(valorStr))
                {
                    decimal.TryParse(valorStr.Replace("R$", "").Trim(), out valorTotal);
                }

                string formaPagamento = GetValorColuna(row, "forma_pagamento", "Pagamento", "formaPagamento");

                if (string.Equals(statusRow, "ABERTA", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(statusRow, "AGUARDANDO_RETIRADA", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(statusRow, "Aberto", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(statusRow, "Em Andamento", StringComparison.OrdinalIgnoreCase))
                {
                    totais.EmAtendimento++;
                }

                if (string.Equals(statusRow, "Concluído", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(statusRow, "FINALIZADA", StringComparison.OrdinalIgnoreCase) ||
                    !string.IsNullOrWhiteSpace(formaPagamento) && formaPagamento != "-")
                {
                    totais.TotalRecebido += valorTotal;
                    totais.QntRecebido++;
                }
                else if (!string.Equals(statusRow, "CANCELADA", StringComparison.OrdinalIgnoreCase) && 
                         !string.Equals(statusRow, "Cancelado", StringComparison.OrdinalIgnoreCase))
                {
                    totais.TotalAReceber += valorTotal;
                }
            }

            return totais;
        }
        
        

        #endregion

        #region Filtro

        public DataTable FiltrarHistorico(int? idCliente, int? idTecnico, string dataInicio, string dataFim, string busca, string status)
        {
            return _ordemServicoRepository.FiltrarHistorico(idCliente, idTecnico, dataInicio, dataFim, busca, status);
        }
        
        
        public (DataTable Dados, int TotalOS, int EmAtendimento, int ParaRetirada, decimal TotalAReceber, decimal TotalRecebido, int QntRecebido, decimal TotalCancelado, int QntCancelado) ObterDadosAtuais()
        {
            var filtro = new OrdemServico();
            var dados = _ordemServicoRepository.ObterTodasOSAtuais();
            var totais = _ordemServicoRepository.ObterTotais(filtro);

            return (dados, totais.TotalOS, totais.EmAtendimento, totais.ParaRetirada, totais.TotalAReceber, totais.TotalRecebido, totais.QntRecebido, totais.TotalCancelado, totais.QntCancelado);
        }

        public (DataTable Dados, int TotalOS, int EmAtendimento, int ParaRetirada, decimal TotalAReceber, decimal TotalRecebido, int QntRecebido, decimal TotalCancelado, int QntCancelado) Filtrar(
            string dataInicio, string dataFim, string busca, int statusIndex, string statusText)
        {
            var filtro = new OrdemServico
            {
                filtroDataInicio = ValidarData(dataInicio) ? dataInicio : null,
                filtroDataConclusao = ValidarData(dataFim) ? dataFim : null,
                filtroBusca = busca?.Trim(),
                filtroStatus = statusIndex > 0 ? statusText : null
            };

            var dados = _ordemServicoRepository.Filtrar(filtro);
            var totais = _ordemServicoRepository.ObterTotais(filtro);

            return (dados, totais.TotalOS, totais.EmAtendimento, totais.ParaRetirada, totais.TotalAReceber, totais.TotalRecebido, totais.QntRecebido, totais.TotalCancelado, totais.QntCancelado);
        }
        

        

        #endregion

        #region Validações Internas
        private bool ValidarData(string data)
        {
            if (string.IsNullOrWhiteSpace(data)) return false;
            return DateTime.TryParseExact(data.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
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

        #endregion
    }
}
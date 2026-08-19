using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using AssisTEC.DTO;
using AssisTec.Models;
using AssisTec.Repository;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace AssisTec.Service
{
    public class ClienteService
    {
        private readonly IClienteRepository repository;
        private readonly IOrdemServicoRepository ordemServicoRepository;

        public ClienteService(IClienteRepository _repository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }
        public ClienteService(IClienteRepository _repository,  IOrdemServicoRepository _ordemServicoRepository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
            this.ordemServicoRepository  = _ordemServicoRepository ?? throw new ArgumentNullException(nameof(_ordemServicoRepository));
        }

        #region Consulta

        public List<Cliente> ObterTodos()
        {
            return repository.ObterTodosClientes();
        }
        

        public Cliente ObterPorId(int id)
        {
            if (id < 0) return null;
            return repository.ObterPorId(id);
        }
        
        public DataTable ObterHistoricoOS(int id)
        {
            return ordemServicoRepository.ObterHistoricoCliente(id);
        }


        #endregion

        #region Gerenciamento

        public (bool sucesso, string mensagem) CadastrarCliente(Cliente cliente)
        {
            if (cliente == null)
                return (false, "Dados do cliente inválidos.");

            if (string.IsNullOrWhiteSpace(cliente.Nome) || string.IsNullOrWhiteSpace(cliente.Cpf))
            {
                return (false, "Campos obrigatórios não preenchidos.");
            }
                

            if (!Validacao.ValidarCPF(cliente.Cpf))
            {
                return (false, "CPF inválido");
            }

            if (!Validacao.ValidarTelefone(cliente.Telefone))
            {
                return (false, "Telefone inválido");
            }
            
            if (!cliente.DataNascimento.HasValue)
            {
                return (false, "Data de nascimento é obrigatório");
            }
            
            var (dataValida, mensagemData) = Validacao.ValidarDataNascimento(cliente.DataNascimento.Value);
            if (!dataValida)
            {
                return (false, mensagemData);
            }
            
            if (repository.CpfExiste(cliente.Cpf))
            {
                return (false, "O CPF informado já está cadastrado no sistema.");
            }

            bool inserirCliente = repository.InserirCliente(cliente);
            if (inserirCliente)
            {
                return (true, "Cliente cadastrado com sucesso!");
            }

            return (false, "Erro interno ao tentar salvar o cliente.");
        }

        public (bool sucesso, string mensagem) EditarCliente(Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nome) || string.IsNullOrWhiteSpace(cliente.Cpf))
            {
                return (false, "Campos obrigatórios não preenchidos.");
            }
                

            if (!Validacao.ValidarCPF(cliente.Cpf))
            {
                return (false, "CPF inválido");
            }

            if (!Validacao.ValidarTelefone(cliente.Telefone))
            {
                return (false, "Telefone inválido");
            }
            
            if (!cliente.DataNascimento.HasValue)
            {
                return (false, "Data de nascimento é obrigatório");
            }
            
            var (dataValida, mensagemData) = Validacao.ValidarDataNascimento(cliente.DataNascimento.Value);
            if (!dataValida)
            {
                return (false, mensagemData);
            }
            
            if (repository.CpfExiste(cliente.Cpf) && cliente.Cpf != cliente.Cpf)
            {
                return (false, "O CPF informado já está cadastrado no sistema.");
            }

            bool atualizou = repository.AtualizarCliente(cliente);
            if (atualizou)
            {
                return (true, "Cliente atualizado com sucesso!");
            }

            return (false, "Erro interno ao tentar atualizar o cliente.");
        }
        
        public bool AlterarStatus(int id)
        {
            var validacao = ValidarAntesDeDesativarCliente(id);
            if (!validacao.sucesso)
            {
                throw new ArgumentException(validacao.mensagem);
            }

            try
            {
                return repository.AlterarStatus(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao alterar o status do cliente.", ex);
            }
        }

        #endregion

        #region Filtro

        public List<Cliente> ObterComFiltros(string busca, bool ApenasInativos)
        {
            try
            {
                return repository.ObterComFiltros(busca, ApenasInativos);
            }
            catch (Exception e)
            {
                throw new Exception("Erro ao obter os clientes.", e);
            }
        }

        #endregion

        #region Relatorio

        public void GerarRelatorioClientesPdf(string nome, bool exibirDesativados, string caminhoDestino)
    {
        try
        {
            List<Cliente> clientesFiltrados = repository.ObterComFiltros(nome, exibirDesativados);

            ClienteDTO.ClientesRelatorioDTO relatorio = new ClienteDTO.ClientesRelatorioDTO
            {
                FiltroNome = string.IsNullOrEmpty(nome) ? "Todos" : nome,
                FiltroStatus = exibirDesativados ? "Todos (Ativados/Desativados)" : "Apenas Ativados",
                TotalAtivos = 0,
                TotalInativos = 0,
                TotalGeral = 0,
                Itens = new List<ClienteDTO.ClienteRelatorioDTO>()
            };

            if (clientesFiltrados != null)
            {
                foreach (var cliente in clientesFiltrados)
                {
                    string statusAtual = cliente.Status ?? string.Empty;
                    bool inativo = statusAtual.Equals("Desativado", StringComparison.OrdinalIgnoreCase) ||
                                   statusAtual.Equals("Inativo", StringComparison.OrdinalIgnoreCase);

                    if (inativo)
                    {
                        relatorio.TotalInativos++;
                    }
                    else
                    {
                        relatorio.TotalAtivos++;
                    }
                    relatorio.TotalGeral++;

                    relatorio.Itens.Add(new ClienteDTO.ClienteRelatorioDTO
                    {
                        Id = cliente.Id,
                        Nome = cliente.Nome ?? string.Empty,
                        Cpf = cliente.Cpf ?? string.Empty,
                        Telefone = cliente.Telefone ?? string.Empty,
                        Cidade = cliente.Cidade ?? string.Empty,
                        Estado = cliente.Estado ?? string.Empty,
                        Status = statusAtual
                    });
                }
            }

            GeradorPdfCliente.GerarRelatorioGeral(relatorio, caminhoDestino);
        }
        catch (Exception ex)
        {
            string mensagemDetalhada = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
            throw new Exception("Falha ao processar o relatório de clientes: " + mensagemDetalhada, ex);
        }
    }

        public void GerarRelatorioIndividualClientePdf(int idCliente, string caminhoDestino)
        {
            try
            {
                Cliente cliente = repository.ObterPorId(idCliente);
                if (cliente == null)
                {
                    throw new Exception("Cliente não encontrado para a geração do relatório.");
                }

                DataTable tabelaOS = ordemServicoRepository.ObterHistoricoCliente(idCliente);

                ClienteDTO.ClienteComOrdemServicoDTO relatorio = new ClienteDTO.ClienteComOrdemServicoDTO
                {
                    IdCliente = cliente.Id,
                    Nome = cliente.Nome ?? string.Empty,
                    Cpf = cliente.Cpf ?? string.Empty,
                    Telefone = cliente.Telefone ?? string.Empty,
                    StatusCliente = cliente.Status ?? string.Empty,
                    TotalOrdens = 0,
                    OrdensAbertas = 0,
                    OrdensFinalizadas = 0,
                    TotalGasto = 0m,
                    Ordens = new List<ClienteDTO.OrdemServicoItemDTO>()
                };

                if (tabelaOS != null && tabelaOS.Rows.Count > 0)
                {
                    foreach (DataRow row in tabelaOS.Rows)
                    {
                        string statusOS = ObterValorColuna(row, tabelaOS, "STATUS", "ABERTA");
                        string tecnico = ObterValorColuna(row, tabelaOS, "TECNICO", ObterValorColuna(row, tabelaOS, "NOME_TECNICO", "Não Atribuído"));
                        string equipamento = ObterValorColuna(row, tabelaOS, "EQUIPAMENTO", ObterValorColuna(row, tabelaOS, "DISPOSITIVO", "Sem Equipamento"));

                        decimal valor = 0m;
                        string colValor = tabelaOS.Columns.Contains("VALOR_TOTAL") ? "VALOR_TOTAL" : (tabelaOS.Columns.Contains("VALOR") ? "VALOR" : null);
                        if (colValor != null && row[colValor] != DBNull.Value)
                        {
                            valor = Convert.ToDecimal(row[colValor]);
                        }

                        DateTime? dataFim = null;
                        string colDataFim = tabelaOS.Columns.Contains("DATA_FECHAMENTO") ? "DATA_FECHAMENTO" : (tabelaOS.Columns.Contains("DATA_FINALIZACAO") ? "DATA_FINALIZACAO" : null);
                        if (colDataFim != null && row[colDataFim] != DBNull.Value)
                        {
                            dataFim = Convert.ToDateTime(row[colDataFim]);
                        }

                        DateTime dataAbertura = DateTime.Now;
                        string colDataIni = tabelaOS.Columns.Contains("DATA_ABERTURA") ? "DATA_ABERTURA" : (tabelaOS.Columns.Contains("DATA") ? "DATA" : null);
                        if (colDataIni != null && row[colDataIni] != DBNull.Value)
                        {
                            dataAbertura = Convert.ToDateTime(row[colDataIni]);
                        }

                        int idOS = 0;
                        string colIdOS = tabelaOS.Columns.Contains("ID_ORDEM") ? "ID_ORDEM" : (tabelaOS.Columns.Contains("ID") ? "ID" : null);
                        if (colIdOS != null && row[colIdOS] != DBNull.Value)
                        {
                            idOS = Convert.ToInt32(row[colIdOS]);
                        }

                        relatorio.TotalOrdens++;

                        if (statusOS.Equals("ABERTA", StringComparison.OrdinalIgnoreCase) ||
                            statusOS.Equals("Aberto", StringComparison.OrdinalIgnoreCase) ||
                            statusOS.Equals("Em Andamento", StringComparison.OrdinalIgnoreCase))
                        {
                            relatorio.OrdensAbertas++;
                        }
                        else if (statusOS.Equals("Finalizado", StringComparison.OrdinalIgnoreCase) ||
                                 statusOS.Equals("Entregue", StringComparison.OrdinalIgnoreCase) ||
                                 statusOS.Equals("FINALIZADA", StringComparison.OrdinalIgnoreCase))
                        {
                            relatorio.OrdensFinalizadas++;
                            relatorio.TotalGasto += valor;
                        }

                        relatorio.Ordens.Add(new ClienteDTO.OrdemServicoItemDTO
                        {
                            IdOrdemServico = idOS,
                            Tecnico = tecnico,
                            Equipamento = equipamento,
                            DataAbertura = dataAbertura,
                            DataFechamento = dataFim,
                            ValorTotal = valor,
                            Status = statusOS
                        });
                    }
                }

                GeradorPdfCliente.GerarRelatorioIndividual(relatorio, caminhoDestino);
            }
            catch (Exception ex)
            {
                string mensagemDetalhada = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception("Erro ao processar o relatório individual do cliente: " + mensagemDetalhada, ex);
            }
        }

        #endregion

        #region Outras Funcoes

        public (bool sucesso, string mensagem, string rua, string bairro, string cidade, string estado) ConsultarCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep))
            {
                return (false, "O CEP não pode estar vazio.", null, null, null, null);
            }

            string cepLimpo = cep.Replace("-", "").Trim();
            if (cepLimpo.Length != 8)
            {
                return (false, "Formato de CEP inválido. Certifique-se de que possui 8 dígitos.", null, null, null, null);
            }

            try
            {
                BuscaCEP buscaCep = new BuscaCEP();
                buscaCep.Cep = cepLimpo;
                buscaCep.Consultar();

                if (string.IsNullOrWhiteSpace(buscaCep.Cidade) || 
                    string.IsNullOrWhiteSpace(buscaCep.Rua) || 
                    string.IsNullOrWhiteSpace(buscaCep.Bairro))
                {
                    return (false, "Falha ao localizar as informações do CEP informado.", null, null, null, null);
                }

                return (true, "CEP localizado com sucesso!", buscaCep.Rua, buscaCep.Bairro, buscaCep.Cidade, buscaCep.Estado);
            }
            catch (Exception ex)
            {
                return (false, $"Erro ao consultar o CEP: {ex.Message}", null, null, null, null);
            }
        }

        
        public (bool sucesso, string mensagem) ValidarAntesDeDesativarCliente(int idCliente)
        {
            if (idCliente <= 0)
            {
                return (false, "Selecione um cliente válido.");
            }

            bool possuiOsAberta = ordemServicoRepository.ExisteOSAbertaPorCliente(idCliente);
            if (possuiOsAberta)
            {
                return (false, "Não é possível alterar o status do cliente pois ele possui Ordens de Serviço em ABERTA.");
            }

            return (true, string.Empty);
        }

        #endregion
       
        

        
        

        

        
        
        

        private string ObterValorColuna(DataRow row, DataTable table, string nomeColuna, string valorPadrao)
        {
            if (table.Columns.Contains(nomeColuna) && row[nomeColuna] != DBNull.Value)
            {
                return row[nomeColuna].ToString();
            }
            return valorPadrao;
        }

        public DataTable ObterHistoricoOsCliente(int id)
        {
            return ordemServicoRepository.ObterHistoricoCliente(id);
        }
    }
}
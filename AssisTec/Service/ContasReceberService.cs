using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Windows.Forms;
using AssisTec.Models;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class ContasReceberService
    {
        private IContaReceberRepository _repository;
        private IPagamentoRepository _pagamentoRepository;

        public ContasReceberService()
        {
            CriarNovoContexto();
        }

        public void CriarNovoContexto()
        {
            var context = new AppDbContext();
            _repository = new ContasReceberRepository(context);
            _pagamentoRepository = new PagamentoRepository(context);
        }

        public void AlterarParaAtrasado()
        {
            try
            {
                var contas = _repository.ObterTodasContasReceber();
                var dataAtual = DateTime.Today;
                foreach (var conta in contas)
                {
                    if (conta.status == "PENDENTE" && conta.data_vencimento.Date < dataAtual)
                    {
                        _repository.AtualizarParaAtrasado(conta.id_conta_receber);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Falha ao processar atualização de contas atrasadas.", ex);
            }
        }

        public void DeletarContasReceber(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido para exclusão.");
            try
            {
                bool excluiu = _repository.ExcluirContasReceber(id);
                if (!excluiu)
                {
                    throw new Exception("A conta informada não foi localizada para exclusão.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Falha na camada de negócio ao excluir conta a receber.", ex);
            }
        }

        public DataTable CarregarFormasPagamentoComPadrao()
        {
            try
            {
                DataTable dtFormaPagamento = _pagamentoRepository.carregarFormasPamento();
                DataRow dr = dtFormaPagamento.NewRow();
                dr["id_forma_pagamento"] = 0;
                dr["exibicao"] = "Todas as formas de pagamento";
                dtFormaPagamento.Rows.InsertAt(dr, 0);
                return dtFormaPagamento;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao preparar formas de pagamento.", ex);
            }
        }

        public DataTable CarregarFormasPagamentoApenasValidas()
        {
            try
            {
                return _pagamentoRepository.carregarFormasPamento();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar formas de pagamento.", ex);
            }
        }

        public List<ContasReceber> CarregarTodasContas()
        {
            try
            {
                AlterarParaAtrasado();
                return _repository.ObterTodasContasReceber();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao carregar dados das contas: " + ex.Message, ex);
            }
        }
        
        public ContasReceber ObterContaPorId(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID fornecido para busca é inválido.");

            try
            {
                var conta = _repository.ObterPorId(id);
                if (conta == null)
                {
                    throw new Exception("Conta a receber não foi localizada no banco de dados.");
                }
        
                return conta;
            }
            catch (Exception ex)
            {
                throw new Exception("Falha na camada de negócio ao obter dados da conta: " + ex.Message, ex);
            }
        }
        
        public void SalvarContaReceber(ContasReceber conta, int modo)
        {
            ValidarCamposObrigatorios(conta);

            try
            {
                if (modo == 1)
                {
                    InserirNovaConta(conta);
                }
                else if (modo == 2)
                {
                    AtualizarContaExistente(conta);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao persistir os dados da conta: " + ex.Message, ex);
            }
        }

        private void InserirNovaConta(ContasReceber conta)
        {
            bool inseriu = _repository.InserirContasReceber(conta);
            if (!inseriu)
            {
                throw new Exception("Não foi possível inserir a conta a receber no banco de dados.");
            }
        }

        private void AtualizarContaExistente(ContasReceber conta)
        {
            if (conta.id_conta_receber <= 0)
                throw new ArgumentException("ID inválido para atualização.");

            bool atualizou = _repository.AtualizarContasReceber(conta);
            if (!atualizou)
            {
                throw new Exception("A conta a receber não foi localizada ou não pôde ser updated.");
            }
        }

        private void ValidarCamposObrigatorios(ContasReceber conta)
        {
            if (string.IsNullOrWhiteSpace(conta.descricao))
                throw new ArgumentException("A descrição é obrigatória.");

            if (conta.valor <= 0)
                throw new ArgumentException("O valor deve ser maior que zero.");

            if (string.IsNullOrWhiteSpace(conta.status))
                throw new ArgumentException("O status é obrigatório.");

            if (conta.data_emissao == DateTime.MinValue)
                throw new ArgumentException("A data de emissão é inválida.");

            if (conta.data_vencimento == DateTime.MinValue)
                throw new ArgumentException("A data de vencimento é inválida.");
        }

        public (DataTable dados, decimal totalGeral, decimal totalRecebido, decimal totalPendente, decimal totalAtrasado) FiltrarContas(
            string dataInicioTxt, 
            string dataFimTxt, 
            string buscaTxt, 
            int comboSelectedIndex, 
            string comboSelectedItem, 
            object comboSelectedValue)
        {
            bool temDataInicio = dataInicioTxt.Replace("/", "").Replace("_", "").Trim().Length > 0;
            bool temDataFim = dataFimTxt.Replace("/", "").Replace("_", "").Trim().Length > 0;
            if (temDataInicio && !DateTime.TryParseExact(dataInicioTxt, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                throw new ArgumentException("Data de início inválida!");
            }
            if (temDataFim && !DateTime.TryParseExact(dataFimTxt, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                throw new ArgumentException("Data fim inválida!");
            }
            int idForma = 0;
            int.TryParse(comboSelectedValue?.ToString(), out idForma);
            ContasReceber filtroModel = new ContasReceber
            {
                filtroDataInicio = temDataInicio ? dataInicioTxt : null,
                filtroDataFim = temDataFim ? dataFimTxt : null,
                filtroDescricao = buscaTxt.Trim(),
                filtroStatus = comboSelectedIndex > 0 ? comboSelectedItem : null,
                filtroIdFormaPagamento = idForma
            };
            DataTable dadosFiltrados = _repository.FiltrarContasReceber(filtroModel);
            var totais = _repository.AtualizarTotais(filtroModel);
            return (dadosFiltrados, totais.totalGeral, totais.totalRecebido, totais.totalPendente, totais.totalAtrasado);
        }

        public (decimal totalGeral, decimal totalRecebido, decimal totalPendente, decimal totalAtrasado) ObterTotaisPadrao()
        {
            try
            {
                ContasReceber filtroVazio = new ContasReceber();
                var totais = _repository.AtualizarTotais(filtroVazio);
                return (totais.totalGeral, totais.totalRecebido, totais.totalPendente, totais.totalAtrasado);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao processar totais gerais: " + ex.Message, ex);
            }
        }

        public void ValidarRegistoPagamento(DataGridViewRow currentRow)
        {
            if (currentRow == null)
                throw new InvalidOperationException("Nenhuma conta selecionada.");
            if (currentRow.Cells["Status"].Value?.ToString() == "PAGA")
            {
                throw new InvalidOperationException("Registro de Pagamento apenas para contas não pagas");
            }
        }
    }
}
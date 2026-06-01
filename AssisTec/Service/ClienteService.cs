using System;
using System.Collections.Generic;
using AssisTec.Models;
using AssisTec.Repository;

namespace AssisTec.Service
{
    public class ClienteService
    {
        private readonly IClienteRepository repository;

        public ClienteService(IClienteRepository _repository)
        {
            this.repository = _repository ?? throw new ArgumentNullException(nameof(_repository));
        }

        public List<Cliente> ObterTodos()
        {
            return repository.ObterTodosClientes();
        }

        public List<Cliente> FiltrarClientes(string busca)
        {
            return repository.ObterComFiltros(busca);
        }

        public Cliente ObterPorId(int id)
        {
            if (id <= 0) return null;
            return repository.ObterPorId(id);
        }

        public (bool sucesso, string mensagem) CadastrarCliente(Cliente cliente)
        {
            if (cliente == null)
                return (false, "Dados do cliente inválidos.");

            if (string.IsNullOrWhiteSpace(cliente.Nome) || string.IsNullOrWhiteSpace(cliente.Cpf))
                return (false, "Campos obrigatórios não preenchidos.");

            string cpfLimpo = cliente.Cpf.Replace(".", "").Replace("-", "").Trim();
            if (repository.CpfExiste(cpfLimpo))
            {
                return (false, "O CPF informado já está cadastrado no sistema.");
            }

            cliente.Cpf = cpfLimpo;

            bool inseriu = repository.InserirCliente(cliente);
            if (inseriu)
            {
                return (true, "Cliente cadastrado com sucesso!");
            }

            return (false, "Erro interno ao tentar salvar o cliente.");
        }

        public (bool sucesso, string mensagem) EditarCliente(Cliente cliente)
        {
            if (cliente == null || cliente.Id <= 0)
                return (false, "Dados do cliente inválidos para edição.");

            if (string.IsNullOrWhiteSpace(cliente.Nome))
                return (false, "O nome do cliente não pode ficar vazio.");

            cliente.Cpf = cliente.Cpf.Replace(".", "").Replace("-", "").Trim();

            bool atualizou = repository.CorrigirAtualizarCliente(cliente);
            if (atualizou)
            {
                return (true, "Cliente atualizado com sucesso!");
            }

            return (false, "Erro interno ao tentar atualizar o cliente.");
        }

        public (bool podeExcluir, string mensagem) ValidarExclusao(int id)
        {
            if (id <= 0)
            {
                return (false, "Selecione um cliente válido para exclusão.");
            }

            return (true, string.Empty);
        }

        public bool DeletarCliente(int id)
        {
            if (id <= 0) return false;
            return repository.ExcluirCliente(id);
        }
        
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
    }
}
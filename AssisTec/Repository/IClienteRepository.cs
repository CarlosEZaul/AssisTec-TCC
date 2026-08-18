using System.Collections.Generic;
using System.Data;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IClienteRepository
    {
        bool InserirCliente(Cliente cliente);
        List<Cliente> ObterTodosClientes();
        Cliente ObterPorId(int id);
        bool AtualizarCliente(Cliente cliente);
        Cliente ObterPorCpf(string cpf);
        bool CpfExiste(string cpf); 
        List<Cliente> ObterComFiltros(string busca);
        bool AlterarStatus(int id);
        List<Cliente> ObterComFiltros(string nome, bool exibirDesativados);
        
    }
}
using AssisTec.Data;

namespace AssisTec.Business
{
    public class ClienteService
    {
        ClienteRository repository = new ClienteRository();

        public (bool sucesso, string mensagem) CadastrarCliente(Cliente cliente)
        {
            if (!cliente.ValidarDados(out string erro))
                return (false, erro);

            if (repository.CpfExiste(cliente.cpf))
                return (false, "Cliente com este CPF já existe");
            
            bool sucesso = repository.novoCliente(cliente);
            
            if (sucesso)
            {
                return (true, "Usuário cadastrado com sucesso!");
            }
            else
            {
                return (false, "Erro ao cadastrar usuário");
            }
        }
        
        public (bool sucesso, string mensagem) EditarCliente(Cliente cliente)
        {
            if (cliente == null)
                return (false, "Dados inválidos");
            
            if (!cliente.ValidarDados(out string erro))
                return (false, erro);
            
            if (repository.CpfExiste(cliente.cpf, cliente.id))
                return (false, "CPF já cadastrado para outro usuário");

            bool sucesso = repository.editarCliente(cliente);
            return sucesso 
                ? (true, "Cliente editado com sucesso") 
                : (false, "Erro ao editar Cliente");
        }
        
        public (bool podeExcluir, string mensagem) ValidarExclusao(int idCliente)
        {
            int osEmAndamento = repository.ContarOsEmAndamento(idCliente);
            
            if (osEmAndamento > 0)
                return (false, "Não é possível excluir usuário com Ordem de Serviço em andamento");

            return (true, null);
        }
        
        public bool DeletarCliente(int id)
        {
            return repository.DeletarCliente(id);
        }
    }
}
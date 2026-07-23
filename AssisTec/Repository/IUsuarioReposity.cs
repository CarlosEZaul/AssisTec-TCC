using System.Collections.Generic;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IUsuarioReposity
    {
        bool InserirUsuario(Usuario usuario);
        List<Usuario> ObterTodosUsuarios();
        List<Usuario> ObterTodosTecnicosAtivados();
        Usuario ObterPorId(int id);
        Usuario ObterPorCpf(string cpf);
        Usuario ObterPorEmail(string email);
        bool AtualizarUsuario(Usuario usuario);
        bool ExcluirUsuario(int id);
        bool AlterarStatus(int id);
        bool CpfExiste(string cpf);
        bool EmailExiste(string email);
        bool ExisteGerenteAtivo();
        
        List<Usuario> ObterComFiltros(string nome, bool exibirDesativados, int nivel);
    }
}
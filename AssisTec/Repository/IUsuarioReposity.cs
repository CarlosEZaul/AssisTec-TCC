using System.Collections.Generic;
using AssisTec.Models;

namespace AssisTec.Repository
{
    public interface IUsuarioReposity
    {
        bool InserirUsuario(Usuario usuario);
        List<Usuario> ObterTodosUsuarios();
        Usuario ObterPorId(int id);
        Usuario ObterPorCpf(string cpf);
        bool AtualizarUsuario(Usuario usuario);
        bool ExcluirUsuario(int id);
        bool CpfExiste(string cpf);
        bool ExisteGerenteAtivo();
        List<Usuario> ObterComFiltros(string nome, bool apenasInativos, int nivel);
    }
}
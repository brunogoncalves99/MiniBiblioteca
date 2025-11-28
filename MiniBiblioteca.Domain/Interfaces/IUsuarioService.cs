using MiniBiblioteca.Domain.Entities;

namespace MiniBiblioteca.Domain.Interfaces
{
    public interface IUsuarioService
    {
        Task<Usuario> GetUsuarioPorIdAsync(int id);
        Task<Usuario> GetUsuarioPorEmailAsync(string email);
        Task<IEnumerable<Usuario>> GetTodosUsuariosAsync();
        Task<Usuario> CriarUsuarioAsync(Usuario usuario);
        Task AtualizarUsuarioAsync(Usuario usuario);
        Task DeletarUsuarioAsync(int id);
        Task<bool> VerificarEmailExisteAsync(string email);
    }
}

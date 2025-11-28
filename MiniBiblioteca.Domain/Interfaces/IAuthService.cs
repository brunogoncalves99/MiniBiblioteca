using MiniBiblioteca.Domain.Entities;

namespace MiniBiblioteca.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<Usuario> LoginAsync(string email, string senha);
        Task<Usuario> RegistrarAsync(Usuario usuario, string senha);
        string HashSenha(string senha);
        bool VerificarSenha(string senha, string senhaHash);
    }
}

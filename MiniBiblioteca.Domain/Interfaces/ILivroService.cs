using MiniBiblioteca.Domain.Entities;

namespace MiniBiblioteca.Domain.Interfaces
{
    public interface ILivroService
    {
        Task<IEnumerable<Livro>> GetTodosLivrosAsync();
        Task<IEnumerable<Livro>> GetLivrosDisponiveisAsync();
        Task<Livro> GetLivroPorIdAsync(int id);
        Task<IEnumerable<Livro>> BuscarLivrosAsync(string termo);
        Task<Livro> AdicionarLivroAsync(Livro livro);
        Task AtualizarLivroAsync(Livro livro);
        Task RemoverLivroAsync(int id);
        Task<bool> VerificarDisponibilidadeAsync(int livroId);
    }
}

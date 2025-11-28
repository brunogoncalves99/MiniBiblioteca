using MiniBiblioteca.Domain.Entities;

namespace MiniBiblioteca.Domain.Interfaces
{
    public interface IAluguelService
    {
        Task<Aluguel> RealizarAluguelAsync(int usuarioId, int livroId, int diasAluguel, string observacoes = null);
        Task<Aluguel> DevolverLivroAsync(int aluguelId);
        Task<IEnumerable<Aluguel>> GetAlugueisUsuarioAsync(int usuarioId);
        Task<IEnumerable<Aluguel>> GetAlugueisAtivosUsuarioAsync(int usuarioId);
        Task<IEnumerable<Aluguel>> GetTodosAlugueisAtivosAsync();
        Task<IEnumerable<Aluguel>> GetAlugueisAtrasadosAsync();
        Task AtualizarStatusAlugueisAsync();
        Task<decimal> CalcularMultaAsync(int aluguelId);
    }
}

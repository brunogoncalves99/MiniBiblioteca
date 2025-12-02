using MiniBiblioteca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Domain.Interfaces
{
    public interface IAluguelRepository : IRepository<Aluguel>
    {
        Task<IEnumerable<Aluguel>> GetAlugueisAtivosPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<Aluguel>> GetAlugueisAtrasadosAsync();
        Task<IEnumerable<Aluguel>> GetHistoricoUsuarioAsync(int usuarioId);
        Task<IEnumerable<Aluguel>> GetAlugueisAtivosPorLivroAsync(int livroId);
        Task<IEnumerable<Aluguel>> GetTodosAlugueisAtivosAsync();
        Task<IEnumerable<Aluguel>> GetTodosAlugueisAsync();
        Task<int> GetQuantidadeAlugueisAtivosPorUsuarioAsync(int usuarioId);
    }
}

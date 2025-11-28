using MiniBiblioteca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Domain.Interfaces
{
    public interface ILivroRepository : IRepository<Livro>
    {
        Task<IEnumerable<Livro>> GetLivrosDisponiveisAsync();
        Task<IEnumerable<Livro>> GetLivrosAtivoAsync();
        Task<IEnumerable<Livro>> BuscarLivrosAsync(string termo);
        //Task<IEnumerable<Livro>> GetLivrosPorCategoriaAsync(string categoria);
        Task<bool> VerificarDisponibilidadeAsync(int livroId);
        Task AtualizarQuantidadeAsync(int livroId, int quantidade);
    }
}

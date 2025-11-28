using Microsoft.EntityFrameworkCore;
using MiniBiblioteca.Domain.Entities;
using MiniBiblioteca.Domain.Enums;
using MiniBiblioteca.Domain.Interfaces;
using MiniBiblioteca.Infrastructure.Data;

namespace MiniBiblioteca.Infrastructure.Repositories
{
    public class ReservaRepository : Repository<Reserva>, IReservaRepository
    {
        public ReservaRepository(BibliotecaDbContext context) : base(context) { }

        public async Task<IEnumerable<Reserva>> GetReservasAtivasPorUsuarioAsync(int usuarioId)
        {
            return await _dbSet
                .Include(r => r.Livro)
                .Where(r => r.idUsuario == usuarioId && r.Status == StatusReserva.Ativa)
                .OrderBy(r => r.DataReserva)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetReservasAtivasPorLivroAsync(int livroId)
        {
            return await _dbSet
                .Include(r => r.Usuario)
                .Where(r => r.idLivro == livroId && r.Status == StatusReserva.Ativa)
                .OrderBy(r => r.DataReserva)
                .ToListAsync();
        }

        public async Task<Reserva> GetProximaReservaAsync(int livroId)
        {
            return await _dbSet
                .Include(r => r.Usuario)
                .Where(r => r.idLivro == livroId && r.Status == StatusReserva.Ativa)
                .OrderBy(r => r.DataReserva)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Reserva>> GetReservasExpiradasAsync()
        {
            var hoje = DateTime.Now;
            return await _dbSet
                .Include(r => r.Usuario)
                .Include(r => r.Livro)
                .Where(r => r.Status == StatusReserva.Ativa &&
                       r.DataExpiracao.HasValue &&
                       r.DataExpiracao.Value < hoje)
                .ToListAsync();
        }
    }
}

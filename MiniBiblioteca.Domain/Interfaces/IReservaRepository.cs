using MiniBiblioteca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Domain.Interfaces
{
    public interface IReservaRepository : IRepository<Reserva>
    {
        Task<IEnumerable<Reserva>> GetReservasAtivasPorUsuarioAsync(int usuarioId);
        Task<IEnumerable<Reserva>> GetReservasAtivasPorLivroAsync(int livroId);
        Task<Reserva> GetProximaReservaAsync(int livroId);
        Task<IEnumerable<Reserva>> GetReservasExpiradasAsync();
    }
}

using MiniBiblioteca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Domain.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario> GetByEmailAsync(string email);
        Task<Usuario> GetByCpfAsync(string cpf);
        Task<bool> VerificarEmailExisteAsync(string email);
        Task<bool> VerificarCpfExisteAsync(string cpf);
    }
}

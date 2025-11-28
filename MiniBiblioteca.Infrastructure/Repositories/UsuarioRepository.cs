using Microsoft.EntityFrameworkCore;
using MiniBiblioteca.Domain.Entities;
using MiniBiblioteca.Domain.Interfaces;
using MiniBiblioteca.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Infrastructure.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(BibliotecaDbContext context) : base(context) { }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> GetByCpfAsync(string cpf)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Cpf == cpf);
        }

        public async Task<bool> VerificarEmailExisteAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> VerificarCpfExisteAsync(string cpf)
        {
            return await _dbSet.AnyAsync(u => u.Cpf == cpf);
        }
    }
}

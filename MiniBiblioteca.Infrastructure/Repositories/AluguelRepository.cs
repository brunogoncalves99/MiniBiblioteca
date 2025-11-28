using Microsoft.EntityFrameworkCore;
using MiniBiblioteca.Domain.Entities;
using MiniBiblioteca.Domain.Enums;
using MiniBiblioteca.Domain.Interfaces;
using MiniBiblioteca.Infrastructure.Data;

namespace MiniBiblioteca.Infrastructure.Repositories;

public class AluguelRepository : Repository<Aluguel>, IAluguelRepository
{
    public AluguelRepository(BibliotecaDbContext context) : base(context) { }


    public async Task<IEnumerable<Aluguel>> GetAlugueisAtivosPorUsuarioAsync(int usuarioId)
    {
        return await _dbSet
            .Include(a => a.Livro)
            .Where(a => a.idUsuario == usuarioId && (a.Status == StatusAluguel.Ativo || a.Status == StatusAluguel.Atrasado))
            .OrderByDescending(a => a.DataAluguel)
            .ToListAsync();
    }

    public async Task<IEnumerable<Aluguel>> GetAlugueisAtrasadosAsync()
    {
        var hoje = DateTime.Now.Date;
        return await _dbSet
            .Include(a => a.Usuario)
            .Include(a => a.Livro)
            .Where(a => a.Status == StatusAluguel.Ativo && a.DataPrevistaDevolucao < hoje).ToListAsync();
    }

    public async Task<IEnumerable<Aluguel>> GetHistoricoUsuarioAsync(int usuarioId)
    {
        return await _dbSet
            .Include(a => a.Livro)
            .Where(a => a.idUsuario == usuarioId)
            .OrderByDescending(a => a.DataAluguel)
            .ToListAsync();
    }

    public async Task<IEnumerable<Aluguel>> GetTodosAlugueisAtivosAsync()
    {
        return await _dbSet
            .Include(a => a.Usuario)
            .Include(a => a.Livro)  
            .Where(a => a.Status == StatusAluguel.Ativo).ToListAsync();
    }

    public async Task<int> GetQuantidadeAlugueisAtivosPorUsuarioAsync(int usuarioId)
    {
        return await _dbSet
        .CountAsync(a => a.idUsuario == usuarioId && (a.Status == StatusAluguel.Ativo || a.Status == StatusAluguel.Atrasado));
    }

    public async Task<IEnumerable<Aluguel>> GetAlugueisAtivosPorLivroAsync(int livroId)
    {
        return await _dbSet
            .Include(a => a.Usuario)
            .Where(a => a.idLivro == livroId && (a.Status == StatusAluguel.Ativo || a.Status == StatusAluguel.Atrasado))
            .ToListAsync();
    }

}

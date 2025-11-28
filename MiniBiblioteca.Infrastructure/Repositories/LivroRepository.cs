using Microsoft.EntityFrameworkCore;
using MiniBiblioteca.Domain.Entities;
using MiniBiblioteca.Domain.Interfaces;
using MiniBiblioteca.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Infrastructure.Repositories;

public class LivroRepository : Repository<Livro>, ILivroRepository
{
    public LivroRepository (BibliotecaDbContext context) : base (context) { }

    public async Task<IEnumerable<Livro>> GetLivrosDisponiveisAsync()
    {
        return await _dbSet
            .Where(l => l.Ativo && l.QuantidadeDisponivel > 0)
            .OrderBy(l => l.Titulo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Livro>> GetLivrosAtivoAsync()
    {
        return await _dbSet
            .Where(l => l.Ativo == true)
            .OrderBy(l => l.Titulo)
            .ToListAsync();
    }

    public async Task<IEnumerable<Livro>> BuscarLivrosAsync(string termo)
    {
        return await _dbSet
            .Where(l => l.Ativo &&
                   (l.Titulo.Contains(termo) ||
                    l.Autor.Contains(termo) ||
                    l.Categoria.Contains(termo)))
            .ToListAsync();
    }

    //public async Task<IEnumerable<Livro>> GetLivrosPorCategoriaAsync(string categoria)
    //{
    //    return await _dbSet
    //        .Where(l => l.Ativo &&
    //               (l.Titulo.Contains(termo) ||
    //                l.Autor.Contains(termo) ||
    //                l.Categoria.Contains(termo)))
    //        .ToListAsync()
    //}

    public async Task<bool> VerificarDisponibilidadeAsync(int livroId)
    {
        var livro = await GetByIdAsync(livroId);
        return livro != null && livro.Ativo && livro.QuantidadeDisponivel > 0;
    }

    public async Task AtualizarQuantidadeAsync(int livroId, int quantidade)
    {
        var livro = await GetByIdAsync(livroId);
        if (livro != null)
        {
            livro.QuantidadeDisponivel += quantidade;
            await UpdateAsync(livro);
        }

    }
}

using MiniBiblioteca.Domain.Entities;
using MiniBiblioteca.Domain.Interfaces;

namespace MiniBiblioteca.Application.Services
{
    public class LivroService : ILivroService
    {
        private readonly ILivroRepository _livroRepository;

        public LivroService(ILivroRepository livroRepository)
        {
            _livroRepository = livroRepository;
        }

        public async Task<IEnumerable<Livro>> GetTodosLivrosAsync()
        {
            return await _livroRepository.GetLivrosAtivoAsync();
        }

        public async Task<IEnumerable<Livro>> GetLivrosDisponiveisAsync()
        {
            return await _livroRepository.GetLivrosDisponiveisAsync();
        }

        public async Task<Livro> GetLivroPorIdAsync(int id)
        {
            return await _livroRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Livro>> BuscarLivrosAsync(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return await GetLivrosDisponiveisAsync();

            return await _livroRepository.BuscarLivrosAsync(nome);
        }

        public async Task<Livro> AdicionarLivroAsync(Livro livro)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(livro.Titulo))
                throw new Exception("Título é obrigatório");

            if (string.IsNullOrWhiteSpace(livro.Autor))
                throw new Exception("Autor é obrigatório");

            if (livro.QuantidadeTotal < 0)
                throw new Exception("Quantidade total não pode ser negativa");

            livro.QuantidadeDisponivel = livro.QuantidadeTotal;
            livro.Ativo = true;
            livro.DataCadastro = DateTime.Now;

            return await _livroRepository.AddAsync(livro);
        }

        public async Task AtualizarLivroAsync(Livro livro)
        {
            var livroExistente = await _livroRepository.GetByIdAsync(livro.idLivro);
            if (livroExistente == null)
                throw new Exception("Livro não encontrado");

            var diferencaTotal = livro.QuantidadeTotal - livroExistente.QuantidadeTotal;

            livro.QuantidadeDisponivel = livroExistente.QuantidadeDisponivel + diferencaTotal;

            if (livro.QuantidadeDisponivel > livro.QuantidadeTotal)
                livro.QuantidadeDisponivel = livro.QuantidadeTotal;

            if (livro.QuantidadeDisponivel < 0)
                livro.QuantidadeDisponivel = 0;

            await _livroRepository.UpdateAsync(livro);
        }

        public async Task RemoverLivroAsync(int id)
        {
            var livro = await _livroRepository.GetByIdAsync(id);
            if (livro == null)
                throw new Exception("Livro não encontrado");

            livro.Ativo = false;
            await _livroRepository.UpdateAsync(livro);
        }

        public async Task<bool> VerificarDisponibilidadeAsync(int livroId)
        {
            return await _livroRepository.VerificarDisponibilidadeAsync(livroId);
        }
    }
}

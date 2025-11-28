using MiniBiblioteca.Domain.Entities;
using MiniBiblioteca.Domain.Enums;
using MiniBiblioteca.Domain.Interfaces;

namespace MiniBiblioteca.Application.Services
{
    public class AluguelService : IAluguelService
    {
        private readonly IAluguelRepository _aluguelRepository;
        private readonly ILivroRepository _livroRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IReservaRepository _reservaRepository;

        private const decimal VALOR_MULTA_DIARIA = 2.50m;
        private const int MAX_LIVROS_SIMULTANEOS = 3;
        private const int DIAS_MAXIMO_ALUGUEL = 30;

        public AluguelService(IAluguelRepository aluguelRepository,ILivroRepository livroRepository,IUsuarioRepository usuarioRepository,IReservaRepository reservaRepository)
        {
            _aluguelRepository = aluguelRepository;
            _livroRepository = livroRepository;
            _usuarioRepository = usuarioRepository;
            _reservaRepository = reservaRepository;
        }

        public async Task<Aluguel> RealizarAluguelAsync(int usuarioId, int livroId, int diasAluguel, string observacoes = null)
        {
            // Validações
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null || !usuario.Ativo)
                throw new Exception("Usuário não encontrado ou inativo");

            var livro = await _livroRepository.GetByIdAsync(livroId);
            if (livro == null || !livro.Ativo)
                throw new Exception("Livro não encontrado ou inativo");

            if (livro.QuantidadeDisponivel <= 0)
                throw new Exception("Livro indisponível no momento");

            if (diasAluguel <= 0 || diasAluguel > DIAS_MAXIMO_ALUGUEL)
                throw new Exception($"Dias de aluguel deve ser entre 1 e {DIAS_MAXIMO_ALUGUEL}");

            var alugueisAtivos = await _aluguelRepository.GetQuantidadeAlugueisAtivosPorUsuarioAsync(usuarioId);
            if (alugueisAtivos >= MAX_LIVROS_SIMULTANEOS)
                throw new Exception($"Você já atingiu o limite de {MAX_LIVROS_SIMULTANEOS} livros alugados simultaneamente");

            var aluguel = new Aluguel
            {
                idUsuario = usuarioId,
                idLivro = livroId,
                DataAluguel = DateTime.Now,
                DiasAluguel = diasAluguel,
                DataPrevistaDevolucao = DateTime.Now.AddDays(diasAluguel),
                Status = StatusAluguel.Ativo,
                Observacoes = observacoes
            };

            var novoAluguel = await _aluguelRepository.AddAsync(aluguel);

            await _livroRepository.AtualizarQuantidadeAsync(livroId, -1);

            return novoAluguel;
        }

        public async Task<Aluguel> DevolverLivroAsync(int aluguelId)
        {
            var aluguel = await _aluguelRepository.GetByIdAsync(aluguelId);
            if (aluguel == null)
                throw new Exception("Aluguel não encontrado");

            if (aluguel.Status == StatusAluguel.Devolvido || aluguel.Status == StatusAluguel.DevolvidoComAtraso)
                throw new Exception("Este livro já foi devolvido");

            // Registrar devolução
            aluguel.DataDevolucao = DateTime.Now;

            // Calcular se há atraso e multa
            if (aluguel.DataDevolucao.Value.Date > aluguel.DataPrevistaDevolucao.Date)
            {
                var diasAtraso = (aluguel.DataDevolucao.Value.Date - aluguel.DataPrevistaDevolucao.Date).Days;
                aluguel.ValorMulta = diasAtraso * VALOR_MULTA_DIARIA;
                aluguel.Status = StatusAluguel.DevolvidoComAtraso;
            }
            else
            {
                aluguel.Status = StatusAluguel.Devolvido;
                aluguel.ValorMulta = 0;
            }

            await _aluguelRepository.UpdateAsync(aluguel);

            await _livroRepository.AtualizarQuantidadeAsync(aluguel.idLivro, 1);

            var proximaReserva = await _reservaRepository.GetProximaReservaAsync(aluguel.idLivro);
            if (proximaReserva != null)
            {
                proximaReserva.DataExpiracao = DateTime.Now.AddDays(3);
                await _reservaRepository.UpdateAsync(proximaReserva);
            }

            return aluguel;
        }

        public async Task<IEnumerable<Aluguel>> GetAlugueisUsuarioAsync(int usuarioId)
        {
            return await _aluguelRepository.GetHistoricoUsuarioAsync(usuarioId);
        }

        public async Task<IEnumerable<Aluguel>> GetAlugueisAtivosUsuarioAsync(int usuarioId)
        {
            return await _aluguelRepository.GetAlugueisAtivosPorUsuarioAsync(usuarioId);
        }

        public async Task<IEnumerable<Aluguel>> GetTodosAlugueisAtivosAsync()
        {
            return await _aluguelRepository.GetTodosAlugueisAtivosAsync();
        }

        public async Task<IEnumerable<Aluguel>> GetAlugueisAtrasadosAsync()
        {
            return await _aluguelRepository.GetAlugueisAtrasadosAsync();
        }

        public async Task AtualizarStatusAlugueisAsync()
        {
            var alugueisAtrasados = await _aluguelRepository.GetAlugueisAtrasadosAsync();

            foreach (var aluguel in alugueisAtrasados)
            {
                if (aluguel.Status == StatusAluguel.Ativo)
                {
                    aluguel.Status = StatusAluguel.Atrasado;
                    await _aluguelRepository.UpdateAsync(aluguel);
                }
            }
        }

        public async Task<decimal> CalcularMultaAsync(int aluguelId)
        {
            var aluguel = await _aluguelRepository.GetByIdAsync(aluguelId);
            if (aluguel == null)
                throw new Exception("Aluguel não encontrado");

            if (aluguel.DataDevolucao.HasValue)
            {
                return aluguel.ValorMulta ?? 0;
            }

            if (DateTime.Now.Date > aluguel.DataPrevistaDevolucao.Date)
            {
                var diasAtraso = (DateTime.Now.Date - aluguel.DataPrevistaDevolucao.Date).Days;
                return diasAtraso * VALOR_MULTA_DIARIA;
            }

            return 0;
        }
    }
}

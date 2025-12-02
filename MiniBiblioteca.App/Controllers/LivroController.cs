using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBiblioteca.Application.DTOs;
using MiniBiblioteca.Domain.Interfaces;

namespace MiniBiblioteca.App.Controllers
{
    [Authorize]
    public class LivroController : Controller
    {
        private readonly ILivroService _livroService;
        private readonly IAluguelRepository _aluguelRepository;

        public LivroController(ILivroService livroService, IAluguelRepository aluguelRepository)
        {
            _livroService = livroService;
            _aluguelRepository = aluguelRepository;
        }

        #region Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region Obter livros
        [HttpGet]
        public async Task<IActionResult> GetLivros(string termo = "")
        {
            try
            {
                var livros = string.IsNullOrWhiteSpace(termo)
                    ? await _livroService.GetTodosLivrosAsync() : await _livroService.BuscarLivrosAsync(termo);

                var livrosDto = livros.Select(l => new LivroDTO
                {
                    idLivro = l.idLivro,
                    Titulo = l.Titulo,
                    Autor = l.Autor,
                    Categoria = l.Categoria,
                    Descricao = l.Descricao,
                    QuantidadeTotal = l.QuantidadeTotal,
                    QuantidadeDisponivel = l.QuantidadeDisponivel,
                    ImagemCapa = l.ImagemCapa,
                    Ativo = l.Ativo

                }).ToList();

                return Json(new { success = true, data = livrosDto });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Obter detalhe do livro
        [HttpGet]
        public async Task<IActionResult> Detalhes(int id)
        {
            try
            {
                var livro = await _livroService.GetLivroPorIdAsync(id);
                if (livro == null)
                    return NotFound();

                var livroDto = new LivroDTO
                {
                    idLivro = livro.idLivro,
                    Titulo = livro.Titulo,
                    Autor = livro.Autor,
                    Categoria = livro.Categoria,
                    Descricao = livro.Descricao,
                    QuantidadeTotal = livro.QuantidadeTotal,
                    QuantidadeDisponivel = livro.QuantidadeDisponivel,
                    ImagemCapa = livro.ImagemCapa,
                    Ativo = livro.Ativo
                };

                return View(livroDto);
            }
            catch (Exception ex)
            {
                TempData["Erro"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        #endregion
    }
}

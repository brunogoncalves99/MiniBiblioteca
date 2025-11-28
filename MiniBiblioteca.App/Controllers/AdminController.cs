using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBiblioteca.App.Models;
using MiniBiblioteca.Application.DTOs;
using MiniBiblioteca.Application.Services;
using MiniBiblioteca.Application.Validators;
using MiniBiblioteca.Domain.Entities;
using MiniBiblioteca.Domain.Enums;
using MiniBiblioteca.Domain.Interfaces;
using System.Diagnostics;

namespace MiniBiblioteca.App.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly ILivroService _livroService;
        private readonly IAluguelService _aluguelService;
        private readonly IUsuarioService _usuarioService;
        private readonly IAluguelRepository _aluguelRepository;
        private readonly ILivroRepository _livroRepository;
        private readonly IAuthService _authService;

        public AdminController(ILivroService livroService, IAluguelService aluguelService, IUsuarioService usuarioService, IAluguelRepository aluguelRepository, ILivroRepository livroRepository, IAuthService authService)
        {
            _livroService = livroService;
            _aluguelService = aluguelService;
            _usuarioService = usuarioService;
            _aluguelRepository = aluguelRepository;
            _livroRepository = livroRepository;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboardData()
        {
            try
            {
                var todosLivros = await _livroRepository.GetAllAsync();
                var livrosAtivos = todosLivros.Where(l => l.Ativo).ToList();
                var alugueisAtivos = await _aluguelService.GetTodosAlugueisAtivosAsync();
                var alugueisAtrasados = await _aluguelService.GetAlugueisAtrasadosAsync();
                var usuarios = await _usuarioService.GetTodosUsuariosAsync();

                var dashboard = new DashboardDTO
                {
                    TotalLivros = livrosAtivos.Count,
                    LivrosDisponiveis = livrosAtivos.Count(l => l.QuantidadeDisponivel > 0),
                    LivrosAlugados = livrosAtivos.Sum(l => l.QuantidadeTotal - l.QuantidadeDisponivel),
                    AlugueisAtivos = alugueisAtivos.Count(),
                    AlugueisAtrasados = alugueisAtrasados.Count(),
                    TotalUsuarios = usuarios.Count(),
                    MultasAcumuladas = alugueisAtrasados.Sum(a => a.ValorMulta ?? 0),
                    AlugueisRecentes = alugueisAtivos.Take(1000).Select(a => new AluguelDTO
                    {
                        idAluguel = a.idAluguel,
                        NomeUsuario = a.Usuario.Nome,
                        TituloLivro = a.Livro.Titulo,
                        DataAluguel = a.DataAluguel,
                        DataPrevistaDevolucao = a.DataPrevistaDevolucao,
                        Status = a.Status
                    }).ToList()
                };

                return Json(new { success = true, data = dashboard });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GerenciarLivros()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTodosLivros()
        {
            try
            {
                var livros = await _livroService.GetTodosLivrosAsync();
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

        [HttpPost]
        public async Task<IActionResult> AdicionarLivro([FromBody] LivroDTO model)
        {
            try
            {
                var livro = new Livro
                {
                    Titulo = model.Titulo,
                    Autor = model.Autor,
                    Categoria = model.Categoria,
                    Descricao = model.Descricao,
                    QuantidadeTotal = model.QuantidadeTotal,
                    ImagemCapa = model.ImagemCapa,
                    Ativo = true
                };

                var erros = LivroValidator.Validar(livro);
                if (erros.Any())
                {
                    return Json(new { success = false, message = string.Join(", ", erros) });
                }

                await _livroService.AdicionarLivroAsync(livro);

                return Json(new { success = true, message = "Livro adicionado com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarLivro([FromBody] LivroDTO model)
        {
            try
            {
                var livro = await _livroService.GetLivroPorIdAsync(model.idLivro);
                if (livro == null)
                {
                    return Json(new { success = false, message = "Livro não encontrado" });
                }

                livro.Titulo = model.Titulo;
                livro.Autor = model.Autor;
                livro.Categoria = model.Categoria;
                livro.Descricao = model.Descricao;
                livro.QuantidadeTotal = model.QuantidadeTotal;

                if (!string.IsNullOrWhiteSpace(model.ImagemCapa))
                    livro.ImagemCapa = model.ImagemCapa;

                var erros = LivroValidator.Validar(livro);
                if (erros.Any())
                {
                    return Json(new { success = false, message = string.Join(", ", erros) });
                }

                await _livroService.AtualizarLivroAsync(livro);

                return Json(new { success = true, message = "Livro atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpDelete]
        public async Task<IActionResult> RemoverLivro([FromBody] int id)
        {
            try
            {
                await _livroService.RemoverLivroAsync(id);
                return Json(new { success = true, message = "Livro removido com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GerenciarAlugueis()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTodosAlugueis()
        {
            try
            {
                var alugueis = await _aluguelService.GetTodosAlugueisAtivosAsync();
                var alugueisDto = alugueis.Select(a => new AluguelDTO
                {
                    idAluguel = a.idAluguel,
                    idUsuario = a.idUsuario,
                    idLivro = a.idLivro,
                    NomeUsuario = a.Usuario.Nome,
                    EmailUsuario = a.Usuario.Email,
                    TituloLivro = a.Livro.Titulo,
                    NomeAutor = a.Livro.Autor,
                    DataAluguel = a.DataAluguel,
                    DataPrevistaDevolucao = a.DataPrevistaDevolucao,
                    DataDevolucao = a.DataDevolucao,
                    DiasAluguel = a.DiasAluguel,
                    Status = a.Status,
                    ValorMulta = a.ValorMulta,
                    Observacoes = a.Observacoes
                }).ToList();

                return Json(new { success = true, data = alugueisDto });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult Relatorios()
        {
            return View();
        }
    }
}

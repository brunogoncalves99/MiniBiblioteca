using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBiblioteca.Application.DTOs;
using MiniBiblioteca.Application.Validators;
using MiniBiblioteca.Domain.Interfaces;
using System.Security.Claims;

namespace MiniBiblioteca.App.Controllers
{
    [Authorize]
    public class AluguelController : Controller
    {
        private readonly IAluguelService _aluguelService;
        private readonly ILivroService _livroService;

        public AluguelController(IAluguelService aluguelService, ILivroService livroService)
        {
            _aluguelService = aluguelService;
            _livroService = livroService;
        }

        #region Obter ID Usuario logado
        private int GetUsuarioIdLogado()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(userId);
        }
        #endregion

        #region Meus Alugueis
        [HttpGet]
        public IActionResult MeusAlugueis()
        {
            return View();
        }
        #endregion

        #region Obter meus alugueis

        [HttpGet]
        public async Task<IActionResult> GetMeusAlugueis()
        {
            try
            {
                var usuarioId = GetUsuarioIdLogado();
                var alugueis = await _aluguelService.GetAlugueisUsuarioAsync(usuarioId);

                var alugueisDto = alugueis.Select(a => new AluguelDTO
                {
                    idAluguel = a.idAluguel,
                    idLivro = a.idLivro,
                    idUsuario = a.idUsuario,
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

        #endregion

        #region Obter Alugueis Ativos
        [HttpGet]
        public async Task<IActionResult> GetAlugueisAtivos()
        {
            try
            {
                var usuarioId = GetUsuarioIdLogado();
                var alugueis = await _aluguelService.GetAlugueisAtivosUsuarioAsync(usuarioId);

                var alugueisDto = alugueis.Select(a => new AluguelDTO
                {
                    idAluguel = a.idAluguel,
                    idLivro = a.idLivro,
                    idUsuario = a.idUsuario,    
                    TituloLivro = a.Livro.Titulo,
                    NomeAutor = a.Livro.Autor,
                    DataAluguel = a.DataAluguel,
                    DataPrevistaDevolucao = a.DataPrevistaDevolucao,
                    DiasAluguel = a.DiasAluguel,
                    Status = a.Status,
                    ValorMulta = a.ValorMulta
                }).ToList();

                return Json(new { success = true, data = alugueisDto });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Realizar Aluguel
        [HttpPost]
        public async Task<IActionResult> RealizarAluguel([FromBody] CheckoutDTO model)
        {
            try
            {
                var erros = AluguelValidator.ValidarCheckout(model.idLivro, model.DiasAluguel);
                if (erros.Any())
                {
                    return Json(new { success = false, message = string.Join(", ", erros) });
                }

                var usuarioId = GetUsuarioIdLogado();
                var aluguel = await _aluguelService.RealizarAluguelAsync(
                    usuarioId,
                    model.idLivro,
                    model.DiasAluguel,
                    model.Observacoes);

                return Json(new { success = true, message = "Aluguel realizado com sucesso!", data = aluguel.idAluguel });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Devolver Livro
        [HttpPost]
        public async Task<IActionResult> DevolverLivro(int id)
        {
            try
            {
                var aluguel = await _aluguelService.DevolverLivroAsync(id);

                var message = aluguel.ValorMulta > 0  ? $"Livro devolvido com sucesso! Multa: R$ {aluguel.ValorMulta:F2}" : "Livro devolvido com sucesso!";

                return Json(new { success = true, message = message, multa = aluguel.ValorMulta });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Calcular Multa da devolução do livro
        [HttpGet]
        public async Task<IActionResult> CalcularMulta(int id)
        {
            try
            {
                var multa = await _aluguelService.CalcularMultaAsync(id);
                return Json(new { success = true, multa = multa });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion
    }
}

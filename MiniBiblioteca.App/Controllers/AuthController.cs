using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MiniBiblioteca.Application.DTOs;
using MiniBiblioteca.Domain.Enums;
using MiniBiblioteca.Domain.Interfaces;
using System.Security.Claims;

namespace MiniBiblioteca.App.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        #endregion

        #region Realizar Login no sistema
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDTO model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Senha))
                {
                    return Json(new { success = false, message = "Email e senha são obrigatórios" });
                }

                var usuario = await _authService.LoginAsync(model.Email, model.Senha);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.idUsuario.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nome),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, usuario.Tipo.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = model.LembrarMe,
                    ExpiresUtc = model.LembrarMe ? DateTimeOffset.UtcNow.AddHours(2) : DateTimeOffset.UtcNow.AddMinutes(20)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                var redirectUrl = usuario.Tipo == TipoUsuario.Admin ? Url.Action("Dashboard", "Admin") : Url.Action("Index", "Livro");

                return Json(new { success = true, redirectUrl = redirectUrl });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        #endregion

        #region Acesso negado
        public IActionResult AccessDenied()
        {
            return View();
        }

        #endregion
    }
}
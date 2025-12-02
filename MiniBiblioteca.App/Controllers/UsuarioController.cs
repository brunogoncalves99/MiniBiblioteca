using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniBiblioteca.Application.DTOs;
using MiniBiblioteca.Application.Services;
using MiniBiblioteca.Application.Validators;
using MiniBiblioteca.Domain.Entities;
using MiniBiblioteca.Domain.Enums;
using MiniBiblioteca.Domain.Interfaces;

namespace MiniBiblioteca.App.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IAuthService _authService;

        public UsuarioController(IUsuarioService usuarioService, IAuthService authService)
        {
            _usuarioService = usuarioService;
            _authService = authService;
        }

        #region Registro
        [HttpGet]
        public IActionResult Registro()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
        #endregion

        #region Criar Usuário
        [HttpPost]
        public async Task<IActionResult> Registro([FromBody] UsuarioDTO model)
        {
            try
            {
                var usuario = new Usuario
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Cpf = model.CPF,
                    Telefone = model.Telefone,
                    Tipo = TipoUsuario.Usuario
                };

                var erros = UsuarioValidator.Validar(usuario, model.Senha);
                if (erros.Any())
                {
                    return Json(new { success = false, message = string.Join(", ", erros) });
                }

                await _authService.RegistrarAsync(usuario, model.Senha);

                return Json(new { success = true, message = "Cadastro realizado com sucesso! Faça login para continuar." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Gerenciar Usuário
        [HttpGet]
        public IActionResult GerenciarUsuario()
        {
            return View();
        }
        #endregion

        #region Obter todos Usuários
        /// <summary>
        /// Metodo para obter todos os Usuarios
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetTodosUsuarios()
        {
            try
            {
                var usuarios = await _usuarioService.GetTodosUsuariosAsync();
                var usuariosDto = usuarios.Select(a => new UsuarioDTO
                {
                    idUsuario = a.idUsuario,
                    Nome = a.Nome,
                    CPF = a.Cpf,
                    Email = a.Email,
                    Telefone = a.Telefone,
                    Tipo = a.Tipo,
                    Ativo = a.Ativo,
                    DataCadastro = a.DataCadastro
                }).ToList();

                return Json(new { success = true, data = usuariosDto });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

        #region Cadastrar Usuario
        /// <summary>
        /// Metodo para cadastar usuario
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CadastrarUsuario([FromBody] UsuarioDTO model)
        {
            try
            {
                var usuario = new Usuario
                {
                    Nome = model.Nome,
                    Email = model.Email,
                    Cpf = model.CPF,
                    Telefone = model.Telefone,
                    Tipo = TipoUsuario.Usuario
                };

                var erros = UsuarioValidator.Validar(usuario, model.Senha);
                if (erros.Any())
                {
                    return Json(new { success = false, message = string.Join(", ", erros) });
                }

                await _authService.RegistrarAsync(usuario, model.Senha);

                return Json(new { success = true, message = "Cadastro realizado com sucesso! Faça login para continuar." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        #endregion

        #region Atualizar os dados dos usuários
        /// <summary>
        /// Metodo para atualizar Usuário
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<IActionResult> AtualizarUsuario([FromBody] UsuarioDTO model)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioPorIdAsync(model.idUsuario);
                if (usuario == null)
                {
                    return Json(new { success = false, message = "Usuário não encontrado" });
                }

                usuario.Nome = model.Nome;
                usuario.Cpf = model.CPF;
                usuario.Telefone = model.Telefone;
                usuario.Email = model.Email;
                usuario.Ativo = model.Ativo;
                usuario.Tipo = model.Tipo;

                var erros = UsuarioValidator.Validar(usuario, model.Senha);
                if (erros.Any())
                {
                    return Json(new { success = false, message = string.Join(",", erros) });
                }

                await _usuarioService.AtualizarUsuarioAsync(usuario);

                return Json(new { sucess = true, message = "Usuário atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { sucess = false, message = ex.Message });
            }
        }

        #endregion

        #region Deletar Usuário
        /// <summary>
        /// Remover usuário do sistema, delete lógico.
        /// </summary>
        /// <param name="idUsuario"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task <IActionResult> RemoverUsuario(int idUsuario)
        {
            try
            {
                var usuario = await _usuarioService.GetUsuarioPorIdAsync(idUsuario);
                if(usuario == null)
                {
                    return Json(new { success = false, message = "Usuário não encontrado" });
                }

                await _usuarioService.DeletarUsuarioAsync(idUsuario);

                return Json(new { success = true, message = "Livro removido com sucesso!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        #endregion

    }
}
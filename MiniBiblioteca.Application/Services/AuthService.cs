using MiniBiblioteca.Application.DTOs;
using MiniBiblioteca.Domain.Entities;
using MiniBiblioteca.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace MiniBiblioteca.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<Usuario> LoginAsync(string email, string senha)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);

            if (usuario == null || !VerificarSenha(senha, usuario.Senha))
                return null;

            if (!usuario.Ativo)
                throw new Exception("Usuário inativo");

            return usuario;
        }

        public async Task<Usuario> RegistrarAsync(Usuario usuario, string senha)
        {
            if (await _usuarioRepository.VerificarEmailExisteAsync(usuario.Email))
                throw new Exception("Email já cadastrado no sistema");

            if (await _usuarioRepository.VerificarCpfExisteAsync(usuario.Cpf))
                throw new Exception("CPF já cadastrado no sistema");

            usuario.Senha = HashSenha(senha);
            usuario.Ativo = true;
            usuario.DataCadastro = DateTime.Now;

            return await _usuarioRepository.AddAsync(usuario);
        }

        public string HashSenha(string senha)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(senha);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }

        public bool VerificarSenha(string senha, string senhaHash)
        {
            var hash = HashSenha(senha);
            return hash == senhaHash;
        }

    }
}

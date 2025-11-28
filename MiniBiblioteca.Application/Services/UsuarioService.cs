using MiniBiblioteca.Domain.Entities;
using MiniBiblioteca.Domain.Interfaces;

namespace MiniBiblioteca.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;  
        private readonly IAuthService _authService;

        public UsuarioService(IUsuarioRepository usuarioService, IAuthService authService)
        {
            _usuarioRepository = usuarioService;
            _authService = authService;
        }

        public async Task<Usuario> GetUsuarioPorIdAsync(int id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }
        public async Task<Usuario> GetUsuarioPorEmailAsync(string email)
        {
            return await _usuarioRepository.GetByEmailAsync(email);
        }

        public async Task<IEnumerable<Usuario>> GetTodosUsuariosAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }

        public async Task <Usuario> CriarUsuarioAsync(Usuario usuario)
        {
            if (await _usuarioRepository.VerificarEmailExisteAsync(usuario.Email))
                throw new Exception("Email já cadastrado");

            if (await _usuarioRepository.VerificarCpfExisteAsync(usuario.Cpf))
                throw new Exception("CPF já cadastrado");

            usuario.DataCadastro = DateTime.Now;
            usuario.Ativo = true;

            return await _usuarioRepository.AddAsync(usuario);
        }

        public async Task AtualizarUsuarioAsync(Usuario usuario)
        {
            var usuarioExistente = await _usuarioRepository.GetByIdAsync(usuario.idUsuario);
            if (usuarioExistente == null)
                throw new Exception("Usuário não encontrado");

            if (usuarioExistente.Email != usuario.Email)
            {
                if (await _usuarioRepository.VerificarEmailExisteAsync(usuario.Email))
                    throw new Exception("Email já cadastrado para outro usuário");
            }

            await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task DeletarUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                throw new Exception("Usuário não encontrado");

            usuario.Ativo = false;
            await _usuarioRepository.UpdateAsync(usuario);
        }

        public async Task<bool> VerificarEmailExisteAsync(string email)
        {
            return await _usuarioRepository.VerificarEmailExisteAsync(email);
        }

    }
}

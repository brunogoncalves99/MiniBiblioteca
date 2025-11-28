using Microsoft.AspNetCore.Mvc;
using MiniBiblioteca.Domain.Interfaces;

namespace MiniBiblioteca.App.Controllers 
{
    public class UtilController : Controller
    {
        private readonly ILivroService _livroService;

        public UtilController(ILivroService livroService) 
        {
            _livroService = livroService;
        }

        [HttpGet]
        public async Task<IActionResult> CorrigirImagensLivros()
        {
            try
            {
                var livros = await _livroService.GetTodosLivrosAsync();  
                int corrigidos = 0;

                foreach (var livro in livros)
                {
                    if (!string.IsNullOrEmpty(livro.ImagemCapa) &&  
                        (livro.ImagemCapa.StartsWith("C:\\") ||
                         livro.ImagemCapa.StartsWith("D:\\") ||
                         livro.ImagemCapa.StartsWith("E:\\") ||
                         livro.ImagemCapa.Contains(":\\")  
                        ))
                    {
                        var nomeArquivo = Path.GetFileName(livro.ImagemCapa);

                        // Novo caminho relativo
                        livro.ImagemCapa = $"/imagens/{nomeArquivo}";

                        await _livroService.AtualizarLivroAsync(livro);
                        corrigidos++;
                    }
                }

                return Ok(new
                {
                    sucesso = true,
                    mensagem = $"{corrigidos} livros corrigidos com sucesso!",
                    totalLivros = livros.Count(),
                    corrigidos = corrigidos
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    sucesso = false,
                    mensagem = "Erro ao corrigir imagens: " + ex.Message
                });
            }
        }
    }
}
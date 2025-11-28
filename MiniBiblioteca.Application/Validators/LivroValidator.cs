using MiniBiblioteca.Domain.Entities;

namespace MiniBiblioteca.Application.Validators
{
    public class LivroValidator
    {
        public static List<string>Validar(Livro livro)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(livro.Titulo))
                erros.Add("Título é obrigatório");

            else if (livro.Titulo.Length > 300)
                erros.Add("Título não pode ter mais de 300 caracteres");

            if (string.IsNullOrWhiteSpace(livro.Autor))
                erros.Add("Autor é obrigatório");

            else if (livro.Autor.Length > 200)
                erros.Add("Autor não pode ter mais de 200 caracteres");

            if (livro.QuantidadeTotal < 0)
                erros.Add("Quantidade total não pode ser negativa");

            if (livro.QuantidadeDisponivel < 0)
                erros.Add("Quantidade disponível não pode ser negativa");

            if (livro.QuantidadeDisponivel > livro.QuantidadeTotal)
                erros.Add("Quantidade disponível não pode ser maior que a quantidade total");

            return erros;
        }

    }
}

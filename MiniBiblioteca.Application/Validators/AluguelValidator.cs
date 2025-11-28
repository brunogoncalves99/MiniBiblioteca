using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Application.Validators
{
    public class AluguelValidator
    {
        private const int DIAS_MINIMO = 1;
        private const int DIAS_MAXIMO = 30;

        public static List<string> ValidarCheckout(int livroId, int diasAluguel)
        {
            var erros = new List<string>();

            if (livroId <= 0)
                erros.Add("Livro inválido");

            if (diasAluguel < DIAS_MINIMO)
                erros.Add($"Dias de aluguel deve ser no mínimo {DIAS_MINIMO}");

            if (diasAluguel > DIAS_MAXIMO)
                erros.Add($"Dias de aluguel não pode exceder {DIAS_MAXIMO}");

            return erros;
        }
    }
}

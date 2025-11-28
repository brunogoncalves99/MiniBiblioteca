using MiniBiblioteca.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiniBiblioteca.Application.Validators
{
    public class UsuarioValidator
    {
        public static List<string> Validar(Usuario usuario, string senha = null)
        {
            var erros = new List<string>();

            if (string.IsNullOrWhiteSpace(usuario.Nome))
                erros.Add("Nome é obrigatório");
            else if (usuario.Nome.Length < 3)
                erros.Add("Nome deve ter no mínimo 3 caracteres");
            else if (usuario.Nome.Length > 200)
                erros.Add("Nome não pode ter mais de 200 caracteres");

            if (string.IsNullOrWhiteSpace(usuario.Email))
                erros.Add("Email é obrigatório");
            else if (!ValidarEmail(usuario.Email))
                erros.Add("Email inválido");

            if (string.IsNullOrWhiteSpace(usuario.Cpf))
                erros.Add("CPF é obrigatório");
            else if (!ValidarCPF(usuario.Cpf))
                erros.Add("CPF inválido");

            if (!string.IsNullOrWhiteSpace(senha))
            {
                if (senha.Length < 6)
                    erros.Add("Senha deve ter no mínimo 6 caracteres");
            }

            return erros;
        }

        private static bool ValidarEmail(string email)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            return regex.IsMatch(email);
        }

        private static bool ValidarCPF(string cpf)
        {
            cpf = Regex.Replace(cpf, @"[^\d]", "");

            if (cpf.Length != 11)
                return false;

            if (cpf.Distinct().Count() == 1)
                return false;

            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            var resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            var digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;

            digito = digito + resto.ToString();

            return cpf.EndsWith(digito);
        }
    }
}

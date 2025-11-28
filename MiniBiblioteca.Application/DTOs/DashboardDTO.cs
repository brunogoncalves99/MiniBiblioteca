using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Application.DTOs
{
    public class DashboardDTO
    {
        public int TotalLivros { get; set; }
        public int LivrosDisponiveis { get; set; }
        public int LivrosAlugados { get; set; }
        public int AlugueisAtivos { get; set; }
        public int AlugueisAtrasados { get; set; }
        public int TotalUsuarios { get; set; }
        public decimal MultasAcumuladas { get; set; }
        public List<LivroMaisAlugadoDTO> LivrosMaisAlugados { get; set; }
        public List<AluguelDTO> AlugueisRecentes { get; set; }

        public DashboardDTO()
        {
            LivrosMaisAlugados = new List<LivroMaisAlugadoDTO>();
            AlugueisRecentes = new List<AluguelDTO>();
        }

        public class LivroMaisAlugadoDTO
        {
            public int idLivro { get; set; }
            public string Titulo { get; set; }
            public string Autor { get; set; }
            public int TotalAlugueis { get; set; }
        }
    }
}

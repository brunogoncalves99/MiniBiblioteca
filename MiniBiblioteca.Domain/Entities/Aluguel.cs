using MiniBiblioteca.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Domain.Entities
{
    public class Aluguel
    {
        public int idAluguel { get; set; }
        public int idUsuario { get; set; }
        public int idLivro { get; set; }
        public DateTime DataAluguel { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public int DiasAluguel { get; set; }
        public StatusAluguel Status { get; set; }
        public decimal? ValorMulta { get; set; }
        public decimal ValorTotal { get; set; }
        public string Observacoes { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual Livro Livro { get; set; }
    }
}

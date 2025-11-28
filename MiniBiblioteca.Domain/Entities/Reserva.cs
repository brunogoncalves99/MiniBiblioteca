using MiniBiblioteca.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Domain.Entities
{
    public class Reserva
    {
        public int idReserva { get; set; }
        public int idUsuario { get; set; }
        public int idLivro { get; set; }
        public DateTime DataReserva { get; set; }
        public StatusReserva Status { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Observacoes { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Livro Livro { get; set; }
    }
}

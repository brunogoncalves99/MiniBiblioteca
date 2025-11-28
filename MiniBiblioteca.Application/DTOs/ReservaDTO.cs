using MiniBiblioteca.Domain.Enums;

namespace MiniBiblioteca.Application.DTOs
{
    public class ReservaDTO
    {
        public int idReserva { get; set; }
        public int idUsuario { get; set; }
        public int idLivro { get; set; }
        public string NomeUsuario { get; set; }
        public string TituloLivro { get; set; }
        public DateTime DataReserva { get; set; }
        public StatusReserva Status { get; set; }
        public DateTime? DataExpiracao { get; set; }
        public string Observacoes { get; set; }
    }
}

using MiniBiblioteca.Domain.Enums;

namespace MiniBiblioteca.Domain.Entities
{
    public class Usuario
    {
        public int idUsuario { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public TipoUsuario Tipo { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }

        public virtual ICollection<Aluguel> Alugueis { get; set; }
        public virtual ICollection<Reserva> Reservas { get; set; }
        
        public Usuario()
        {
            Alugueis = new List<Aluguel>();
            Reservas = new List<Reserva>();
        }

    }
}

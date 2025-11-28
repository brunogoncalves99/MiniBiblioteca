using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Domain.Entities
{
    public class Livro
    {
        public int idLivro { get; set; }
        public string Titulo { get; set; }
        public string Autor { get; set; }
        public string Categoria { get; set; }
        public string Descricao { get; set; }
        public int QuantidadeTotal { get; set; }
        public int QuantidadeDisponivel { get; set; }
        public string ImagemCapa { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        public virtual ICollection<Aluguel> Alugueis { get; set; }
        public virtual ICollection<Reserva> Reservas { get; set; }

        public Livro()
        {
            Alugueis = new List<Aluguel>();
            Reservas = new List<Reserva>();
        }
    }
}

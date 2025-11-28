using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Application.DTOs
{
    public class LivroDTO
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
        public bool Disponivel => QuantidadeDisponivel > 0;
    }
}
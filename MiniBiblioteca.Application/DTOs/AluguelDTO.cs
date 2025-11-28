using MiniBiblioteca.Domain.Entities;
using MiniBiblioteca.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Application.DTOs
{
    public class AluguelDTO
    {
        public int idAluguel { get; set; }
        public int idLivro { get; set; }
        public int idUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string EmailUsuario { get; set; }
        public string NomeAutor { get; set; }
        public string TituloLivro { get; set; }
        public DateTime DataAluguel { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public int DiasAluguel { get; set; }
        public StatusAluguel Status { get; set; }
        public string StatusTexto => Status.ToString();
        public decimal? ValorMulta { get; set; }
        public string Observacoes { get; set; }
        public int DiasAtraso
        {
            get
            {
                if (DataDevolucao.HasValue)
                {
                    var atraso = (DataDevolucao.Value.Date - DataPrevistaDevolucao.Date).Days;
                    return atraso > 0 ? atraso : 0;
                }
                else if (DateTime.Now.Date > DataPrevistaDevolucao.Date)
                {
                    return (DateTime.Now.Date - DataPrevistaDevolucao.Date).Days;
                }
                return 0;
            }
        }
        public bool EstaAtrasado => Status == StatusAluguel.Atrasado;
    }
}

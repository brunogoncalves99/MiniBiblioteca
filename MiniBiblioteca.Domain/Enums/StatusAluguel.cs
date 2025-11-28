using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniBiblioteca.Domain.Enums;

public enum StatusAluguel
{
    Ativo = 1,
    Devolvido = 2,
    Atrasado = 3,
    DevolvidoComAtraso = 4
}

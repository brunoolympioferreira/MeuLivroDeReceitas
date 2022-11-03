using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Domain.Entidades;

public class EntidadeBase
{
    public long Id { get; set; }
    public DateTime DataCriacao { get; set; }
}

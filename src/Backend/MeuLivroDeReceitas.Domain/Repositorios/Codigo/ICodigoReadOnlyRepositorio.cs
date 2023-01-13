using MeuLivroDeReceitas.Domain.Entidades;

namespace MeuLivroDeReceitas.Domain.Repositorios.Codigo;
public interface ICodigoReadOnlyRepositorio
{
    Task<Codigos> RecuperarEntidadeCodigo(string codigo);
}

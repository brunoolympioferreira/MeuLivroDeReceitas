namespace MeuLivroDeReceitas.Domain.Repositorios.Codigo;
public interface ICodigoWriteOnlyRepositorio
{
    Task Registrar(Domain.Entidades.Codigos codigo);
}

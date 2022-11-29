using MeuLivroDeReceitas.Domain.Entidades;

namespace MeuLivroDeReceitas.Domain.Repositorios.Usuario;

public interface IUsuarioWriteOnlyRepositorio
{
    Task Adicionar(Entidades.Usuario usuario);
}

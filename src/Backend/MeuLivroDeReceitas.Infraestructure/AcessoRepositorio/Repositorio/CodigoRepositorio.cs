using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infraestructure.AcessoRepositorio.Repositorio;
public class CodigoRepositorio : ICodigoWriteOnlyRepositorio, ICodigoReadOnlyRepositorio
{
    private readonly MeuLivroDeReceitasContext _contexto;
    public CodigoRepositorio(MeuLivroDeReceitasContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<Codigos> RecuperarEntidadeCodigo(string codigo)
    {
        return await _contexto.Codigos.AsNoTracking()
            .FirstOrDefaultAsync(c => c.Codigo == codigo);
    }

    public async Task Registrar(Codigos codigo)
    {
        var codigoBancoDeDados = await _contexto.Codigos.FirstOrDefaultAsync(c => c.UsuarioId == codigo.UsuarioId);

        if (codigoBancoDeDados is not null)
        {
            codigoBancoDeDados.Codigo = codigo.Codigo;
            _contexto.Codigos.Update(codigoBancoDeDados);
        }
        else
        {
            await _contexto.Codigos.AddAsync(codigo);
        }
    }
}

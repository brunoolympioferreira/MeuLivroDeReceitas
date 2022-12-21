﻿using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Domain.Repositorios.Receita;
using Microsoft.EntityFrameworkCore;

namespace MeuLivroDeReceitas.Infraestructure.AcessoRepositorio.Repositorio;
public class ReceitaRepositorio : IReceitaWriteOnlyRepositorio, IReceitaReadOnlyRepositorio
{
    private readonly MeuLivroDeReceitasContext _contexto;
    public ReceitaRepositorio(MeuLivroDeReceitasContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IList<Receita>> RecuperarTodasDoUsuario(long usuarioId)
    {
        return await _contexto.Receitas.Where(r => r.UsuarioId == usuarioId).ToListAsync();
    }

    public async Task Registrar(Receita receita)
    {
        await _contexto.Receitas.AddAsync(receita);
    }
}

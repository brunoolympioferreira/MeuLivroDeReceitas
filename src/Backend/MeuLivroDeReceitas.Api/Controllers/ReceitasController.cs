﻿using MeuLivroDeReceitas.Api.Binder;
using MeuLivroDeReceitas.Api.Filtros;
using MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;
using MeuLivroDeReceitas.Application.UseCases.Receita.RecuperarPorId;
using MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers;

[ServiceFilter(typeof(UsuarioAutenticadoAttribute))]
public class ReceitasController : MeuLivroDeReceitasController
{
    [HttpPost]
    [ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status201Created)]
    public async Task<IActionResult> Registrar(
        [FromServices] IRegistrarReceitaUseCase useCase,
        [FromBody] RequisicaoReceitaJson requisicao)
    {
        var resposta = await useCase.Executar(requisicao);

        return Created(string.Empty, resposta);
    }

    [HttpGet]
    [Route("{id:hashids}")]
    [ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> RecuperarPorId(
    [FromServices] IRecuperarReceitaPorIdUseCase useCase,
    [FromRoute] [ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        var resposta = await useCase.Executar(id);

        return Ok(resposta);
    }

    [HttpPut]
    [Route("{id:hashids}")]
    [ProducesResponseType(typeof(RespostaReceitaJson), StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Atualizar(
    [FromServices] IAtualizarReceitaUseCase useCase,
    [FromBody] RequisicaoReceitaJson requisicao,
    [FromRoute][ModelBinder(typeof(HashidsModelBinder))] long id)
    {
        await useCase.Executar(id, requisicao);

        return NoContent();
    }
}

using FluentAssertions;
using MeuLivroDeReceitas.Application.UseCases.Conexao.RecusarConexao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilitarioParaOsTestes.Entidades;
using UtilitarioParaOsTestes.Hashids;
using UtilitarioParaOsTestes.Repositorios;
using UtilitarioParaOsTestes.UsuarioLogado;
using Xunit;

namespace UseCases.Test.Conexao;
public class RecusarConexaoUseCaseTeste
{
    [Fact]
    public async Task Validar_Sucesso()
    {
        var hashids = HashidsBuilder.Instance().Build();

        (var usuario, var _) = UsuarioBuilder.Construir();

        var useCase = CriarUseCase(usuario);

        var resultado = await useCase.Executar();

        resultado.Should().NotBeNullOrWhiteSpace();
        resultado.Should().Be(hashids.EncodeLong(usuario.Id));
    }

    private static RecusarConexaoUseCase CriarUseCase(MeuLivroDeReceitas.Domain.Entidades.Usuario usuario)
    {
        var usuarioLogado = UsuarioLogadoBuilder.Instancia().RecuperarUsuario(usuario).Construir();
        var repositorioWrite = CodigoWriteOnlyRepositorioBuilder.Instancia().Construir();
        var unidadeDeTrabalho = UnidadeDeTrabalhoBuilder.Instancia().Construir();
        var hashids = HashidsBuilder.Instance().Build();

        return new RecusarConexaoUseCase(repositorioWrite, usuarioLogado, unidadeDeTrabalho, hashids);
    }
}

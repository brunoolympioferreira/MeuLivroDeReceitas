using HashidsNet;
using MeuLivroDeReceitas.Application.Servicos.UsuarioLogado;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Repositorios.Codigo;
using MeuLivroDeReceitas.Domain.Repositorios.Conexao;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;

namespace MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;
public class QRCodeLidoUseCase : IQRCodeLidoUseCase
{
    private readonly IHashids _hashids;
    private readonly IConexaoReadOnlyRepositorio _repositirioConexao;
    private readonly ICodigoReadOnlyRepositorio _repositorio;
    private readonly IUsuarioLogado _usuarioLogado;

    public QRCodeLidoUseCase(
        ICodigoReadOnlyRepositorio repositorio, 
        IUsuarioLogado usuarioLogado, 
        IConexaoReadOnlyRepositorio repositirioConexao, 
        IHashids hashids)
    {
        _repositorio = repositorio;
        _usuarioLogado = usuarioLogado;
        _repositirioConexao = repositirioConexao;
        _hashids = hashids;
    }
    public async Task<(RespostaUsuarioConexaoJson usuarioParaSeConectar, string idUsuarioQueGerouQRCode)> Executar(string codigoConexao)
    {
        var usuarioLogado = await _usuarioLogado.RecuperarUsuario();
        var codigo = await _repositorio.RecuperarEntidadeCodigo(codigoConexao);

        await Validar(codigo, usuarioLogado);

        var usuarioParaSeConectar = new RespostaUsuarioConexaoJson
        {
            Id = _hashids.EncodeLong(usuarioLogado.Id),
            Nome = usuarioLogado.Nome
        };

        return (usuarioParaSeConectar, _hashids.EncodeLong(codigo.UsuarioId));
    }

    private async Task Validar(Domain.Entidades.Codigos codigo, Domain.Entidades.Usuario usuarioLogado)
    {
        if (codigo is null)
        {
            throw new MeuLivroDeReceitasException("");
        }

        if (codigo.UsuarioId == usuarioLogado.Id)
        {
            throw new MeuLivroDeReceitasException("");
        }

        var existeConexao = await _repositirioConexao.ExisteConexao(codigo.UsuarioId, usuarioLogado.Id);

        if (existeConexao)
        {
            throw new MeuLivroDeReceitasException("");
        }
    }
}

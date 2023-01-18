﻿using MeuLivroDeReceitas.Application.UseCases.Conexao.GerarQRCode;
using MeuLivroDeReceitas.Application.UseCases.Conexao.QRCodeLido;
using MeuLivroDeReceitas.Application.UseCases.Conexao.RecusarConexao;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Exceptions;
using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace MeuLivroDeReceitas.Api.WebSockets;

[Authorize(Policy = "UsuarioLogado")]
public class AdicionarConexao : Hub
{
    private readonly Broadcaster _broadcaster;

    private readonly IRecusarConexaoUseCase _recusarConexaoUseCase;
    private readonly IQRCodeLidoUseCase _qrCodeLidoUseCase;
    private readonly IGerarQRCodeUseCase _gerarQRCodeUseCase;
    private readonly IHubContext<AdicionarConexao> _hubContext;
    public AdicionarConexao(
        IHubContext<AdicionarConexao> hubContext, 
        IGerarQRCodeUseCase gerarQRCodeUseCase, 
        IQRCodeLidoUseCase qrCodeLidoUseCase, 
        IRecusarConexaoUseCase recusarConexaoUseCase)
    {
        _broadcaster = Broadcaster.Instance;
        _gerarQRCodeUseCase = gerarQRCodeUseCase;
        _hubContext = hubContext;
        _qrCodeLidoUseCase = qrCodeLidoUseCase;
        _recusarConexaoUseCase = recusarConexaoUseCase;
    }

    public async Task GetQRCode()
    {
        (var qrCode, var idUsuario) = await _gerarQRCodeUseCase.Executar();

        _broadcaster.InicializarConexao(_hubContext,idUsuario, Context.ConnectionId);

        await Clients.Caller.SendAsync("ResultadoQRCode", qrCode);
    }

    public async Task QRCodeLido(string codigoConexao)
    {
        try
        {
            (var usuarioParaSeConectar, var idUsuarioQueGerouQRCode) = await _qrCodeLidoUseCase.Executar(codigoConexao);

            var connectionId = _broadcaster.GetConnectionIdDoUsuario(idUsuarioQueGerouQRCode);

            _broadcaster.ResetarTempoExpiracao(connectionId);
            _broadcaster.SetConnectionIdUsuarioLeitorQRCode(idUsuarioQueGerouQRCode, Context.ConnectionId);

            await Clients.Client("").SendAsync("ResultadoQRCodeLido", usuarioParaSeConectar);
        }
        catch (MeuLivroDeReceitasException ex)
        {
            await Clients.Caller.SendAsync("Erro", ex.Message);
        }
        catch
        {
            await Clients.Caller.SendAsync("Erro", ResourceMensagensDeErro.ERRO_DESCONHECIDO);
        }
    }

    public async Task RecusarConexao()
    {
        try
        {
            var connectionIdUsuarioQueGerouQRCode = Context.ConnectionId;

            var usuarioId = await _recusarConexaoUseCase.Executar();

            var connectionIdUsuarioQueLeuQRCode = _broadcaster.Remover(connectionIdUsuarioQueGerouQRCode, usuarioId);

            await Clients.Client(connectionIdUsuarioQueLeuQRCode).SendAsync("OnConexaoRecusada");
        }
        catch (MeuLivroDeReceitasException ex)
        {
            await Clients.Caller.SendAsync("Erro", ex.Message);
        }
        catch
        {
            await Clients.Caller.SendAsync("Erro", ResourceMensagensDeErro.ERRO_DESCONHECIDO);
        }
    }
}

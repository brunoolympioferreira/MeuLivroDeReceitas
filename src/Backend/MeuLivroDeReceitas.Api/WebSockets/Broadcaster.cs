using MeuLivroDeReceitas.Exceptions.ExceptionsBase;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace MeuLivroDeReceitas.Api.WebSockets;

public class Broadcaster
{
    private readonly static Lazy<Broadcaster> _instance = new(() => new Broadcaster());

    public static Broadcaster Instance { get { return _instance.Value; } }

    private ConcurrentDictionary<string, object> _dictionary { get; set; }

    public Broadcaster()
    {
        _dictionary = new ConcurrentDictionary<string, object>();
    }

    public void InicializarConexao(IHubContext<AdicionarConexao> hubContext,string idUsuarioQueGerouQrCode, string connectionId)
    {
        var conexao = new Conexao(hubContext, connectionId);

        _dictionary.TryAdd(connectionId, conexao);
        _dictionary.TryAdd(idUsuarioQueGerouQrCode, connectionId);

        conexao.IniciarContagemTempo(CallbackTempoExpirado);
    }

    private void CallbackTempoExpirado(string connectionId)
    {
        _dictionary.TryRemove(connectionId, out _);
    }

    public string GetConnectionIdDoUsuario(string usuarioId)
    {
        if (!_dictionary.TryGetValue(usuarioId, out var connectionId))
        {
            throw new MeuLivroDeReceitasException("");
        }

        return connectionId.ToString();
    }
}

using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace MeuLivroDeReceitas.Api.WebSockets;

public class Broadcaster
{
    private readonly static Lazy<Broadcaster> _instance = new(() => new Broadcaster());

    public static Broadcaster Instance { get { return _instance.Value; } }

    private ConcurrentDictionary<string, Conexao> _dictionary { get; set; }

    public Broadcaster()
    {
        _dictionary = new ConcurrentDictionary<string, Conexao>();
    }

    public void InicializarConexao(IHubContext<AdicionarConexao> hubContext, string connectionId)
    {
        var conexao = new Conexao(hubContext, connectionId);

        _dictionary.TryAdd(connectionId, conexao);

        conexao.IniciarContagemTempo(CallbackTempoExpirado);
    }

    private void CallbackTempoExpirado(string connectionId)
    {
        _dictionary.TryRemove(connectionId, out _);
    }
}

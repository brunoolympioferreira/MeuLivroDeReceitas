using FluentAssertions;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace WebApi.Test.V1.Login.FazerLogin;

public class LoginTest : ControllerBase
{
    private const string METODO = "login";

    private MeuLivroDeReceitas.Domain.Entidades.Usuario _usuario;
    private string _senha;
    public LoginTest(MeuLivroReceitaWebApplicationFactory<Program> factory) : base(factory)
    {
        _usuario = factory.RecuperarUsuario();
        _senha = factory.RecuperarSenha();
    }

    [Fact]
    public async Task Validar_Sucesso()
    {
        var requisicao = new MeuLivroDeReceitas.Comunicacao.Requisicoes.RequisicaoLoginJson
        {
            Email = _usuario.Email,
            Senha = _senha
        };

        var resposta = await PostRequest(METODO, requisicao);

        resposta.StatusCode.Should().Be(HttpStatusCode.OK);

        await using var respostaBody = await resposta.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(respostaBody);

        responseData.RootElement.GetProperty("nome").GetString().Should().NotBeNullOrWhiteSpace().And.Be(_usuario.Nome);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();
    }
}

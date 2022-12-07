using MeuLivroDeReceitas.Application.Servicos.Token;

namespace UtilitarioParaOsTestes.Token;

public class TokenControllerBuilder
{
    public static TokenController Instancia()
    {
        return new TokenController(1000, "b0BBMHFhVWRAU2VPb0lDUWxPdzQydVpCelZDOGdZ");
    }

    public static TokenController TokenExpirado()
    {
        return new TokenController(0.016667, "b0BBMHFhVWRAU2VPb0lDUWxPdzQydVpCelZDOGdZ");
    }
}

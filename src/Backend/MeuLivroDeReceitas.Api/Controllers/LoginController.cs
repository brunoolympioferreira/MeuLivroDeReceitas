using MeuLivroDeReceitas.Application.UseCases.Usuario.Login.FazerLogin;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using Microsoft.AspNetCore.Mvc;

namespace MeuLivroDeReceitas.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : MeuLivroDeReceitasController
    {
        [HttpPost]
        [ProducesResponseType(typeof(RespostaLoginJson), StatusCodes.Status200OK)]
        public async Task<IActionResult> Login(
            [FromServices] ILoginUseCase useCase,
            [FromBody] RequisicaoLoginJson requisicao)
        {
            var resposta = await useCase.Executar(requisicao);

            return Ok(resposta);
        }
    }
}

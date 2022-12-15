using FluentValidation;
using FluentValidation.Results;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Registrar;
public class RegistrarReceitaValidator : AbstractValidator<RequisicaoRegistrarReceitaJson>
{
	public RegistrarReceitaValidator()
	{
        RuleFor(x => x).SetValidator(new ReceitaValidator());
    }
}

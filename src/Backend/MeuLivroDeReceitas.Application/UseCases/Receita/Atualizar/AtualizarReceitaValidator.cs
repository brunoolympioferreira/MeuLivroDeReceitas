using FluentValidation;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Domain.Entidades;
using MeuLivroDeReceitas.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeuLivroDeReceitas.Application.UseCases.Receita.Atualizar;
public class AtualizarReceitaValidator : AbstractValidator<RequisicaoReceitaJson>
{
    public AtualizarReceitaValidator()
    {
        RuleFor(x => x).SetValidator(new ReceitaValidator());
    }
}

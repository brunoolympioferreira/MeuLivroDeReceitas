using Bogus;
using MeuLivroDeReceitas.Comunicacao.Enum;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace UtilitarioParaOsTestes.Requisicoes;
public class RequisicaoReceitaBuilder
{
    public static RequisicaoReceitaJson Construir()
    {
        return new Faker<RequisicaoReceitaJson>()
            .RuleFor(c => c.Titulo, f => f.Commerce.Department())
            .RuleFor(c => c.Categoria, f => f.PickRandom<Categoria>())
            .RuleFor(c => c.ModoPreparo, f => f.Lorem.Paragraph())
            .RuleFor(c => c.Ingredientes, f => RandomIngredientes(f));
    }

    private static List<RequisicaoIngredientesJson> RandomIngredientes(Faker f)
    {
        List<RequisicaoIngredientesJson> ingredientes = new();

        for (int i = 0; i < f.Random.Int(1, 10); i++)
        {
            ingredientes.Add(new RequisicaoIngredientesJson
            {
                Produto = f.Commerce.ProductName(),
                Quantidade = $"{f.Random.Double(1, 10)} {f.Random.Word()}"
            });
        }

        return ingredientes;
    }
}

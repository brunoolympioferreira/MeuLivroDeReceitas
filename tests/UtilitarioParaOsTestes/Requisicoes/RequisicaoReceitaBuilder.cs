using Bogus;
using MeuLivroDeReceitas.Comunicacao.Enum;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;

namespace UtilitarioParaOsTestes.Requisicoes;
public class RequisicaoReceitaBuilder
{
    public static RequisicaoRegistrarReceitaJson Construir()
    {
        return new Faker<RequisicaoRegistrarReceitaJson>()
            .RuleFor(c => c.Titulo, f => f.Commerce.Department())
            .RuleFor(c => c.Categoria, f => f.PickRandom<Categoria>())
            .RuleFor(c => c.ModoPreparo, f => f.Lorem.Paragraph())
            .RuleFor(c => c.Ingredientes, f => RandomIngredientes(f));
    }

    private static List<RequisicaoRegistrarIngredientesJson> RandomIngredientes(Faker f)
    {
        List<RequisicaoRegistrarIngredientesJson> ingredientes = new();

        for (int i = 0; i < f.Random.Int(1, 10); i++)
        {
            ingredientes.Add(new RequisicaoRegistrarIngredientesJson
            {
                Produto = f.Commerce.ProductName(),
                Quantidade = $"{f.Random.Double(1, 10)} {f.Random.Word()}"
            });
        }

        return ingredientes;
    }
}

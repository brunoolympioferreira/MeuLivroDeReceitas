using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.Automapper;

namespace UtilitarioParaOsTestes.Mapper;

public class MapperBuilder
{
    public static IMapper Instancia()
    {
        var configuracao = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutomapperConfiguracao>();
        });

        return configuracao.CreateMapper();
    }
}

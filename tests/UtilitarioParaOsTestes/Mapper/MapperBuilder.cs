using AutoMapper;
using MeuLivroDeReceitas.Application.Servicos.Automapper;
using UtilitarioParaOsTestes.Hashids;

namespace UtilitarioParaOsTestes.Mapper;

public class MapperBuilder
{
    public static IMapper Instancia()
    {
        var hashids = HashidsBuilder.Instance().Build();

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutomapperConfiguracao(hashids));
        });
        return mockMapper.CreateMapper();
    }
}

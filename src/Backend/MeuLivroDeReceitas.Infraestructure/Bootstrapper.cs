using FluentMigrator.Runner;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Domain.Repositorios;
using MeuLivroDeReceitas.Infraestructure.AcessoRepositorio;
using MeuLivroDeReceitas.Infraestructure.AcessoRepositorio.Repositorio;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MeuLivroDeReceitas.Infraestructure;

public static class Bootstrapper
{
    public static void AddRepositorio(this IServiceCollection services, IConfiguration configurationManager)
    {
        AddFluentMigrator(services, configurationManager);

        AddContexto(services, configurationManager);
        AddUnidadeDeTrabalho(services);
        AddRepositorios(services);

    }

    private static void AddContexto(IServiceCollection services, IConfiguration configurationManager)
    {
        bool.TryParse(configurationManager.GetSection("Configuracoes:BancoDeDadosInMemory").Value, out bool bancoDeDadosInMemory);

        if (!bancoDeDadosInMemory)
        {
            var versaoServidor = new MySqlServerVersion(new Version(8, 0, 30));
            var connectionString = configurationManager.GetConexaoCompleta();

            services.AddDbContext<MeuLivroDeReceitasContext>(dbContextoOpcoes =>
            {
                dbContextoOpcoes.UseMySql(connectionString, versaoServidor);
            });
        }    
    }

    private static void AddUnidadeDeTrabalho(IServiceCollection services)
    {
        services.AddScoped<IUnidadeDeTrabalho, UnidadeDeTrabalho>();
    }

    private static void AddRepositorios(IServiceCollection services)
    {
        services.AddScoped<IUsuarioWriteOnlyRepositorio, UsuarioRepositorio>();
        services.AddScoped<IUsuarioReadOnlyRepositorio, UsuarioRepositorio>();
    }

    private static void AddFluentMigrator(IServiceCollection services, IConfiguration configurationManager)
    {
        bool.TryParse(configurationManager.GetSection("Configuracoes:BancoDeDadosInMemory").Value, out bool bancoDeDadosInMemory);

        if (!bancoDeDadosInMemory)
        {
            services.AddFluentMigratorCore().ConfigureRunner(c =>
            c.AddMySql5()
            .WithGlobalConnectionString(configurationManager.GetConexaoCompleta())
            .ScanIn(Assembly.Load("MeuLivroDeReceitas.Infraestructure")).For.All());
        }
    }
}

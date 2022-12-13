using HashidsNet;
using MeuLivroDeReceitas.Api.Filtros;
using MeuLivroDeReceitas.Api.Middleware;
using MeuLivroDeReceitas.Application;
using MeuLivroDeReceitas.Application.Servicos.Automapper;
using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Infraestructure;
using MeuLivroDeReceitas.Infraestructure.AcessoRepositorio;
using MeuLivroDeReceitas.Infraestructure.Migrations;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(option => option.LowercaseUrls = true);

builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication(builder.Configuration);

builder.Services.AddMvc(options => options.Filters.Add(typeof(FiltroDasExceptions)));

builder.Services.AddScoped(provider => new AutoMapper.MapperConfiguration(cfg =>
{
    cfg.AddProfile(new AutomapperConfiguracao(provider.GetService<IHashids>()));
}).CreateMapper());

#region AutoMapper Sem Converter HashId
//builder.Services.AddAutoMapper(typeof(AutomapperConfiguracao));
#endregion


builder.Services.AddScoped<UsuarioAutenticadoAttribute>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

AtualzarBaseDeDados();

app.UseMiddleware<CultureMiddleware>();

app.Run();

void AtualzarBaseDeDados()
{
    using var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

    using var context = serviceScope.ServiceProvider.GetService<MeuLivroDeReceitasContext>();

    bool? databaseInMemory = context?.Database?.ProviderName?.Equals("Microsoft.EntityFrameworkCore.InMemory");

    if (!databaseInMemory.HasValue || !databaseInMemory.Value)
    {
        var conexao = builder.Configuration.GetConexao();
        var nomeDatabase = builder.Configuration.GetNomeDatabase();

        Database.CriarDatabase(conexao, nomeDatabase);

        app.MigrateBancoDeDados();
    }
}

public partial class Program { }

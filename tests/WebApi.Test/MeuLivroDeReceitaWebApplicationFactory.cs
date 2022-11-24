﻿using MeuLivroDeReceitas.Infraestructure.AcessoRepositorio;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace WebApi.Test;

public class MeuLivroDeReceitaWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var descritor = services.SingleOrDefault(d => d.ServiceType == typeof(MeuLivroDeReceitasContext));
                if (descritor is not null)
                    services.Remove(descritor);

                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<MeuLivroDeReceitasContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                    options.UseInternalServiceProvider(provider);
                });

                var serviceProvider = services.BuildServiceProvider();


                using var scope = serviceProvider.CreateScope();
                var scopeService = scope.ServiceProvider;

                var database = scopeService.GetRequiredService<MeuLivroDeReceitasContext>();

                database.Database.EnsureDeleted();
            });
    }
}
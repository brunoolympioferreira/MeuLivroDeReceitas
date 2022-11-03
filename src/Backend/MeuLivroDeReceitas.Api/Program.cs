using MeuLivroDeReceitas.Domain.Extension;
using MeuLivroDeReceitas.Infraestructure;
using MeuLivroDeReceitas.Infraestructure.Migrations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRepositorio(builder.Configuration);

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

app.Run();

void AtualzarBaseDeDados()
{
    var conexao = builder.Configuration.GetConexao();
    var nomeDatabase = builder.Configuration.GetNomeDatabase();

    Database.CriarDatabase(conexao,nomeDatabase);

    app.MigrateBancoDeDados();
}

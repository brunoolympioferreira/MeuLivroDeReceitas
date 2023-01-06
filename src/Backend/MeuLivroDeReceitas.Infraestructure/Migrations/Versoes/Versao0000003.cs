using FluentMigrator;

namespace MeuLivroDeReceitas.Infraestructure.Migrations.Versoes;

[Migration((long)NumeroVersoes.AlterarTabelaReceitas, "Adicionando coluna tempo para o preparo")]
public class Versao0000003 : Migration
{
    public override void Down()
    {
    }

    public override void Up()
    {
        Alter.Table("receitas").AddColumn("TempoPreparo").AsInt32().NotNullable().WithDefaultValue(0);
    }
}

using AutoMapper;
using HashidsNet;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Comunicacao.Respostas;
using MeuLivroDeReceitas.Domain.Entidades;

namespace MeuLivroDeReceitas.Application.Servicos.Automapper;

public class AutomapperConfiguracao : Profile
{
	private readonly IHashids _hashIds;

    public AutomapperConfiguracao(IHashids hashIds)
	{
		_hashIds = hashIds;

		RequisicaoParaEntidade();
		EntidadeParaResposta();
    }

	private void RequisicaoParaEntidade()
	{
        CreateMap<RequisicaoRegistrarUsuarioJson, Usuario>()
            .ForMember(destino => destino.Senha, config => config.Ignore());

		CreateMap<RequisicaoReceitaJson, Receita>();
		CreateMap<RequisicaoIngredientesJson, Ingrediente>();
    }

	private void EntidadeParaResposta()
	{
		CreateMap<Receita, RespostaReceitaJson>()
			.ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashIds.EncodeLong(origem.Id)));

        CreateMap<Ingrediente, RespostaIngredienteJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashIds.EncodeLong(origem.Id)));

        CreateMap<Receita, RespostaReceitaDashboardJson>()
            .ForMember(destino => destino.Id, config => config.MapFrom(origem => _hashIds.EncodeLong(origem.Id)))
			.ForMember(destino => destino.QuantidadeIngredientes, config => config.MapFrom(origem => origem.Ingredientes.Count));

		CreateMap<Usuario, RespostaPerfilUsuarioJson>();
    }
}

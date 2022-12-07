using AutoMapper;
using MeuLivroDeReceitas.Comunicacao.Requisicoes;
using MeuLivroDeReceitas.Domain.Entidades;

namespace MeuLivroDeReceitas.Application.Servicos.Automapper;

public class AutomapperConfiguracao : Profile
{
	public AutomapperConfiguracao()
	{
		CreateMap<RequisicaoRegistrarUsuarioJson, Usuario>()
			.ForMember(destino => destino.Senha, config => config.Ignore());
	}
}

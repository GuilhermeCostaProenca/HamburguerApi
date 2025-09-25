using HamburguerApi.Dtos;
using HamburguerApi.Models;

namespace HamburguerApi.Mappings;

public static class HamburguerMapping
{
    public static HamburguerReadDto ToReadDto(this Hamburguer e) =>
        new(e.Id, e.Nome, e.Descricao, e.Preco, e.Ativo, e.CriadoEm);

    public static Hamburguer ToEntity(this HamburguerCreateDto dto) =>
        new() { Nome = dto.Nome, Descricao = dto.Descricao, Preco = dto.Preco, Ativo = dto.Ativo };

    public static void Apply(this Hamburguer e, HamburgurguerUpdateDto dto)
    {
        e.Nome = dto.Nome;
        e.Descricao = dto.Descricao;
        e.Preco = dto.Preco;
        e.Ativo = dto.Ativo;
    }
}

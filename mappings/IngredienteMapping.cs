using HamburguerApi.Dtos;
using HamburguerApi.Models;

namespace HamburguerApi.Mappings;

public static class IngredienteMapping
{
    public static IngredienteReadDto ToReadDto(this Ingrediente e) =>
        new(e.Id, e.Nome, e.Alergeno, e.Custo, e.Ativo);

    public static Ingrediente ToEntity(this IngredienteCreateDto dto) =>
        new() { Nome = dto.Nome, Alergeno = dto.Alergeno, Custo = dto.Custo, Ativo = dto.Ativo };

    public static void Apply(this Ingrediente e, IngredienteUpdateDto dto)
    {
        e.Nome = dto.Nome;
        e.Alergeno = dto.Alergeno;
        e.Custo = dto.Custo;
        e.Ativo = dto.Ativo;
    }
}

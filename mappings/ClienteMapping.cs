using HamburguerApi.Dtos;
using HamburguerApi.Models;

namespace HamburguerApi.Mappings;

public static class ClienteMapping
{
    public static ClienteReadDto ToReadDto(this Cliente e) =>
        new(e.Id, e.Nome, e.Telefone, e.Email, e.CriadoEm);

    public static Cliente ToEntity(this ClienteCreateDto dto) =>
        new() { Nome = dto.Nome, Telefone = dto.Telefone, Email = dto.Email };

    public static void Apply(this Cliente e, ClienteUpdateDto dto)
    {
        e.Nome = dto.Nome;
        e.Telefone = dto.Telefone;
        e.Email = dto.Email;
    }
}

using HamburguerApi.Dtos;
using HamburguerApi.Models;

namespace HamburguerApi.Mappings;

// ---------- BEBIDA ----------
public static class BebidaMappings
{
    public static BebidaReadDto ToReadDto(this Bebida e) =>
        new(e.Id, e.Nome, e.Preco, e.Ativo);

    public static Bebida ToEntity(this BebidaCreateDto d) =>
        new() { Nome = d.Nome.Trim(), Preco = d.Preco, Ativo = d.Ativo };

    public static void Apply(this Bebida e, BebidaUpdateDto d)
    {
        e.Nome = d.Nome.Trim();
        e.Preco = d.Preco;
        e.Ativo = d.Ativo;
    }
}

// ---------- ACOMPANHAMENTO ----------
public static class AcompanhamentoMappings
{
    public static AcompanhamentoReadDto ToReadDto(this Acompanhamento e) =>
        new(e.Id, e.Nome, e.Preco, e.Ativo);

    public static Acompanhamento ToEntity(this AcompanhamentoCreateDto d) =>
        new() { Nome = d.Nome.Trim(), Preco = d.Preco, Ativo = d.Ativo };

    public static void Apply(this Acompanhamento e, AcompanhamentoUpdateDto d)
    {
        e.Nome = d.Nome.Trim();
        e.Preco = d.Preco;
        e.Ativo = d.Ativo;
    }
}

// ---------- SOBREMESA ----------
public static class SobremesaMappings
{
    public static SobremesaReadDto ToReadDto(this Sobremesa e) =>
        new(e.Id, e.Nome, e.Preco, e.Ativo);

    public static Sobremesa ToEntity(this SobremesaCreateDto d) =>
        new() { Nome = d.Nome.Trim(), Preco = d.Preco, Ativo = d.Ativo };

    public static void Apply(this Sobremesa e, SobremesaUpdateDto d)
    {
        e.Nome = d.Nome.Trim();
        e.Preco = d.Preco;
        e.Ativo = d.Ativo;
    }
}

namespace HamburguerApi.Dtos;

public record PedidoItemCreateDto(
    int HamburguerId,
    int Qtde,
    decimal PrecoUnit,
    int[]? RemoverIngredientesIds
);

/// <summary>Tipo: Acompanhamento | Bebida | Sobremesa</summary>
public record PedidoExtraCreateDto(
    string Tipo,
    string Nome,
    int Qtde,
    decimal PrecoUnit
);

public record PedidoCreateDto(
    int? ClienteId,
    List<PedidoItemCreateDto> Itens,
    List<PedidoExtraCreateDto>? Extras
);

public record PedidoReadDto(
    int Id,
    int? ClienteId,
    string Status,
    decimal ValorTotal,
    DateTime CriadoEm
);

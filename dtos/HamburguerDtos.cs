namespace HamburguerApi.Dtos;

public record HamburguerReadDto(
    int Id,
    string Nome,
    string? Descricao,
    decimal Preco,
    bool Ativo,
    DateTime CriadoEm
);

public record HamburguerCreateDto(
    string Nome,
    string? Descricao,
    decimal Preco,
    bool Ativo = true
);

public record HamburguerUpdateDto( // <- nome correto
    int Id,
    string Nome,
    string? Descricao,
    decimal Preco,
    bool Ativo
);

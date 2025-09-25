namespace HamburguerApi.Dtos;

public record HamburguerReadDto(
    int Id, string Nome, string? Descricao, decimal Preco, bool Ativo, DateTime CriadoEm);

public record HamburguerCreateDto(
    string Nome, string? Descricao, decimal Preco, bool Ativo = true);

public record HamburgurguerUpdateDto( // sim, nome diferente pra evitar confusão
    int Id, string Nome, string? Descricao, decimal Preco, bool Ativo);

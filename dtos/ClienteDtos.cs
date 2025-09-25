namespace HamburguerApi.Dtos;

public record ClienteReadDto(int Id, string Nome, string? Telefone, string? Email, DateTime CriadoEm);
public record ClienteCreateDto(string Nome, string? Telefone, string? Email);
public record ClienteUpdateDto(int Id, string Nome, string? Telefone, string? Email);

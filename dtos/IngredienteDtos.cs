namespace HamburguerApi.Dtos;

public record IngredienteReadDto(int Id, string Nome, bool Alergeno, decimal Custo, bool Ativo);
public record IngredienteCreateDto(string Nome, bool Alergeno, decimal Custo, bool Ativo = true);
public record IngredienteUpdateDto(int Id, string Nome, bool Alergeno, decimal Custo, bool Ativo);

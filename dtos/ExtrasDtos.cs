namespace HamburguerApi.Dtos;

// ===== Bebida =====
public record BebidaReadDto(int Id, string Nome, decimal Preco, bool Ativo);
public record BebidaCreateDto(string Nome, decimal Preco, bool Ativo = true);
public record BebidaUpdateDto(int Id, string Nome, decimal Preco, bool Ativo);

// ===== Acompanhamento =====
public record AcompanhamentoReadDto(int Id, string Nome, decimal Preco, bool Ativo);
public record AcompanhamentoCreateDto(string Nome, decimal Preco, bool Ativo = true);
public record AcompanhamentoUpdateDto(int Id, string Nome, decimal Preco, bool Ativo);

// ===== Sobremesa =====
public record SobremesaReadDto(int Id, string Nome, decimal Preco, bool Ativo);
public record SobremesaCreateDto(string Nome, decimal Preco, bool Ativo = true);
public record SobremesaUpdateDto(int Id, string Nome, decimal Preco, bool Ativo);

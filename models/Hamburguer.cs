using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HamburguerApi.Models;

public class Hamburguer
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Nome { get; set; } = default!;

    [MaxLength(500)]
    public string? Descricao { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    // Valor padrão vem do DbContext: .HasDefaultValueSql("GETUTCDATE()")
    public DateTime CriadoEm { get; set; }

    public bool Ativo { get; set; } = true;
}

public class PedidoItemRemocaoIngrediente
{
    // Chave composta configurada no DbContext (PedidoItemId + IngredienteId)
    public int PedidoItemId { get; set; }
    public int IngredienteId { get; set; }

    public PedidoItem? PedidoItem { get; set; }
    public Ingrediente? Ingrediente { get; set; }
}

public enum TipoExtra { Acompanhamento, Bebida, Sobremesa }

public class PedidoExtra
{
    public int Id { get; set; }

    // FK + navegação (ajuda no Include e no cascade delete)
    public int PedidoId { get; set; }
    public Pedido? Pedido { get; set; }

    public TipoExtra Tipo { get; set; }

    [Required, MaxLength(160)]
    public string Nome { get; set; } = default!;

    public int Qtde { get; set; } = 1;

    [Column(TypeName = "decimal(10,2)")]
    public decimal PrecoUnit { get; set; }
}

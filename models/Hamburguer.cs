namespace HamburguerApi.Models;

public class Hamburguer
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public DateTime CriadoEm { get; set; } // <-- sem inicializador dinâmico
    public bool Ativo { get; set; } = true;
}

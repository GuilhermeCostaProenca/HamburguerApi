namespace HamburguerApi.Models;

public class Bebida
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public decimal Preco { get; set; }
    public bool Ativo { get; set; } = true;
}

public class Acompanhamento
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public decimal Preco { get; set; }
    public bool Ativo { get; set; } = true;
}

public class Sobremesa
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public decimal Preco { get; set; }
    public bool Ativo { get; set; } = true;
}

namespace HamburguerApi.Models;
public class Ingrediente
{
    public int Id { get; set; }
    public string Nome { get; set; } = default!;
    public bool Alergeno { get; set; } = false;
    public decimal Custo { get; set; }
    public bool Ativo { get; set; } = true;
}

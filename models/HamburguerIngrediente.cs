namespace HamburguerApi.Models;
public class HamburguerIngrediente
{
    public int HamburguerId { get; set; }
    public int IngredienteId { get; set; }
    public decimal Quantidade { get; set; }

    public Hamburguer? Hamburguer { get; set; }
    public Ingrediente? Ingrediente { get; set; }
}

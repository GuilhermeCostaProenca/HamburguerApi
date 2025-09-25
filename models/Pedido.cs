namespace HamburguerApi.Models;

public enum StatusPedido { Recebido, EmPreparo, Pronto, SaiuEntrega, Finalizado, Cancelado }

public class Pedido
{
    public int Id { get; set; }
    public int? ClienteId { get; set; }
    public Cliente? Cliente { get; set; }
    public StatusPedido Status { get; set; } = StatusPedido.Recebido;
    public decimal ValorTotal { get; set; }
    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    public List<PedidoItem> Itens { get; set; } = new();
}

public class PedidoItem
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int HamburguerId { get; set; }
    public int Qtde { get; set; }
    public decimal PrecoUnit { get; set; }
}

using HamburguerApi.Models;
using Microsoft.EntityFrameworkCore;

namespace HamburguerApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<Hamburguer> Hamburgueres => Set<Hamburguer>();
    public DbSet<Ingrediente> Ingredientes => Set<Ingrediente>();
    public DbSet<HamburguerIngrediente> HamburguerIngredientes => Set<HamburguerIngrediente>();
    public DbSet<Cliente> Clientes => Set<Cliente>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoItem> PedidoItens => Set<PedidoItem>();
    public DbSet<PedidoItemRemocaoIngrediente> PedidoItemRemocoes => Set<PedidoItemRemocaoIngrediente>();
    public DbSet<PedidoExtra> PedidoExtras => Set<PedidoExtra>();
    public DbSet<Bebida> Bebidas => Set<Bebida>();
    public DbSet<Acompanhamento> Acompanhamentos => Set<Acompanhamento>();
    public DbSet<Sobremesa> Sobremesas => Set<Sobremesa>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // HAMBURGUER
        mb.Entity<Hamburguer>(e =>
        {
            e.ToTable("Hamburguer");
            e.HasKey(h => h.Id);
            e.Property(h => h.Nome).IsRequired().HasMaxLength(120);
            e.Property(h => h.Descricao).HasMaxLength(500);
            e.Property(h => h.Preco).HasColumnType("decimal(10,2)");
            e.Property(h => h.CriadoEm).HasDefaultValueSql("GETUTCDATE()");
            e.Property(h => h.Ativo).HasDefaultValue(true);
        });

        // INGREDIENTE
        mb.Entity<Ingrediente>(e =>
        {
            e.ToTable("Ingrediente");
            e.HasKey(i => i.Id);
            e.Property(i => i.Nome).IsRequired().HasMaxLength(120);
            e.Property(i => i.Custo).HasColumnType("decimal(10,2)");
        });

        // HAMBURGUER x INGREDIENTE (N:N com payload Quantidade)
        mb.Entity<HamburguerIngrediente>(e =>
        {
            e.ToTable("HamburguerIngrediente");
            e.HasKey(x => new { x.HamburguerId, x.IngredienteId });
            e.Property(x => x.Quantidade).HasColumnType("decimal(10,2)").HasDefaultValue(1);

            e.HasOne(x => x.Hamburguer)
             .WithMany()
             .HasForeignKey(x => x.HamburguerId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Ingrediente)
             .WithMany()
             .HasForeignKey(x => x.IngredienteId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // CLIENTE
        mb.Entity<Cliente>(e =>
        {
            e.ToTable("Cliente");
            e.HasKey(c => c.Id);
            e.Property(c => c.Nome).IsRequired().HasMaxLength(160);
            e.Property(c => c.Email).HasMaxLength(200);
            e.Property(c => c.Telefone).HasMaxLength(30);
            e.Property(c => c.CriadoEm).HasDefaultValueSql("GETUTCDATE()");
        });

        // PEDIDO
        mb.Entity<Pedido>(e =>
        {
            e.ToTable("Pedido");
            e.HasKey(p => p.Id);
            e.Property(p => p.ValorTotal).HasColumnType("decimal(10,2)");
            e.Property(p => p.CriadoEm).HasDefaultValueSql("GETUTCDATE()");
            e.HasMany(p => p.Itens)
             .WithOne()
             .HasForeignKey(i => i.PedidoId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // PEDIDO ITEM
        mb.Entity<PedidoItem>(e =>
        {
            e.ToTable("PedidoItem");
            e.HasKey(i => i.Id);
            e.Property(i => i.PrecoUnit).HasColumnType("decimal(10,2)");
            // (opcional) se houver FK para Hamburguer:
            // e.HasOne<Hamburguer>().WithMany().HasForeignKey(i => i.HamburguerId).OnDelete(DeleteBehavior.Restrict);
        });

        // REMOÇÕES DE INGREDIENTE POR ITEM
        mb.Entity<PedidoItemRemocaoIngrediente>(e =>
        {
            e.ToTable("PedidoItemRemocaoIngrediente");
            e.HasKey(x => new { x.PedidoItemId, x.IngredienteId });

            e.HasOne(x => x.PedidoItem)
             .WithMany()
             .HasForeignKey(x => x.PedidoItemId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Ingrediente)
             .WithMany()
             .HasForeignKey(x => x.IngredienteId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // EXTRAS (lançamentos anexados ao pedido)
        mb.Entity<PedidoExtra>(e =>
        {
            e.ToTable("PedidoExtra");
            e.HasKey(x => x.Id);
            e.Property(x => x.PrecoUnit).HasColumnType("decimal(10,2)");

            e.HasOne<Pedido>()
             .WithMany()
             .HasForeignKey(x => x.PedidoId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        // TABELAS DE MENU (Bebida / Acompanhamento / Sobremesa) – usando propriedades reais
        mb.Entity<Bebida>(e =>
        {
            e.ToTable("Bebida");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).IsRequired().HasMaxLength(120);
            e.Property(x => x.Preco).HasColumnType("decimal(10,2)");
            e.Property(x => x.Ativo).HasDefaultValue(true);
        });

        mb.Entity<Acompanhamento>(e =>
        {
            e.ToTable("Acompanhamento");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).IsRequired().HasMaxLength(120);
            e.Property(x => x.Preco).HasColumnType("decimal(10,2)");
            e.Property(x => x.Ativo).HasDefaultValue(true);
        });

        mb.Entity<Sobremesa>(e =>
        {
            e.ToTable("Sobremesa");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).IsRequired().HasMaxLength(120);
            e.Property(x => x.Preco).HasColumnType("decimal(10,2)");
            e.Property(x => x.Ativo).HasDefaultValue(true);
        });
    }
}

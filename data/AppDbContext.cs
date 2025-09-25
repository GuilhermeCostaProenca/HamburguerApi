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

    protected override void OnModelCreating(ModelBuilder mb)
    {
        // HAMBURGUER
        mb.Entity<Hamburguer>(e =>
        {
            e.ToTable("Hamburguer");
            e.HasKey(h => h.Id);
            e.Property(h => h.Nome).IsRequired().HasMaxLength(120);
            e.Property(h => h.Preco).HasColumnType("decimal(10,2)");
            e.Property(h => h.CriadoEm).HasDefaultValueSql("GETUTCDATE()");
        });

        // INGREDIENTE
        mb.Entity<Ingrediente>(e =>
        {
            e.ToTable("Ingrediente");
            e.HasKey(i => i.Id);
            e.Property(i => i.Nome).IsRequired().HasMaxLength(120);
            e.Property(i => i.Custo).HasColumnType("decimal(10,2)");
        });

        // HAMBURGUER x INGREDIENTE (N:N) com payload Quantidade
        mb.Entity<HamburguerIngrediente>(e =>
        {
            e.ToTable("HamburguerIngrediente");
            e.HasKey(x => new { x.HamburguerId, x.IngredienteId });
            e.Property(x => x.Quantidade).HasColumnType("decimal(10,2)").HasDefaultValue(1);
            e.HasOne(x => x.Hamburguer).WithMany().HasForeignKey(x => x.HamburguerId);
            e.HasOne(x => x.Ingrediente).WithMany().HasForeignKey(x => x.IngredienteId);
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
    }
}

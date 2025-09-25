using HamburguerApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

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
        var seedDate = new DateTime(2024, 01, 01, 0, 0, 0, DateTimeKind.Utc);

        // ===== HAMBURGUER =====
        mb.Entity<Hamburguer>(e =>
        {
            e.ToTable("Hamburguer");
            e.HasKey(h => h.Id);
            e.Property(h => h.Nome).IsRequired().HasMaxLength(120);
            e.Property(h => h.Preco).HasColumnType("decimal(10,2)");
            e.Property(h => h.CriadoEm).HasDefaultValueSql("GETUTCDATE()");
        });

        // ===== INGREDIENTE =====
        mb.Entity<Ingrediente>(ConfigureIngrediente);
        static void ConfigureIngrediente(EntityTypeBuilder<Ingrediente> e)
        {
            e.ToTable("Ingrediente");
            e.HasKey(i => i.Id);
            e.Property(i => i.Nome).IsRequired().HasMaxLength(120);
            e.Property(i => i.Custo).HasColumnType("decimal(10,2)");
        }

        // ===== HAMBURGUER x INGREDIENTE =====
        mb.Entity<HamburguerIngrediente>(e =>
        {
            e.ToTable("HamburguerIngrediente");
            e.HasKey(x => new { x.HamburguerId, x.IngredienteId });
            e.Property(x => x.Quantidade).HasColumnType("decimal(10,2)").HasDefaultValue(1);
            e.HasOne(x => x.Hamburguer).WithMany().HasForeignKey(x => x.HamburguerId);
            e.HasOne(x => x.Ingrediente).WithMany().HasForeignKey(x => x.IngredienteId);
        });

        // ===== CLIENTE =====
        mb.Entity<Cliente>(e =>
        {
            e.ToTable("Cliente");
            e.HasKey(c => c.Id);
            e.Property(c => c.Nome).IsRequired().HasMaxLength(160);
            e.Property(c => c.Email).HasMaxLength(200);
            e.Property(c => c.Telefone).HasMaxLength(30);
            e.Property(c => c.CriadoEm).HasDefaultValueSql("GETUTCDATE()");
        });

        // ===== PEDIDO =====
        mb.Entity<Pedido>(e =>
        {
            e.ToTable("Pedido");
            e.HasKey(p => p.Id);
            e.Property(p => p.ValorTotal).HasColumnType("decimal(10,2)");
            e.Property(p => p.CriadoEm).HasDefaultValueSql("GETUTCDATE()");
            e.HasMany(p => p.Itens).WithOne().HasForeignKey(i => i.PedidoId);
        });

        // ===== PEDIDO ITEM =====
        mb.Entity<PedidoItem>(e =>
        {
            e.ToTable("PedidoItem");
            e.HasKey(i => i.Id);
            e.Property(i => i.PrecoUnit).HasColumnType("decimal(10,2)");
        });

        // ===== REMOÇÃO DE INGREDIENTE NO ITEM =====
        mb.Entity<PedidoItemRemocaoIngrediente>(e =>
        {
            e.ToTable("PedidoItemRemocaoIngrediente");
            e.HasKey(x => new { x.PedidoItemId, x.IngredienteId });
            e.HasOne(x => x.PedidoItem).WithMany().HasForeignKey(x => x.PedidoItemId);
            e.HasOne(x => x.Ingrediente).WithMany().HasForeignKey(x => x.IngredienteId);
        });

        // ===== EXTRAS (pedido) =====
        mb.Entity<PedidoExtra>(e =>
        {
            e.ToTable("PedidoExtra");
            e.HasKey(x => x.Id);
            e.Property(x => x.PrecoUnit).HasColumnType("decimal(10,2)");
        });

        // ===== CATÁLOGOS (Bebida/Acompanhamento/Sobremesa) =====
        mb.Entity<Bebida>(e =>
        {
            e.ToTable("Bebida");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).IsRequired().HasMaxLength(120);
            e.Property(x => x.Preco).HasColumnType("decimal(10,2)");
        });

        mb.Entity<Acompanhamento>(e =>
        {
            e.ToTable("Acompanhamento");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).IsRequired().HasMaxLength(120);
            e.Property(x => x.Preco).HasColumnType("decimal(10,2)");
        });

        mb.Entity<Sobremesa>(e =>
        {
            e.ToTable("Sobremesa");
            e.HasKey(x => x.Id);
            e.Property(x => x.Nome).IsRequired().HasMaxLength(120);
            e.Property(x => x.Preco).HasColumnType("decimal(10,2)");
        });

        // ===========================
        // SEED DATA (IDs altos)
        // ===========================

        // HAMBURGUERES
        mb.Entity<Hamburguer>().HasData(
            new Hamburguer { Id = 101, Nome = "Cheeseburger", Descricao = "Clássico com queijo", Preco = 25.90m, Ativo = true, CriadoEm = seedDate },
            new Hamburguer { Id = 102, Nome = "X-Bacon", Descricao = "Carne + bacon crocante + cheddar", Preco = 29.90m, Ativo = true, CriadoEm = seedDate },
            new Hamburguer { Id = 103, Nome = "Veggie Burger", Descricao = "Grão-de-bico, salada e molho da casa", Preco = 27.50m, Ativo = true, CriadoEm = seedDate }
        );

        // INGREDIENTES
        mb.Entity<Ingrediente>().HasData(
            new Ingrediente { Id = 201, Nome = "Pão Brioche", Custo = 3.50m, Alergeno = false, Ativo = true },
            new Ingrediente { Id = 202, Nome = "Carne 160g", Custo = 10.00m, Alergeno = false, Ativo = true },
            new Ingrediente { Id = 203, Nome = "Queijo Cheddar", Custo = 4.00m, Alergeno = true, Ativo = true },
            new Ingrediente { Id = 204, Nome = "Bacon", Custo = 4.50m, Alergeno = false, Ativo = true },
            new Ingrediente { Id = 205, Nome = "Alface", Custo = 0.90m, Alergeno = false, Ativo = true },
            new Ingrediente { Id = 206, Nome = "Tomate", Custo = 1.10m, Alergeno = false, Ativo = true },
            new Ingrediente { Id = 207, Nome = "Molho da Casa", Custo = 0.80m, Alergeno = false, Ativo = true }
        );

        // RELAÇÃO HAMBURGUER x INGREDIENTES
        mb.Entity<HamburguerIngrediente>().HasData(
            // Cheeseburger (101)
            new HamburguerIngrediente { HamburguerId = 101, IngredienteId = 201, Quantidade = 1 }, // pão
            new HamburguerIngrediente { HamburguerId = 101, IngredienteId = 202, Quantidade = 1 }, // carne
            new HamburguerIngrediente { HamburguerId = 101, IngredienteId = 203, Quantidade = 1 }, // cheddar
            new HamburguerIngrediente { HamburguerId = 101, IngredienteId = 205, Quantidade = 1 }, // alface
            new HamburguerIngrediente { HamburguerId = 101, IngredienteId = 206, Quantidade = 1 }, // tomate
            // X-Bacon (102)
            new HamburguerIngrediente { HamburguerId = 102, IngredienteId = 201, Quantidade = 1 },
            new HamburguerIngrediente { HamburguerId = 102, IngredienteId = 202, Quantidade = 1 },
            new HamburguerIngrediente { HamburguerId = 102, IngredienteId = 203, Quantidade = 1 },
            new HamburguerIngrediente { HamburguerId = 102, IngredienteId = 204, Quantidade = 1 },
            new HamburguerIngrediente { HamburguerId = 102, IngredienteId = 207, Quantidade = 1 },
            // Veggie (103)
            new HamburguerIngrediente { HamburguerId = 103, IngredienteId = 201, Quantidade = 1 },
            new HamburguerIngrediente { HamburguerId = 103, IngredienteId = 205, Quantidade = 1 },
            new HamburguerIngrediente { HamburguerId = 103, IngredienteId = 206, Quantidade = 1 },
            new HamburguerIngrediente { HamburguerId = 103, IngredienteId = 207, Quantidade = 1 }
        );

        // CLIENTES
        mb.Entity<Cliente>().HasData(
            new Cliente { Id = 301, Nome = "João Silva", Email = "joao@email.com", Telefone = "11999999999", CriadoEm = seedDate },
            new Cliente { Id = 302, Nome = "Maria Souza", Email = "maria@email.com", Telefone = "11988888888", CriadoEm = seedDate },
            new Cliente { Id = 303, Nome = "Ana Ribeiro", Email = "ana.r@example.com", Telefone = "11977777777", CriadoEm = seedDate }
        );

        // CATÁLOGO: BEBIDAS
        mb.Entity<Bebida>().HasData(
            new Bebida { Id = 401, Nome = "Coca-Cola Lata", Preco = 7.50m, Ativo = true },
            new Bebida { Id = 402, Nome = "Guaraná Lata", Preco = 6.90m, Ativo = true },
            new Bebida { Id = 403, Nome = "Água 500ml", Preco = 4.50m, Ativo = true }
        );

        // CATÁLOGO: ACOMPANHAMENTOS
        mb.Entity<Acompanhamento>().HasData(
            new Acompanhamento { Id = 501, Nome = "Batata Média", Preco = 12.90m, Ativo = true },
            new Acompanhamento { Id = 502, Nome = "Batata Grande", Preco = 16.90m, Ativo = true },
            new Acompanhamento { Id = 503, Nome = "Nuggets (6un)", Preco = 14.90m, Ativo = true }
        );

        // CATÁLOGO: SOBREMESAS
        mb.Entity<Sobremesa>().HasData(
            new Sobremesa { Id = 601, Nome = "Brownie", Preco = 10.90m, Ativo = true },
            new Sobremesa { Id = 602, Nome = "Sorvete Baunilha", Preco = 8.50m, Ativo = true }
        );
    }
}

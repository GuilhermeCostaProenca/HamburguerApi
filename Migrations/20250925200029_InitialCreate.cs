using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HamburguerApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hamburguer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Preco = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CriadoEm = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hamburguer", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Hamburguer",
                columns: new[] { "Id", "Ativo", "CriadoEm", "Descricao", "Nome", "Preco" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 9, 25, 20, 0, 28, 749, DateTimeKind.Utc).AddTicks(9481), "Clássico com queijo", "Cheeseburger", 22.90m },
                    { 2, true, new DateTime(2025, 9, 25, 20, 0, 28, 750, DateTimeKind.Utc).AddTicks(235), "Com bacon crocante", "Bacon Burger", 27.50m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hamburguer");
        }
    }
}

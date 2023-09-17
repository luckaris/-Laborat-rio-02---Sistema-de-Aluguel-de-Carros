using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cliente.API.Migrations;

/// <inheritdoc />
public partial class InitialMigration : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Enderecos",
            columns: table => new
            {
                CEP = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                CpfCliente = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                Rua = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                Numero = table.Column<int>(type: "int", maxLength: 6, nullable: false),
                Bairro = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Cidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Estado = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Enderecos", x => x.CEP);
            });

        migrationBuilder.CreateTable(
            name: "Clientes",
            columns: table => new
            {
                Cpf = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                Nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Rg = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                Profissao = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                Empregador = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                EnderecoCEP = table.Column<string>(type: "nvarchar(8)", nullable: false),
                RendimentoMensal = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clientes", x => x.Cpf);
                table.ForeignKey(
                    name: "FK_Clientes_Enderecos_EnderecoCEP",
                    column: x => x.EnderecoCEP,
                    principalTable: "Enderecos",
                    principalColumn: "CEP",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Clientes_EnderecoCEP",
            table: "Clientes",
            column: "EnderecoCEP");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Clientes");

        migrationBuilder.DropTable(
            name: "Enderecos");
    }
}

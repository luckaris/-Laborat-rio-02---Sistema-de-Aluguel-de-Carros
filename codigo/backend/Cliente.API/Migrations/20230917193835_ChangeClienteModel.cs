using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cliente.API.Migrations;

/// <inheritdoc />
public partial class ChangeClienteModel : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropForeignKey(
            name: "FK_Clientes_Enderecos_EnderecoCEP",
            table: "Clientes");

        migrationBuilder.DropIndex(
            name: "IX_Clientes_EnderecoCEP",
            table: "Clientes");

        migrationBuilder.AlterColumn<string>(
            name: "EnderecoCEP",
            table: "Clientes",
            type: "nvarchar(max)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(8)");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterColumn<string>(
            name: "EnderecoCEP",
            table: "Clientes",
            type: "nvarchar(8)",
            nullable: false,
            oldClrType: typeof(string),
            oldType: "nvarchar(max)");

        migrationBuilder.CreateIndex(
            name: "IX_Clientes_EnderecoCEP",
            table: "Clientes",
            column: "EnderecoCEP");

        migrationBuilder.AddForeignKey(
            name: "FK_Clientes_Enderecos_EnderecoCEP",
            table: "Clientes",
            column: "EnderecoCEP",
            principalTable: "Enderecos",
            principalColumn: "CEP",
            onDelete: ReferentialAction.Cascade);
    }
}

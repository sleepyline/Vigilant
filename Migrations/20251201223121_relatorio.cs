using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VigiLant.Migrations
{
    /// <inheritdoc />
    public partial class relatorio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Titulo",
                table: "Relatorios",
                newName: "Nome");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Relatorios",
                newName: "Titulo");
        }
    }
}

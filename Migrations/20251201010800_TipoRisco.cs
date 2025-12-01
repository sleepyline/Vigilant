using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VigiLant.Migrations
{
    /// <inheritdoc />
    public partial class TipoRisco : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TipoRisco",
                table: "Riscos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TipoRisco",
                table: "Riscos");
        }
    }
}

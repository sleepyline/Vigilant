using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VigiLant.Migrations
{
    /// <inheritdoc />
    public partial class Configurar : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MqttUser",
                table: "Equipamentos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "MqttPassword",
                table: "Equipamentos",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "MqttUser",
                keyValue: null,
                column: "MqttUser",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "MqttUser",
                table: "Equipamentos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "Equipamentos",
                keyColumn: "MqttPassword",
                keyValue: null,
                column: "MqttPassword",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "MqttPassword",
                table: "Equipamentos",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}

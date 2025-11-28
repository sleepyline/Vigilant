using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VigiLant.Migrations
{
    /// <inheritdoc />
    public partial class EquipeNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MqttPassword",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "MqttUser",
                table: "Equipamentos");

            migrationBuilder.AddColumn<string>(
                name: "ClientId",
                table: "Equipamentos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "Equipamentos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "Equipamentos");

            migrationBuilder.AddColumn<string>(
                name: "MqttPassword",
                table: "Equipamentos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "MqttUser",
                table: "Equipamentos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}

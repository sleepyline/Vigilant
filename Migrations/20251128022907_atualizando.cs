using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VigiLant.Migrations
{
    /// <inheritdoc />
    public partial class atualizando : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MqttConfigurations");

            migrationBuilder.DropColumn(
                name: "BrokerHost",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "DataCadastro",
                table: "Equipamentos");

            migrationBuilder.RenameColumn(
                name: "MqttTopic",
                table: "Equipamentos",
                newName: "Topico");

            migrationBuilder.RenameColumn(
                name: "ClientId",
                table: "Equipamentos",
                newName: "Endereco");

            migrationBuilder.RenameColumn(
                name: "BrokerPort",
                table: "Equipamentos",
                newName: "Porta");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Topico",
                table: "Equipamentos",
                newName: "MqttTopic");

            migrationBuilder.RenameColumn(
                name: "Porta",
                table: "Equipamentos",
                newName: "BrokerPort");

            migrationBuilder.RenameColumn(
                name: "Endereco",
                table: "Equipamentos",
                newName: "ClientId");

            migrationBuilder.AddColumn<string>(
                name: "BrokerHost",
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

            migrationBuilder.CreateTable(
                name: "MqttConfigurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    BaseTopic = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BrokerHost = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    BrokerPort = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MqttConfigurations", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}

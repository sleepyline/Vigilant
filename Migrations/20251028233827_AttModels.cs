using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VigiLant.Migrations
{
    /// <inheritdoc />
    public partial class AttModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequerTreinamentoEspecifico",
                table: "Riscos");

            migrationBuilder.DropColumn(
                name: "ColaboradorResponsavelId",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "EmManutencao",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "SetorId",
                table: "Colaboradores");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Riscos",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "Nivel",
                table: "Riscos",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "AcoesMitigacao",
                table: "Riscos",
                newName: "NivelGravidade");

            migrationBuilder.RenameColumn(
                name: "NumeroSerie",
                table: "Equipamentos",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "DataAquisicao",
                table: "Equipamentos",
                newName: "DataManutencao");

            migrationBuilder.RenameColumn(
                name: "NomeCompleto",
                table: "Colaboradores",
                newName: "Telefone");

            migrationBuilder.RenameColumn(
                name: "Contato",
                table: "Colaboradores",
                newName: "Nome");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataIdentificacao",
                table: "Riscos",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Equipamentos",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Departamento",
                table: "Colaboradores",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Colaboradores",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataIdentificacao",
                table: "Riscos");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Equipamentos");

            migrationBuilder.DropColumn(
                name: "Departamento",
                table: "Colaboradores");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Colaboradores");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Riscos",
                newName: "Tipo");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Riscos",
                newName: "Nivel");

            migrationBuilder.RenameColumn(
                name: "NivelGravidade",
                table: "Riscos",
                newName: "AcoesMitigacao");

            migrationBuilder.RenameColumn(
                name: "Tipo",
                table: "Equipamentos",
                newName: "NumeroSerie");

            migrationBuilder.RenameColumn(
                name: "DataManutencao",
                table: "Equipamentos",
                newName: "DataAquisicao");

            migrationBuilder.RenameColumn(
                name: "Telefone",
                table: "Colaboradores",
                newName: "NomeCompleto");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Colaboradores",
                newName: "Contato");

            migrationBuilder.AddColumn<bool>(
                name: "RequerTreinamentoEspecifico",
                table: "Riscos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ColaboradorResponsavelId",
                table: "Equipamentos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "EmManutencao",
                table: "Equipamentos",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "SetorId",
                table: "Colaboradores",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

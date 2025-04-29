﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIDevSteam.Migrations
{
    /// <inheritdoc />
    public partial class CupomCarrinho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Cupons");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Cupons",
                newName: "Nome");

            migrationBuilder.AddColumn<int>(
                name: "LimiteUso",
                table: "Cupons",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CuponsCarrinhos",
                columns: table => new
                {
                    CupomCarrinhoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CarrinhoId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CupomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DataAplicacao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuponsCarrinhos", x => x.CupomCarrinhoId);
                    table.ForeignKey(
                        name: "FK_CuponsCarrinhos_Carrinhos_CarrinhoId",
                        column: x => x.CarrinhoId,
                        principalTable: "Carrinhos",
                        principalColumn: "CarrinhoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CuponsCarrinhos_Cupons_CupomId",
                        column: x => x.CupomId,
                        principalTable: "Cupons",
                        principalColumn: "CupomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuponsCarrinhos_CarrinhoId",
                table: "CuponsCarrinhos",
                column: "CarrinhoId");

            migrationBuilder.CreateIndex(
                name: "IX_CuponsCarrinhos_CupomId",
                table: "CuponsCarrinhos",
                column: "CupomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuponsCarrinhos");

            migrationBuilder.DropColumn(
                name: "LimiteUso",
                table: "Cupons");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Cupons",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Cupons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}

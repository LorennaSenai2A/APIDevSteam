using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace APIDevSteam.Migrations
{
    /// <inheritdoc />
    public partial class Usuario : Migration
    {
        public object UserName { get; internal set; }
        public string Email { get; internal set; }
        public string NormalizedEmail { get; internal set; }
        public int NormalizedUserName { get; internal set; }
        public bool EmailConfirmed { get; internal set; }
        public bool PhoneNumberConfirmed { get; internal set; }
        public bool TwoFactorEnabled { get; internal set; }
        public bool LockoutEnabled { get; internal set; }
        public object PhoneNumber { get; internal set; }
        public object NomeCompleto { get; internal set; }
        public object DataNascimento { get; internal set; }
        public object Id { get; internal set; }

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateOnly>(
                name: "DataNascimento",
                table: "AspNetUsers",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<string>(
                name: "NomeCompleto",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DataNascimento",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NomeCompleto",
                table: "AspNetUsers");
        }
    }
}

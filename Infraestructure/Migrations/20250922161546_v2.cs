using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Age",
                table: "Users");

            migrationBuilder.AddColumn<DateOnly>(
                name: "Birthday",
                table: "Users",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<bool>(
                name: "IsPreemium",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "AlreadySeen",
                table: "Contents",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Birthday",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsPreemium",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "AlreadySeen",
                table: "Contents");

            migrationBuilder.AddColumn<int>(
                name: "Age",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

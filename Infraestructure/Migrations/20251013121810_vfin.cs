using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class vfin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Contents");

            migrationBuilder.DropColumn(
                name: "category",
                table: "Contents");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "FavoriteContents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "FavoriteContents");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Contents");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Contents",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "category",
                table: "Contents",
                type: "int",
                nullable: true);
        }
    }
}

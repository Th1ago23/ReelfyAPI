using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class chega : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContentUser");

            migrationBuilder.DropColumn(
                name: "AlreadySeen",
                table: "Contents");

            migrationBuilder.CreateTable(
                name: "AlreadySeenContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ContentId = table.Column<int>(type: "int", nullable: false),
                    ViewedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlreadySeenContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlreadySeenContents_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlreadySeenContents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ContentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FavoriteContents_Contents_ContentId",
                        column: x => x.ContentId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteContents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlreadySeenContents_ContentId",
                table: "AlreadySeenContents",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_AlreadySeenContents_UserId",
                table: "AlreadySeenContents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteContents_ContentId",
                table: "FavoriteContents",
                column: "ContentId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteContents_UserId",
                table: "FavoriteContents",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlreadySeenContents");

            migrationBuilder.DropTable(
                name: "FavoriteContents");

            migrationBuilder.AddColumn<bool>(
                name: "AlreadySeen",
                table: "Contents",
                type: "bit",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ContentUser",
                columns: table => new
                {
                    FavoriteContentsId = table.Column<int>(type: "int", nullable: false),
                    FavoritedByUsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentUser", x => new { x.FavoriteContentsId, x.FavoritedByUsersId });
                    table.ForeignKey(
                        name: "FK_ContentUser_Contents_FavoriteContentsId",
                        column: x => x.FavoriteContentsId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentUser_Users_FavoritedByUsersId",
                        column: x => x.FavoritedByUsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentUser_FavoritedByUsersId",
                table: "ContentUser",
                column: "FavoritedByUsersId");
        }
    }
}

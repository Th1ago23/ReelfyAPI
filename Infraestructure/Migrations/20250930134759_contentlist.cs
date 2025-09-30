using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class contentlist : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentUser_Contents_ContentsId",
                table: "ContentUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentUser_Users_UserId",
                table: "ContentUser");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ContentUser",
                newName: "FavoritedByUsersId");

            migrationBuilder.RenameColumn(
                name: "ContentsId",
                table: "ContentUser",
                newName: "FavoriteContentsId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentUser_UserId",
                table: "ContentUser",
                newName: "IX_ContentUser_FavoritedByUsersId");

            migrationBuilder.CreateTable(
                name: "ContentsLists",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentsLists", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContentsLists_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContentContentsList",
                columns: table => new
                {
                    ContentsId = table.Column<int>(type: "int", nullable: false),
                    InUserContentListsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentContentsList", x => new { x.ContentsId, x.InUserContentListsId });
                    table.ForeignKey(
                        name: "FK_ContentContentsList_ContentsLists_InUserContentListsId",
                        column: x => x.InUserContentListsId,
                        principalTable: "ContentsLists",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentContentsList_Contents_ContentsId",
                        column: x => x.ContentsId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentContentsList_InUserContentListsId",
                table: "ContentContentsList",
                column: "InUserContentListsId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentsLists_UserId",
                table: "ContentsLists",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentUser_Contents_FavoriteContentsId",
                table: "ContentUser",
                column: "FavoriteContentsId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentUser_Users_FavoritedByUsersId",
                table: "ContentUser",
                column: "FavoritedByUsersId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentUser_Contents_FavoriteContentsId",
                table: "ContentUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ContentUser_Users_FavoritedByUsersId",
                table: "ContentUser");

            migrationBuilder.DropTable(
                name: "ContentContentsList");

            migrationBuilder.DropTable(
                name: "ContentsLists");

            migrationBuilder.RenameColumn(
                name: "FavoritedByUsersId",
                table: "ContentUser",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "FavoriteContentsId",
                table: "ContentUser",
                newName: "ContentsId");

            migrationBuilder.RenameIndex(
                name: "IX_ContentUser_FavoritedByUsersId",
                table: "ContentUser",
                newName: "IX_ContentUser_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentUser_Contents_ContentsId",
                table: "ContentUser",
                column: "ContentsId",
                principalTable: "Contents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContentUser_Users_UserId",
                table: "ContentUser",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

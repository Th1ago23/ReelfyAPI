using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class n : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Casts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Casts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    category = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Crews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crews", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Streamings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Streamings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(90)", maxLength: 90, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContentUser",
                columns: table => new
                {
                    ContentsId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContentUser", x => new { x.ContentsId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ContentUser_Contents_ContentsId",
                        column: x => x.ContentsId,
                        principalTable: "Contents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContentUser_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Preferences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Preferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CastPreference",
                columns: table => new
                {
                    CastsId = table.Column<int>(type: "int", nullable: false),
                    PreferencesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CastPreference", x => new { x.CastsId, x.PreferencesId });
                    table.ForeignKey(
                        name: "FK_CastPreference_Casts_CastsId",
                        column: x => x.CastsId,
                        principalTable: "Casts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CastPreference_Preferences_PreferencesId",
                        column: x => x.PreferencesId,
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CrewPreference",
                columns: table => new
                {
                    CrewsId = table.Column<int>(type: "int", nullable: false),
                    PreferencesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CrewPreference", x => new { x.CrewsId, x.PreferencesId });
                    table.ForeignKey(
                        name: "FK_CrewPreference_Crews_CrewsId",
                        column: x => x.CrewsId,
                        principalTable: "Crews",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CrewPreference_Preferences_PreferencesId",
                        column: x => x.PreferencesId,
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GenrePreference",
                columns: table => new
                {
                    GenresId = table.Column<int>(type: "int", nullable: false),
                    PreferencesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GenrePreference", x => new { x.GenresId, x.PreferencesId });
                    table.ForeignKey(
                        name: "FK_GenrePreference_Genres_GenresId",
                        column: x => x.GenresId,
                        principalTable: "Genres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GenrePreference_Preferences_PreferencesId",
                        column: x => x.PreferencesId,
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreferenceStreaming",
                columns: table => new
                {
                    PreferencesId = table.Column<int>(type: "int", nullable: false),
                    StreamingsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreferenceStreaming", x => new { x.PreferencesId, x.StreamingsId });
                    table.ForeignKey(
                        name: "FK_PreferenceStreaming_Preferences_PreferencesId",
                        column: x => x.PreferencesId,
                        principalTable: "Preferences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PreferenceStreaming_Streamings_StreamingsId",
                        column: x => x.StreamingsId,
                        principalTable: "Streamings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CastPreference_PreferencesId",
                table: "CastPreference",
                column: "PreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_ContentUser_UserId",
                table: "ContentUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CrewPreference_PreferencesId",
                table: "CrewPreference",
                column: "PreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_GenrePreference_PreferencesId",
                table: "GenrePreference",
                column: "PreferencesId");

            migrationBuilder.CreateIndex(
                name: "IX_Preferences_UserId",
                table: "Preferences",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PreferenceStreaming_StreamingsId",
                table: "PreferenceStreaming",
                column: "StreamingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CastPreference");

            migrationBuilder.DropTable(
                name: "ContentUser");

            migrationBuilder.DropTable(
                name: "CrewPreference");

            migrationBuilder.DropTable(
                name: "GenrePreference");

            migrationBuilder.DropTable(
                name: "PreferenceStreaming");

            migrationBuilder.DropTable(
                name: "Casts");

            migrationBuilder.DropTable(
                name: "Contents");

            migrationBuilder.DropTable(
                name: "Crews");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Preferences");

            migrationBuilder.DropTable(
                name: "Streamings");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

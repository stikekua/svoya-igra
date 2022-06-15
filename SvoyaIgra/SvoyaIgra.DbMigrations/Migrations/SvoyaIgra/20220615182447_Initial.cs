using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SvoyaIgra.DbMigrations.Migrations.SvoyaIgra
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "SvoyaIgra");

            migrationBuilder.CreateTable(
                name: "Theme",
                schema: "SvoyaIgra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Theme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                schema: "SvoyaIgra",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    ThemeId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MultimediaId = table.Column<string>(type: "nchar(36)", fixedLength: true, maxLength: 36, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Theme_ThemeId",
                        column: x => x.ThemeId,
                        principalSchema: "SvoyaIgra",
                        principalTable: "Theme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "SvoyaIgra",
                table: "Theme",
                columns: new[] { "Id", "Difficulty", "Name" },
                values: new object[] { 1, 1, "Tema1" });

            migrationBuilder.InsertData(
                schema: "SvoyaIgra",
                table: "Theme",
                columns: new[] { "Id", "Difficulty", "Name" },
                values: new object[] { 2, 2, "Tema2" });

            migrationBuilder.InsertData(
                schema: "SvoyaIgra",
                table: "Question",
                columns: new[] { "Id", "Answer", "Difficulty", "MultimediaId", "Text", "ThemeId", "Type" },
                values: new object[] { 1, "Answer!", 1, "00000000-0000-0000-0000-000000000000", "Question?", 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_Question_ThemeId",
                schema: "SvoyaIgra",
                table: "Question",
                column: "ThemeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Question",
                schema: "SvoyaIgra");

            migrationBuilder.DropTable(
                name: "Theme",
                schema: "SvoyaIgra");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SvoyaIgra.DbMigrations.Migrations.QBank
{
    public partial class CreateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "QBank");

            migrationBuilder.CreateTable(
                name: "Author",
                schema: "QBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Author", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Topic",
                schema: "QBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Topic_Author_AuthorId",
                        column: x => x.AuthorId,
                        principalSchema: "QBank",
                        principalTable: "Author",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                schema: "QBank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Difficulty = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MultimediaId = table.Column<string>(type: "nchar(36)", fixedLength: true, maxLength: 36, nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Topic_TopicId",
                        column: x => x.TopicId,
                        principalSchema: "QBank",
                        principalTable: "Topic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_TopicId",
                schema: "QBank",
                table: "Question",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_Topic_AuthorId",
                schema: "QBank",
                table: "Topic",
                column: "AuthorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Question",
                schema: "QBank");

            migrationBuilder.DropTable(
                name: "Topic",
                schema: "QBank");

            migrationBuilder.DropTable(
                name: "Author",
                schema: "QBank");
        }
    }
}

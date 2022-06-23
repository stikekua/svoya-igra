using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SvoyaIgra.DbMigrations.Migrations.QBank
{
    public partial class AddInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "QBank",
                table: "Author",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Test" });

            migrationBuilder.InsertData(
                schema: "QBank",
                table: "Topic",
                columns: new[] { "Id", "AuthorId", "Difficulty", "Name" },
                values: new object[] { 1, 1, 1, "Tema1" });

            migrationBuilder.InsertData(
                schema: "QBank",
                table: "Question",
                columns: new[] { "Id", "Answer", "Difficulty", "MultimediaId", "Text", "TopicId", "Type" },
                values: new object[,]
                {
                    { 1, "Answer1!", 1, "00000000-0000-0000-0000-000000000000", "Question1?", 1, 1 },
                    { 2, "Answer2!", 2, "00000000-0000-0000-0000-000000000000", "Question2?", 1, 1 },
                    { 3, "Answer3!", 3, "00000000-0000-0000-0000-000000000000", "Question3?", 1, 1 },
                    { 4, "Answer4!", 4, "00000000-0000-0000-0000-000000000000", "Question4?", 1, 1 },
                    { 5, "Answer5!", 5, "00000000-0000-0000-0000-000000000000", "Question5?", 1, 1 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "QBank",
                table: "Question",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "QBank",
                table: "Question",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                schema: "QBank",
                table: "Question",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                schema: "QBank",
                table: "Question",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                schema: "QBank",
                table: "Question",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                schema: "QBank",
                table: "Topic",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                schema: "QBank",
                table: "Author",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}

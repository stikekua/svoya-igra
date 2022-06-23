using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SvoyaIgra.DbMigrations.Migrations.QBank
{
    public partial class AuthorReferenceChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Topic_Author_AuthorId",
                schema: "QBank",
                table: "Topic");

            migrationBuilder.DropIndex(
                name: "IX_Topic_AuthorId",
                schema: "QBank",
                table: "Topic");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                schema: "QBank",
                table: "Topic");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                schema: "QBank",
                table: "Question",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "QBank",
                table: "Question",
                keyColumn: "Id",
                keyValue: 1,
                column: "AuthorId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "QBank",
                table: "Question",
                keyColumn: "Id",
                keyValue: 2,
                column: "AuthorId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "QBank",
                table: "Question",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Answer", "AuthorId" },
                values: new object[] { "Answer3!", 1 });

            migrationBuilder.UpdateData(
                schema: "QBank",
                table: "Question",
                keyColumn: "Id",
                keyValue: 4,
                column: "AuthorId",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "QBank",
                table: "Question",
                keyColumn: "Id",
                keyValue: 5,
                column: "AuthorId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Question_AuthorId",
                schema: "QBank",
                table: "Question",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Author_AuthorId",
                schema: "QBank",
                table: "Question",
                column: "AuthorId",
                principalSchema: "QBank",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Author_AuthorId",
                schema: "QBank",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_AuthorId",
                schema: "QBank",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                schema: "QBank",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                schema: "QBank",
                table: "Topic",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "QBank",
                table: "Question",
                keyColumn: "Id",
                keyValue: 3,
                column: "Answer",
                value: "Answer!");

            migrationBuilder.UpdateData(
                schema: "QBank",
                table: "Topic",
                keyColumn: "Id",
                keyValue: 1,
                column: "AuthorId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Topic_AuthorId",
                schema: "QBank",
                table: "Topic",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Topic_Author_AuthorId",
                schema: "QBank",
                table: "Topic",
                column: "AuthorId",
                principalSchema: "QBank",
                principalTable: "Author",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

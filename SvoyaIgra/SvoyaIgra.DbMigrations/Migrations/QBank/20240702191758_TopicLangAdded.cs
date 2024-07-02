using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SvoyaIgra.DbMigrations.Migrations.QBank
{
    public partial class TopicLangAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lang",
                schema: "QBank",
                table: "Topic",
                type: "nvarchar(2)",
                maxLength: 2,
                nullable: false,
                defaultValue: "ru");

            migrationBuilder.UpdateData(
                schema: "QBank",
                table: "Topic",
                keyColumn: "Id",
                keyValue: 1,
                column: "Lang",
                value: "ru");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lang",
                schema: "QBank",
                table: "Topic");
        }
    }
}

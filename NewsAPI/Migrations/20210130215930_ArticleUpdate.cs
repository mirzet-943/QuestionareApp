using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsAPI.Migrations
{
    public partial class ArticleUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WriterFullName",
                table: "Articles");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_WriterId",
                table: "Articles",
                column: "WriterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Articles_Users_WriterId",
                table: "Articles",
                column: "WriterId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Articles_Users_WriterId",
                table: "Articles");

            migrationBuilder.DropIndex(
                name: "IX_Articles_WriterId",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "WriterFullName",
                table: "Articles",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace NewsAPI.Migrations
{
    public partial class candidate_questionare : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questionares_Users_UserId",
                table: "Questionares");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Questionares",
                newName: "CandidateId");

            migrationBuilder.RenameIndex(
                name: "IX_Questionares_UserId",
                table: "Questionares",
                newName: "IX_Questionares_CandidateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questionares_Candidates_CandidateId",
                table: "Questionares",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "CandidateId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Questionares_Candidates_CandidateId",
                table: "Questionares");

            migrationBuilder.RenameColumn(
                name: "CandidateId",
                table: "Questionares",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Questionares_CandidateId",
                table: "Questionares",
                newName: "IX_Questionares_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Questionares_Users_UserId",
                table: "Questionares",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace SuaveKeys.Infrastructure.Data.Migrations
{
    public partial class AuthCodeNewProperties3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "AuthorizationCodes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationCodes_UserId",
                table: "AuthorizationCodes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationCodes_Users_UserId",
                table: "AuthorizationCodes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationCodes_Users_UserId",
                table: "AuthorizationCodes");

            migrationBuilder.DropIndex(
                name: "IX_AuthorizationCodes_UserId",
                table: "AuthorizationCodes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AuthorizationCodes");
        }
    }
}

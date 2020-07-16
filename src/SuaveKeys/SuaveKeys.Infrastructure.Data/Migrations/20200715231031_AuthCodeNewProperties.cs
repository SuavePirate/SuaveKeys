using Microsoft.EntityFrameworkCore.Migrations;

namespace SuaveKeys.Infrastructure.Data.Migrations
{
    public partial class AuthCodeNewProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChallengeMethod",
                table: "AuthorizationCodes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "AuthorizationCodes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChallengeMethod",
                table: "AuthorizationCodes");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AuthorizationCodes");
        }
    }
}

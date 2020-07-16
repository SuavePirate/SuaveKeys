using Microsoft.EntityFrameworkCore.Migrations;

namespace SuaveKeys.Infrastructure.Data.Migrations
{
    public partial class AuthCodeNewProperties2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OriginHost",
                table: "AuthClients",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginHost",
                table: "AuthClients");
        }
    }
}

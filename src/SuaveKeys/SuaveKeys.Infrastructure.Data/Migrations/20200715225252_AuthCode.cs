using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SuaveKeys.Infrastructure.Data.Migrations
{
    public partial class AuthCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthCodeRedirectUrlHost",
                table: "AuthClients",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TokenRedirectUrlHost",
                table: "AuthClients",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AuthorizationCodes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    ChallengeCode = table.Column<string>(nullable: true),
                    AuthClientId = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    ExpirationDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorizationCodes_AuthClients_AuthClientId",
                        column: x => x.AuthClientId,
                        principalTable: "AuthClients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationCodes_AuthClientId",
                table: "AuthorizationCodes",
                column: "AuthClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizationCodes");

            migrationBuilder.DropColumn(
                name: "AuthCodeRedirectUrlHost",
                table: "AuthClients");

            migrationBuilder.DropColumn(
                name: "TokenRedirectUrlHost",
                table: "AuthClients");
        }
    }
}

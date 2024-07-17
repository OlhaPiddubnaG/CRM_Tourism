using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddListUsersToClientAndClientStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientUser",
                columns: table => new
                {
                    ClientsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientUser", x => new { x.ClientsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ClientUser_Clients_ClientsId",
                        column: x => x.ClientsId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientUser_UsersId",
                table: "ClientUser",
                column: "UsersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientUser");
        }
    }
}

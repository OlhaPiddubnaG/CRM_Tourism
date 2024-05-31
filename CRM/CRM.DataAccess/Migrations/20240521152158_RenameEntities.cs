using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RenameEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Users",
                newName: "UpdatedUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Stays",
                newName: "UpdatedUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Payments",
                newName: "UpdatedUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "PassportInfo",
                newName: "UpdatedUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "OrderStatusHistory",
                newName: "UpdatedUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Companies",
                newName: "UpdatedUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Clients",
                newName: "UpdatedUserId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "ClientPrivateDatas",
                newName: "UpdatedUserId");

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedUserId",
                table: "Users",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "Stays",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedUserId",
                table: "Stays",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedUserId",
                table: "Payments",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "PassportInfo",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedUserId",
                table: "PassportInfo",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedUserId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedUserId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "OrderStatusHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedUserId",
                table: "OrderStatusHistory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "Companies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedUserId",
                table: "Companies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "Clients",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedUserId",
                table: "Clients",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "ClientPrivateDatas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedUserId",
                table: "ClientPrivateDatas",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleType = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TaskStatus = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedUserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserId",
                table: "UserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTasks_UserId",
                table: "UserTasks",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTasks");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "PassportInfo");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "PassportInfo");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "OrderStatusHistory");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "OrderStatusHistory");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "ClientPrivateDatas");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "ClientPrivateDatas");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserId",
                table: "Users",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserId",
                table: "Stays",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserId",
                table: "Payments",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserId",
                table: "PassportInfo",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserId",
                table: "OrderStatusHistory",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserId",
                table: "Companies",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserId",
                table: "Clients",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "UpdatedUserId",
                table: "ClientPrivateDatas",
                newName: "UserId");

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RoleType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    TaskStatus = table.Column<int>(type: "integer", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Roles_UserId",
                table: "Roles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_UserId",
                table: "Tasks",
                column: "UserId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ChangeToAuditableInTouroperator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Touroperators_Companies_CompanyId",
                table: "Touroperators");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Touroperators",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Touroperators",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedUserId",
                table: "Touroperators",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Touroperators",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeletedUserId",
                table: "Touroperators",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Touroperators",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UpdatedUserId",
                table: "Touroperators",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_Touroperators_Companies_CompanyId",
                table: "Touroperators",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Touroperators_Companies_CompanyId",
                table: "Touroperators");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Touroperators");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "Touroperators");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Touroperators");

            migrationBuilder.DropColumn(
                name: "DeletedUserId",
                table: "Touroperators");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Touroperators");

            migrationBuilder.DropColumn(
                name: "UpdatedUserId",
                table: "Touroperators");

            migrationBuilder.AlterColumn<Guid>(
                name: "CompanyId",
                table: "Touroperators",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Touroperators_Companies_CompanyId",
                table: "Touroperators",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

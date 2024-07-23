using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddListTouroperatorToCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Touroperators_TouroperatorId",
                table: "Orders");

            migrationBuilder.AddColumn<Guid?>(
                name: "CompanyId",
                table: "Touroperators",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TouroperatorId",
                table: "Orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateIndex(
                name: "IX_Touroperators_CompanyId",
                table: "Touroperators",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Touroperators_TouroperatorId",
                table: "Orders",
                column: "TouroperatorId",
                principalTable: "Touroperators",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Touroperators_Companies_CompanyId",
                table: "Touroperators",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Touroperators_TouroperatorId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Touroperators_Companies_CompanyId",
                table: "Touroperators");

            migrationBuilder.DropIndex(
                name: "IX_Touroperators_CompanyId",
                table: "Touroperators");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Touroperators");

            migrationBuilder.AlterColumn<Guid>(
                name: "TouroperatorId",
                table: "Orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Touroperators_TouroperatorId",
                table: "Orders",
                column: "TouroperatorId",
                principalTable: "Touroperators",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddTouroperatorIdToOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Touroperators_Orders_OrderId",
                table: "Touroperators");

            migrationBuilder.DropIndex(
                name: "IX_Touroperators_OrderId",
                table: "Touroperators");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Touroperators");

            migrationBuilder.AddColumn<Guid?>(
                name: "TouroperatorId",
                table: "Orders",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TouroperatorId",
                table: "Orders",
                column: "TouroperatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Touroperators_TouroperatorId",
                table: "Orders",
                column: "TouroperatorId",
                principalTable: "Touroperators",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull); // or ReferentialAction.Restrict
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Touroperators_TouroperatorId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TouroperatorId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TouroperatorId",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Touroperators",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Touroperators_OrderId",
                table: "Touroperators",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Touroperators_Orders_OrderId",
                table: "Touroperators",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
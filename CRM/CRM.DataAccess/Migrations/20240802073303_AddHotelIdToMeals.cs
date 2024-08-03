using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelIdToMeals : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Hotels_HotelId",
                table: "Meals");

            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Stays_StaysId",
                table: "Meals");

            migrationBuilder.DropIndex(
                name: "IX_Meals_StaysId",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "StaysId",
                table: "Meals");

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelId",
                table: "Meals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Hotels_HotelId",
                table: "Meals");

            migrationBuilder.AlterColumn<Guid>(
                name: "HotelId",
                table: "Meals",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddColumn<Guid>(
                name: "StaysId",
                table: "Meals",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Meals_StaysId",
                table: "Meals",
                column: "StaysId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Hotels_HotelId",
                table: "Meals",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Stays_StaysId",
                table: "Meals",
                column: "StaysId",
                principalTable: "Stays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

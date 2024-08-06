using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddHotelAndRoomType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Stays");

            migrationBuilder.AddColumn<Guid>(
                name: "HotelId",
                table: "Stays",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "HotelId",
                table: "Meals",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RoomTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoomTypes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Hotels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomTypeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Comment = table.Column<string>(type: "text", nullable: false),
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
                    table.PrimaryKey("PK_Hotels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Hotels_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Hotels_RoomTypes_RoomTypeId",
                        column: x => x.RoomTypeId,
                        principalTable: "RoomTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stays_HotelId",
                table: "Stays",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_HotelId",
                table: "Meals",
                column: "HotelId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_CompanyId",
                table: "Hotels",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Hotels_RoomTypeId",
                table: "Hotels",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RoomTypes_CompanyId",
                table: "RoomTypes",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Meals_Hotels_HotelId",
                table: "Meals",
                column: "HotelId",
                principalTable: "Hotels",
                principalColumn: "Id");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Meals_Hotels_HotelId",
                table: "Meals");

            migrationBuilder.DropForeignKey(
                name: "FK_Stays_Hotels_HotelId",
                table: "Stays");

            migrationBuilder.DropTable(
                name: "Hotels");

            migrationBuilder.DropTable(
                name: "RoomTypes");

            migrationBuilder.DropIndex(
                name: "IX_Stays_HotelId",
                table: "Stays");

            migrationBuilder.DropIndex(
                name: "IX_Meals_HotelId",
                table: "Meals");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Stays");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Meals");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Stays",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}

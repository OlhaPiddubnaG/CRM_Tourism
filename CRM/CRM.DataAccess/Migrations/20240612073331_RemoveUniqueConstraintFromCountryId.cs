using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRM.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUniqueConstraintFromCountryId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_CountryId",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CountryId",
                table: "Clients",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_CountryId",
                table: "Clients");

            migrationBuilder.CreateIndex(
                name: "IX_Clients_CountryId",
                table: "Clients",
                column: "CountryId",
                unique: true);
        }
    }
}

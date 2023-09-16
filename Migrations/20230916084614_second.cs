using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Group_BeanBooking.Migrations
{
    /// <inheritdoc />
    public partial class second : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_RestaurantAreaId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RestaurantAreaId",
                table: "Reservations",
                column: "RestaurantAreaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_RestaurantAreaId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_RestaurantAreaId",
                table: "Reservations",
                column: "RestaurantAreaId",
                unique: true);
        }
    }
}

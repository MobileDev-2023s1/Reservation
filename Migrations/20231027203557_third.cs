using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Group_BeanBooking.Migrations
{
    /// <inheritdoc />
    public partial class third : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationRestaurantTable_Reservations_reservationsId",
                table: "ReservationRestaurantTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationRestaurantTable",
                table: "ReservationRestaurantTable");

            migrationBuilder.DropIndex(
                name: "IX_ReservationRestaurantTable_reservationsId",
                table: "ReservationRestaurantTable");

            migrationBuilder.RenameColumn(
                name: "reservationsId",
                table: "ReservationRestaurantTable",
                newName: "ReservationsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationRestaurantTable",
                table: "ReservationRestaurantTable",
                columns: new[] { "ReservationsId", "RestaurantTablesId" });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRestaurantTable_RestaurantTablesId",
                table: "ReservationRestaurantTable",
                column: "RestaurantTablesId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationRestaurantTable_Reservations_ReservationsId",
                table: "ReservationRestaurantTable",
                column: "ReservationsId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReservationRestaurantTable_Reservations_ReservationsId",
                table: "ReservationRestaurantTable");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReservationRestaurantTable",
                table: "ReservationRestaurantTable");

            migrationBuilder.DropIndex(
                name: "IX_ReservationRestaurantTable_RestaurantTablesId",
                table: "ReservationRestaurantTable");

            migrationBuilder.RenameColumn(
                name: "ReservationsId",
                table: "ReservationRestaurantTable",
                newName: "reservationsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReservationRestaurantTable",
                table: "ReservationRestaurantTable",
                columns: new[] { "RestaurantTablesId", "reservationsId" });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationRestaurantTable_reservationsId",
                table: "ReservationRestaurantTable",
                column: "reservationsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReservationRestaurantTable_Reservations_reservationsId",
                table: "ReservationRestaurantTable",
                column: "reservationsId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

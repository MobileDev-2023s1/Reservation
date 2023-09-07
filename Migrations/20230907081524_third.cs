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
                name: "FK_Reservations_Sittings_SittingID",
                table: "Reservations");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Sittings_SittingID",
                table: "Reservations",
                column: "SittingID",
                principalTable: "Sittings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Sittings_SittingID",
                table: "Reservations");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Sittings_SittingID",
                table: "Reservations",
                column: "SittingID",
                principalTable: "Sittings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

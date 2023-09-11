using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Group_BeanBooking.Migrations
{
    /// <inheritdoc />
    public partial class fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Pax",
                table: "Reservations",
                newName: "Guests");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Guests",
                table: "Reservations",
                newName: "Pax");
        }
    }
}

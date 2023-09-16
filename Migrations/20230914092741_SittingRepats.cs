using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Group_BeanBooking.Migrations
{
    /// <inheritdoc />
    public partial class SittingRepats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                table: "Sittings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Interval",
                table: "Sittings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RepeatPattern",
                table: "Sittings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Repeats",
                table: "Sittings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "Sittings");

            migrationBuilder.DropColumn(
                name: "Interval",
                table: "Sittings");

            migrationBuilder.DropColumn(
                name: "RepeatPattern",
                table: "Sittings");

            migrationBuilder.DropColumn(
                name: "Repeats",
                table: "Sittings");
        }
    }
}

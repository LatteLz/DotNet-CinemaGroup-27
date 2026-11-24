using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaManagement.Migrations
{
    public partial class AllowComboOnlyBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ShowtimeId",
                table: "Bookings",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM \"Payments\" WHERE \"BookingId\" IN (SELECT \"Id\" FROM \"Bookings\" WHERE \"ShowtimeId\" IS NULL);");
            migrationBuilder.Sql("DELETE FROM \"BookingCombos\" WHERE \"BookingId\" IN (SELECT \"Id\" FROM \"Bookings\" WHERE \"ShowtimeId\" IS NULL);");
            migrationBuilder.Sql("DELETE FROM \"Bookings\" WHERE \"ShowtimeId\" IS NULL;");

            migrationBuilder.AlterColumn<int>(
                name: "ShowtimeId",
                table: "Bookings",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}

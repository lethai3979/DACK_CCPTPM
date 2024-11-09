using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoWheels_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDriverBookingProps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "PrePayment",
                table: "DriversBooking",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PrePayment",
                table: "DriversBooking");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoWheels_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_DriversBooking_DriverBookingId",
                table: "Invoices");

            migrationBuilder.AlterColumn<int>(
                name: "DriverBookingId",
                table: "Invoices",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_DriversBooking_DriverBookingId",
                table: "Invoices",
                column: "DriverBookingId",
                principalTable: "DriverBookings",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_DriversBooking_DriverBookingId",
                table: "Invoices");

            migrationBuilder.AlterColumn<int>(
                name: "DriverBookingId",
                table: "Invoices",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_DriversBooking_DriverBookingId",
                table: "Invoices",
                column: "DriverBookingId",
                principalTable: "DriverBookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

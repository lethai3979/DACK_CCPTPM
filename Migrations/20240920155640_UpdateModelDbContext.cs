using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GoWheels_WebAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_UserId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Post_PostId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Promotions_PromotionId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_AspNetUsers_UserId",
                table: "Favorite");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorite_Post_PostId",
                table: "Favorite");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_AspNetUsers_UserId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_CarTypes_CarTypeId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_Companies_CompanyId",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_PostAmenity_Amentities_AmenityId",
                table: "PostAmenity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostAmenity_Post_PostId",
                table: "PostAmenity");

            migrationBuilder.DropForeignKey(
                name: "FK_PostImage_Post_PostId",
                table: "PostImage");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_AspNetUsers_UserId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Post_PostId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_Post_PostId",
                table: "Report");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_ReportType_ReportTypeId",
                table: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportType",
                table: "ReportType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Report",
                table: "Report");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rating",
                table: "Rating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostImage",
                table: "PostImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostAmenity",
                table: "PostAmenity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Post");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorite",
                table: "Favorite");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "ReportType",
                newName: "ReportTypes");

            migrationBuilder.RenameTable(
                name: "Report",
                newName: "Reports");

            migrationBuilder.RenameTable(
                name: "Rating",
                newName: "Ratings");

            migrationBuilder.RenameTable(
                name: "PostImage",
                newName: "PostImages");

            migrationBuilder.RenameTable(
                name: "PostAmenity",
                newName: "PostAmenities");

            migrationBuilder.RenameTable(
                name: "Post",
                newName: "Posts");

            migrationBuilder.RenameTable(
                name: "Favorite",
                newName: "Favorites");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "Bookings");

            migrationBuilder.RenameIndex(
                name: "IX_Report_ReportTypeId",
                table: "Reports",
                newName: "IX_Reports_ReportTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Report_PostId",
                table: "Reports",
                newName: "IX_Reports_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_UserId",
                table: "Ratings",
                newName: "IX_Ratings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Rating_PostId",
                table: "Ratings",
                newName: "IX_Ratings_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostImage_PostId",
                table: "PostImages",
                newName: "IX_PostImages_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostAmenity_PostId",
                table: "PostAmenities",
                newName: "IX_PostAmenities_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostAmenity_AmenityId",
                table: "PostAmenities",
                newName: "IX_PostAmenities_AmenityId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_UserId",
                table: "Posts",
                newName: "IX_Posts_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_CompanyId",
                table: "Posts",
                newName: "IX_Posts_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Post_CarTypeId",
                table: "Posts",
                newName: "IX_Posts_CarTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorite_UserId",
                table: "Favorites",
                newName: "IX_Favorites_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorite_PostId",
                table: "Favorites",
                newName: "IX_Favorites_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_UserId",
                table: "Bookings",
                newName: "IX_Bookings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_PromotionId",
                table: "Bookings",
                newName: "IX_Bookings_PromotionId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_PostId",
                table: "Bookings",
                newName: "IX_Bookings_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportTypes",
                table: "ReportTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reports",
                table: "Reports",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostImages",
                table: "PostImages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostAmenities",
                table: "PostAmenities",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Invoices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ReturnOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BookingId = table.Column<int>(type: "int", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invoices_Bookings_BookingId",
                        column: x => x.BookingId,
                        principalTable: "Bookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_BookingId",
                table: "Invoices",
                column: "BookingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Posts_PostId",
                table: "Bookings",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Promotions_PromotionId",
                table: "Bookings",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Posts_PostId",
                table: "Favorites",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostAmenities_Amentities_AmenityId",
                table: "PostAmenities",
                column: "AmenityId",
                principalTable: "Amentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostAmenities_Posts_PostId",
                table: "PostAmenities",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_CarTypes_CarTypeId",
                table: "Posts",
                column: "CarTypeId",
                principalTable: "CarTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Companies_CompanyId",
                table: "Posts",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId",
                table: "Ratings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ratings_Posts_PostId",
                table: "Ratings",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_ReportTypes_ReportTypeId",
                table: "Reports",
                column: "ReportTypeId",
                principalTable: "ReportTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_AspNetUsers_UserId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Posts_PostId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Promotions_PromotionId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_UserId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Posts_PostId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_PostAmenities_Amentities_AmenityId",
                table: "PostAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_PostAmenities_Posts_PostId",
                table: "PostAmenities");

            migrationBuilder.DropForeignKey(
                name: "FK_PostImages_Posts_PostId",
                table: "PostImages");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_UserId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_CarTypes_CarTypeId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Companies_CompanyId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_AspNetUsers_UserId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Ratings_Posts_PostId",
                table: "Ratings");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Posts_PostId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_ReportTypes_ReportTypeId",
                table: "Reports");

            migrationBuilder.DropTable(
                name: "Invoices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReportTypes",
                table: "ReportTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reports",
                table: "Reports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ratings",
                table: "Ratings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostImages",
                table: "PostImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PostAmenities",
                table: "PostAmenities");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Favorites",
                table: "Favorites");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "ReportTypes",
                newName: "ReportType");

            migrationBuilder.RenameTable(
                name: "Reports",
                newName: "Report");

            migrationBuilder.RenameTable(
                name: "Ratings",
                newName: "Rating");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Post");

            migrationBuilder.RenameTable(
                name: "PostImages",
                newName: "PostImage");

            migrationBuilder.RenameTable(
                name: "PostAmenities",
                newName: "PostAmenity");

            migrationBuilder.RenameTable(
                name: "Favorites",
                newName: "Favorite");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "Booking");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_ReportTypeId",
                table: "Report",
                newName: "IX_Report_ReportTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Reports_PostId",
                table: "Report",
                newName: "IX_Report_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_UserId",
                table: "Rating",
                newName: "IX_Rating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Ratings_PostId",
                table: "Rating",
                newName: "IX_Rating_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_UserId",
                table: "Post",
                newName: "IX_Post_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CompanyId",
                table: "Post",
                newName: "IX_Post_CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_CarTypeId",
                table: "Post",
                newName: "IX_Post_CarTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_PostImages_PostId",
                table: "PostImage",
                newName: "IX_PostImage_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostAmenities_PostId",
                table: "PostAmenity",
                newName: "IX_PostAmenity_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_PostAmenities_AmenityId",
                table: "PostAmenity",
                newName: "IX_PostAmenity_AmenityId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_UserId",
                table: "Favorite",
                newName: "IX_Favorite_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Favorites_PostId",
                table: "Favorite",
                newName: "IX_Favorite_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_UserId",
                table: "Booking",
                newName: "IX_Booking_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_PromotionId",
                table: "Booking",
                newName: "IX_Booking_PromotionId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_PostId",
                table: "Booking",
                newName: "IX_Booking_PostId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReportType",
                table: "ReportType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Report",
                table: "Report",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rating",
                table: "Rating",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Post",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostImage",
                table: "PostImage",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PostAmenity",
                table: "PostAmenity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Favorite",
                table: "Favorite",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_UserId",
                table: "Booking",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Post_PostId",
                table: "Booking",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Promotions_PromotionId",
                table: "Booking",
                column: "PromotionId",
                principalTable: "Promotions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_AspNetUsers_UserId",
                table: "Favorite",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorite_Post_PostId",
                table: "Favorite",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_AspNetUsers_UserId",
                table: "Post",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_CarTypes_CarTypeId",
                table: "Post",
                column: "CarTypeId",
                principalTable: "CarTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Companies_CompanyId",
                table: "Post",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostAmenity_Amentities_AmenityId",
                table: "PostAmenity",
                column: "AmenityId",
                principalTable: "Amentities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostAmenity_Post_PostId",
                table: "PostAmenity",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PostImage_Post_PostId",
                table: "PostImage",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_AspNetUsers_UserId",
                table: "Rating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Post_PostId",
                table: "Rating",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_Post_PostId",
                table: "Report",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_ReportType_ReportTypeId",
                table: "Report",
                column: "ReportTypeId",
                principalTable: "ReportType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

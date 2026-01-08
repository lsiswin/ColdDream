using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColdDream.Api.Migrations
{
    /// <inheritdoc />
    public partial class inti2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Itinerary",
                table: "TourRoutes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RouteMapUrl",
                table: "TourRoutes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Itinerary",
                table: "TourRoutes");

            migrationBuilder.DropColumn(
                name: "RouteMapUrl",
                table: "TourRoutes");
        }
    }
}

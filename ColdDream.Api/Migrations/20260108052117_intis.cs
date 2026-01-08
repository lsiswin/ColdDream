using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColdDream.Api.Migrations
{
    /// <inheritdoc />
    public partial class intis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ButlerTourRoute");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Butlers");

            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "PeopleCount",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "TravelDate",
                table: "Bookings",
                newName: "BookingDate");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Bookings",
                newName: "TotalPrice");

            migrationBuilder.RenameColumn(
                name: "TimeSlot",
                table: "Bookings",
                newName: "Travelers");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "Butlers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                table: "Butlers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TourRouteId",
                table: "Butlers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPhone",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactName",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Butlers_TourRouteId",
                table: "Butlers",
                column: "TourRouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Butlers_TourRoutes_TourRouteId",
                table: "Butlers",
                column: "TourRouteId",
                principalTable: "TourRoutes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Butlers_TourRoutes_TourRouteId",
                table: "Butlers");

            migrationBuilder.DropIndex(
                name: "IX_Butlers_TourRouteId",
                table: "Butlers");

            migrationBuilder.DropColumn(
                name: "TourRouteId",
                table: "Butlers");

            migrationBuilder.RenameColumn(
                name: "Travelers",
                table: "Bookings",
                newName: "TimeSlot");

            migrationBuilder.RenameColumn(
                name: "TotalPrice",
                table: "Bookings",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "Bookings",
                newName: "TravelDate");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "Butlers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AvatarUrl",
                table: "Butlers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Butlers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "ContactPhone",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "ContactName",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PeopleCount",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ButlerTourRoute",
                columns: table => new
                {
                    ButlersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourRoutesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ButlerTourRoute", x => new { x.ButlersId, x.TourRoutesId });
                    table.ForeignKey(
                        name: "FK_ButlerTourRoute_Butlers_ButlersId",
                        column: x => x.ButlersId,
                        principalTable: "Butlers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ButlerTourRoute_TourRoutes_TourRoutesId",
                        column: x => x.TourRoutesId,
                        principalTable: "TourRoutes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ButlerTourRoute_TourRoutesId",
                table: "ButlerTourRoute",
                column: "TourRoutesId");
        }
    }
}

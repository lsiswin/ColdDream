using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ColdDream.Api.Migrations
{
    /// <inheritdoc />
    public partial class inti4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Budget",
                table: "Guides",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Guides",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Guides",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Guides",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Budget",
                table: "Guides");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Guides");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Guides");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Guides");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepeaterCouncil.Web.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldCoordinateColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Repeaters");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Repeaters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Repeaters",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Repeaters",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}

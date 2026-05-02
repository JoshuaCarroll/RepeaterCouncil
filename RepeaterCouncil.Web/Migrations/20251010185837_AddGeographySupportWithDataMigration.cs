using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace RepeaterCouncil.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddGeographySupportWithDataMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add new geography columns
            migrationBuilder.AddColumn<Geometry>(
                name: "Borders",
                table: "Tenants",
                type: "geography",
                nullable: true);

            migrationBuilder.AddColumn<Point>(
                name: "Location",
                table: "Repeaters",
                type: "geography",
                nullable: true);

            // Convert existing Latitude/Longitude data to geography Point
            // Only convert non-zero coordinates (skip default/invalid coordinates)
            migrationBuilder.Sql(@"
                UPDATE Repeaters 
                SET Location = geography::Point(Latitude, Longitude, 4326)
                WHERE Latitude != 0 AND Longitude != 0 
                  AND Latitude IS NOT NULL AND Longitude IS NOT NULL
                  AND Latitude BETWEEN -90 AND 90 
                  AND Longitude BETWEEN -180 AND 180");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Borders",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Repeaters");
        }
    }
}

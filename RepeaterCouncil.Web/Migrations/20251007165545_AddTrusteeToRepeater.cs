using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepeaterCouncil.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddTrusteeToRepeater : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TrusteeId",
                table: "Repeaters",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Repeaters_TrusteeId",
                table: "Repeaters",
                column: "TrusteeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repeaters_AspNetUsers_TrusteeId",
                table: "Repeaters",
                column: "TrusteeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repeaters_AspNetUsers_TrusteeId",
                table: "Repeaters");

            migrationBuilder.DropIndex(
                name: "IX_Repeaters_TrusteeId",
                table: "Repeaters");

            migrationBuilder.DropColumn(
                name: "TrusteeId",
                table: "Repeaters");
        }
    }
}

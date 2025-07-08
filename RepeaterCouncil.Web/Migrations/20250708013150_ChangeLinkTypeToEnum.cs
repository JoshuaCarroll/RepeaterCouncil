using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepeaterCouncil.Web.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLinkTypeToEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Destination",
                table: "Links",
                newName: "LinkDetails");

            migrationBuilder.AlterColumn<int>(
                name: "LinkType",
                table: "Links",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "LinkedRepeaterId",
                table: "Links",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Links_LinkedRepeaterId",
                table: "Links",
                column: "LinkedRepeaterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Repeaters_LinkedRepeaterId",
                table: "Links",
                column: "LinkedRepeaterId",
                principalTable: "Repeaters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Repeaters_LinkedRepeaterId",
                table: "Links");

            migrationBuilder.DropIndex(
                name: "IX_Links_LinkedRepeaterId",
                table: "Links");

            migrationBuilder.DropColumn(
                name: "LinkedRepeaterId",
                table: "Links");

            migrationBuilder.RenameColumn(
                name: "LinkDetails",
                table: "Links",
                newName: "Destination");

            migrationBuilder.AlterColumn<string>(
                name: "LinkType",
                table: "Links",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

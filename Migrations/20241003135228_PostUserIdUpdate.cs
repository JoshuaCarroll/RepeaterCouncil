using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepeaterCouncil.Migrations
{
    /// <inheritdoc />
    public partial class PostUserIdUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "RepeaterChangeLogs",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "RequestedByUserId",
                table: "ProposedCoordinationsLog",
                newName: "RequestedByUserID");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Requests",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TrusteeID",
                table: "Repeaters",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(256)",
                oldMaxLength: 256,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "IdentityUserId",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "AspNetUsers",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Callsign",
                table: "AspNetUsers",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                type: "varchar(24)",
                unicode: false,
                maxLength: 24,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastLogin",
                table: "AspNetUsers",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "LicenseExpired",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OldID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Password",
                table: "AspNetUsers",
                type: "varbinary(8000)",
                maxLength: 8000,
                nullable: true,
                defaultValueSql: "(NULL)");

            migrationBuilder.AddColumn<string>(
                name: "PhoneCell",
                table: "AspNetUsers",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneHome",
                table: "AspNetUsers",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneWork",
                table: "AspNetUsers",
                type: "varchar(15)",
                unicode: false,
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "SK",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "AspNetUsers",
                type: "varchar(2)",
                unicode: false,
                maxLength: 2,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ZIP",
                table: "AspNetUsers",
                type: "varchar(10)",
                unicode: false,
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_IdentityUserId",
                table: "Requests",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RequestNotes_IdentityUserId",
                table: "RequestNotes",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "Index_TrusteeID",
                table: "Repeaters",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_RepeaterChangeLogs_IdentityUserId",
                table: "RepeaterChangeLogs",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedCoordinationsLog_IdentityUserId",
                table: "ProposedCoordinationsLog",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_IdentityUserId",
                table: "Permissions",
                column: "IdentityUserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ID",
                table: "AspNetUsers",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "IdentityUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "IdentityUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "IdentityUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "IdentityUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_UserId",
                table: "Permissions",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "IdentityUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProposedCoordinationsLog_RequestedByUserId",
                table: "ProposedCoordinationsLog",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_RepeaterChangeLogs_UserId",
                table: "RepeaterChangeLogs",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repeaters_TrusteeID",
                table: "Repeaters",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "IdentityUserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestNotes_UserID",
                table: "RequestNotes",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "IdentityUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_UserID",
                table: "Requests",
                column: "IdentityUserId",
                principalTable: "AspNetUsers",
                principalColumn: "IdentityUserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Permissions_UserId",
                table: "Permissions");

            migrationBuilder.DropForeignKey(
                name: "FK_ProposedCoordinationsLog_RequestedByUserId",
                table: "ProposedCoordinationsLog");

            migrationBuilder.DropForeignKey(
                name: "FK_RepeaterChangeLogs_UserId",
                table: "RepeaterChangeLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Repeaters_TrusteeID",
                table: "Repeaters");

            migrationBuilder.DropForeignKey(
                name: "FK_RequestNotes_UserID",
                table: "RequestNotes");

            migrationBuilder.DropForeignKey(
                name: "FK_Requests_UserID",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_Requests_IdentityUserId",
                table: "Requests");

            migrationBuilder.DropIndex(
                name: "IX_RequestNotes_IdentityUserId",
                table: "RequestNotes");

            migrationBuilder.DropIndex(
                name: "Index_TrusteeID",
                table: "Repeaters");

            migrationBuilder.DropIndex(
                name: "IX_RepeaterChangeLogs_IdentityUserId",
                table: "RepeaterChangeLogs");

            migrationBuilder.DropIndex(
                name: "IX_ProposedCoordinationsLog_IdentityUserId",
                table: "ProposedCoordinationsLog");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_IdentityUserId",
                table: "Permissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "RequestNotes");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Repeaters");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "RepeaterChangeLogs");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "ProposedCoordinationsLog");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IdentityUserId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Callsign",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastLogin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LicenseExpired",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "OldID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneCell",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneHome",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "PhoneWork",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SK",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "State",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ZIP",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "RepeaterChangeLogs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "RequestedByUserID",
                table: "ProposedCoordinationsLog",
                newName: "RequestedByUserId");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Requests",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "TrusteeID",
                table: "Repeaters",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "AspNetUsers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "AspNetUsers",
                type: "nvarchar(256)",
                maxLength: 256,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldUnicode: false,
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetUsers",
                table: "AspNetUsers",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Address = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Callsign = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    City = table.Column<string>(type: "varchar(24)", unicode: false, maxLength: 24, nullable: true),
                    Email = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    FullName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime", nullable: true),
                    LicenseExpired = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    OldID = table.Column<int>(type: "int", nullable: true),
                    Password = table.Column<byte[]>(type: "varbinary(8000)", maxLength: 8000, nullable: true, defaultValueSql: "(NULL)"),
                    PhoneCell = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    PhoneHome = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    PhoneWork = table.Column<string>(type: "varchar(15)", unicode: false, maxLength: 15, nullable: true),
                    SK = table.Column<bool>(type: "bit", nullable: true, defaultValue: false),
                    State = table.Column<string>(type: "varchar(2)", unicode: false, maxLength: 2, nullable: true),
                    ZIP = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Users__3214EC27AB5231B4", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requests_UserID",
                table: "Requests",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestNotes_UserID",
                table: "RequestNotes",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "Index_TrusteeID",
                table: "Repeaters",
                column: "TrusteeID");

            migrationBuilder.CreateIndex(
                name: "IX_RepeaterChangeLogs_UserId",
                table: "RepeaterChangeLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposedCoordinationsLog_RequestedByUserId",
                table: "ProposedCoordinationsLog",
                column: "RequestedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Permissions_UserId",
                table: "Permissions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                table: "AspNetUserClaims",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                table: "AspNetUserLogins",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                table: "AspNetUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Permissions_UserId",
                table: "Permissions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_ProposedCoordinationsLog_RequestedByUserId",
                table: "ProposedCoordinationsLog",
                column: "RequestedByUserId",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_RepeaterChangeLogs_UserId",
                table: "RepeaterChangeLogs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Repeaters_TrusteeID",
                table: "Repeaters",
                column: "TrusteeID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestNotes_UserID",
                table: "RequestNotes",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Requests_UserID",
                table: "Requests",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID");
        }
    }
}

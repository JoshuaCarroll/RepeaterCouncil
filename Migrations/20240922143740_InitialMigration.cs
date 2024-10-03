using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace RepeaterCouncil.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                ALTER TABLE [dbo].[Requests] DROP CONSTRAINT [FK_Requests_UserID];
                ALTER TABLE [dbo].[RequestNotes] DROP CONSTRAINT [FK_RequestNotes_UserID];
                ALTER TABLE [dbo].[Repeaters] DROP CONSTRAINT [FK_Repeaters_TrusteeID];
                ALTER TABLE [dbo].[RepeaterChangeLogs] DROP CONSTRAINT [FK_RepeaterChangeLogs_UserId];
                ALTER TABLE [dbo].[ProposedCoordinationsLog] DROP CONSTRAINT [FK_ProposedCoordinationsLog_RequestedByUserId];
                ALTER TABLE [dbo].[Permissions] DROP CONSTRAINT [FK_Permissions_UserId];
                ALTER TABLE [dbo].[Users] DROP CONSTRAINT [PK__Users__3214EC27AB5231B4] WITH ( ONLINE = OFF );

                ALTER TABLE Users ADD IdentityUserId NVARCHAR(450) NOT NULL DEFAULT NEWID();
                ALTER TABLE Users ADD CONSTRAINT UQ_IdentityUserId UNIQUE (IdentityUserId);
            ");
            migrationBuilder.Sql(@"
                ALTER TABLE Requests ADD IdentityUserId NVARCHAR(450) NULL;
                ALTER TABLE [RequestNotes] ADD IdentityUserId NVARCHAR(450) NULL;
                ALTER TABLE Repeaters ADD IdentityUserId NVARCHAR(450) NULL;
                ALTER TABLE RepeaterChangeLogs ADD IdentityUserId NVARCHAR(450) NULL;
                ALTER TABLE ProposedCoordinationsLog ADD IdentityUserId NVARCHAR(450) NULL;
                ALTER TABLE Permissions ADD IdentityUserId NVARCHAR(450) NULL;
            ");
            migrationBuilder.Sql(@"
                UPDATE Requests SET IdentityUserId = u.IdentityUserId
	                FROM Requests r
	                JOIN Users u ON r.UserID = u.ID;
                ALTER TABLE Requests
	                ALTER COLUMN IdentityUserId NVARCHAR(450) NOT NULL;
                ALTER TABLE [dbo].[Requests]  WITH CHECK ADD  CONSTRAINT [FK_Requests_UserID] FOREIGN KEY(IdentityUserId)
	                REFERENCES [dbo].[Users] ([IdentityUserId]);
                ALTER TABLE [dbo].[Requests] CHECK CONSTRAINT [FK_Requests_UserID];

                UPDATE [RequestNotes] SET IdentityUserId = u.IdentityUserId
	                FROM RequestNotes r
	                JOIN Users u ON r.UserID = u.ID;
                ALTER TABLE [RequestNotes]
	                ALTER COLUMN IdentityUserId NVARCHAR(450) NOT NULL;
                ALTER TABLE [dbo].[RequestNotes]  WITH CHECK ADD  CONSTRAINT [FK_RequestNotes_UserID] FOREIGN KEY(IdentityUserId)
	                REFERENCES [dbo].[Users] ([IdentityUserId]);
                ALTER TABLE [dbo].RequestNotes CHECK CONSTRAINT [FK_RequestNotes_UserID];

                UPDATE Repeaters SET IdentityUserId = u.IdentityUserId
	                FROM Repeaters r
	                JOIN Users u ON r.TrusteeID = u.ID;
                ALTER TABLE Repeaters
	                ALTER COLUMN IdentityUserId NVARCHAR(450) NOT NULL;
                ALTER TABLE [dbo].Repeaters  WITH CHECK ADD  CONSTRAINT [FK_Repeaters_IdentityUserId] FOREIGN KEY(IdentityUserId)
	                REFERENCES [dbo].[Users] ([IdentityUserId]);
                ALTER TABLE [dbo].Repeaters CHECK CONSTRAINT [FK_Repeaters_IdentityUserId];

                UPDATE RepeaterChangeLogs SET IdentityUserId = u.IdentityUserId
	                FROM RepeaterChangeLogs r
	                JOIN Users u ON r.UserId = u.ID;
                ALTER TABLE RepeaterChangeLogs
	                ALTER COLUMN IdentityUserId NVARCHAR(450) NOT NULL;
                ALTER TABLE [dbo].RepeaterChangeLogs  WITH CHECK ADD  CONSTRAINT [FK_RepeaterChangeLogs_IdentityUserId] FOREIGN KEY(IdentityUserId)
	                REFERENCES [dbo].[Users] ([IdentityUserId]);
                ALTER TABLE [dbo].RepeaterChangeLogs CHECK CONSTRAINT [FK_RepeaterChangeLogs_IdentityUserId];

                UPDATE ProposedCoordinationsLog SET IdentityUserId = u.IdentityUserId
	                FROM ProposedCoordinationsLog r
	                JOIN Users u ON r.RequestedByUserId = u.ID;
                ALTER TABLE ProposedCoordinationsLog
	                ALTER COLUMN IdentityUserId NVARCHAR(450) NOT NULL;
                ALTER TABLE [dbo].ProposedCoordinationsLog  WITH CHECK ADD  CONSTRAINT [FK_ProposedCoordinationsLog_IdentityUserId] FOREIGN KEY(IdentityUserId)
	                REFERENCES [dbo].[Users] ([IdentityUserId]);
                ALTER TABLE [dbo].ProposedCoordinationsLog CHECK CONSTRAINT [FK_ProposedCoordinationsLog_IdentityUserId];

                UPDATE Permissions SET IdentityUserId = u.IdentityUserId
	                FROM Permissions r
	                JOIN Users u ON r.UserId = u.ID;
                ALTER TABLE Permissions
	                ALTER COLUMN IdentityUserId NVARCHAR(450) NOT NULL;
                ALTER TABLE [dbo].Permissions  WITH CHECK ADD  CONSTRAINT [FK_Permissions_IdentityUserId] FOREIGN KEY(IdentityUserId)
	                REFERENCES [dbo].[Users] ([IdentityUserId]);
                ALTER TABLE [dbo].Permissions CHECK CONSTRAINT [FK_Permissions_IdentityUserId];
            ");


            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}

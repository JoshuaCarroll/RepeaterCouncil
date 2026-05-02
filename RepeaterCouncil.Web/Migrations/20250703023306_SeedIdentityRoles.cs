using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepeaterCouncil.Web.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
                    { "00000000-0000-0000-0000-000000000001", "SiteAdministrator", "SITEADMINISTRATOR", "11111111-1111-1111-1111-111111111111" },
                    { "00000000-0000-0000-0000-000000000002", "TenantCoordinator", "TENANTCOORDINATOR", "22222222-2222-2222-2222-222222222222" },
                    { "00000000-0000-0000-0000-000000000003", "GeneralUser", "GENERALUSER", "33333333-3333-3333-3333-333333333333" }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

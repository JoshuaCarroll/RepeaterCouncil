using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepeaterCouncil.Web.Migrations
{
    /// <inheritdoc />
    public partial class RenameTenantsPlural : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeTable_Tenant_TenantId",
                table: "CodeTable");

            migrationBuilder.DropForeignKey(
                name: "FK_CoordinationRules_Tenant_TenantId",
                table: "CoordinationRules");

            migrationBuilder.DropForeignKey(
                name: "FK_Repeaters_Tenant_TenantId",
                table: "Repeaters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenant",
                table: "Tenant");

            migrationBuilder.RenameTable(
                name: "Tenant",
                newName: "Tenants");

            migrationBuilder.RenameColumn(
                name: "Slug",
                table: "Tenants",
                newName: "Url");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenants",
                table: "Tenants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeTable_Tenants_TenantId",
                table: "CodeTable",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoordinationRules_Tenants_TenantId",
                table: "CoordinationRules",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Repeaters_Tenants_TenantId",
                table: "Repeaters",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CodeTable_Tenants_TenantId",
                table: "CodeTable");

            migrationBuilder.DropForeignKey(
                name: "FK_CoordinationRules_Tenants_TenantId",
                table: "CoordinationRules");

            migrationBuilder.DropForeignKey(
                name: "FK_Repeaters_Tenants_TenantId",
                table: "Repeaters");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tenants",
                table: "Tenants");

            migrationBuilder.RenameTable(
                name: "Tenants",
                newName: "Tenant");

            migrationBuilder.RenameColumn(
                name: "Url",
                table: "Tenant",
                newName: "Slug");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tenant",
                table: "Tenant",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CodeTable_Tenant_TenantId",
                table: "CodeTable",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CoordinationRules_Tenant_TenantId",
                table: "CoordinationRules",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Repeaters_Tenant_TenantId",
                table: "Repeaters",
                column: "TenantId",
                principalTable: "Tenant",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

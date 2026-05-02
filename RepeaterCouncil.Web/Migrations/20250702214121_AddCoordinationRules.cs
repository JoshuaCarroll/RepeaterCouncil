using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RepeaterCouncil.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddCoordinationRules : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tenant",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CodeTable",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayText = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CodeTable_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoordinationRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    FrequencyStart = table.Column<double>(type: "float", nullable: false),
                    FrequencyEnd = table.Column<double>(type: "float", nullable: false),
                    SpacingMHz = table.Column<double>(type: "float", nullable: false),
                    SeparationMiles = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoordinationRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoordinationRules_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Repeaters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TenantId = table.Column<int>(type: "int", nullable: false),
                    Callsign = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SiteDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    AltitudeMeters = table.Column<double>(type: "float", nullable: false),
                    OutputPowerWatts = table.Column<double>(type: "float", nullable: false),
                    EffectiveRadiatedPower = table.Column<double>(type: "float", nullable: false),
                    AntennaGain = table.Column<double>(type: "float", nullable: false),
                    AntennaHeightMeters = table.Column<double>(type: "float", nullable: false),
                    TransmitFreq = table.Column<double>(type: "float", nullable: false),
                    ReceiveFreq = table.Column<double>(type: "float", nullable: false),
                    InputToneType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InputToneValue = table.Column<double>(type: "float", nullable: true),
                    OutputToneType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OutputToneValue = table.Column<double>(type: "float", nullable: true),
                    AnalogBandwidth = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCoordinated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateDecoordinated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repeaters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Repeaters_Tenant_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepeaterId = table.Column<int>(type: "int", nullable: false),
                    LinkType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_Repeaters_RepeaterId",
                        column: x => x.RepeaterId,
                        principalTable: "Repeaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RepeaterNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepeaterId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RepeaterNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RepeaterNotes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RepeaterNotes_Repeaters_RepeaterId",
                        column: x => x.RepeaterId,
                        principalTable: "Repeaters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeTable_TenantId",
                table: "CodeTable",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_CoordinationRules_TenantId",
                table: "CoordinationRules",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_RepeaterId",
                table: "Links",
                column: "RepeaterId");

            migrationBuilder.CreateIndex(
                name: "IX_RepeaterNotes_RepeaterId",
                table: "RepeaterNotes",
                column: "RepeaterId");

            migrationBuilder.CreateIndex(
                name: "IX_RepeaterNotes_UserId",
                table: "RepeaterNotes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Repeaters_TenantId",
                table: "Repeaters",
                column: "TenantId");

            migrationBuilder.InsertData(
                table: "Tenant",
                columns: new[] { "Name", "Slug" },
                values: new object[,]
                {
                    { "Arkansas", "arkansasrepeatercouncil.org" }
                });

            migrationBuilder.InsertData(
                table: "CoordinationRules",
                columns: new[] { "TenantId", "FrequencyStart", "FrequencyEnd", "SpacingMHz", "SeparationMiles" },
                values: new object[,]
                {
                    { 1, 50.000000, 54.000000, 0.020, 20 },
                    { 1, 144.000000, 148.000000, 0.015, 40 },
                    { 1, 144.000000, 148.000000, 0.020, 25 },
                    { 1, 144.000000, 148.000000, 0.030, 20 },
                    { 1, 220.000000, 225.000000, 0.020, 40 },
                    { 1, 220.000000, 225.000000, 0.040, 5 },
                    { 1, 420.000000, 450.000000, 0.025, 5 },
                    { 1, 420.000000, 450.000000, 0.040, 1 },
                    { 1, 902.000000, 928.000000, 0.025, 5 },
                    { 1, 902.000000, 928.000000, 0.050, 1 },
                    { 1, 1240.000000, 1300.000000, 0.025, 5 },
                    { 1, 1240.000000, 1300.000000, 0.050, 1 },
                    { 1, 50.000000, 54.000000, 0.000, 90 },
                    { 1, 144.000000, 148.000000, 0.000, 90 },
                    { 1, 220.000000, 225.000000, 0.000, 90 },
                    { 1, 420.000000, 450.000000, 0.000, 90 },
                    { 1, 902.000000, 928.000000, 0.000, 90 },
                    { 1, 1240.000000, 1300.000000, 0.000, 90 },
                    { 1, 28.300000, 29.700000, 0.000, 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CodeTable");

            migrationBuilder.DropTable(
                name: "CoordinationRules");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "RepeaterNotes");

            migrationBuilder.DropTable(
                name: "Repeaters");

            migrationBuilder.DropTable(
                name: "Tenant");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenFreeSchools.Data.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "concerns");

            migrationBuilder.EnsureSchema(
                name: "openFreeSchool");

            migrationBuilder.CreateTable(
                name: "AuditLog",
                schema: "concerns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeOfChange = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ChangeType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__AuditLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "openFreeSchool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchoolName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicationWave = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SRMAReason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRMAReason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SRMACases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseUrn = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    CloseStatusId = table.Column<int>(type: "int", nullable: true),
                    ReasonId = table.Column<int>(type: "int", nullable: true),
                    DateOffered = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateReportSentToTrust = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateAccepted = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StartDateOfVisit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDateOfVisit = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SRMACases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SRMACases_SRMAReason_ReasonId",
                        column: x => x.ReasonId,
                        principalTable: "SRMAReason",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SRMACases_ReasonId",
                table: "SRMACases",
                column: "ReasonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLog",
                schema: "concerns");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "openFreeSchool");

            migrationBuilder.DropTable(
                name: "SRMACases");

            migrationBuilder.DropTable(
                name: "SRMAReason");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenFreeSchools.Data.Migrations
{
    /// <inheritdoc />
    public partial class updatedbmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "[Project Status.Free schools application number]",
                schema: "openFreeSchool",
                table: "Projects",
                newName: "Project Status.Free schools application number");

            migrationBuilder.RenameColumn(
                name: "[Project Status.Free school application wave]",
                schema: "openFreeSchool",
                table: "Projects",
                newName: "Project Status.Free school application wave");

            migrationBuilder.RenameColumn(
                name: "[Project Status.Current free school name]",
                schema: "openFreeSchool",
                table: "Projects",
                newName: "Project Status.Current free school name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Project Status.Free schools application number",
                schema: "openFreeSchool",
                table: "Projects",
                newName: "[Project Status.Free schools application number]");

            migrationBuilder.RenameColumn(
                name: "Project Status.Free school application wave",
                schema: "openFreeSchool",
                table: "Projects",
                newName: "[Project Status.Free school application wave]");

            migrationBuilder.RenameColumn(
                name: "Project Status.Current free school name",
                schema: "openFreeSchool",
                table: "Projects",
                newName: "[Project Status.Current free school name]");
        }
    }
}

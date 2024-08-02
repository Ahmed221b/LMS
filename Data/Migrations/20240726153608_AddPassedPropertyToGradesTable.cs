using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddPassedPropertyToGradesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Passed",
                table: "Grades",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Passed",
                table: "Grades");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AlterAssignmentSubmissionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileContent",
                table: "AssignmentSubmissions");

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "AssignmentSubmissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "AssignmentSubmissions");

            migrationBuilder.AddColumn<byte[]>(
                name: "FileContent",
                table: "AssignmentSubmissions",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}

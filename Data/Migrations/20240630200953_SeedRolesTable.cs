using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "Name", "NormalizedName", "ConcurrencyStamp" },
                values: new object[,]
                {
                    { "1", "Admin", "ADMIN", Guid.NewGuid().ToString() },
                    { "2", "Instructor", "INSTRUCTOR", Guid.NewGuid().ToString() },
                    { "3", "Student", "STUDENT", Guid.NewGuid().ToString() }
                    // Add more roles as needed
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValues: new object[] { "1", "2", "3" }); // Adjust based on your actual data
        }


    }
}

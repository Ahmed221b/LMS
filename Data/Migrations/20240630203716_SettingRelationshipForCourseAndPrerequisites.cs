using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class SettingRelationshipForCourseAndPrerequisites : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrerequisiteCourse",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    PrerequisiteId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrerequisiteCourse", x => new { x.PrerequisiteId, x.CourseId });
                    table.ForeignKey(
                        name: "FK_PrerequisiteCourse_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrerequisiteCourse_Courses_PrerequisiteId",
                        column: x => x.PrerequisiteId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrerequisiteCourse_CourseId",
                table: "PrerequisiteCourse",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrerequisiteCourse");
        }
    }
}

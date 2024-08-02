using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemovingExistingStudentCoursesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
              name: "StudentCourses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "StudentCourses",
               columns: table => new
               {
                   CoursesId = table.Column<int>(type: "int", nullable: false),
                   StudentsId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                   Practical = table.Column<int>(type: "int", nullable: false),
                   Assignments = table.Column<int>(type: "int", nullable: false),
                   Midterm = table.Column<int>(type: "int", nullable: false),
                   Final = table.Column<int>(type: "int", nullable: false),

               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_StudentCourses", x => new { x.CoursesId, x.StudentsId });
                   table.ForeignKey(
                       name: "FK_StudentCourses_Courses_CoursesId",
                       column: x => x.CoursesId,
                       principalTable: "Courses",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.Cascade);
                   table.ForeignKey(
                       name: "FK_StudentCourses_Students_StudentsId",
                       column: x => x.StudentsId,
                       principalTable: "Students",
                       principalColumn: "Id",
                       onDelete: ReferentialAction.NoAction);
               });
        }
    }
}

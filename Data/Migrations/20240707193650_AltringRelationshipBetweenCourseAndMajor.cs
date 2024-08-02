using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AltringRelationshipBetweenCourseAndMajor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MajorCourses");

            migrationBuilder.AddColumn<byte>(
                name: "MajorId",
                table: "Courses",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_MajorId",
                table: "Courses",
                column: "MajorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Majors_MajorId",
                table: "Courses",
                column: "MajorId",
                principalTable: "Majors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Majors_MajorId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_MajorId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "MajorId",
                table: "Courses");

            migrationBuilder.CreateTable(
                name: "MajorCourses",
                columns: table => new
                {
                    CoursesId = table.Column<int>(type: "int", nullable: false),
                    MajorsId = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MajorCourses", x => new { x.CoursesId, x.MajorsId });
                    table.ForeignKey(
                        name: "FK_MajorCourses_Courses_CoursesId",
                        column: x => x.CoursesId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MajorCourses_Majors_MajorsId",
                        column: x => x.MajorsId,
                        principalTable: "Majors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MajorCourses_MajorsId",
                table: "MajorCourses",
                column: "MajorsId");
        }
    }
}

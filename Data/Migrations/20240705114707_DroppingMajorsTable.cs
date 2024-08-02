using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class DroppingMajorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Majors_MajorId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "MajorCourses");

            migrationBuilder.DropTable(
                name: "Majors");

            migrationBuilder.DropIndex(
                name: "IX_Students_MajorId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MajorId",
                table: "Students");

            migrationBuilder.DropSequence(
                name: "MajorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "MajorId");

            migrationBuilder.AddColumn<byte>(
                name: "MajorId",
                table: "Students",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateTable(
                name: "Majors",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "tinyint", nullable: false, defaultValueSql: "Next Value for MajorId"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Majors", x => x.Id);
                });

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
                name: "IX_Students_MajorId",
                table: "Students",
                column: "MajorId");

            migrationBuilder.CreateIndex(
                name: "IX_MajorCourses_MajorsId",
                table: "MajorCourses",
                column: "MajorsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Majors_MajorId",
                table: "Students",
                column: "MajorId",
                principalTable: "Majors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

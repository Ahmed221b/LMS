using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipBetweenStudentAndMajor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "MajorId",
                table: "Students",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);

            migrationBuilder.CreateIndex(
                name: "IX_Students_MajorId",
                table: "Students",
                column: "MajorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Majors_MajorId",
                table: "Students",
                column: "MajorId",
                principalTable: "Majors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Majors_MajorId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_MajorId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MajorId",
                table: "Students");
        }
    }
}

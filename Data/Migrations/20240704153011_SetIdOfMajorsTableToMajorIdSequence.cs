using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CourseManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class SetIdOfMajorsTableToMajorIdSequence : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "MajorId");

            migrationBuilder.AlterColumn<byte>(
                name: "Id",
                table: "Majors",
                type: "tinyint",
                nullable: false,
                defaultValueSql: "Next Value for MajorId",
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropSequence(
                name: "MajorId");

            migrationBuilder.AlterColumn<byte>(
                name: "Id",
                table: "Majors",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint",
                oldDefaultValueSql: "Next Value for MajorId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LMSCoreMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentNameToAssignment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StudentName",
                table: "Assignment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentName",
                table: "Assignment");
        }
    }
}

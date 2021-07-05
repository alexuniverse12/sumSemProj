using Microsoft.EntityFrameworkCore.Migrations;

namespace summerSemesterProj.Migrations
{
    public partial class AddNotesToUserFixOne : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Note",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Note");
        }
    }
}

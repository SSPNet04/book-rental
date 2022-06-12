using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Models.Migrations
{
    public partial class AddMalID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MalID",
                table: "Books",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MalID",
                table: "Books");
        }
    }
}

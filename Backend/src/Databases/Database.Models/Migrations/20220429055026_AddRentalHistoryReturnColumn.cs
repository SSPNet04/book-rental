using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Models.Migrations
{
    public partial class AddRentalHistoryReturnColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Return",
                table: "RentalHistory",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Return",
                table: "RentalHistory");
        }
    }
}

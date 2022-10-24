using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemocracyBot.DataAccess.Migrations
{
    public partial class AddPollQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Question",
                table: "Polls",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Question",
                table: "Polls");
        }
    }
}

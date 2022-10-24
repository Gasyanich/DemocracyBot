using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemocracyBot.DataAccess.Migrations
{
    public partial class UpdatePollQuestion2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Number",
                table: "PollAnswers",
                type: "int",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "tinyint");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte>(
                name: "Number",
                table: "PollAnswers",
                type: "tinyint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}

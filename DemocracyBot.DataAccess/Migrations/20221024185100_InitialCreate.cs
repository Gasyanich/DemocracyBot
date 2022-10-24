using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DemocracyBot.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    IsNotificationsActivated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meets",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChatId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Meets_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BotUserChat",
                columns: table => new
                {
                    ChatsId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotUserChat", x => new { x.ChatsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_BotUserChat_Chats_ChatsId",
                        column: x => x.ChatsId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BotUserChat_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ChatId = table.Column<long>(type: "bigint", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Polls_Chats_ChatId",
                        column: x => x.ChatId,
                        principalTable: "Chats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Polls_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BotUserMeet",
                columns: table => new
                {
                    MeetsId = table.Column<long>(type: "bigint", nullable: false),
                    UsersId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotUserMeet", x => new { x.MeetsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_BotUserMeet_Meets_MeetsId",
                        column: x => x.MeetsId,
                        principalTable: "Meets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BotUserMeet_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PollAnswers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<byte>(type: "tinyint", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VoteCount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PollId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PollAnswers_Polls_PollId",
                        column: x => x.PollId,
                        principalTable: "Polls",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_BotUserChat_UsersId",
                table: "BotUserChat",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_BotUserMeet_UsersId",
                table: "BotUserMeet",
                column: "UsersId");

            migrationBuilder.CreateIndex(
                name: "IX_Meets_ChatId",
                table: "Meets",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_PollAnswers_PollId",
                table: "PollAnswers",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_ChatId",
                table: "Polls",
                column: "ChatId");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_UserId",
                table: "Polls",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BotUserChat");

            migrationBuilder.DropTable(
                name: "BotUserMeet");

            migrationBuilder.DropTable(
                name: "PollAnswers");

            migrationBuilder.DropTable(
                name: "Meets");

            migrationBuilder.DropTable(
                name: "Polls");

            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

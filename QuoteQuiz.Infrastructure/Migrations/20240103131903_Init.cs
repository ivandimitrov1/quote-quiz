using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuoteQuiz.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Quiz",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Published = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Pk_Quiz_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Pk_User_Id", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Quote",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorectAnswer = table.Column<int>(type: "int", nullable: false),
                    Answers = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuizId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Pk_Quote_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quote_Quiz_QuizId",
                        column: x => x.QuizId,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TokenHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Fk_User_Id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Pk_RefreshToken_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshToken_User_Fk_User_Id",
                        column: x => x.Fk_User_Id,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fk_User_Id = table.Column<int>(type: "int", nullable: false),
                    Fk_Quiz_Id = table.Column<int>(type: "int", nullable: false),
                    Fk_Quote_Id = table.Column<int>(type: "int", nullable: true),
                    UserAnswer = table.Column<int>(type: "int", nullable: true),
                    OnDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    QuizFinished = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Pk_UserAnswer_Id", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserAnswer_Quiz_Fk_Quiz_Id",
                        column: x => x.Fk_Quiz_Id,
                        principalTable: "Quiz",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserAnswer_Quote_Fk_Quote_Id",
                        column: x => x.Fk_Quote_Id,
                        principalTable: "Quote",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserAnswer_User_Fk_User_Id",
                        column: x => x.Fk_User_Id,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Quote_QuizId",
                table: "Quote",
                column: "QuizId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Fk_User_Id",
                table: "RefreshToken",
                column: "Fk_User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_User_Login",
                table: "User",
                column: "Login",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_Fk_Quiz_Id",
                table: "UserAnswer",
                column: "Fk_Quiz_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_Fk_Quote_Id",
                table: "UserAnswer",
                column: "Fk_Quote_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserAnswer_Fk_User_Id",
                table: "UserAnswer",
                column: "Fk_User_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "UserAnswer");

            migrationBuilder.DropTable(
                name: "Quote");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Quiz");
        }
    }
}

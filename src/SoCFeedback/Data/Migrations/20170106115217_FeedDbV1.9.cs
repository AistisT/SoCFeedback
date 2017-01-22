using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "PossibleAnswer");

            migrationBuilder.DropColumn(
                "QuestionOrder",
                "ModuleQuestions");

            migrationBuilder.AddColumn<int>(
                "QuestionOrderNumberInCategory",
                "Question",
                nullable: false,
                defaultValueSql: "99");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "QuestionOrderNumberInCategory",
                "Question");

            migrationBuilder.AddColumn<int>(
                "QuestionOrder",
                "ModuleQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                "PossibleAnswer",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Answer = table.Column<string>(maxLength: 200, nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PossibleAnswer", x => x.Id);
                    table.ForeignKey(
                        "FK_PossibleAnswer_Question",
                        x => x.QuestionId,
                        "Question",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_PossibleAnswer_QuestionId",
                "PossibleAnswer",
                "QuestionId");
        }
    }
}
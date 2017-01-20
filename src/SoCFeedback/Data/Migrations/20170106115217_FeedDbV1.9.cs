using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV19 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PossibleAnswer");

            migrationBuilder.DropColumn(
                name: "QuestionOrder",
                table: "ModuleQuestions");

            migrationBuilder.AddColumn<int>(
                name: "QuestionOrderNumberInCategory",
                table: "Question",
                nullable: false,
                defaultValueSql: "99");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuestionOrderNumberInCategory",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "QuestionOrder",
                table: "ModuleQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PossibleAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Answer = table.Column<string>(maxLength: 200, nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PossibleAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PossibleAnswer_Question",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PossibleAnswer_QuestionId",
                table: "PossibleAnswer",
                column: "QuestionId");
        }
    }
}

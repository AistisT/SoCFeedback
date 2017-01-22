using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV191 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "ModuleCode",
                "Answer");

            migrationBuilder.RenameColumn(
                "QuestionOrderNumberInCategory",
                "Question",
                "QuestionNumber");

            migrationBuilder.AddColumn<Guid>(
                "ModuleId",
                "Answer",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "ModuleId",
                "Answer");

            migrationBuilder.RenameColumn(
                "QuestionNumber",
                "Question",
                "QuestionOrderNumberInCategory");

            migrationBuilder.AddColumn<string>(
                "ModuleCode",
                "Answer",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV191 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleCode",
                table: "Answer");

            migrationBuilder.RenameColumn(
                name: "QuestionOrderNumberInCategory",
                table: "Question",
                newName: "QuestionNumber");

            migrationBuilder.AddColumn<Guid>(
                name: "ModuleId",
                table: "Answer",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ModuleId",
                table: "Answer");

            migrationBuilder.RenameColumn(
                name: "QuestionNumber",
                table: "Question",
                newName: "QuestionOrderNumberInCategory");

            migrationBuilder.AddColumn<string>(
                name: "ModuleCode",
                table: "Answer",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }
    }
}

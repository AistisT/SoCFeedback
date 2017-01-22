using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                "QuestionOrder",
                "ModuleQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                "YearId",
                "Level",
                nullable: true);

            migrationBuilder.CreateIndex(
                "IX_Level_YearId",
                "Level",
                "YearId");

            migrationBuilder.AddForeignKey(
                "FK_Level_Year_YearId",
                "Level",
                "YearId",
                "Year",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Level_Year_YearId",
                "Level");

            migrationBuilder.DropIndex(
                "IX_Level_YearId",
                "Level");

            migrationBuilder.DropColumn(
                "QuestionOrder",
                "ModuleQuestions");

            migrationBuilder.DropColumn(
                "YearId",
                "Level");
        }
    }
}
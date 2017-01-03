using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV16 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuestionOrder",
                table: "ModuleQuestions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "YearId",
                table: "Level",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Level_YearId",
                table: "Level",
                column: "YearId");

            migrationBuilder.AddForeignKey(
                name: "FK_Level_Year_YearId",
                table: "Level",
                column: "YearId",
                principalTable: "Year",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Level_Year_YearId",
                table: "Level");

            migrationBuilder.DropIndex(
                name: "IX_Level_YearId",
                table: "Level");

            migrationBuilder.DropColumn(
                name: "QuestionOrder",
                table: "ModuleQuestions");

            migrationBuilder.DropColumn(
                name: "YearId",
                table: "Level");
        }
    }
}

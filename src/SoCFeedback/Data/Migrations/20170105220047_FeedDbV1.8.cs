using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV18 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                "FK_Level_Year_YearId",
                "Level");

            migrationBuilder.DropIndex(
                "IX_Level_YearId",
                "Level");

            migrationBuilder.DropColumn(
                "YearId",
                "Level");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
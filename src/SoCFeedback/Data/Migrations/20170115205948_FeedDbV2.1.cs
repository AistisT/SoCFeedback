using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                 name: "Year",
                 table: "Answer");
            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Answer",
                nullable: false);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

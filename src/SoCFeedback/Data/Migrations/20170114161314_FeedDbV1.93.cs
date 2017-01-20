using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV193 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CategoryOrder",
                table: "Category",
                nullable: false,
                defaultValueSql: "99",
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CategoryOrder",
                table: "Category",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValueSql: "99");
        }
    }
}

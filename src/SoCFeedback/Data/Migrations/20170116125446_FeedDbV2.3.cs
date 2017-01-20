using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Supervisor",
                maxLength: 254,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "Module",
                maxLength: 254,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Module",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "Answer",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 2000);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Supervisor",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 254);

            migrationBuilder.AlterColumn<string>(
                name: "URL",
                table: "Module",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 254,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Module",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 3000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Answer",
                table: "Answer",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 3000);
        }
    }
}

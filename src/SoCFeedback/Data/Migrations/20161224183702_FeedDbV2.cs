using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Question",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Module",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorId",
                table: "Module",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Level",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Category",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Supervisor",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Forename = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNr = table.Column<string>(maxLength: 20, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Surname = table.Column<string>(maxLength: 50, nullable: false),
                    Title = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Supervisor", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Module_SupervisorId",
                table: "Module",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Supervisor",
                table: "Module",
                column: "SupervisorId",
                principalTable: "Supervisor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Module_Supervisor",
                table: "Module");

            migrationBuilder.DropTable(
                name: "Supervisor");

            migrationBuilder.DropIndex(
                name: "IX_Module_SupervisorId",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Level");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Category");
        }
    }
}

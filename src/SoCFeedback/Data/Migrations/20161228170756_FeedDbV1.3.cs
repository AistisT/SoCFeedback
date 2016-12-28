using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV13 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Module_Level",
                table: "Module");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Supervisor",
                table: "Module");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Category",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Supervisor",
                table: "Supervisor");

            migrationBuilder.DropIndex(
                name: "IX_Module_SupervisorId",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Level",
                table: "Level");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Supervisor");

            migrationBuilder.DropColumn(
                name: "Section",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "SupervisorId",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Level");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "ModuleCode",
                table: "YearModules",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNr",
                table: "Supervisor",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CategoryId",
                table: "Question",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "ModuleCode",
                table: "ModuleQuestions",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "LevelId",
                table: "Module",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Module",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(20)");

            migrationBuilder.AddColumn<string>(
                name: "SupervisorForename",
                table: "Module",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SupervisorSurname",
                table: "Module",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Level",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(50)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Category",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supervisor",
                table: "Supervisor",
                columns: new[] { "Forename", "Surname" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Title",
                table: "Level",
                column: "Title");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Title",
                table: "Category",
                column: "Title");

            migrationBuilder.CreateIndex(
                name: "IX_Module_SupervisorForename_SupervisorSurname",
                table: "Module",
                columns: new[] { "SupervisorForename", "SupervisorSurname" });

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Level",
                table: "Module",
                column: "LevelId",
                principalTable: "Level",
                principalColumn: "Title",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Supervisor",
                table: "Module",
                columns: new[] { "SupervisorForename", "SupervisorSurname" },
                principalTable: "Supervisor",
                principalColumns: new[] { "Forename", "Surname" },
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Category",
                table: "Question",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Title",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Module_Level",
                table: "Module");

            migrationBuilder.DropForeignKey(
                name: "FK_Module_Supervisor",
                table: "Module");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Category",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Supervisor",
                table: "Supervisor");

            migrationBuilder.DropIndex(
                name: "IX_Module_SupervisorForename_SupervisorSurname",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Title",
                table: "Level");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Title",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "SupervisorForename",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "SupervisorSurname",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Category");

            migrationBuilder.AlterColumn<string>(
                name: "ModuleCode",
                table: "YearModules",
                type: "nchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNr",
                table: "Supervisor",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Supervisor",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "CategoryId",
                table: "Question",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "Section",
                table: "Question",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "ModuleCode",
                table: "ModuleQuestions",
                type: "nchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<Guid>(
                name: "LevelId",
                table: "Module",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Code",
                table: "Module",
                type: "nchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20);

            migrationBuilder.AddColumn<Guid>(
                name: "SupervisorId",
                table: "Module",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Level",
                type: "nchar(50)",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 50);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Level",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Category",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Supervisor",
                table: "Supervisor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Level",
                table: "Level",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Module_SupervisorId",
                table: "Module",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Level",
                table: "Module",
                column: "LevelId",
                principalTable: "Level",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Supervisor",
                table: "Module",
                column: "SupervisorId",
                principalTable: "Supervisor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Category",
                table: "Question",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

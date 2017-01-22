using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                "Category",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(maxLength: 200, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Category", x => x.Id); });

            migrationBuilder.CreateTable(
                "Level",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Level", x => x.Id); });

            migrationBuilder.CreateTable(
                "Supervisor",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Email = table.Column<string>(maxLength: 50, nullable: true),
                    Forename = table.Column<string>(maxLength: 50, nullable: false),
                    PhoneNr = table.Column<string>(maxLength: 30, nullable: true),
                    Status = table.Column<int>(nullable: false),
                    Surname = table.Column<string>(maxLength: 50, nullable: false),
                    Title = table.Column<int>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Supervisor", x => x.Id); });

            migrationBuilder.CreateTable(
                "Year",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Year", x => x.Id); });

            migrationBuilder.CreateTable(
                "Question",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CategoryId = table.Column<Guid>(nullable: false),
                    Optional = table.Column<bool>(nullable: false, defaultValueSql: "0"),
                    Question = table.Column<string>(maxLength: 250, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        "FK_Question_Category",
                        x => x.CategoryId,
                        "Category",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Module",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(maxLength: 20, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    LevelId = table.Column<Guid>(maxLength: 50, nullable: false),
                    Status = table.Column<int>(nullable: false),
                    SupervisorId = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 200, nullable: false),
                    URL = table.Column<string>(maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x.Id);
                    table.ForeignKey(
                        "FK_Module_Level",
                        x => x.LevelId,
                        "Level",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_Module_Supervisor",
                        x => x.SupervisorId,
                        "Supervisor",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "Answer",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Answer = table.Column<string>(maxLength: 500, nullable: false),
                    ModuleCode = table.Column<string>(maxLength: 20, nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false),
                    Timestamp = table.Column<DateTime>("datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        "FK_Answer_Question",
                        x => x.QuestionId,
                        "Question",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "PossibleAnswer",
                table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Answer = table.Column<string>(maxLength: 200, nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PossibleAnswer", x => x.Id);
                    table.ForeignKey(
                        "FK_PossibleAnswer_Question",
                        x => x.QuestionId,
                        "Question",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "ModuleQuestions",
                table => new
                {
                    ModuleId = table.Column<Guid>(maxLength: 20, nullable: false),
                    QuestionId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleQuestions", x => new {x.ModuleId, x.QuestionId});
                    table.ForeignKey(
                        "FK_ModuleQuestions_Module",
                        x => x.ModuleId,
                        "Module",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_ModuleQuestions_Question",
                        x => x.QuestionId,
                        "Question",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                "YearModules",
                table => new
                {
                    YearId = table.Column<Guid>(nullable: false),
                    ModuleId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearModules", x => new {x.YearId, x.ModuleId});
                    table.ForeignKey(
                        "FK_YearModules_Module",
                        x => x.ModuleId,
                        "Module",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        "FK_YearModules_Year",
                        x => x.YearId,
                        "Year",
                        "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                "IX_Answer_QuestionId",
                "Answer",
                "QuestionId");

            migrationBuilder.CreateIndex(
                "IX_Module_LevelId",
                "Module",
                "LevelId");

            migrationBuilder.CreateIndex(
                "IX_Module_SupervisorId",
                "Module",
                "SupervisorId");

            migrationBuilder.CreateIndex(
                "IX_ModuleQuestions_QuestionId",
                "ModuleQuestions",
                "QuestionId");

            migrationBuilder.CreateIndex(
                "IX_PossibleAnswer_QuestionId",
                "PossibleAnswer",
                "QuestionId");

            migrationBuilder.CreateIndex(
                "IX_Question_CategoryId",
                "Question",
                "CategoryId");

            migrationBuilder.CreateIndex(
                "IX_YearModules_ModuleId",
                "YearModules",
                "ModuleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                "Answer");

            migrationBuilder.DropTable(
                "ModuleQuestions");

            migrationBuilder.DropTable(
                "PossibleAnswer");

            migrationBuilder.DropTable(
                "YearModules");

            migrationBuilder.DropTable(
                "Question");

            migrationBuilder.DropTable(
                "Module");

            migrationBuilder.DropTable(
                "Year");

            migrationBuilder.DropTable(
                "Category");

            migrationBuilder.DropTable(
                "Level");

            migrationBuilder.DropTable(
                "Supervisor");
        }
    }
}
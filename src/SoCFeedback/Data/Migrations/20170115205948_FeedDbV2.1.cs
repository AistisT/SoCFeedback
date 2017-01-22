using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                "Year",
                "Answer");
            migrationBuilder.AddColumn<int>(
                "Year",
                "Answer",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
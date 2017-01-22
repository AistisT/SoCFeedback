using Microsoft.EntityFrameworkCore.Migrations;

namespace SoCFeedback.Data.Migrations
{
    public partial class FeedDbV193 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                "CategoryOrder",
                "Category",
                nullable: false,
                defaultValueSql: "99",
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                "CategoryOrder",
                "Category",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValueSql: "99");
        }
    }
}
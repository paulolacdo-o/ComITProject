using Microsoft.EntityFrameworkCore.Migrations;

namespace OurFiction.Data.Migrations
{
    public partial class modifiedModels2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StorySequenceNumber",
                table: "Fragments");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StorySequenceNumber",
                table: "Fragments",
                nullable: false,
                defaultValue: 0);
        }
    }
}

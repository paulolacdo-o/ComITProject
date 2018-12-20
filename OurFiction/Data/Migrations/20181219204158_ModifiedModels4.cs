using Microsoft.EntityFrameworkCore.Migrations;

namespace OurFiction.Data.Migrations
{
    public partial class ModifiedModels4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VoterId",
                table: "Votes",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Fragments",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_VoterId",
                table: "Votes",
                column: "VoterId");

            migrationBuilder.CreateIndex(
                name: "IX_Fragments_OwnerId",
                table: "Fragments",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fragments_AspNetUsers_OwnerId",
                table: "Fragments",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AspNetUsers_VoterId",
                table: "Votes",
                column: "VoterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fragments_AspNetUsers_OwnerId",
                table: "Fragments");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AspNetUsers_VoterId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_VoterId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Fragments_OwnerId",
                table: "Fragments");

            migrationBuilder.DropColumn(
                name: "VoterId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Fragments");
        }
    }
}

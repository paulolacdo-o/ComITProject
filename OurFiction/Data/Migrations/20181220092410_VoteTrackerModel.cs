using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OurFiction.Data.Migrations
{
    public partial class VoteTrackerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AspNetUsers_VoterId",
                table: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Votes_VoterId",
                table: "Votes");

            migrationBuilder.DropColumn(
                name: "VoterId",
                table: "Votes");

            migrationBuilder.CreateTable(
                name: "Tracker",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    VoterId = table.Column<string>(nullable: true),
                    VoteId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracker", x => x.id);
                    table.ForeignKey(
                        name: "FK_Tracker_Votes_VoteId",
                        column: x => x.VoteId,
                        principalTable: "Votes",
                        principalColumn: "VoteId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tracker_AspNetUsers_VoterId",
                        column: x => x.VoterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tracker_VoteId",
                table: "Tracker",
                column: "VoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracker_VoterId",
                table: "Tracker",
                column: "VoterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tracker");

            migrationBuilder.AddColumn<string>(
                name: "VoterId",
                table: "Votes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Votes_VoterId",
                table: "Votes",
                column: "VoterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AspNetUsers_VoterId",
                table: "Votes",
                column: "VoterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

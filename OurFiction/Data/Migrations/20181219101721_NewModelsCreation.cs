using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OurFiction.Data.Migrations
{
    public partial class NewModelsCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_AspNetUsers_UserId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryFragments_Entries_EntryId",
                table: "StoryFragments");

            migrationBuilder.DropForeignKey(
                name: "FK_StoryFragments_Stories_StoryId",
                table: "StoryFragments");

            migrationBuilder.DropTable(
                name: "EntryCycles");

            migrationBuilder.DropIndex(
                name: "IX_Entries_UserId",
                table: "Entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StoryFragments",
                table: "StoryFragments");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Votes",
                table: "Entries");

            migrationBuilder.RenameTable(
                name: "StoryFragments",
                newName: "Fragments");

            migrationBuilder.RenameIndex(
                name: "IX_StoryFragments_StoryId",
                table: "Fragments",
                newName: "IX_Fragments_StoryId");

            migrationBuilder.RenameIndex(
                name: "IX_StoryFragments_EntryId",
                table: "Fragments",
                newName: "IX_Fragments_EntryId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Stories",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "FragCount",
                table: "Stories",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InitialContent",
                table: "Stories",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Stories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "OwnerId",
                table: "Stories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Synopsis",
                table: "Stories",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Entries",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "StoryId",
                table: "Entries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Fragments",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Fragments",
                table: "Fragments",
                column: "FragmentId");

            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    VoteId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    FragmentId = table.Column<int>(nullable: true),
                    EntryId = table.Column<int>(nullable: true),
                    VotePoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.VoteId);
                    table.ForeignKey(
                        name: "FK_Votes_Entries_EntryId",
                        column: x => x.EntryId,
                        principalTable: "Entries",
                        principalColumn: "EntryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Votes_Fragments_FragmentId",
                        column: x => x.FragmentId,
                        principalTable: "Fragments",
                        principalColumn: "FragmentId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stories_OwnerId",
                table: "Stories",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Entries_StoryId",
                table: "Entries",
                column: "StoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_EntryId",
                table: "Votes",
                column: "EntryId");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_FragmentId",
                table: "Votes",
                column: "FragmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_Stories_StoryId",
                table: "Entries",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "StoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fragments_Entries_EntryId",
                table: "Fragments",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "EntryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fragments_Stories_StoryId",
                table: "Fragments",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "StoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stories_AspNetUsers_OwnerId",
                table: "Stories",
                column: "OwnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Entries_Stories_StoryId",
                table: "Entries");

            migrationBuilder.DropForeignKey(
                name: "FK_Fragments_Entries_EntryId",
                table: "Fragments");

            migrationBuilder.DropForeignKey(
                name: "FK_Fragments_Stories_StoryId",
                table: "Fragments");

            migrationBuilder.DropForeignKey(
                name: "FK_Stories_AspNetUsers_OwnerId",
                table: "Stories");

            migrationBuilder.DropTable(
                name: "Votes");

            migrationBuilder.DropIndex(
                name: "IX_Stories_OwnerId",
                table: "Stories");

            migrationBuilder.DropIndex(
                name: "IX_Entries_StoryId",
                table: "Entries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Fragments",
                table: "Fragments");

            migrationBuilder.DropColumn(
                name: "FragCount",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "InitialContent",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "Synopsis",
                table: "Stories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "StoryId",
                table: "Entries");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Fragments");

            migrationBuilder.RenameTable(
                name: "Fragments",
                newName: "StoryFragments");

            migrationBuilder.RenameIndex(
                name: "IX_Fragments_StoryId",
                table: "StoryFragments",
                newName: "IX_StoryFragments_StoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Fragments_EntryId",
                table: "StoryFragments",
                newName: "IX_StoryFragments_EntryId");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Stories",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Entries",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Entries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Votes",
                table: "Entries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StoryFragments",
                table: "StoryFragments",
                column: "FragmentId");

            migrationBuilder.CreateTable(
                name: "EntryCycles",
                columns: table => new
                {
                    EntryCycleId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EndTimeDate = table.Column<DateTimeOffset>(nullable: false),
                    StartTimeDate = table.Column<DateTimeOffset>(nullable: false),
                    StoryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryCycles", x => x.EntryCycleId);
                    table.ForeignKey(
                        name: "FK_EntryCycles_Stories_StoryId",
                        column: x => x.StoryId,
                        principalTable: "Stories",
                        principalColumn: "StoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Entries_UserId",
                table: "Entries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EntryCycles_StoryId",
                table: "EntryCycles",
                column: "StoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Entries_AspNetUsers_UserId",
                table: "Entries",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFragments_Entries_EntryId",
                table: "StoryFragments",
                column: "EntryId",
                principalTable: "Entries",
                principalColumn: "EntryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoryFragments_Stories_StoryId",
                table: "StoryFragments",
                column: "StoryId",
                principalTable: "Stories",
                principalColumn: "StoryId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

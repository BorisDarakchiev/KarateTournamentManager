using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class addTatamiModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matchеs_AspNetUsers_TimerManagerId",
                table: "Matchеs");

            migrationBuilder.DropIndex(
                name: "IX_Matchеs_TimerManagerId",
                table: "Matchеs");

            migrationBuilder.DropColumn(
                name: "LoserNextMatchId",
                table: "Matchеs");

            migrationBuilder.DropColumn(
                name: "TimerManagerId",
                table: "Matchеs");

            migrationBuilder.DropColumn(
                name: "WinnerNextMatchId",
                table: "Matchеs");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Tournaments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tournaments",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Participants",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "Tatamis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    TimerManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tatamis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tatamis_Participants_TimerManagerId",
                        column: x => x.TimerManagerId,
                        principalTable: "Participants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tatamis_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tatamis_TimerManagerId",
                table: "Tatamis",
                column: "TimerManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_Tatamis_TournamentId",
                table: "Tatamis",
                column: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tatamis");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Tournaments",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Stages",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Participants",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<Guid>(
                name: "LoserNextMatchId",
                table: "Matchеs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimerManagerId",
                table: "Matchеs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WinnerNextMatchId",
                table: "Matchеs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matchеs_TimerManagerId",
                table: "Matchеs",
                column: "TimerManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchеs_AspNetUsers_TimerManagerId",
                table: "Matchеs",
                column: "TimerManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

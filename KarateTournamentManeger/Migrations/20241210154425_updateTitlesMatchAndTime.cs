using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class updateTitlesMatchAndTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timer_Matches_MatchId",
                table: "Timer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Timer",
                table: "Timer");

            migrationBuilder.RenameTable(
                name: "Timer",
                newName: "Timers");

            migrationBuilder.RenameIndex(
                name: "IX_Timer_MatchId",
                table: "Timers",
                newName: "IX_Timers_MatchId");

            migrationBuilder.AddColumn<Guid>(
                name: "TimerId",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Timers",
                table: "Timers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Timers_Matches_MatchId",
                table: "Timers",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timers_Matches_MatchId",
                table: "Timers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Timers",
                table: "Timers");

            migrationBuilder.DropColumn(
                name: "TimerId",
                table: "Matches");

            migrationBuilder.RenameTable(
                name: "Timers",
                newName: "Timer");

            migrationBuilder.RenameIndex(
                name: "IX_Timers_MatchId",
                table: "Timer",
                newName: "IX_Timer_MatchId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Timer",
                table: "Timer",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Timer_Matches_MatchId",
                table: "Timer",
                column: "MatchId",
                principalTable: "Matches",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

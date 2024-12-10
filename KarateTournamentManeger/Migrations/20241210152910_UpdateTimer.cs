using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_Participants_Participant1Id",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Participants_Participant2Id",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Participants_ParticipantId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Participants_WinnerId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Stages_StageId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Tournaments_TournamentId",
                table: "Match");

            migrationBuilder.DropTable(
                name: "Timers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Match",
                table: "Match");

            migrationBuilder.RenameTable(
                name: "Match",
                newName: "Matches");

            migrationBuilder.RenameIndex(
                name: "IX_Match_WinnerId",
                table: "Matches",
                newName: "IX_Matches_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_TournamentId",
                table: "Matches",
                newName: "IX_Matches_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_StageId",
                table: "Matches",
                newName: "IX_Matches_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_ParticipantId",
                table: "Matches",
                newName: "IX_Matches_ParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_Participant2Id",
                table: "Matches",
                newName: "IX_Matches_Participant2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Match_Participant1Id",
                table: "Matches",
                newName: "IX_Matches_Participant1Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Timer",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountdownTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsRunning = table.Column<bool>(type: "bit", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timer_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timer_MatchId",
                table: "Timer",
                column: "MatchId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Participants_Participant1Id",
                table: "Matches",
                column: "Participant1Id",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Participants_Participant2Id",
                table: "Matches",
                column: "Participant2Id",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Participants_ParticipantId",
                table: "Matches",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Participants_WinnerId",
                table: "Matches",
                column: "WinnerId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Stages_StageId",
                table: "Matches",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Tournaments_TournamentId",
                table: "Matches",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Participants_Participant1Id",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Participants_Participant2Id",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Participants_ParticipantId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Participants_WinnerId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Stages_StageId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Tournaments_TournamentId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "Timer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Match");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_WinnerId",
                table: "Match",
                newName: "IX_Match_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_TournamentId",
                table: "Match",
                newName: "IX_Match_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_StageId",
                table: "Match",
                newName: "IX_Match_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_ParticipantId",
                table: "Match",
                newName: "IX_Match_ParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_Participant2Id",
                table: "Match",
                newName: "IX_Match_Participant2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_Participant1Id",
                table: "Match",
                newName: "IX_Match_Participant1Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Match",
                table: "Match",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Timers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountdownTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsRunning = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timers_Match_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Match",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Timers_MatchId",
                table: "Timers",
                column: "MatchId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Participants_Participant1Id",
                table: "Match",
                column: "Participant1Id",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Participants_Participant2Id",
                table: "Match",
                column: "Participant2Id",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Participants_ParticipantId",
                table: "Match",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Participants_WinnerId",
                table: "Match",
                column: "WinnerId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Stages_StageId",
                table: "Match",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Tournaments_TournamentId",
                table: "Match",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

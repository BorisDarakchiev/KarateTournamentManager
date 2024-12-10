using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMatchesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matchеs_Participants_Participant1Id",
                table: "Matchеs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchеs_Participants_Participant2Id",
                table: "Matchеs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchеs_Participants_ParticipantId",
                table: "Matchеs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchеs_Participants_WinnerId",
                table: "Matchеs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchеs_Stages_StageId",
                table: "Matchеs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchеs_Tournaments_TournamentId",
                table: "Matchеs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matchеs",
                table: "Matchеs");

            migrationBuilder.RenameTable(
                name: "Matchеs",
                newName: "Matches");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_WinnerId",
                table: "Matches",
                newName: "IX_Matches_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_TournamentId",
                table: "Matches",
                newName: "IX_Matches_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_StageId",
                table: "Matches",
                newName: "IX_Matches_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_ParticipantId",
                table: "Matches",
                newName: "IX_Matches_ParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_Participant2Id",
                table: "Matches",
                newName: "IX_Matches_Participant2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_Participant1Id",
                table: "Matches",
                newName: "IX_Matches_Participant1Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

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

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Matchеs");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_WinnerId",
                table: "Matchеs",
                newName: "IX_Matchеs_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_TournamentId",
                table: "Matchеs",
                newName: "IX_Matchеs_TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_StageId",
                table: "Matchеs",
                newName: "IX_Matchеs_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_ParticipantId",
                table: "Matchеs",
                newName: "IX_Matchеs_ParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_Participant2Id",
                table: "Matchеs",
                newName: "IX_Matchеs_Participant2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_Participant1Id",
                table: "Matchеs",
                newName: "IX_Matchеs_Participant1Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matchеs",
                table: "Matchеs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchеs_Participants_Participant1Id",
                table: "Matchеs",
                column: "Participant1Id",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchеs_Participants_Participant2Id",
                table: "Matchеs",
                column: "Participant2Id",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchеs_Participants_ParticipantId",
                table: "Matchеs",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchеs_Participants_WinnerId",
                table: "Matchеs",
                column: "WinnerId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchеs_Stages_StageId",
                table: "Matchеs",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchеs_Tournaments_TournamentId",
                table: "Matchеs",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

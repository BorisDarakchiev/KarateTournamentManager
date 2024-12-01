using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class AllowNullParticipantsInMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tournaments_TournamentId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_AspNetUsers_TimerManagerId",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Participants_Participant1Id",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Participants_Participant2Id",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Participants_ParticipantId",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Participants_WinnerId",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Stages_StageId",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Stages_Tournaments_TournamentId",
                table: "Stages");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TournamentId",
                table: "AspNetUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matchs",
                table: "Matchs");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "Matchs",
                newName: "Matchеs");

            migrationBuilder.RenameIndex(
                name: "IX_Matchs_WinnerId",
                table: "Matchеs",
                newName: "IX_Matchеs_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchs_TimerManagerId",
                table: "Matchеs",
                newName: "IX_Matchеs_TimerManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchs_StageId",
                table: "Matchеs",
                newName: "IX_Matchеs_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchs_ParticipantId",
                table: "Matchеs",
                newName: "IX_Matchеs_ParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchs_Participant2Id",
                table: "Matchеs",
                newName: "IX_Matchеs_Participant2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Matchs_Participant1Id",
                table: "Matchеs",
                newName: "IX_Matchеs_Participant1Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matchеs",
                table: "Matchеs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchеs_AspNetUsers_TimerManagerId",
                table: "Matchеs",
                column: "TimerManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Stages_Tournaments_TournamentId",
                table: "Stages",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matchеs_AspNetUsers_TimerManagerId",
                table: "Matchеs");

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
                name: "FK_Stages_Tournaments_TournamentId",
                table: "Stages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matchеs",
                table: "Matchеs");

            migrationBuilder.RenameTable(
                name: "Matchеs",
                newName: "Matchs");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_WinnerId",
                table: "Matchs",
                newName: "IX_Matchs_WinnerId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_TimerManagerId",
                table: "Matchs",
                newName: "IX_Matchs_TimerManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_StageId",
                table: "Matchs",
                newName: "IX_Matchs_StageId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_ParticipantId",
                table: "Matchs",
                newName: "IX_Matchs_ParticipantId");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_Participant2Id",
                table: "Matchs",
                newName: "IX_Matchs_Participant2Id");

            migrationBuilder.RenameIndex(
                name: "IX_Matchеs_Participant1Id",
                table: "Matchs",
                newName: "IX_Matchs_Participant1Id");

            migrationBuilder.AddColumn<Guid>(
                name: "TournamentId",
                table: "AspNetUsers",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matchs",
                table: "Matchs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TournamentId",
                table: "AspNetUsers",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tournaments_TournamentId",
                table: "AspNetUsers",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_AspNetUsers_TimerManagerId",
                table: "Matchs",
                column: "TimerManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Participants_Participant1Id",
                table: "Matchs",
                column: "Participant1Id",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Participants_Participant2Id",
                table: "Matchs",
                column: "Participant2Id",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Participants_ParticipantId",
                table: "Matchs",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Participants_WinnerId",
                table: "Matchs",
                column: "WinnerId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Stages_StageId",
                table: "Matchs",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stages_Tournaments_TournamentId",
                table: "Stages",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id");
        }
    }
}

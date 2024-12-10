using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMatchName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timers_KarateMatch_MatchId",
                table: "Timers");

            migrationBuilder.DropTable(
                name: "KarateMatch");

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Tatami = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    Participant1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Participant2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Participant1Score = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    Participant2Score = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    WinnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_Participants_Participant1Id",
                        column: x => x.Participant1Id,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Participants_Participant2Id",
                        column: x => x.Participant2Id,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Match_Participants_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Match_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Match_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Match_Participant1Id",
                table: "Match",
                column: "Participant1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_Participant2Id",
                table: "Match",
                column: "Participant2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Match_ParticipantId",
                table: "Match",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_StageId",
                table: "Match",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_TournamentId",
                table: "Match",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_WinnerId",
                table: "Match",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timers_Match_MatchId",
                table: "Timers",
                column: "MatchId",
                principalTable: "Match",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Timers_Match_MatchId",
                table: "Timers");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.CreateTable(
                name: "KarateMatch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Participant1Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Participant2Id = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TournamentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WinnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Participant1Score = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    Participant2Score = table.Column<int>(type: "int", maxLength: 100, nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Period = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Tatami = table.Column<int>(type: "int", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KarateMatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KarateMatch_Participants_Participant1Id",
                        column: x => x.Participant1Id,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KarateMatch_Participants_Participant2Id",
                        column: x => x.Participant2Id,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_KarateMatch_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_KarateMatch_Participants_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_KarateMatch_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_KarateMatch_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_KarateMatch_Participant1Id",
                table: "KarateMatch",
                column: "Participant1Id");

            migrationBuilder.CreateIndex(
                name: "IX_KarateMatch_Participant2Id",
                table: "KarateMatch",
                column: "Participant2Id");

            migrationBuilder.CreateIndex(
                name: "IX_KarateMatch_ParticipantId",
                table: "KarateMatch",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_KarateMatch_StageId",
                table: "KarateMatch",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_KarateMatch_TournamentId",
                table: "KarateMatch",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_KarateMatch_WinnerId",
                table: "KarateMatch",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Timers_KarateMatch_MatchId",
                table: "Timers",
                column: "MatchId",
                principalTable: "KarateMatch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

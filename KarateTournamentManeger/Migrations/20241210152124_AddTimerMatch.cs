using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class AddTimerMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.CreateTable(
                name: "KarateMatch",
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

            migrationBuilder.CreateTable(
                name: "Timers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountdownTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsRunning = table.Column<bool>(type: "bit", nullable: false),
                    MatchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Timers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Timers_KarateMatch_MatchId",
                        column: x => x.MatchId,
                        principalTable: "KarateMatch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.CreateIndex(
                name: "IX_Timers_MatchId",
                table: "Timers",
                column: "MatchId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Timers");

            migrationBuilder.DropTable(
                name: "KarateMatch");

            migrationBuilder.CreateTable(
                name: "Matches",
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
                    RemainingTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Tatami = table.Column<int>(type: "int", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_Participants_Participant1Id",
                        column: x => x.Participant1Id,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Participants_Participant2Id",
                        column: x => x.Participant2Id,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Matches_Participants_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Matches_Stages_StageId",
                        column: x => x.StageId,
                        principalTable: "Stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Matches_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Participant1Id",
                table: "Matches",
                column: "Participant1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_Participant2Id",
                table: "Matches",
                column: "Participant2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ParticipantId",
                table: "Matches",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_StageId",
                table: "Matches",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_TournamentId",
                table: "Matches",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_WinnerId",
                table: "Matches",
                column: "WinnerId");
        }
    }
}

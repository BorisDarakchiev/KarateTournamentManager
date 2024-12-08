using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class AddOneToOneRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tatamis_Participants_ParticipantId",
                table: "Tatamis");

            migrationBuilder.DropForeignKey(
                name: "FK_Tatamis_Participants_TimerManagerId",
                table: "Tatamis");

            migrationBuilder.DropIndex(
                name: "IX_Tatamis_ParticipantId",
                table: "Tatamis");

            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "Tatamis");

            migrationBuilder.AddForeignKey(
                name: "FK_Tatamis_Participants_TimerManagerId",
                table: "Tatamis",
                column: "TimerManagerId",
                principalTable: "Participants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tatamis_Participants_TimerManagerId",
                table: "Tatamis");

            migrationBuilder.AddColumn<Guid>(
                name: "ParticipantId",
                table: "Tatamis",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tatamis_ParticipantId",
                table: "Tatamis",
                column: "ParticipantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tatamis_Participants_ParticipantId",
                table: "Tatamis",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tatamis_Participants_TimerManagerId",
                table: "Tatamis",
                column: "TimerManagerId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

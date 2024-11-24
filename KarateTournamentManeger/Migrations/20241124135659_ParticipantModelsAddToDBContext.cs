using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManeger.Migrations
{
    /// <inheritdoc />
    public partial class ParticipantModelsAddToDBContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Participant_WinnerId",
                table: "Matchs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participant",
                table: "Participant");

            migrationBuilder.RenameTable(
                name: "Participant",
                newName: "Participants");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participants",
                table: "Participants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Participants_WinnerId",
                table: "Matchs",
                column: "WinnerId",
                principalTable: "Participants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Participants_WinnerId",
                table: "Matchs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Participants",
                table: "Participants");

            migrationBuilder.RenameTable(
                name: "Participants",
                newName: "Participant");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Participant",
                table: "Participant",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Participant_WinnerId",
                table: "Matchs",
                column: "WinnerId",
                principalTable: "Participant",
                principalColumn: "Id");
        }
    }
}

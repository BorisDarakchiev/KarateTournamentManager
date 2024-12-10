using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteConfigurations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Participants_ParticipantId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tatamis_AspNetUsers_TimerManagerId",
                table: "Tatamis");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Participants_ParticipantId",
                table: "AspNetUsers",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tatamis_AspNetUsers_TimerManagerId",
                table: "Tatamis",
                column: "TimerManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Participants_ParticipantId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tatamis_AspNetUsers_TimerManagerId",
                table: "Tatamis");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Participants_ParticipantId",
                table: "AspNetUsers",
                column: "ParticipantId",
                principalTable: "Participants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tatamis_AspNetUsers_TimerManagerId",
                table: "Tatamis",
                column: "TimerManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

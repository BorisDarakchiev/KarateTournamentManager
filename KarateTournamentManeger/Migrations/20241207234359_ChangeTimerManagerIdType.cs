using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTimerManagerIdType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tatamis_Participants_TimerManagerId",
                table: "Tatamis");

            migrationBuilder.AlterColumn<string>(
                name: "TimerManagerId",
                table: "Tatamis",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tatamis_AspNetUsers_TimerManagerId",
                table: "Tatamis",
                column: "TimerManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tatamis_AspNetUsers_TimerManagerId",
                table: "Tatamis");

            migrationBuilder.AlterColumn<Guid>(
                name: "TimerManagerId",
                table: "Tatamis",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Tatamis_Participants_TimerManagerId",
                table: "Tatamis",
                column: "TimerManagerId",
                principalTable: "Participants",
                principalColumn: "Id");
        }
    }
}

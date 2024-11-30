using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelsRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_AspNetUsers_TimerManagerId",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Stages_StageId",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Tournaments_TournamentId",
                table: "Matchs");

            migrationBuilder.DropIndex(
                name: "IX_Matchs_TournamentId",
                table: "Matchs");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "Matchs");

            migrationBuilder.AlterColumn<Guid>(
                name: "TournamentId",
                table: "Stages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "StageId",
                table: "Matchs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_AspNetUsers_TimerManagerId",
                table: "Matchs",
                column: "TimerManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Stages_StageId",
                table: "Matchs",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_AspNetUsers_TimerManagerId",
                table: "Matchs");

            migrationBuilder.DropForeignKey(
                name: "FK_Matchs_Stages_StageId",
                table: "Matchs");

            migrationBuilder.AlterColumn<Guid>(
                name: "TournamentId",
                table: "Stages",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "StageId",
                table: "Matchs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "TournamentId",
                table: "Matchs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Matchs_TournamentId",
                table: "Matchs",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_AspNetUsers_TimerManagerId",
                table: "Matchs",
                column: "TimerManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Stages_StageId",
                table: "Matchs",
                column: "StageId",
                principalTable: "Stages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Matchs_Tournaments_TournamentId",
                table: "Matchs",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

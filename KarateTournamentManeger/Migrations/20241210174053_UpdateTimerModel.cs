using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KarateTournamentManager.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTimerModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimerId",
                table: "Matches");

            migrationBuilder.AddColumn<DateTime>(
                name: "StartedAt",
                table: "Timers",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartedAt",
                table: "Timers");

            migrationBuilder.AddColumn<Guid>(
                name: "TimerId",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}

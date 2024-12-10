using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using KarateTournamentManager.Controllers;
using KarateTournamentManager.Data.Models;
using Match = KarateTournamentManager.Controllers.Match;

namespace KarateTournamentManager.Models
{
    public class Timer
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public TimeSpan CountdownTime { get; set; } = TimeSpan.FromMinutes(2);

        [Required]
        public bool IsRunning { get; set; } = false;

        [Required]
        public Guid MatchId { get; set; }
        public DateTime? StartedAt { get; set; }

        [ForeignKey(nameof(MatchId))]
        public Match? Match { get; set; }

    }
}

using KarateTournamentManeger.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateTournamentManeger.Data.Models
{
    public class Match
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public int Tatami { get; set; }

        [Required]
        [ForeignKey(nameof(TournamentId))]
        public Guid TournamentId { get; set; }
    
        [Required]
        [ForeignKey(nameof(Participant1Id))]
        public Guid Participant1Id { get; set; }

        [Required]
        [ForeignKey(nameof(Participant2Id))]
        public Guid Participant2Id { get; set; }

        public int Participant1Score { get; set; } = 0;
        public int Participant2Score { get; set; } = 0;

        [Required]
        public MatchStatus Status { get; set; } = MatchStatus.Upcoming;

        [Required]
        public MatchPeriod Period { get; set; } = MatchPeriod.Main;

        public TimeSpan RemainingTime { get; set; } 

        public Guid? WinnerId { get; set; }
        public Participant? Winner { get; set; }
    }
}

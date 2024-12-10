using KarateTournamentManager.Identity;
using KarateTournamentManager.Data.Models;
using KarateTournamentManager.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using static KarateTournamentManager.Constants.ModelConstants;


namespace KarateTournamentManager.Controllers
{
    public class Match
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(MaxTatamiLenght)]
        public int Tatami { get; set; }

        public Guid? Participant1Id { get; set; }

        [ForeignKey(nameof(Participant1Id))]
        public Participant? Participant1 { get; set; }

        public Guid? Participant2Id { get; set; }

        [ForeignKey(nameof(Participant2Id))]
        public Participant? Participant2 { get; set; }

        [StringLength(MaxScoreLenght)]
        public int Participant1Score { get; set; } = 0;

        [StringLength(MaxScoreLenght)]
        public int Participant2Score { get; set; } = 0;

        [Required]
        public MatchStatus Status { get; set; } = MatchStatus.Upcoming;

        [Required]
        public MatchPeriod Period { get; set; } = MatchPeriod.Main;

        public TimeSpan RemainingTime { get; set; } = TimeSpan.FromMinutes(2);

        public Guid? WinnerId { get; set; }

        [ForeignKey(nameof(WinnerId))]
        public Participant? Winner { get; set; }

        [Required]
        public Guid StageId { get; set; }

        [Required]
        [ForeignKey(nameof(StageId))]
        [StringLength(MaxStageLenght)]
        public Stage Stage { get; set; } = null!;

        public Guid TournamentId { get; set; }

        [Required]
        [ForeignKey(nameof(TournamentId))]
        public Tournament Tournament { get; set; } = null!;

    }
}
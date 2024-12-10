using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManager.Models.Requests
{
    public class TimerRequest
    {
        public Guid MatchId { get; set; }
    }

    public class SetDurationRequest
    {
        public Guid MatchId { get; set; }

        [Range(1, 600, ErrorMessage = "между 1 и 600 секунди.")]
        public int Duration { get; set; }
    }

    public class UpdateScoreRequest
    {
        public Guid MatchId { get; set; }
        public string Participant { get; set; }
        public int Points { get; set; }
    }

    public class SetWinnerRequest
    {
        public Guid MatchId { get; set; }
        public Guid WinnerId { get; set; }
    }

    public class StartMatchRequest
    {
        public Guid MatchId { get; set; }
        public int DurationInSeconds { get; set; }
    }

    public class StopTimerRequest
    {
        public Guid MatchId { get; set; }
    }
}

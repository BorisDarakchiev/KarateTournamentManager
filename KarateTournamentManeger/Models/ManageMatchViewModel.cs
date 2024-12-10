namespace KarateTournamentManager.Models
{
    public class ManageMatchViewModel
    {
        public Guid MatchId { get; set; }
        public string Participant1Name { get; set; } = string.Empty;
        public string Participant2Name { get; set; } = string.Empty;
        public int Participant1Score { get; set; }
        public int Participant2Score { get; set; }
        public int Tatami { get; set; }
        public TimeSpan RemainingTime { get; set; }
        public string Status { get; set; } = string.Empty;


    }

}

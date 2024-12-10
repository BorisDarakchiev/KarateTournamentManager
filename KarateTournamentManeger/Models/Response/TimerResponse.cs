namespace KarateTournamentManager.Models.Response
{
    public class UpdateScoreResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Participant1Score { get; set; }
        public int Participant2Score { get; set; }
    }
}

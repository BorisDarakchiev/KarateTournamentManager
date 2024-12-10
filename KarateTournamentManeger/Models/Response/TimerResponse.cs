namespace KarateTournamentManager.Models.Response
{
    public class UpdateScoreResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int Participant1Score { get; set; }
        public int Participant2Score { get; set; }
    }

    public class SetWinnerResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid? WinnerId { get; set; }
        public string WinnerName { get; set; }
        public string MatchStatus { get; set; }

        public SetWinnerResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }

        public SetWinnerResponse(bool success, string message, Guid? winnerId, string winnerName, string matchStatus)
        {
            Success = success;
            Message = message;
            WinnerId = winnerId;
            WinnerName = winnerName;
            MatchStatus = matchStatus;
        }
    }
}


using KarateTournamentManager.Models;
using KarateTournamentManager.Models.Requests;
using KarateTournamentManager.Models.Response;
using KarateTournamentManager.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace KarateTournamentManager.Services
{
    public interface ITimerManagerService
    {
        Task<MatchViewModel> GetMatchViewModelAsync(Guid matchId);
        Task<TournamentViewModel> GetMatchesForTimerManagerAsync(string? timerManagerName);
        Task<List<TournamentViewModel>> GetTournamentsAsync();
        Task<bool> StartMatchAsync(Guid matchId);
        Task<bool> SetDurationAsync(Guid matchId, TimeSpan duration);
        Task<bool> SetWinnerAsync(Guid matchId, Guid winnerId);
        Task<bool> StartTimerAsync(Guid matchId);
        Task<bool> StopTimerAsync(Guid matchId);
        Task<UpdateScoreResponse> UpdateScoreAsync(Guid matchId, string participant, int points);
        Task<bool> AddExtraPeriodAsync(Guid matchId);
        Task<bool> StopMatchAsync(Guid matchId);
    }
}

using KarateTournamentManager.Controllers;
using KarateTournamentManager.Models;
using KarateTournamentManager.Models.ViewModels;

namespace KarateTournamentManager.Services
{
    public interface ITimerManagerService
    {
        Task<TournamentViewModel> GetMatchesForTimerManagerAsync(string? name);
        Task<List<TournamentViewModel>> GetTournamentsAsync();
    }
}

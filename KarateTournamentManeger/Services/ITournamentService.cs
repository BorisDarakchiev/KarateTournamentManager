using KarateTournamentManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarateTournamentManager.Services
{
    public interface ITournamentService
    {
        Task<List<TournamentViewModel>> GetTournamentsAsync();
        Task<TournamentViewModel> CreateTournamentViewModelAsync();
        Task<bool> CreateTournamentAsync(TournamentViewModel model);
        Task<bool> AddTatamiAsync(Guid tournamentId);
        Task<bool> RemoveTatamiAsync(Guid tatamiId);
        Task<TournamentViewModel> GetTournamentDetailsAsync(Guid id);
        Task<bool> DeleteTournamentAsync(Guid id);
        Task<bool> RemoveParticipantAsync(Guid tournamentId, Guid participantId);
        Task<string?> FinalizeEnrollmentAsync(Guid tournamentId);
        Task<List<TournamentViewModel>> GetTournamentsViewModelsAsync(string userId);
        

    }
}

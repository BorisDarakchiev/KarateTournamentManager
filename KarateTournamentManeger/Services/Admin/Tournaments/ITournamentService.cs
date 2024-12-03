using KarateTournamentManager.Models;
using Microsoft.AspNetCore.Mvc;

namespace KarateTournamentManager.Services.Admin.Tournaments
{
    public interface ITournamentService
    {
        Task<List<TournamentViewModel>> GetTournamentsAsync();
        Task<TournamentViewModel> CreateTournamentViewModelAsync();
        Task<bool> CreateTournamentAsync(TournamentViewModel model);
        Task<TournamentViewModel> GetTournamentDetailsAsync(Guid id);
        Task<bool> DeleteTournamentAsync(Guid id);
        Task<bool> RemoveParticipantAsync(Guid tournamentId, Guid participantId);
        Task<IActionResult> FinalizeEnrollment(Guid tournamentId);

    }
}

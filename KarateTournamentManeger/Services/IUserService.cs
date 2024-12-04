using KarateTournamentManager.Models.ViewModels;

namespace KarateTournamentManager.Services
{
    public interface IUserService
    {
        Task<List<ManageUsersViewModel>> GetManageUsersViewModelAsync();
        Task<bool> RemoveParticipantAsync(Guid tournamentId, Guid participantId);
        Task<bool> UpdateUserRoleAsync(string userId, string selectedRole);



    }
}

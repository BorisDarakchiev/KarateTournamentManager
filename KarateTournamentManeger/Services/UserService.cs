using KarateTournamentManager.Data;
using KarateTournamentManager.Identity;
using KarateTournamentManager.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KarateTournamentManager.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;

        public UserService(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, ApplicationDbContext _context)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            context = _context;
        }

        public async Task<List<ManageUsersViewModel>> GetManageUsersViewModelAsync()
        {
            var users = userManager.Users.ToList();
            var roles = await roleManager.Roles.ToListAsync();

            var model = new List<ManageUsersViewModel>();

            foreach (var user in users)
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var participant = await context.Participants
                                                  .FirstOrDefaultAsync(p => p.Id == user.ParticipantId);

                var viewModel = new ManageUsersViewModel
                {
                    UserId = user.Id,
                    UserName = participant?.Name ?? user.UserName,
                    Email = user.Email,
                    CurrentRole = userRoles.FirstOrDefault(),
                    AvailableRoles = roles.Select(r => r.Name).ToList()
                };

                model.Add(viewModel);
            }

            return model;
        }


        public async Task<bool> RemoveParticipantAsync(Guid tournamentId, Guid participantId)
        {
            var tournament = await context.Tournaments
                .Include(t => t.EnrolledParticipants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null)
            {
                return false;
            }

            var participant = tournament.EnrolledParticipants.FirstOrDefault(p => p.Id == participantId);
            if (participant == null)
            {
                return false;
            }

            tournament.EnrolledParticipants.Remove(participant);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateUserRoleAsync(string userId, string selectedRole)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false;
            }

            var currentRoles = await userManager.GetRolesAsync(user);

            if (currentRoles.Contains(selectedRole))
            {
                return true;
            }

            await userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await userManager.AddToRoleAsync(user, selectedRole);

            return result.Succeeded;
        }

    }
}

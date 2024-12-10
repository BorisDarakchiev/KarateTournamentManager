using KarateTournamentManager.Controllers;
using KarateTournamentManager.Data;
using KarateTournamentManager.Data.Models;
using KarateTournamentManager.Identity;
using KarateTournamentManager.Models;
using KarateTournamentManager.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace KarateTournamentManager.Services
{
    public class TimerManagerService : ITimerManagerService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public TimerManagerService(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            context = _context;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public async Task<TournamentViewModel> GetMatchesForTimerManagerAsync(string? userId)
        {
            var tatamis = await context.Tatamis
                .Where(t => t.TimerManagerId == userId).ToListAsync();


            var matches = new List<Match>();

            foreach (var tatami in tatamis)
            {
                var matchForManager = await context.Matchеs.Include(m => m.Participant1).Include(m => m.Participant2).Where(m => m.TournamentId == tatami.TournamentId && tatami.Number == m.Tatami).ToListAsync();
                matches.AddRange(matchForManager);
            }

            var model = new TournamentViewModel();

            foreach (var match in matches)
            {
                if (match != null)
                {
                    var tournament = await context.Tournaments.FirstOrDefaultAsync(t => t.Id == match.TournamentId);
                    var tatami = await context.Tatamis.Include(t => t.TimerManager).Where(t => t.TournamentId == tournament.Id).ToListAsync();

                    var tournamentViewModel = new TournamentViewModel
                    {
                        Id = tournament.Id,
                        Location = tournament.Location,
                        Description = tournament.Description,
                        Date = tournament.Date,
                        Status = tournament.Status,
                        Tatami = tatami
                    };

                    var matchViewModel = new MatchViewModel
                    {
                        Id = match.Id,
                        Participant1Name = match.Participant1?.Name ?? "N/A",
                        Participant2Name = match.Participant2?.Name ?? "N/A",
                        Participant1Score = match.Participant1Score,
                        Participant2Score = match.Participant2Score,
                        Period = match.Period,
                        Tatami = match.Tatami,
                        RemainingTime = match.RemainingTime.ToString(@"mm\:ss"),
                        Status = match.Status,
                        TournamentId = match.TournamentId,
                        WinnerName = match.Winner?.Name
                    };

                    tournamentViewModel.Matches.Add(matchViewModel);

                    model = tournamentViewModel;
                }
            }

            return model;
        }

        public async Task<List<TournamentViewModel>> GetTournamentsAsync()
        {
            return await context.Tournaments
                .OrderByDescending(t => t.Date)
                .Select(t => new TournamentViewModel
                {
                    Id = t.Id,
                    Location = t.Location,
                    Description = t.Description,
                    Date = t.Date,
                    Status = t.Status,
                    EnrolledParticipantsCount = t.EnrolledParticipants.Count
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}

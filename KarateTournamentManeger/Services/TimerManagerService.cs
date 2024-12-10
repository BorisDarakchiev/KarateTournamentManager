using KarateTournamentManager.Controllers;
using KarateTournamentManager.Data;
using KarateTournamentManager.Data.Models;
using KarateTournamentManager.Enums;
using KarateTournamentManager.Identity;
using KarateTournamentManager.Models;
using KarateTournamentManager.Models.Response;
using KarateTournamentManager.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
                var matchForManager = await context.Matches.Include(m => m.Participant1).Include(m => m.Participant2).Where(m => m.TournamentId == tatami.TournamentId && tatami.Number == m.Tatami).ToListAsync();
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
                        Participant1Id = match.Participant1.Id,
                        Participant2Name = match.Participant2?.Name ?? "N/A",
                        Participant2Id = match.Participant2.Id,
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

        public async Task<MatchViewModel> GetMatchViewModelAsync(Guid matchId)
        {
            var match = await context.Matches
                .Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .Include(m => m.Tournament)
                .FirstOrDefaultAsync(m => m.Id == matchId);

            if (match == null)
            {
                throw new InvalidOperationException("Мачът не е намерен.");
            }

            return new MatchViewModel
            {
                Id = match.Id,
                Participant1Name = match.Participant1?.Name ?? "Непосочен",
                Participant2Name = match.Participant2?.Name ?? "Непосочен",
                Participant1Id = match.Participant1Id.Value,
                Participant1Score = match.Participant1Score,
                Participant2Score = match.Participant2Score,
                Participant2Id = match.Participant2Id.Value,
                Period = match.Period,
                Tatami = match.Tatami,
                RemainingTime = match.RemainingTime != default(TimeSpan) ? match.RemainingTime.ToString(@"mm\:ss") : "00:00",
                Status = match.Status,
                TournamentId = match.TournamentId,
                TournamentName = match.Tournament?.Location ?? "Непосочен",
                WinnerName = match.Winner?.Name
            };
        }

        public async Task<bool> StartMatchAsync(Guid matchId)
        {
            var match = await context.Matches.FindAsync(matchId);
            if (match == null)
            {
                throw new InvalidOperationException("Мачът не е намерен.");
            }

            match.Status = MatchStatus.InProgress;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StopMatchAsync(Guid matchId)
        {
            var match = await context.Matches.FindAsync(matchId);
            if (match == null)
            {
                throw new InvalidOperationException("Мачът не е намерен.");
            }

            match.Status = MatchStatus.Paused;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddExtraPeriodAsync(Guid matchId)
        {
            var match = await context.Matches.FindAsync(matchId);
            if (match == null)
            {
                throw new InvalidOperationException("Мачът не е намерен.");
            }

            match.Period = MatchPeriod.Extratime;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetDurationAsync(Guid matchId, TimeSpan duration)
        {
            var match = await context.Matches.FindAsync(matchId);
            if (match == null)
            {
                return false; // Мачът не съществува
            }

            match.RemainingTime = duration; // Задаваме оставащото време
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetWinnerAsync(Guid matchId, Guid winnerId)
        {
            var match = await context.Matches
                .Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .FirstOrDefaultAsync(m => m.Id == matchId);

            if (match == null)
            {
                return false; // Мачът не съществува
            }

            if (match.Participant1?.Id == winnerId)
            {
                match.Winner = match.Participant1;
            }
            else if (match.Participant2?.Id == winnerId)
            {
                match.Winner = match.Participant2;
            }
            else
            {
                return false; // Невалиден победител
            }

            match.Status = MatchStatus.Finished;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StartTimerAsync(Guid matchId)
        {
            var match = await context.Matches.FindAsync(matchId);
            if (match == null)
            {
                return false; // Мачът не съществува
            }

            match.Status = MatchStatus.InProgress;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> StopTimerAsync(Guid matchId)
        {
            var match = await context.Matches.FindAsync(matchId);
            if (match == null)
            {
                return false; // Мачът не съществува
            }

            match.Status = MatchStatus.Paused;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<UpdateScoreResponse> UpdateScoreAsync(Guid matchId, string participant, int points)
        {
            var match = await context.Matches
                .FirstOrDefaultAsync(m => m.Id == matchId);

            if (match == null)
            {
                return new UpdateScoreResponse
                {
                    Success = false,
                    Message = "Мачът не съществува"
                };
            }

            if (participant == "Participant1")
            {
                if (match.Participant1Score + points >= 0)
                {
                    match.Participant1Score += points;
                }
                else
                {
                    return new UpdateScoreResponse
                    {
                        Success = false,
                        Message = "Невалид брой точки (отрицателен резултат не е позволен)"
                    };
                }
            }
            else if (participant == "Participant2")
            {
                if (match.Participant2Score + points >= 0)
                {
                    match.Participant2Score += points;
                }
                else
                {
                    return new UpdateScoreResponse
                    {
                        Success = false,
                        Message = "Невалид брой точки (отрицателен резултат не е позволен)"
                    };
                }
            }
            else
            {
                return new UpdateScoreResponse
                {
                    Success = false,
                    Message = "Невалиден участник"
                };
            }

            await context.SaveChangesAsync();

            return new UpdateScoreResponse
            {
                Success = true,
                Participant1Score = match.Participant1Score,
                Participant2Score = match.Participant2Score
            };
        }
    }
}

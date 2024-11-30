using KarateTournamentManager.Identity;
using KarateTournamentManager.Data;
using KarateTournamentManager.Models;
using KarateTournamentManager.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KarateTournamentManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;

        public HomeController(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, ApplicationDbContext _context)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            context = _context;
        }

        public async Task<IActionResult> Index()
        {
            var tournamentViewModels = await GetTournamentsViewModels();
            ViewData["Tournaments"] = tournamentViewModels;
            return View();
        }

        private async Task<List<TournamentViewModel>> GetTournamentsViewModels()
        {
            var tournaments = await context.Tournaments
                .Include(t => t.EnrolledParticipants)
                .Where(t => t.Date >= DateTime.Now)
                .OrderBy(t => t.Date)
                .ToListAsync();

            var currentUser = await userManager.GetUserAsync(User);
            var currentUserParticipantId = currentUser?.ParticipantId;

            return tournaments.Select(t => new TournamentViewModel
            {
                Id = t.Id,
                Location = t.Location,
                Description = t.Description,
                Date = t.Date,
                Status = t.Status,
                EnrolledParticipantsCount = t.EnrolledParticipants?.Count ?? 0,
                EnrolledParticipants = t.EnrolledParticipants != null
                    ? t.EnrolledParticipants
                        .Select(p => new ParticipantViewModel { Name = p.Name, Id = p.Id })
                        .ToList()
                    : new List<ParticipantViewModel>(),
                IsParticipant = currentUserParticipantId.HasValue &&
                                t.EnrolledParticipants.Any(p => p.Id == currentUserParticipantId.Value),
                Stages = t.Stages.Select(s => new StageViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToList()
            }).ToList();
        }

        public async Task<IActionResult> TournamentListPartial()
        {
            var tournamentViewModels = await GetTournamentsViewModels();
            return PartialView("_TournamentListPartial", tournamentViewModels);
        }


        public async Task<IActionResult> UpcomingTournamentsPartial()
        {
            var tournamentViewModels = await GetTournamentsViewModels();
            return PartialView("_UpcomingTournamentsPartial", tournamentViewModels);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

using KarateTournamentManager.Identity;
using KarateTournamentManeger.Data;
using KarateTournamentManeger.Models;
using KarateTournamentManeger.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace KarateTournamentManeger.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext context;

        public HomeController(ApplicationDbContext _context)
        {
            context = _context;
        }

        public async Task<IActionResult> Index()
        {
            var tournaments = await context.Tournaments
                .Where(t => t.Date >= DateTime.Now)
                .OrderBy(t => t.Date)
                .ToListAsync();

            var tournamentViewModels = tournaments.Select(t => new TournamentViewModel
            {
                Id = t.Id,
                Location = t.Location,
                Description = t.Description,
                Date = t.Date,
                Status = t.Status,
                EnrolledParticipants = t.EnrolledParticipants != null && t.EnrolledParticipants.Any()
                ? t.EnrolledParticipants
                    .Select(p => new ParticipantViewModel { Name = p.Name, Id = p.Id })
                    .ToList()
                : new List<ParticipantViewModel>(),


                Stages = t.Stages.Select(s => new StageViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToList()
            }).ToList();

            ViewData["Tournaments"] = tournamentViewModels;
            return View();
        }

        public IActionResult TournamentListPartial()
        {
            var tournaments = context.Tournaments
                .Where(t => t.Date >= DateTime.Now)
                .OrderBy(t => t.Date)
                .ToList();

            var tournamentViewModels = tournaments.Select(t => new TournamentViewModel
            {
                Id = t.Id,
                Location = t.Location,
                Description = t.Description,
                Date = t.Date,
                Status = t.Status,
                EnrolledParticipants = t.EnrolledParticipants != null && t.EnrolledParticipants.Any()
                ? t.EnrolledParticipants
                    .Select(p => new ParticipantViewModel { Name = p.Name, Id = p.Id })
                    .ToList()
                : new List<ParticipantViewModel>(),
                Stages = t.Stages.Select(s => new StageViewModel
                {
                    Id = s.Id,
                    Name = s.Name,
                }).ToList()
            }).ToList();

            return PartialView("_TournamentListPartial", tournamentViewModels);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

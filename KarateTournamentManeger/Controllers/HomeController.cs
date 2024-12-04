using KarateTournamentManager.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KarateTournamentManager.Services;

namespace KarateTournamentManager.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITournamentService tournamentService;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(ITournamentService _tournamentService, UserManager<ApplicationUser> _userManager)
        {
            tournamentService = _tournamentService;
            userManager = _userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = userManager.GetUserId(User);
            var tournamentViewModels = await tournamentService.GetTournamentsViewModelsAsync(currentUserId);
            ViewData["Tournaments"] = tournamentViewModels;
            return View();
        }

        public async Task<IActionResult> TournamentListPartial()
        {
            var currentUserId = userManager.GetUserId(User);
            var tournamentViewModels = await tournamentService.GetTournamentsViewModelsAsync(currentUserId);
            return PartialView("_TournamentListPartial", tournamentViewModels);
        }

        public async Task<IActionResult> UpcomingTournamentsPartial()
        {
            var currentUserId = userManager.GetUserId(User);
            var tournamentViewModels = await tournamentService.GetTournamentsViewModelsAsync(currentUserId);
            return PartialView("_UpcomingTournamentsPartial", tournamentViewModels);
        }
    }
}

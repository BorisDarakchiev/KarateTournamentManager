using KarateTournamentManager.Data;
using KarateTournamentManager.Identity;
using KarateTournamentManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;


namespace KarateTournamentManager.Controllers
{
    [Route("TimerManager")]
    [Authorize(Roles = "TimerManager")]
    public class TimerManagerController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ApplicationDbContext context;
        private readonly ITimerManagerService timerManagerService;


        public TimerManagerController(UserManager<ApplicationUser> _userManager, ApplicationDbContext _context, ITimerManagerService _timerManagerService)
        {
            userManager = _userManager;
            context = _context;
            timerManagerService = _timerManagerService;

        }

        [HttpGet]
        [Route("Tournaments")]
        public async Task<IActionResult> Tournaments()
        {
            var model = await timerManagerService.GetTournamentsAsync();
            return View(model);
        }

        [HttpGet]
        [Route("Matches")]
        public async Task<IActionResult> Matches()
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var matches = await timerManagerService.GetMatchesForTimerManagerAsync(userId);
            return View(matches);
        }


        [HttpPost]
    public IActionResult UpdateScore(Guid matchId, int scoreA, int scoreB)
    {
        return RedirectToAction("Matches");
    }
}
}
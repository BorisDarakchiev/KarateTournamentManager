using KarateTournamentManager.Data;
using KarateTournamentManager.Identity;
using KarateTournamentManager.Models.ViewModels;
using KarateTournamentManager.Models;
using KarateTournamentManager.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;
using KarateTournamentManager.Models.Requests;


namespace KarateTournamentManager.Controllers
{
    [ApiController]
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


        [HttpGet]
        public async Task<IActionResult> ManageMatch(Guid matchId)
        {
            try
            {
                var viewModel = await timerManagerService.GetMatchViewModelAsync(matchId);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Грешка: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddExtraPeriod(Guid matchId)
        {
            try
            {
                await timerManagerService.AddExtraPeriodAsync(matchId);
                return RedirectToAction("ManageMatch", new { matchId });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }



        [HttpPost]
        public async Task<IActionResult> StartMatch(Guid matchId)
        {
            await timerManagerService.StartMatchAsync(matchId);
            return RedirectToAction(nameof(ManageMatch), new { matchId });
        }

        [HttpPost]
        public async Task<IActionResult> StopMatch(Guid matchId)
        {
            await timerManagerService.StopMatchAsync(matchId);
            return RedirectToAction(nameof(ManageMatch), new { matchId });
        }

        [HttpPost]
        public async Task<IActionResult> SetWinner(Guid matchId, Guid winnerId)
        {
            try
            {
                await timerManagerService.SetWinnerAsync(matchId, winnerId);
                return RedirectToAction(nameof(ManageMatch), new { matchId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var matchViewModel = await timerManagerService.GetMatchViewModelAsync(matchId);
                return View("MatchDetails", matchViewModel);
            }
        }

        //AJAX


        [HttpPost("starttimer")]
        public async Task<IActionResult> StartTimer([FromBody] TimerRequest request)
        {
            await timerManagerService.StartTimerAsync(request.MatchId);
            return Ok("Таймерът е стартиран!");
        }

        [HttpPost("stoptimer")]
        public async Task<IActionResult> StopTimer([FromBody] TimerRequest request)
        {
            await timerManagerService.StopTimerAsync(request.MatchId);
            return Ok("Таймерът е спрян!");
        }

        [HttpPost("setduration")]
        public async Task<IActionResult> SetDuration([FromBody] SetDurationRequest request)
        {
            await timerManagerService.SetDurationAsync(request.MatchId, TimeSpan.FromSeconds(request.Duration));
            return Ok($"Продължителността е зададена на {request.Duration} секунди.");
        }

        [HttpPost("updatescore")]
        public async Task<IActionResult> UpdateScore([FromBody] UpdateScoreRequest request)
        {
            var result = await timerManagerService.UpdateScoreAsync(request.MatchId, request.Participant, request.Points);

            if (!result.Success)
            {
                return BadRequest(new { success = false, message = result.Message });
            }

            return Ok(new
            {
                success = true,
                participant1Score = result.Participant1Score,
                participant2Score = result.Participant2Score
            });
        }


        [HttpPost("setwinner")]
        public async Task<IActionResult> SetWinner([FromBody] SetWinnerRequest request)
        {
            await timerManagerService.SetWinnerAsync(request.MatchId, request.WinnerId);
            return Ok($"Победител е: {request.WinnerId}");
        }
    }
}
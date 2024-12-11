using KarateTournamentManager.Identity;
using KarateTournamentManager.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KarateTournamentManager.Enums;
using KarateTournamentManager.Models.ViewModels;
using KarateTournamentManager.Models;
using KarateTournamentManager.Data.Models;
using System.Security.Claims;


namespace KarateTournamentManager.Controllers
{
    [Route("Participant")]
    [Authorize(Roles = "Participant")]
    public class ParticipantController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;

        public ParticipantController(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, ApplicationDbContext _context)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            context = _context;
        }
        public IActionResult Tournaments()
        {
            return View();
        }

        [HttpGet]
        [Route("Matches")]
        public async Task<IActionResult> Matches()
        {
            return View();
        }



        [HttpPost]
        [Route("RegisterForTournament")]
        public IActionResult Register(Guid tournamentId)
        {
            return RedirectToAction("Tournaments");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterForTournament(Guid tournamentId)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("Трябва да сте логнат, за да се запишете за турнир.");
            }

            if (string.IsNullOrEmpty(currentUser.ParticipantId?.ToString()) || !Guid.TryParse(currentUser.ParticipantId.ToString(), out var participantId))
            {
                return BadRequest("Не сте свързан като участник.");
            }

            var participant = await context.Participants
                .FirstOrDefaultAsync(p => p.Id == participantId);

            if (participant == null)
            {
                return BadRequest("Не сте регистриран като участник.");
            }

            var tournament = await context.Tournaments
                .Include(t => t.EnrolledParticipants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null)
            {
                return NotFound("Турнирът не е намерен.");
            }

            if (tournament.EnrolledParticipants == null)
            {
                tournament.EnrolledParticipants = new List<Participant>();
            }

            if (tournament.EnrolledParticipants.Any(p => p.Id == participant.Id))
            {
                return BadRequest("Вече сте записан за този турнир.");
            }

            tournament.EnrolledParticipants.Add(participant);
            await context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }


    }
}
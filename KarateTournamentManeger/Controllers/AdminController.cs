using KarateTournamentManager.Enums;
using KarateTournamentManager.Identity;
using KarateTournamentManager.Data;
using KarateTournamentManager.Data.Models;
using KarateTournamentManager.Models;
using KarateTournamentManager.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Cryptography.X509Certificates;
using System.Diagnostics;
using KarateTournamentManager.Services;
using Microsoft.EntityFrameworkCore.Metadata.Internal;


namespace KarateTournamentManager.Controllers
{
    [Route("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly ITournamentService tournamentService;
        private readonly IUserService userService;
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public AdminController(ITournamentService _tournamentService, IUserService _userService, ApplicationDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            tournamentService = _tournamentService;
            userService = _userService;
            context = _context;
            userManager = _userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("ManageUsers")]
        public async Task<IActionResult> ManageUsers()
        {
            var model = await userService.GetManageUsersViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [Route("Admin/RemoveParticipant")]
        public async Task<IActionResult> RemoveParticipant(Guid tournamentId, Guid participantId)
        {
            var success = await tournamentService.RemoveParticipantAsync(tournamentId, participantId);

            if (!success)
            {
                return NotFound("Участникът не е намерен в този турнир.");
            }

            TempData["SuccessMessage"] = "Участникът беше успешно премахнат от турнира.";
            return RedirectToAction("TournamentDetails", new { id = tournamentId });
        }

        [HttpPost]
        [Route("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(string userId, string selectedRole)
        {
            var success = await userService.UpdateUserRoleAsync(userId, selectedRole);

            if (!success)
            {
                return NotFound();
            }

            return RedirectToAction("ManageUsers");
        }

        [Route("Tournaments")]
        public async Task<IActionResult> Tournaments()
        {
            var model = await tournamentService.GetTournamentsAsync();
            return View(model);
        }

        [HttpGet]
        [Route("Admin/CreateTournament")]
        public async Task<IActionResult> CreateTournament()
        {
            var model = await tournamentService.CreateTournamentViewModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/CreateTournament")]
        public async Task<IActionResult> CreateTournament(TournamentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await tournamentService.CreateTournamentAsync(model);
                if (result)
                {
                    return RedirectToAction("Tournaments");
                }
            }

            return View(model);
        }

        [HttpGet]
        [Route("TournamentDetails/{id}")]
        public async Task<IActionResult> TournamentDetails(Guid id)
        {
            var model = await tournamentService.GetTournamentDetailsAsync(id);

            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        [Route("Admin/DeleteTournament")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTournament(Guid id)
        {
            var result = await tournamentService.DeleteTournamentAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("Tournaments");
        }

        [HttpPost]
        [Route("AddTatami")]
        public async Task<IActionResult> AddTatami(Guid tournamentId)
        {
            var result = await tournamentService.AddTatamiAsync(tournamentId);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("TournamentDetails", "Admin", new { id = tournamentId });
        }

        [HttpPost]
        [Route("RemoveTatami")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveTatami(Guid tournamentId, Guid tatamiId)
        {
            var result = await tournamentService.RemoveTatamiAsync(tatamiId);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("TournamentDetails", "Admin", new { id = tournamentId });
        }

        [HttpPost]
        [Route("UpdateTatamiTimerManager")]
        public async Task<IActionResult> UpdateTatamiTimerManager(Guid tatamiId, string tatamiNumber, string selectedTimerManagerId, string tournamentId)
        {
            var result = await tournamentService.UpdateTatamiTimerManagerAsync(tatamiId, tatamiNumber, selectedTimerManagerId);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("TournamentDetails", "Admin", new { id = tournamentId });
        }
        
        [HttpPost]
        [Route("RemoveTatamiTimerManager")]
        public async Task<IActionResult> RemoveTatamiTimerManager(Guid tatamiId, string tatamiNumber, string tournamentId)
        {
            var result = await tournamentService.RemoveTatamiTimerManagerAsync(tatamiId, tatamiNumber);

            if (!result)
            {
                return NotFound();
            }

            return RedirectToAction("TournamentDetails", "Admin", new { id = tournamentId });
        }


        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> FinalizeEnrollment(Guid tournamentId)
        {
            var errorMessage = await tournamentService.FinalizeEnrollmentAsync(tournamentId);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                return View("Error", new ErrorViewModel
                {
                    Message = errorMessage,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                });
            }
            return RedirectToAction("TournamentDetails", "Admin", new { id = tournamentId });
        }
    }
}

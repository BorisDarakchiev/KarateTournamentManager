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


namespace KarateTournamentManager.Controllers
{
    [Route("Admin")]
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbContext context;

        public AdminController(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, ApplicationDbContext _context)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            context = _context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("Tournaments")]
        public async Task<IActionResult> Tournaments()
        {
            var model = await context.Tournaments
                .OrderByDescending(t => t.Date)
                .Select(t => new Tournament()
                {
                    Id = t.Id,
                    Location = t.Location,
                    Description = t.Description,
                    Date = t.Date,
                    Status = t.Status
                })
                .AsNoTracking()
                .ToListAsync();

            return View(model);
        }

        [Route("Participants")]
        public IActionResult Participants()
        {
            return View();
        }

        [HttpGet]
        [Route("ManageUsers")]
        public async Task<IActionResult> ManageUsers()
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

            return View(model);
        }


        [HttpGet]
        [Route("Admin/CreateTournament")]
        public IActionResult CreateTournament()
        {
            var model = new TournamentViewModel
            {
                Date = DateTime.Now,
                Status = TournamentStatus.Upcoming,
                EnrolledParticipants = new List<ParticipantViewModel>(),
                Stages = new List<StageViewModel>(),
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Admin/CreateTournament")]
        public async Task<IActionResult> CreateTournament(TournamentViewModel model)
        {
            if (ModelState.IsValid)
            {
                var tournament = new Tournament
                {
                    Id = Guid.NewGuid(),
                    Location = model.Location,
                    Description = model.Description,
                    Date = model.Date,
                    Status = TournamentStatus.Upcoming,
                    EnrolledParticipants = new List<Participant>(),
                };

                context.Tournaments.Add(tournament);
                await context.SaveChangesAsync();

                return RedirectToAction("Tournaments");
            }

            return View(model);
        }


        [HttpGet]
        [Route("TournamentDetails/{id}")]
        public async Task<IActionResult> TournamentDetails(Guid id)
        {
            var tournament = await context.Tournaments
                .Include(t => t.EnrolledParticipants)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tournament == null)
            {
                return NotFound();
            }

            var enrolledParticipants = tournament.EnrolledParticipants.Select(p =>
            {
                var user = context.Users.FirstOrDefault(u => u.ParticipantId == p.Id);
                return new ParticipantViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = user?.Email ?? "N/A"
                };
            }).ToList();

            var model = new TournamentViewModel
            {
                Id = tournament.Id,
                Location = tournament.Location,
                Description = tournament.Description,
                Date = tournament.Date,
                Status = tournament.Status,
                EnrolledParticipantsCount = enrolledParticipants.Count,
                EnrolledParticipants = enrolledParticipants,
            };

            return View(model);
        }

        [HttpPost]
        [Route("Admin/DeleteTournament")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTournament(Guid id)
        {
            var tournament = await context.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }

            context.Tournaments.Remove(tournament);
            await context.SaveChangesAsync();

            return RedirectToAction("Tournaments");
        }

        [HttpPost]
        [Route("ApproveParticipant")]
        public IActionResult ApproveParticipant(Guid participantId)
        {
            return RedirectToAction("Participants");
        }

        [HttpPost]
        [Route("Admin/RemoveParticipant")]
        public async Task<IActionResult> RemoveParticipant(Guid tournamentId, Guid participantId)
        {
            var tournament = await context.Tournaments
                .Include(t => t.EnrolledParticipants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null)
            {
                return NotFound("Турнирът не е намерен.");
            }

            var participant = tournament.EnrolledParticipants.FirstOrDefault(p => p.Id == participantId);
            if (participant == null)
            {
                return NotFound("Участникът не е намерен в този турнир.");
            }

            tournament.EnrolledParticipants.Remove(participant);

            await context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"Участникът {participant.Name} беше успешно премахнат от турнира.";
            return RedirectToAction("TournamentDetails", new { id = tournamentId });
        }

        [HttpPost]
        [Route("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(string userId, string selectedRole)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await userManager.GetRolesAsync(user);

            if (currentRoles.Contains(selectedRole))
            {
                return RedirectToAction("ManageUsers");
            }

            await userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await userManager.AddToRoleAsync(user, selectedRole);

            if (result.Succeeded)
            {
                return RedirectToAction("ManageUsers");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View();
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> FinalizeEnrollment(Guid tournamentId)
        {
            var tournament = await context.Tournaments
                .Include(t => t.EnrolledParticipants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null)
            {
                return NotFound("Турнирът не е намерен.");
            }

            var participants = tournament.EnrolledParticipants.ToList();

            if (participants.Count < 2)
            {
                return BadRequest("Необходими са поне двама участници за създаване на етапи.");
            }

            var existingStages = await context.Stages
                .Where(s => s.TournamentId == tournamentId)
                .ToListAsync();

            foreach (var stage in existingStages)
            {
                var matches = await context.Matchs
                    .Where(m => m.StageId == stage.Id)
                    .ToListAsync();

                context.Matchs.RemoveRange(matches);
                context.Stages.Remove(stage);
            }

            await context.SaveChangesAsync();

            if (participants.Count == 2)
            {
                var finalStage = new Stage
                {
                    Name = "Финал",
                    TournamentId = tournamentId
                };

                await context.Stages.AddAsync(finalStage);
                await context.SaveChangesAsync();

                var finalMatch = new Match
                {
                    Participant1Id = participants[0].Id,
                    Participant2Id = participants[1].Id,
                    StageId = finalStage.Id,
                    Tatami = 1
                };

                await context.Matchs.AddAsync(finalMatch);
            }
            else if (participants.Count == 3)
            {
                var groupStage = new Stage
                {
                    Name = "Всеки срещу всеки",
                    TournamentId = tournamentId
                };

                await context.Stages.AddAsync(groupStage);
                await context.SaveChangesAsync();

                var matches = new List<Match>
        {
            new Match
            {
                Participant1Id = participants[0].Id,
                Participant2Id = participants[1].Id,
                StageId = groupStage.Id,
                Tatami = 1
            },
            new Match
            {
                Participant1Id = participants[1].Id,
                Participant2Id = participants[2].Id,
                StageId = groupStage.Id,
                Tatami = 2
            },
            new Match
            {
                Participant1Id = participants[0].Id,
                Participant2Id = participants[2].Id,
                StageId = groupStage.Id,
                Tatami = 3
            }
        };

                await context.Matchs.AddRangeAsync(matches);
            }

            await context.SaveChangesAsync();

            return Ok("Етапите и мачовете бяха успешно създадени.");
        }

    }
}

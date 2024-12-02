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
                .Select(t => new TournamentViewModel()
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

            var matches = await context.Matchеs
                .Include(m => m.Stage)
                .Where(m => m.Stage.TournamentId == id)
                .Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .ToListAsync();

            var stagesWithMatches = matches
                .GroupBy(m => m.Stage)
                .Select(group => new StageViewModel
                {
                    Id = group.Key.Id,
                    Name = group.Key.Name,
                    Matches = group.Select(m => new MatchViewModel
                    {
                        Participant1Name = m.Participant1 != null ? m.Participant1.Name : "Не е определен",
                        Participant2Name = m.Participant2 != null ? m.Participant2.Name : "Не е определен",
                        Participant1Score = m.Participant1Score,
                        Participant2Score = m.Participant2Score,
                        Period = m.Period,
                        RemainingTime = m.RemainingTime.ToString(@"hh\:mm\:ss"),
                        Status = m.Status
                    }).ToList()
                }).ToList();


            var sortedStages = new List<StageViewModel>();

            foreach (StageOrder stageOrder in Enum.GetValues(typeof(StageOrder)))
            {
                var stage = stagesWithMatches.FirstOrDefault(s => s.Name == stageOrder.ToString());

                if (stage != null)
                {
                    sortedStages.Add(stage);
                }
            }

            var model = new TournamentViewModel
            {
                Id = tournament.Id,
                Location = tournament.Location,
                Description = tournament.Description,
                Date = tournament.Date,
                Status = tournament.Status,
                EnrolledParticipantsCount = enrolledParticipants.Count,
                EnrolledParticipants = enrolledParticipants,
                Stages = sortedStages
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

            if (tournament?.Status != TournamentStatus.Upcoming)
            {
                return NotFound("Не може да се изпълните това действие, когато турнира е започнал или завършил!");
            }

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
                var matches = await context.Matchеs
                    .Where(m => m.StageId == stage.Id)
                    .ToListAsync();

                context.Matchеs.RemoveRange(matches);
                context.Stages.Remove(stage);
            }

            await context.SaveChangesAsync();

            if (participants.Count == 2)
            {
                var finalStage = new Stage
                {
                    Name = StageOrder.Final.ToString(),
                    StageOrder = StageOrder.Final,
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

                await context.Matchеs.AddAsync(finalMatch);
            }
            else if (participants.Count == 3)
            {
                var groupStage = new Stage
                {
                    Name = "RoundRobin",
                    StageOrder = StageOrder.RoundRobin,
                    TournamentId = tournamentId
                };

                await context.Stages.AddAsync(groupStage);
                await context.SaveChangesAsync();

                var matches = new List<Match>
        {
            new Match { Participant1Id = participants[0].Id, Participant2Id = participants[1].Id, StageId = groupStage.Id, Tatami = 1 },
            new Match { Participant1Id = participants[1].Id, Participant2Id = participants[2].Id, StageId = groupStage.Id, Tatami = 2 },
            new Match { Participant1Id = participants[0].Id, Participant2Id = participants[2].Id, StageId = groupStage.Id, Tatami = 3 }
        };

                await context.Matchеs.AddRangeAsync(matches);
            }
            else if (participants.Count == 4)
            {
                var semiFinalStage = new Stage
                {
                    Name = StageOrder.SemiFinal.ToString(),
                    StageOrder = StageOrder.SemiFinal,
                    TournamentId = tournamentId
                };

                await context.Stages.AddAsync(semiFinalStage);
                await context.SaveChangesAsync();

                var semiFinalMatches = new List<Match>
        {
            new Match { Participant1Id = participants[0].Id, Participant2Id = participants[1].Id, StageId = semiFinalStage.Id, Tatami = 1 },
            new Match { Participant1Id = participants[2].Id, Participant2Id = participants[3].Id, StageId = semiFinalStage.Id, Tatami = 2 }
        };

                await context.Matchеs.AddRangeAsync(semiFinalMatches);
                await context.SaveChangesAsync();

                var finalStage = new Stage
                {
                    Name = StageOrder.Final.ToString(),
                    StageOrder = StageOrder.Final,
                    TournamentId = tournamentId
                };

                await context.Stages.AddAsync(finalStage);
                await context.SaveChangesAsync();

                var finalMatch = new Match
                {
                    StageId = finalStage.Id,
                    Tatami = 1
                };

                await context.Matchеs.AddAsync(finalMatch);
            }
            else if (participants.Count >= 5 && participants.Count <= 7)
            {
                var preliminaryStage = new Stage
                {
                    Name = StageOrder.Preliminary.ToString(),
                    StageOrder = StageOrder.Preliminary,
                    TournamentId = tournamentId
                };

                await context.Stages.AddAsync(preliminaryStage);
                await context.SaveChangesAsync();

                var preliminaryMatches = new List<Match>();
                var preliminaryParticipants = participants.Take((participants.Count - 4) * 2).ToList();

                for (int i = 0; i < preliminaryParticipants.Count; i += 2)
                {
                    preliminaryMatches.Add(new Match
                    {
                        Participant1Id = preliminaryParticipants[i].Id,
                        Participant2Id = preliminaryParticipants[i + 1].Id,
                        StageId = preliminaryStage.Id,
                        Tatami = i / 2 + 1
                    });
                }

                await context.Matchеs.AddRangeAsync(preliminaryMatches);
                await context.SaveChangesAsync();

                var semiFinalStage = new Stage
                {
                    Name = StageOrder.SemiFinal.ToString(),
                    StageOrder = StageOrder.SemiFinal,
                    TournamentId = tournamentId
                };

                await context.Stages.AddAsync(semiFinalStage);
                await context.SaveChangesAsync();

                var semiFinalMatches = new List<Match>
        {
            new Match { StageId = semiFinalStage.Id, Tatami = 1 },
            new Match { StageId = semiFinalStage.Id, Tatami = 2 }
        };

                await context.Matchеs.AddRangeAsync(semiFinalMatches);
                await context.SaveChangesAsync();

                var finalStage = new Stage
                {
                    Name = StageOrder.Final.ToString(),
                    StageOrder = StageOrder.Final,
                    TournamentId = tournamentId
                };

                await context.Stages.AddAsync(finalStage);
                await context.SaveChangesAsync();

                var finalMatch = new Match
                {
                    StageId = finalStage.Id,
                    Tatami = 1
                };

                await context.Matchеs.AddAsync(finalMatch);
            }

            else if (participants.Count >= 8 && participants.Count <= 15)
            {
                if (participants.Count == 8)
                {
                    var quarterFinalStage = new Stage
                    {
                        Name = StageOrder.QuarterFinal.ToString(),
                        StageOrder = StageOrder.QuarterFinal,
                        TournamentId = tournamentId
                    };

                    await context.Stages.AddAsync(quarterFinalStage);
                    await context.SaveChangesAsync();

                    var quarterFinalMatches = new List<Match>
        {
            new Match { StageId = quarterFinalStage.Id, Tatami = 1 },
            new Match { StageId = quarterFinalStage.Id, Tatami = 2 },
            new Match { StageId = quarterFinalStage.Id, Tatami = 3 },
            new Match { StageId = quarterFinalStage.Id, Tatami = 4 }
        };

                    await context.Matchеs.AddRangeAsync(quarterFinalMatches);
                    await context.SaveChangesAsync();

                    var semiFinalStage = new Stage
                    {
                        Name = StageOrder.SemiFinal.ToString(),
                        StageOrder = StageOrder.SemiFinal,
                        TournamentId = tournamentId
                    };

                    await context.Stages.AddAsync(semiFinalStage);
                    await context.SaveChangesAsync();

                    var semiFinalMatches = new List<Match>
        {
            new Match { StageId = semiFinalStage.Id, Tatami = 1 },
            new Match { StageId = semiFinalStage.Id, Tatami = 2 }
        };

                    await context.Matchеs.AddRangeAsync(semiFinalMatches);
                    await context.SaveChangesAsync();

                    var finalStage = new Stage
                    {
                        Name = StageOrder.Final.ToString(),
                        StageOrder = StageOrder.Final,
                        TournamentId = tournamentId
                    };

                    await context.Stages.AddAsync(finalStage);
                    await context.SaveChangesAsync();

                    var finalMatch = new Match
                    {
                        StageId = finalStage.Id,
                        Tatami = 1
                    };

                    await context.Matchеs.AddAsync(finalMatch);
                }
                else if (participants.Count >= 9 && participants.Count <= 15)
                {
                    var preliminaryStage = new Stage
                    {
                        Name = StageOrder.Preliminary.ToString(),
                        StageOrder = StageOrder.Preliminary,
                        TournamentId = tournamentId
                    };

                    await context.Stages.AddAsync(preliminaryStage);
                    await context.SaveChangesAsync();

                    var preliminaryMatches = new List<Match>();
                    var preliminaryParticipants = participants.Take((participants.Count - 8) * 2).ToList();

                    for (int i = 0; i < preliminaryParticipants.Count; i += 2)
                    {
                        preliminaryMatches.Add(new Match
                        {
                            Participant1Id = preliminaryParticipants[i].Id,
                            Participant2Id = preliminaryParticipants[i + 1].Id,
                            StageId = preliminaryStage.Id,
                            Tatami = i / 2 + 1
                        });
                    }

                    await context.Matchеs.AddRangeAsync(preliminaryMatches);
                    await context.SaveChangesAsync();

                    var quarterFinalStage = new Stage
                    {
                        Name = StageOrder.QuarterFinal.ToString(),
                        StageOrder = StageOrder.QuarterFinal,
                        TournamentId = tournamentId
                    };

                    await context.Stages.AddAsync(quarterFinalStage);
                    await context.SaveChangesAsync();

                    var quarterFinalMatches = new List<Match>
        {
            new Match { StageId = quarterFinalStage.Id, Tatami = 1 },
            new Match { StageId = quarterFinalStage.Id, Tatami = 2 },
            new Match { StageId = quarterFinalStage.Id, Tatami = 3 },
            new Match { StageId = quarterFinalStage.Id, Tatami = 4 }
        };

                    await context.Matchеs.AddRangeAsync(quarterFinalMatches);
                    await context.SaveChangesAsync();

                    var semiFinalStage = new Stage
                    {
                        Name = StageOrder.SemiFinal.ToString(),
                        StageOrder = StageOrder.SemiFinal,
                        TournamentId = tournamentId
                    };

                    await context.Stages.AddAsync(semiFinalStage);
                    await context.SaveChangesAsync();

                    var semiFinalMatches = new List<Match>
        {
            new Match { StageId = semiFinalStage.Id, Tatami = 1 },
            new Match { StageId = semiFinalStage.Id, Tatami = 2 }
        };

                    await context.Matchеs.AddRangeAsync(semiFinalMatches);
                    await context.SaveChangesAsync();

                    var finalStage = new Stage
                    {
                        Name = StageOrder.Final.ToString(),
                        StageOrder = StageOrder.Final,
                        TournamentId = tournamentId
                    };

                    await context.Stages.AddAsync(finalStage);
                    await context.SaveChangesAsync();

                    var finalMatch = new Match
                    {
                        StageId = finalStage.Id,
                        Tatami = 1
                    };

                    await context.Matchеs.AddAsync(finalMatch);
                }
            }

            tournament.Status = TournamentStatus.Ongoing;
            await context.SaveChangesAsync();

            return RedirectToAction("TournamentDetails", new { id = tournamentId });
        }

    }
}

using KarateTournamentManager.Enums;
using KarateTournamentManager.Identity;
using KarateTournamentManeger.Data;
using KarateTournamentManeger.Data.Models;
using KarateTournamentManeger.Models;
using KarateTournamentManeger.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



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
        var roles = await roleManager.Roles.Select(r => r.Name).ToListAsync();

        var users = await userManager.Users
            .Include(u => u.Participant)
            .ToListAsync();

        var userRolesViewModel = new List<ManageUsersViewModel>();

        foreach (var user in users)
        {
            var userRoles = await userManager.GetRolesAsync(user);
            var currentRole = userRoles.FirstOrDefault() ?? "No Role";

            userRolesViewModel.Add(new ManageUsersViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                CurrentRole = currentRole,
                SelectedRole = currentRole,
                AvailableRoles = roles
            });
        }

        return View(userRolesViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Route("ManageUsers")]
    public async Task<IActionResult> ChangeParticipantRole(ParticipantViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid data provided.";
            return RedirectToAction(nameof(ManageUsers));
        }

        var user = await userManager.Users.FirstOrDefaultAsync(u => u.ParticipantId == model.Id);
        if (user == null)
        {
            TempData["Error"] = "No user associated with this participant.";
            return RedirectToAction(nameof(ManageUsers));
        }

        var currentRoles = await userManager.GetRolesAsync(user);

        if (currentRoles.Any())
        {
            await userManager.RemoveFromRolesAsync(user, currentRoles);
        }

        if (!string.IsNullOrEmpty(model.Role))
        {
            var roleExists = await roleManager.RoleExistsAsync(model.Role);
            if (roleExists)
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }
            else
            {
                TempData["Error"] = "The selected role does not exist.";
            }
        }

        return RedirectToAction(nameof(ManageUsers));
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
                Stages = new List<Stage>(),
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
            .Include(t => t.Stages)
            .FirstOrDefaultAsync(t => t.Id == id); 


        if (tournament == null)
        {
            return NotFound();
        }

        var model = new TournamentViewModel
        {
            Id = tournament.Id,
            Location = tournament.Location,
            Description = tournament.Description,
            Date = tournament.Date,
            Status = tournament.Status,
            EnrolledParticipants = tournament.EnrolledParticipants != null && tournament.EnrolledParticipants.Any()
                ? tournament.EnrolledParticipants
                    .Select(p => new ParticipantViewModel { Name = p.Name, Id = p.Id })
                    .ToList()
                : new List<ParticipantViewModel>(),

            Stages = tournament.Stages
                .Select(s => new StageViewModel { Name = s.Name, Id = s.Id })
                .ToList()
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

    // Път за ApproveParticipant
    [HttpPost]
    [Route("ApproveParticipant")]
    public IActionResult ApproveParticipant(Guid participantId)
    {
        // Логика за одобрение на участник
        return RedirectToAction("Participants");
    }

}

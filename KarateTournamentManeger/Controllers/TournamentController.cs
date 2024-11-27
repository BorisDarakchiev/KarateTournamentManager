using KarateTournamentManager.Identity;
using KarateTournamentManeger.Data;
using KarateTournamentManeger.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("Tournaments")]
public class TournamentController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly ApplicationDbContext context;

    public TournamentController(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, ApplicationDbContext _context)
    {
        userManager = _userManager;
        roleManager = _roleManager;
        context = _context;
    }
    [HttpGet("Index")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet("Details/{id}")]
    public IActionResult Details(Guid id)
    {
        return View();
    }


    [HttpGet]
    public async Task<IActionResult> TournamentList()
    {
        var tournaments = context.Tournaments.ToList();
        var currentUser = await userManager.GetUserAsync(User);

        var currentUserParticipant = await context.Participants
            .FirstOrDefaultAsync(p => p.Id == currentUser.ParticipantId);

        var model = tournaments.Select(tournament => new TournamentViewModel
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
            
            IsParticipant = currentUserParticipant != null && tournament.EnrolledParticipants
                .Any(p => p.Id == currentUserParticipant.Id)
        }).ToList();

        return View(model);
    }

}

using KarateTournamentManager.Enums;
using KarateTournamentManager.Identity;
using KarateTournamentManeger.Data;
using KarateTournamentManeger.Data.Models;
using KarateTournamentManeger.Models;
using KarateTournamentManeger.Models.ViewModels;
using KarateTournamentManeger.Models.ViewModels.KarateTournamentManeger.Models.ViewModels;
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

    [Route("Admin/Users")]
    public IActionResult Users()
    {
        //var users = userManager.Users
        //    .Select(user => new UserRoleViewModel
        //    {
        //        UserId = user.Id,
        //        UserName = user.UserName,
        //        Email = user.Email,
        //        Roles = userManager.GetRolesAsync(user).Result
        //    })
        //    .ToList();

        //return View(users);
        return View();
    }

    [HttpPost]
    [Route("Admin/AssignRole")]
    public async Task<IActionResult> AssignRole(string userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (!await roleManager.RoleExistsAsync(role))
        {
            return BadRequest("Role does not exist.");
        }

        if (!await userManager.IsInRoleAsync(user, role))
        {
            await userManager.AddToRoleAsync(user, role);
        }

        return RedirectToAction("Users");
    }

    [HttpPost]
    [Route("Admin/RemoveRole")]
    public async Task<IActionResult> RemoveRole(string userId, string role)
    {
        var user = await userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        if (await userManager.IsInRoleAsync(user, role))
        {
            await userManager.RemoveFromRoleAsync(user, role);
        }

        return RedirectToAction("Users");
    }

    [HttpGet]
    [Route("Admin/CreateTournament")]
    public IActionResult CreateTournament()
    {
        var model = new TournamentViewModel
        {
            Date = DateTime.Now,  // Започваме с текущата дата
            Status = TournamentStatus.Upcoming,  // По подразбиране статусът е "Предстоящ"
            EnrolledParticipants = new List<ParticipantViewModel>(),  // Празен списък
            Stages = new List<StageViewModel>(),  // Празен списък
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
            // Създаваме нов турнир с данните от модела
            var tournament = new Tournament
            {
                Id = Guid.NewGuid(),  // Генерираме нов GUID за турнира
                Location = model.Location,
                Description = model.Description,
                Date = model.Date,
                Status = TournamentStatus.Upcoming,  // Статусът е "Предстоящ"
                EnrolledParticipants = new List<Participant>(),  // Празен списък на участници
                Stages = new List<Stage>(),  // Празен списък на етапи
            };

            // Добавяме турнира в контекста на базата данни
            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            // Пренасочваме към списъка с турнири (или към съответната страница)
            return RedirectToAction("Tournaments");
        }

        // Ако има грешки в модела, връщаме формата със съществуващите данни
        return View(model);
    }


    [HttpGet]
    [Route("TournamentDetails/{id}")]
    public async Task<IActionResult> TournamentDetails(Guid id)
    {
        // Зареждаме турнира с необходимите свързани данни
        var tournament = await context.Tournaments
            .Include(t => t.EnrolledParticipants)  // Включваме участниците
            .Include(t => t.Stages)  // Включваме етапите
            .FirstOrDefaultAsync(t => t.Id == id);  // Търсим турнира по ID

        // Ако турнирът не бъде намерен, връщаме 404
        if (tournament == null)
        {
            return NotFound();
        }

        // Преобразуваме в TournamentViewModel, за да предадем данните към View-то
        var model = new TournamentViewModel
        {
            Id = tournament.Id,
            Location = tournament.Location,
            Description = tournament.Description,
            Date = tournament.Date,
            Status = tournament.Status,
            EnrolledParticipants = tournament.EnrolledParticipants.Any()
                ? tournament.EnrolledParticipants
                    .Select(p => new ParticipantViewModel { Name = p.Name, Id = p.Id })
                    .ToList()
                : new List<ParticipantViewModel>(),  // Ако няма участници, предаваме празен списък
            Stages = tournament.Stages
                .Select(s => new StageViewModel { Name = s.Name, Id = s.Id })
                .ToList()  // Преобразуваме етапите
        };

        // Връщаме данните към View-то
        return View(model);
    }



    //[HttpPost]
    //[Route("Admin/RemoveParticipant")]
    //public async Task<IActionResult> RemoveParticipant(Guid tournamentId, Guid participantId)
    //{
    //    var tournament = await context.Tournaments
    //        .Include(t => t.EnrolledParticipants)
    //        .FirstOrDefaultAsync(t => t.Id == tournamentId);

    //    if (tournament == null)
    //    {
    //        return NotFound();
    //    }

    //    var participant = tournament.EnrolledParticipants.FirstOrDefault(p => p.Id == participantId);
    //    if (participant != null)
    //    {
    //        tournament.EnrolledParticipants.Remove(participant);
    //        await context.SaveChangesAsync();
    //    }

    //    return RedirectToAction("TournamentDetails", new { id = tournamentId });
    //}



    // Път за ApproveParticipant
    [HttpPost]
    [Route("ApproveParticipant")]
    public IActionResult ApproveParticipant(Guid participantId)
    {
        // Логика за одобрение на участник
        return RedirectToAction("Participants");
    }

}

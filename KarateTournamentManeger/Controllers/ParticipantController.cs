using KarateTournamentManager.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("Participant")]
[Authorize(Roles = "Participant")]
public class ParticipantController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    // Конструктор за инжектиране на зависимостите
    public ParticipantController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public IActionResult Tournaments()
    {
        // Логика за преглед на налични турнири
        return View();
    }

    [HttpPost]
    public IActionResult Register(Guid tournamentId)
    {
        // Логика за записване в турнир
        return RedirectToAction("Tournaments");
    }
}

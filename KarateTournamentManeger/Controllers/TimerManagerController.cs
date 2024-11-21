using KarateTournamentManager.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


[Route("TimerManager")]
[Authorize(Roles = "TimerManager")]
public class TimerManagerController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    // Конструктор за инжектиране на зависимостите
    public TimerManagerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public IActionResult Matches()
    {
        // Логика за преглед на мачовете
        return View();
    }

    [HttpPost]
    public IActionResult UpdateScore(Guid matchId, int scoreA, int scoreB)
    {
        // Логика за актуализиране на резултатите
        return RedirectToAction("Matches");
    }
}

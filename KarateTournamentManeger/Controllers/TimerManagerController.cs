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

    public TimerManagerController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }
    public IActionResult Matches()
    {
        return View();
    }

    [HttpPost]
    public IActionResult UpdateScore(Guid matchId, int scoreA, int scoreB)
    {
        return RedirectToAction("Matches");
    }
}

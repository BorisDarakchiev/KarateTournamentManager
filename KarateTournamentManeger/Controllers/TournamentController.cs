using KarateTournamentManager.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("Tournaments")]
public class TournamentController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    // Конструктор за инжектиране на зависимостите
    public TournamentController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
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
}

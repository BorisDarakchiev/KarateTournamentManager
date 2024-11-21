using KarateTournamentManager;
using KarateTournamentManager.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;

[Route("Admin")]
[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    // Конструктор за инжектиране на зависимостите
    public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Tournaments()
    {
        return View();
    }

    [HttpPost]
    public IActionResult CreateTournament(Tournament model)
    {
        if (ModelState.IsValid)
        {
            // Логика за добавяне в базата данни
        }
        return RedirectToAction("Tournaments");
    }

    [HttpPost]
    public IActionResult ApproveParticipant(Guid participantId)
    {
        // Логика за одобрение на участник
        return RedirectToAction("Participants");
    }
}

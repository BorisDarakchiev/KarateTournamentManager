using KarateTournamentManager.Identity;
using KarateTournamentManeger.Data.Models;
using KarateTournamentManeger.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("Admin")]
[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;

    public AdminController(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
    {
        userManager = _userManager;
        roleManager = _roleManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [Route("Tournaments")]
    public IActionResult Tournaments()
    {
        return View();
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



    [HttpPost]
    [Route("CreateTournament")]
    public IActionResult CreateTournament(Tournament model)
    {
        if (ModelState.IsValid)
        {
            // Логика за добавяне в базата данни
        }
        // Пренасочваме към действието Tournaments
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

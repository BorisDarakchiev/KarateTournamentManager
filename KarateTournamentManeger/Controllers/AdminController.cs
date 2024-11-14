using KarateTournamentManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("Admin")]
[Authorize(Roles = "Administrator")]
public class AdminController : Controller
{
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

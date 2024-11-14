using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("Participant")]
[Authorize(Roles = "Participant")]
public class ParticipantController : Controller
{
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

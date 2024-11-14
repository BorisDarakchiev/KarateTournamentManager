using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


[Route("TimerManager")]
[Authorize(Roles = "TimerManager")]
public class TimerManagerController : Controller
{
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

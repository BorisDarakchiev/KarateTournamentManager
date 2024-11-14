using Microsoft.AspNetCore.Mvc;

[Route("Tournaments")]
public class TournamentController : Controller
{
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

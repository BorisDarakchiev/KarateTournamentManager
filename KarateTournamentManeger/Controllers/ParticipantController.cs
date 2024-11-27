﻿using KarateTournamentManager.Identity;
using KarateTournamentManeger.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("Participant")]
[Authorize(Roles = "Participant")]
public class ParticipantController : Controller
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly RoleManager<IdentityRole> roleManager;
    private readonly ApplicationDbContext context;

    public ParticipantController(UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager, ApplicationDbContext _context)
    {
        userManager = _userManager;
        roleManager = _roleManager;
        context = _context;
    }
    public IActionResult Tournaments()
    {
        return View();
    }

    [HttpPost]
    [Route("RegisterForTournament")]
    public IActionResult Register(Guid tournamentId)
    {
        return RedirectToAction("Tournaments");
    }

    [HttpPost]
    public async Task<IActionResult> RegisterForTournament(Guid tournamentId)
    {
        // Проверка дали потребителят е логнат
        var currentUser = await userManager.GetUserAsync(User);
        if (currentUser == null)
        {
            return Unauthorized("Трябва да сте логнат, за да се запишете за турнир.");
        }

        // Проверка дали ParticipantId е валиден
        if (string.IsNullOrEmpty(currentUser.ParticipantId?.ToString()) || !Guid.TryParse(currentUser.ParticipantId.ToString(), out var participantId))
        {
            return BadRequest("Не сте свързан като участник.");
        }

        // Вземаме свързания Participant чрез ParticipantId
        var participant = await context.Participants
            .FirstOrDefaultAsync(p => p.Id == participantId);

        if (participant == null)
        {
            return BadRequest("Не сте регистриран като участник.");
        }

        // Проверяваме дали турнирът съществува
        var tournament = await context.Tournaments
            .Include(t => t.EnrolledParticipants)
            .FirstOrDefaultAsync(t => t.Id == tournamentId);

        if (tournament == null)
        {
            return NotFound("Турнирът не е намерен.");
        }

        // Проверяваме дали EnrolledParticipants е null и го инициализираме
        if (tournament.EnrolledParticipants == null)
        {
            tournament.EnrolledParticipants = new List<Participant>();
        }

        // Проверяваме дали участникът вече е записан
        if (tournament.EnrolledParticipants.Any(p => p.Id == participant.Id))
        {
            return BadRequest("Вече сте записан за този турнир.");
        }

        // Добавяме участника към турнира
        tournament.EnrolledParticipants.Add(participant);
        await context.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }


}

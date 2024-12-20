﻿using KarateTournamentManager.Data.Models;
using KarateTournamentManager.Models.ViewModels;

public class TournamentsAndParticipantsViewModel
{
    public List<Tournament> Tournaments { get; set; } = null!;
    public List<ManageUsersViewModel> Participants { get; set; } = null!;
}

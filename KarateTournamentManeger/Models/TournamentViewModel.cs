﻿using KarateTournamentManager.Data.Models;
using KarateTournamentManager.Enums;
using KarateTournamentManager.Identity;
using KarateTournamentManager.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManager.Models
{
    public class TournamentViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Локация")]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Статус")]
        public TournamentStatus Status { get; set; }

        [Display(Name = "Записани участници")]
        public int EnrolledParticipantsCount { get; set; }

        public List<Tatami>? Tatami { get; set; }
        public List<ParticipantViewModel> EnrolledParticipants { get; set; } = new();

        public List<ApplicationUser> TimerManagers { get; set; } = new();

        public List<StageViewModel> Stages { get; set; } = new();

        public List<MatchViewModel> Matches { get; set; } = new();

        public bool IsParticipant { get; set; }
    }

}

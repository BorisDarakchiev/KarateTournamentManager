﻿using KarateTournamentManager.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManager
{
    public class Tournament
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Location { get; set; } = null!;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public TournamentStatus Status { get; set; } = TournamentStatus.Upcoming;

        public IList<Participant> EnrolledParticipants { get; set; } = new List<Participant>();

        //Колекция за етапите. В началото може да е празна.
        
        public IList<Stage> Stages { get; set; } = new List<Stage>();


    }
}

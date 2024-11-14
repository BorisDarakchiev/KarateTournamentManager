using KarateTournamentManager.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManager
{
    public abstract class Tournament
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

        public ICollection<Participant> EnrolledParticipants { get; set; } = new List<Participant>();

        public ICollection<Stage> Stages { get; set; } = new List<Stage>();


    }
}

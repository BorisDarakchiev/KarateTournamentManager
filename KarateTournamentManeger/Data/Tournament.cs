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
        public DateTime Date { get; set; }

        [Required]
        public TournamentStatus Status { get; set; } = TournamentStatus.Upcoming;

        [Required]
        [Range(1, 5)]
        public int NumberOfTatamis { get; set; }

        public ICollection<Participant> ЕnrolledParticipants { get; set; } = new List<Participant>();

    }
}

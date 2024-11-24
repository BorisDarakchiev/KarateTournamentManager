using KarateTournamentManager.Enums;
using KarateTournamentManager.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManeger.Data.Models
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

        public ICollection<ApplicationUser> Participants { get; set; } = new List<ApplicationUser>();

        public ICollection<Stage> Stages { get; set; } = new List<Stage>();
        public List<Participant> EnrolledParticipants { get; internal set; }
    }
}

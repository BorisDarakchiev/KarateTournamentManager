using KarateTournamentManager.Controllers;
using KarateTournamentManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateTournamentManager.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public Guid? ParticipantId { get; set; }

        [ForeignKey(nameof(ParticipantId))]
        public Participant? Participant { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public bool IsDeleted { get; set; }
    }
}

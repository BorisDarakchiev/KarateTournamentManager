using KarateTournamentManager.Controllers;
using KarateTournamentManager.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateTournamentManager.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey(nameof(ParticipantId))]
        public Guid? ParticipantId { get; set; } 
        public Participant? Participant { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
    }
}

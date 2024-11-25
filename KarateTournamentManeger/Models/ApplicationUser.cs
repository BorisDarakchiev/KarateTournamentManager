using KarateTournamentManeger.Data.Models;
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

    }
}

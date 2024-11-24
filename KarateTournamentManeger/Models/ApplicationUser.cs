using KarateTournamentManeger.Data.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManager.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}

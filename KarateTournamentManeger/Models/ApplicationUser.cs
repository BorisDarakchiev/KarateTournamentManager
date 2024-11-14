using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManager.Identity
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        public bool IsApproved { get; set; } = false;
    }
}

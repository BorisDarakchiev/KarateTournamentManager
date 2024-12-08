using KarateTournamentManager.Data.Models;
using KarateTournamentManager.Identity;
using System.ComponentModel.DataAnnotations;
using static KarateTournamentManager.Constants.ModelConstants;


namespace KarateTournamentManager.Controllers
{
    public class Participant
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [StringLength(MaxLenght)]
        public string Name { get; set; } = null!;

        public ICollection<Match> Matches { get; set; } = new List<Match>();

        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();

        public ApplicationUser? ApplicationUser { get; set; }
    }
}
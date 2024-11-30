using KarateTournamentManager.Data.Models;
using System.ComponentModel.DataAnnotations;


namespace KarateTournamentManager.Controllers
{
    public class Participant
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<Match> Matches { get; set; } = new List<Match>();

        public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
    }
}
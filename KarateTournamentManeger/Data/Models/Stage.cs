using KarateTournamentManager.Controllers;
using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManager.Data.Models
{
    public class Stage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = null!;

        public IList<Match> Matches { get; set; } = new List<Match>();
    }
}

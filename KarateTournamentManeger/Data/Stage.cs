using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManager
{
    public class Stage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        public int StageNumber { get; set; }

        public IList<Match> Matches { get; set; } = new List<Match>();
    }
}

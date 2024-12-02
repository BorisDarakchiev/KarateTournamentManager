using KarateTournamentManager.Controllers;
using KarateTournamentManager.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KarateTournamentManager.Data.Models
{
    public class Stage
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public Guid TournamentId { get; set; }

        [ForeignKey(nameof(TournamentId))]
        public Tournament Tournament { get; set; } = null!;
        public StageOrder StageOrder { get; set; }
    }


}

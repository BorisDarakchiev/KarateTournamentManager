using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManager.Models.ViewModels
{
    public class StageViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Име на етап")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Мачове")]
        public List<MatchViewModel> Matches { get; set; } = new();
    }
}
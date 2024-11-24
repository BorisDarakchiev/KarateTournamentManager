using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManeger.Models
{
    public class ParticipantViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Роля")]
        public string Role { get; set; } = string.Empty;
    }

}
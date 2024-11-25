using KarateTournamentManeger.Data.Models;
using System.ComponentModel.DataAnnotations;

public class Participant
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public DateTime DateOfBirth { get; set; }

    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;
            if (DateOfBirth.Date > today.AddYears(-age)) age--;
            return age;
        }
    }

    public ICollection<Match> Matches { get; set; } = new List<Match>();

    public ICollection<Tournament> Tournaments { get; set; } = new List<Tournament>();
}

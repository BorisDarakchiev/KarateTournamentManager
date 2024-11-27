using KarateTournamentManager.Enums;
using KarateTournamentManeger.Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace KarateTournamentManeger.Models
{
    public class TournamentViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Локация")]
        public string Location { get; set; } = string.Empty;

        [Display(Name = "Описание")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Дата")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Статус")]
        public TournamentStatus Status { get; set; }

        [Display(Name = "Записани участници")]
        public int EnrolledParticipantsCount { get; set; }

        public List<ParticipantViewModel> EnrolledParticipants { get; set; } = new();

        [Display(Name = "Етапи")]
        public List<StageViewModel> Stages { get; set; } = new();

        public bool IsParticipant { get; set; }

    }
}

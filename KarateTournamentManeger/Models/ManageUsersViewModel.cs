namespace KarateTournamentManeger.Models.ViewModels
{
    public class ManageUsersViewModel
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? Email { get; set; }
        public string? CurrentRole { get; set; }
        public List<string> AvailableRoles { get; set; } = new List<string>();
        public string? SelectedRole { get; set; }
    }
}

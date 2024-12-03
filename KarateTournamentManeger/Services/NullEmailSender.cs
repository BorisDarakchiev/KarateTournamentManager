using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace KarateTournamentManager.Services
{
    public class NullEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}

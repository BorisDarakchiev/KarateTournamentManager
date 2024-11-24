using Microsoft.AspNetCore.Identity.UI.Services;
using System.Threading.Tasks;

namespace KarateTournamentManeger.Services
{
    public class NullEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Празна имплементация - не правим нищо
            return Task.CompletedTask;
        }
    }
}

using KarateTournamentManager.Data;
using KarateTournamentManager.Data.Models;
using KarateTournamentManager.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace KarateTournamentManager.Identity
{
    public static class ApplicationDbInitializer
    {
        public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roleNames = { "Administrator", "Participant", "TimerManager" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

        public static async Task SeedUsersAndTournamentAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var context = services.GetRequiredService<ApplicationDbContext>();

            if (!await context.Participants.AnyAsync())
            {
                var adminUser = await userManager.FindByEmailAsync("admin@abv.bg");
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = "Админ Админов",
                        Email = "admin@abv.bg",
                        FirstName = "Админ",
                        LastName = "Админов",
                        IsDeleted = false
                    };
                    await userManager.CreateAsync(adminUser, "asdasd");
                    await userManager.AddToRoleAsync(adminUser, "Administrator");
                }

                var timerManagerUser = await userManager.FindByEmailAsync("asen@abv.bg");
                if (timerManagerUser == null)
                {
                    timerManagerUser = new ApplicationUser
                    {
                        UserName = "Асен Асенов",
                        Email = "asen@abv.bg",
                        FirstName = "Асен",
                        LastName = "Асенов",
                        IsDeleted = false
                    };
                    await userManager.CreateAsync(timerManagerUser, "asdasd");
                    await userManager.AddToRoleAsync(timerManagerUser, "TimerManager");
                }

                for (int i = 1; i <= 10; i++)
                {
                    var participantEmail = $"user{i}@abv.com";
                    var participantUser = await userManager.FindByEmailAsync(participantEmail);
                    if (participantUser == null)
                    {
                        participantUser = new ApplicationUser
                        {
                            UserName = $"Участник {i}",
                            Email = participantEmail,
                            FirstName = $"Име{i}",
                            LastName = $"Фамилия{i}",
                            IsDeleted = false
                        };
                        await userManager.CreateAsync(participantUser, "asdasd");
                        await userManager.AddToRoleAsync(participantUser, "Participant");
                        await context.SaveChangesAsync();
                    }
                }
            }

             if (!await context.Tournaments.AnyAsync())
            {
                var tournaments = new List<Tournament>
                {
                    new Tournament
                    {
                        Id = Guid.NewGuid(),
                        Location = "Велико Търново",
                        Description = "Турнир за всички възрасти",
                        Date = DateTime.Now.AddMonths(1),
                        Status = TournamentStatus.Upcoming,
                    },
                    new Tournament
                    {
                        Id = Guid.NewGuid(),
                        Location = "Пловдив",
                        Description = "Турнир под тепетата",
                        Date = DateTime.Now.AddMonths(2),
                        Status = TournamentStatus.Upcoming
                    },
                    new Tournament
                    {
                        Id = Guid.NewGuid(),
                        Location = "Шумен",
                        Description = "Турнир по Ката",
                        Date = DateTime.Now.AddMonths(3),
                        Status = TournamentStatus.Upcoming
                    },
                    new Tournament
                    {
                        Id = Guid.NewGuid(),
                        Location = "Варна",
                        Description = "Турнир по Ката",
                        Date = DateTime.Now.AddMonths(5),
                        Status = TournamentStatus.Upcoming
                    },
                    new Tournament
                    {
                        Id = Guid.NewGuid(),
                        Location = "Камчия",
                        Description = "Летен лагер Камчия",
                        Date = DateTime.Now.AddMonths(7),
                        Status = TournamentStatus.Upcoming
                    },
                };

                await context.Tournaments.AddRangeAsync(tournaments);
                await context.SaveChangesAsync();
            }
        }
    }
}

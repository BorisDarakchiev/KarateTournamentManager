using KarateTournamentManager.Controllers;
using KarateTournamentManager.Data;
using KarateTournamentManager.Data.Models;
using KarateTournamentManager.Enums;
using KarateTournamentManager.Identity;
using KarateTournamentManager.Models;
using KarateTournamentManager.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;



namespace KarateTournamentManager.Services
{
    public class TournamentService : ITournamentService
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<ApplicationUser> userManager;

        public TournamentService(ApplicationDbContext _context, UserManager<ApplicationUser> _userManager)
        {
            context = _context;
            userManager = _userManager;
        }

        public async Task<List<TournamentViewModel>> GetTournamentsViewModelsAsync(string userId)
        {
            var tournaments = await context.Tournaments
                .Include(t => t.EnrolledParticipants)
                .Where(t => t.Date >= DateTime.Now)
                .OrderBy(t => t.Date)
                .ToListAsync();

            var currentUser = userId != null
                ? await userManager.Users.Include(u => u.Participant).FirstOrDefaultAsync(u => u.Id == userId)
                : null;
            var currentUserParticipantId = currentUser?.Participant?.Id;

            return tournaments.Select(t => new TournamentViewModel
            {
                Id = t.Id,
                Location = t.Location,
                Description = t.Description,
                Date = t.Date,
                Status = t.Status,
                EnrolledParticipantsCount = t.EnrolledParticipants?.Count ?? 0,
                EnrolledParticipants = t.EnrolledParticipants != null
                    ? t.EnrolledParticipants
                        .Select(p => new ParticipantViewModel { Name = p.Name, Id = p.Id })
                        .ToList()
                    : new List<ParticipantViewModel>(),
                IsParticipant = currentUserParticipantId.HasValue &&
                                t.EnrolledParticipants.Any(p => p.Id == currentUserParticipantId.Value)
            }).ToList();
        }

        public async Task<List<TournamentViewModel>> GetTournamentsAsync()
        {
            return await context.Tournaments
                .OrderByDescending(t => t.Date)
                .Select(t => new TournamentViewModel
                {
                    Id = t.Id,
                    Location = t.Location,
                    Description = t.Description,
                    Date = t.Date,
                    Status = t.Status,
                    EnrolledParticipantsCount = t.EnrolledParticipants.Count
                })
                .AsNoTracking()
                .ToListAsync();
        }
        public Task<TournamentViewModel> CreateTournamentViewModelAsync()
        {
            var model = new TournamentViewModel
            {
                Date = DateTime.Now,
                Status = TournamentStatus.Upcoming,
                EnrolledParticipants = new List<ParticipantViewModel>(),
                Stages = new List<StageViewModel>(),
            };

            return Task.FromResult(model);
        }

        public async Task<bool> CreateTournamentAsync(TournamentViewModel model)
        {
            var tournament = new Tournament
            {
                Id = Guid.NewGuid(),
                Location = model.Location,
                Description = model.Description,
                Date = model.Date,
                Status = TournamentStatus.Upcoming,
                EnrolledParticipants = new List<Participant>(),
            };

            context.Tournaments.Add(tournament);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<TournamentViewModel> GetTournamentDetailsAsync(Guid id)
        {
            var tournament = await context.Tournaments
                .Include(t => t.EnrolledParticipants)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tournament == null)
            {
                return null!;
            }

            var enrolledParticipants = tournament.EnrolledParticipants?.Select(p =>
            {
                var user = context.Users.FirstOrDefault(u => u.ParticipantId == p.Id);
                return new ParticipantViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = user?.Email ?? "N/A"
                };
            }).ToList();

            var matches = await context.Matchеs
                .Include(m => m.Stage)
                .Where(m => m.Stage.TournamentId == id)
                .Include(m => m.Participant1)
                .Include(m => m.Participant2)
                .ToListAsync();

            var stagesWithMatches = matches
                .GroupBy(m => m.Stage)
                .Select(group => new StageViewModel
                {
                    Id = group.Key.Id,
                    Name = group.Key.Name,
                    Matches = group.Select(m => new MatchViewModel
                    {
                        Participant1Name = m.Participant1 != null ? m.Participant1.Name : "Не е определен",
                        Participant2Name = m.Participant2 != null ? m.Participant2.Name : "Не е определен",
                        Participant1Score = m.Participant1Score,
                        Participant2Score = m.Participant2Score,
                        Period = m.Period,
                        Tatami = m.Tatami,
                        RemainingTime = m.RemainingTime.ToString(@"hh\:mm\:ss"),
                        Status = m.Status
                    }).ToList()
                }).ToList();

            var sortedStages = new List<StageViewModel>();

            foreach (StageOrder stageOrder in Enum.GetValues(typeof(StageOrder)))
            {
                var stage = stagesWithMatches.FirstOrDefault(s => s.Name == stageOrder.ToString());

                if (stage != null)
                {
                    sortedStages.Add(stage);
                }
            }

            return new TournamentViewModel
            {
                Id = tournament.Id,
                Location = tournament.Location,
                Description = tournament.Description,
                Date = tournament.Date,
                Status = tournament.Status,
                EnrolledParticipantsCount = enrolledParticipants.Count,
                EnrolledParticipants = enrolledParticipants,
                Stages = sortedStages
            };
        }

        public async Task<bool> DeleteTournamentAsync(Guid id)
        {
            var tournament = await context.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return false;
            }

            context.Tournaments.Remove(tournament);
            await context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> RemoveParticipantAsync(Guid tournamentId, Guid participantId)
        {
            var tournament = await context.Tournaments
                .Include(t => t.EnrolledParticipants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null)
            {
                return false;
            }

            var participant = tournament.EnrolledParticipants?.FirstOrDefault(p => p.Id == participantId);
            if (participant == null)
            {
                return false;
            }

            tournament.EnrolledParticipants?.Remove(participant);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<string?> FinalizeEnrollmentAsync(Guid tournamentId)
        {
            var tournament = await context.Tournaments
                .Include(t => t.EnrolledParticipants)
                .FirstOrDefaultAsync(t => t.Id == tournamentId);

            if (tournament == null)
            {
                return "Турнирът не е намерен.";

            }

            if (tournament.Status != TournamentStatus.Upcoming)
            {
                return "Не може да се изпълните това действие, когато турнира е започнал или завършил!";
            }

            var participants = tournament.EnrolledParticipants.ToList();

            if (participants.Count < 2)
            {
                return "Необходими са поне двама участници за създаване на етапи.";
            }

            await RemoveExistingStagesAsync(tournamentId);


            if (participants.Count == 2)
            {
                await CreateFinalStageAsync(tournamentId, participants);
            }
            else if (participants.Count == 3)
            {
                await CreateRoundRobinStageAsync(tournamentId, participants);
            }
            else if (participants.Count == 4)
            {
                await CreateSemiFinalAndFinalStagesAsync(tournamentId, participants);
            }
            else if (participants.Count >= 5 && participants.Count <= 7)
            {
                await CreatePreliminarySemiFinalAndFinalStagesAsync(tournamentId, participants);
            }
            else if (participants.Count == 8)
            {
                await CreateStagesWithQuarterFinalsAsync(tournamentId, participants);
            }
            else if (participants.Count >= 9 && participants.Count <= 15)
            {
                await CreatePreliminaryStagesWithQuarterFinalsAsync(tournamentId, participants);
            }

            tournament.Status = TournamentStatus.Ongoing;
            await context.SaveChangesAsync();

            return string.Empty;
        }

        private async Task RemoveExistingStagesAsync(Guid tournamentId)
        {
            var existingStages = await context.Stages
                .Where(s => s.TournamentId == tournamentId)
                .ToListAsync();

            foreach (var stage in existingStages)
            {
                var matches = await context.Matchеs
                    .Where(m => m.StageId == stage.Id)
                    .ToListAsync();

                context.Matchеs.RemoveRange(matches);
                context.Stages.Remove(stage);
            }

            await context.SaveChangesAsync();
        }

        private async Task CreateFinalStageAsync(Guid tournamentId, List<Participant> participants)
        {
            var finalStage = new Stage
            {
                Name = StageOrder.Final.ToString(),
                StageOrder = StageOrder.Final,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(finalStage);
            await context.SaveChangesAsync();

            var finalMatch = new Match
            {
                Participant1Id = participants[0].Id,
                Participant2Id = participants[1].Id,
                StageId = finalStage.Id,
            };

            await context.Matchеs.AddAsync(finalMatch);
            await context.SaveChangesAsync();
        }

        private async Task CreateRoundRobinStageAsync(Guid tournamentId, List<Participant> participants)
        {
            var roundRobinStage = new Stage
            {
                Name = StageOrder.Preliminary.ToString(),
                StageOrder = StageOrder.Preliminary,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(roundRobinStage);
            await context.SaveChangesAsync();

            var matches = new List<Match>();
            for (int i = 0; i < participants.Count; i++)
            {
                for (int j = i + 1; j < participants.Count; j++)
                {
                    matches.Add(new Match
                    {
                        Participant1Id = participants[i].Id,
                        Participant2Id = participants[j].Id,
                        StageId = roundRobinStage.Id,
                    });
                }
            }

            await context.Matchеs.AddRangeAsync(matches);
            await context.SaveChangesAsync();
        }

        //Логика при 4-ма участника
        private async Task CreateSemiFinalAndFinalStagesAsync(Guid tournamentId, List<Participant> participants)
        {
            if (participants.Count < 4)
            {
                throw new InvalidOperationException("Некоретни данни!");
            }

            var finalStage = new Stage
            {
                Name = StageOrder.Final.ToString(),
                StageOrder = StageOrder.Final,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(finalStage);
            await context.SaveChangesAsync();

            var finalMatch = new Match
            {
                StageId = finalStage.Id,
            };

            await context.Matchеs.AddAsync(finalMatch);
            await context.SaveChangesAsync();

            var semiFinalStage = new Stage
            {
                Name = StageOrder.SemiFinal.ToString(),
                StageOrder = StageOrder.SemiFinal,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(semiFinalStage);
            await context.SaveChangesAsync();

            var semiFinalMatches = new List<Match>

    {
        new Match
{
            Participant1Id = participants[0].Id,
            Participant2Id = participants[1].Id,
            StageId = semiFinalStage.Id,
            WinnerNextMatchId = finalMatch.Id,
        },
        new Match
        {
            Participant1Id = participants[2].Id,
            Participant2Id = participants[3].Id,
            StageId = semiFinalStage.Id,
            WinnerNextMatchId = finalMatch.Id,
        }
    };

            await context.Matchеs.AddRangeAsync(semiFinalMatches);
            await context.SaveChangesAsync();
        }
        //Логика от 5 до 7 участника
        public async Task CreatePreliminarySemiFinalAndFinalStagesAsync(Guid tournamentId, List<Participant> participants)
        {
            if (participants.Count < 5 || participants.Count > 7)
            {
                throw new InvalidOperationException("Некоретни данни!");
            }

            var finalStage = new Stage
            {
                Name = StageOrder.Final.ToString(),
                StageOrder = StageOrder.Final,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(finalStage);
            await context.SaveChangesAsync();

            var finalMatch = new Match
            {
                StageId = finalStage.Id,
            };

            await context.Matchеs.AddAsync(finalMatch);
            await context.SaveChangesAsync();

            var semiFinalStage = new Stage
            {
                Name = StageOrder.SemiFinal.ToString(),
                StageOrder = StageOrder.SemiFinal,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(semiFinalStage);
            await context.SaveChangesAsync();

            var semiFinalMatches = new List<Match>
            {
                new Match { StageId = semiFinalStage.Id, WinnerNextMatchId = finalMatch.Id},
                new Match { StageId = semiFinalStage.Id, WinnerNextMatchId = finalMatch.Id}
            };

            await context.Matchеs.AddRangeAsync(semiFinalMatches);
            await context.SaveChangesAsync();

            var preliminaryStage = new Stage
            {
                Name = StageOrder.Preliminary.ToString(),
                StageOrder = StageOrder.Preliminary,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(preliminaryStage);
            await context.SaveChangesAsync();

            var preliminaryMatches = new List<Match>();
            var preliminaryParticipants = participants.Take((participants.Count - 4) * 2).ToList();

            for (int i = 0; i < preliminaryParticipants.Count; i += 2)
            {
                preliminaryMatches.Add(new Match
                {
                    Participant1Id = preliminaryParticipants[i].Id,
                    Participant2Id = preliminaryParticipants[i + 1].Id,
                    StageId = preliminaryStage.Id,
                });
            }

            await context.Matchеs.AddRangeAsync(preliminaryMatches);
            await context.SaveChangesAsync();
        }

        public async Task CreateStagesWithQuarterFinalsAsync(Guid tournamentId, List<Participant> participants)
        {
            if (participants.Count != 8)
            {
                throw new InvalidOperationException("Некоретни данни!");
            }
            var quarterFinalStage = new Stage
            {
                Name = StageOrder.QuarterFinal.ToString(),
                StageOrder = StageOrder.QuarterFinal,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(quarterFinalStage);
            await context.SaveChangesAsync();

            var quarterFinalMatches = new List<Match>
            {
                new Match { StageId = quarterFinalStage.Id},
                new Match { StageId = quarterFinalStage.Id},
                new Match { StageId = quarterFinalStage.Id},
                new Match { StageId = quarterFinalStage.Id}
            };

            await context.Matchеs.AddRangeAsync(quarterFinalMatches);
            await context.SaveChangesAsync();

            var semiFinalStage = new Stage
            {
                Name = StageOrder.SemiFinal.ToString(),
                StageOrder = StageOrder.SemiFinal,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(semiFinalStage);
            await context.SaveChangesAsync();

            var semiFinalMatches = new List<Match>
            {
                new Match { StageId = semiFinalStage.Id},
                new Match { StageId = semiFinalStage.Id}
            };

            await context.Matchеs.AddRangeAsync(semiFinalMatches);
            await context.SaveChangesAsync();

            var finalStage = new Stage
            {
                Name = StageOrder.Final.ToString(),
                StageOrder = StageOrder.Final,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(finalStage);
            await context.SaveChangesAsync();

            var finalMatch = new Match
            {
                StageId = finalStage.Id,
            };

            await context.Matchеs.AddAsync(finalMatch);
            await context.SaveChangesAsync();
        }

        public async Task CreatePreliminaryStagesWithQuarterFinalsAsync(Guid tournamentId, List<Participant> participants)
        {
            if (participants.Count < 9 && participants.Count > 15)
            {
                throw new InvalidOperationException("Некоретни данни!");
            }

            var preliminaryStage = new Stage
            {
                Name = StageOrder.Preliminary.ToString(),
                StageOrder = StageOrder.Preliminary,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(preliminaryStage);
            await context.SaveChangesAsync();

            var preliminaryMatches = new List<Match>();
            var preliminaryParticipants = participants.Take((participants.Count - 8) * 2).ToList();

            for (int i = 0; i < preliminaryParticipants.Count; i += 2)
            {
                preliminaryMatches.Add(new Match
                {
                    Participant1Id = preliminaryParticipants[i].Id,
                    Participant2Id = preliminaryParticipants[i + 1].Id,
                    StageId = preliminaryStage.Id,
                });
            }

            await context.Matchеs.AddRangeAsync(preliminaryMatches);
            await context.SaveChangesAsync();

            var quarterFinalStage = new Stage
            {
                Name = StageOrder.QuarterFinal.ToString(),
                StageOrder = StageOrder.QuarterFinal,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(quarterFinalStage);
            await context.SaveChangesAsync();

            var quarterFinalMatches = new List<Match>
            {
                new Match { StageId = quarterFinalStage.Id},
                new Match { StageId = quarterFinalStage.Id},
                new Match { StageId = quarterFinalStage.Id},
                new Match { StageId = quarterFinalStage.Id}
            };

            await context.Matchеs.AddRangeAsync(quarterFinalMatches);
            await context.SaveChangesAsync();

            var semiFinalStage = new Stage
            {
                Name = StageOrder.SemiFinal.ToString(),
                StageOrder = StageOrder.SemiFinal,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(semiFinalStage);
            await context.SaveChangesAsync();

            var semiFinalMatches = new List<Match>
            {
                new Match { StageId = semiFinalStage.Id},
                new Match { StageId = semiFinalStage.Id}
            };

            await context.Matchеs.AddRangeAsync(semiFinalMatches);
            await context.SaveChangesAsync();

            var finalStage = new Stage
            {
                Name = StageOrder.Final.ToString(),
                StageOrder = StageOrder.Final,
                TournamentId = tournamentId
            };

            await context.Stages.AddAsync(finalStage);
            await context.SaveChangesAsync();

            var finalMatch = new Match
            {
                StageId = finalStage.Id,
            };

            await context.Matchеs.AddAsync(finalMatch);
            await context.SaveChangesAsync();
        }
    }
}





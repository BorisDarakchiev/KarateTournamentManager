using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KarateTournamentManager.Identity;
using KarateTournamentManager.Data.Models;
using KarateTournamentManager.Controllers;


namespace KarateTournamentManager.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<ApplicationUser>()
                .HasQueryFilter(a => !a.IsDeleted);

            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Participant)
                .WithOne(p => p.ApplicationUser)
                .HasForeignKey<ApplicationUser>(a => a.ParticipantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Participant1)
                .WithMany()
                .HasForeignKey(m => m.Participant1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Participant2)
                .WithMany()
                .HasForeignKey(m => m.Participant2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Winner)
                .WithMany()
                .HasForeignKey(m => m.WinnerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tatami>()
                .HasOne(t => t.TimerManager)
                .WithMany()
                .HasForeignKey(t => t.TimerManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Tatami>()
                .HasOne(t => t.Tournament)
                .WithMany()
                .HasForeignKey(t => t.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Stage>()
                .HasOne(s => s.Tournament)
                .WithMany()
                .HasForeignKey(s => s.TournamentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Stage)
                .WithMany()
                .HasForeignKey(m => m.StageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Tournament)
                .WithMany()
                .HasForeignKey(m => m.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public DbSet<Tatami> Tatamis { get; set; }
    }
}

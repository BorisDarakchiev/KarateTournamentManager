﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

            modelBuilder.Entity<Match>()
                .HasOne(m => m.TimerManager)
                .WithMany()
                .HasForeignKey(m => m.TimerManagerId)
                .HasPrincipalKey(u => u.Id)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ApplicationUser>()
                .HasOne(a => a.Participant)
                .WithOne()
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
        }

        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Stage> Stages { get; set; }
        public DbSet<Match> Matchеs { get; set; }
        public DbSet<Participant> Participants { get; set; }
        public IEnumerable<object> Matches { get; internal set; }
    }
}
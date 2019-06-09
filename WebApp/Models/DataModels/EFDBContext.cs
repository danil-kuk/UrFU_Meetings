using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models.DataModels.Entities;

namespace WebApp.Models.DataModels
{
    public class EFDBContext: DbContext
    {
        public EFDBContext(DbContextOptions<EFDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<EmailValid> EmailValid { get; set; }
        public DbSet<PasswordReset> PasswordReset { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventParticipant>()
                .HasKey(ep => new { ep.EventId, ep.UserId });

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.Event)
                .WithMany(e => e.Participants)
                .HasForeignKey(ep => ep.EventId);

            modelBuilder.Entity<EventParticipant>()
                .HasOne(ep => ep.User)
                .WithMany(u => u.SubscribedEvents)
                .HasForeignKey(ep => ep.UserId);

            modelBuilder
                .Entity<Event>()
                .Property(e => e.EventTheme)
                .HasConversion(
                v => v.ToString(),
                v => (EventTheme)Enum.Parse(typeof(EventTheme), v));
        }
    }
}

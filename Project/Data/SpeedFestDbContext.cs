using Microsoft.EntityFrameworkCore;
using Project.Models;

namespace Project.Data;

public class SpeedFestDbContext : DbContext
{
    public SpeedFestDbContext(DbContextOptions<SpeedFestDbContext> options) : base(options)
    {
    }

    public DbSet<RacingTeam> RacingTeams { get; set; }
    public DbSet<RacingDriver> RacingDrivers { get; set; }
    public DbSet<LapTime> LapTimes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure the entities to match the existing database schema
        modelBuilder.Entity<RacingTeam>(entity =>
        {
            entity.ToTable("RacingTeam");
            entity.HasKey(e => e.RacingTeamId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.TeamPrincipal).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<RacingDriver>(entity =>
        {
            entity.ToTable("RacingDriver");
            entity.HasKey(e => e.RacingDriverId);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            
            entity.HasOne(d => d.Team)
                .WithMany(t => t.Drivers)
                .HasForeignKey(d => d.RacingTeamId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<LapTime>(entity =>
        {
            entity.ToTable("LapTime");
            entity.HasKey(e => e.LapTimeId);
            entity.Property(e => e.StartDateTime).IsRequired();
            
            entity.HasOne(l => l.Driver)
                .WithMany(d => d.LapTimes)
                .HasForeignKey(l => l.RacingDriverId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
using DripChip_API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DripChip_API.DAL;

public class ApplicationDbContext :DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Animal> Animals { get; set; }
    public DbSet<Types> Types { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<LocationInfo> LocationInfo { get; set; }

    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        Database.EnsureCreated();
    }

    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>()
            .HasMany(x => x.animalTypes)
            .WithMany(y => y.animals);
        
    }
}
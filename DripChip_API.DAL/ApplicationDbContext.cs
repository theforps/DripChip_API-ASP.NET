using DripChip_API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DripChip_API.DAL;

public sealed class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Animal> Animals { get; set; } = null!;
    public DbSet<Types> Types { get; set; } = null!;
    public DbSet<Location> Locations { get; set; } = null!;
    public DbSet<LocationInfo> LocationInfo { get; set; } = null!;

    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Animal>()
            .HasMany(x => x.animalTypes)
            .WithMany(y => y.animals);
        
        modelBuilder.Entity<User>().HasData(
            new User[] 
            {
                new User {  
                    id = 1,
                    firstName = "adminFirstName",
                    lastName = "adminLastName",
                    email = "admin@simbirsoft.com",
                    password = "qwerty123",
                    role = "ADMIN"
                },
                new User {  
                    id = 2,
                    firstName = "chipperFirstName",
                    lastName = "chipperLastName",
                    email = "chipper@simbirsoft.com", 
                    password = "qwerty123",
                    role = "CHIPPER"
                },
                new User {  
                    id = 3,
                    firstName = "userFirstName",
                    lastName = "userLastName",
                    email = "user@simbirsoft.com",
                    password = "qwerty123",
                    role = "USER" 
                }
            });
        
    }
}
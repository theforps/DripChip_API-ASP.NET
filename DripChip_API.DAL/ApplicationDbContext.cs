using DripChip_API.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace DripChip_API.DAL;

public class ApplicationDbContext :DbContext
{
    public DbSet<User> Users { get; set; }

    public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=..\\DripChip_API.DAL\\Data\\DripChip.sqlite");
    }
}
using Microsoft.EntityFrameworkCore;
using CinemaPortal.Identity.Models;

namespace AuthenticationServer.DbContexts;

public class AuthDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=.;Database=mydb;Trusted_Connection=True;");
    }

    public DbSet<UserProfile> UserProfile { get; set; }
}

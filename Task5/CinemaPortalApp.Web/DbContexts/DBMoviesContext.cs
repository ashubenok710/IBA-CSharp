using CinemaPortal.Web.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CinemaPortal.Web.DbContexts;

public partial class DBMoviesContext : DbContext
{
    public DBMoviesContext()
    {
    }

    public DBMoviesContext(DbContextOptions<DBMoviesContext> options)
        : base(options)
    {
    }
    public DbSet<UserProfile> UserProfile { get; set; }
    public virtual DbSet<Movie> Movies { get; set; } = null!;
    public virtual DbSet<Person> People { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
            optionsBuilder.UseSqlServer("Server=.;Database=DBMovies;Trusted_Connection=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Movie>(entity =>
        {
            //entity.HasNoKey();

            entity.ToTable("Movie");

            entity.Property(e => e.DirectorId).HasColumnName("director_id");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .HasColumnName("name")
                .IsFixedLength().HasConversion(
                new ValueConverter<string, string>(v => v.TrimEnd(), v => v.TrimEnd())); ;

            entity.Property(e => e.ProductionDate)
                .HasMaxLength(10)
                .HasColumnName("production_date")
                .IsFixedLength();

            entity.Property(e => e.Raiting)
                .HasMaxLength(10)
                .HasColumnName("raiting")
                .IsFixedLength();
        });

        modelBuilder.Entity<Person>(entity =>
        {
            //entity.HasNoKey();

            entity.ToTable("Person");

            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .HasColumnName("first_name")
                .IsFixedLength();

            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .HasColumnName("last_name")
                .IsFixedLength();

            entity.Property(e => e.PersonId)
                .ValueGeneratedOnAdd()
                .HasColumnName("person_id");

            entity.Property(e => e.Role)
                .HasMaxLength(20)
                .HasColumnName("role")
                .IsFixedLength();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
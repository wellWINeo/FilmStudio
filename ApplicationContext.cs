using System;
using Microsoft.EntityFrameworkCore;
using FilmStudio.Models;

namespace FilmStudio;

public class ApplicationContext : DbContext
{
    public DbSet<Ad> Ads { get; set; }
    public DbSet<AdType> AdTypes { get; set; }
    public DbSet<CastingActor> CastingActors { get; set; }
    public DbSet<CastingList> CastingLists { get; set; }
    public DbSet<Cinema> Cinemas { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<FilmSet> FilmSets { get; set; }
    public DbSet<Footage> Footages { get; set; }
    public DbSet<Movie> Movies { get; set; }
    public DbSet<Position> Positions { get; set; }
    public DbSet<Props> Props { get; set; }
    public DbSet<RentAgreement> RentAgreements { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("Server=127.0.0.1;Initial Catalog=WebBlog;User ID=SA;Password=MyVeryStrongPassword123!;Trusted_Connection=False;MultipleActiveResultSets=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
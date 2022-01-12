using System;
using Microsoft.EntityFrameworkCore;
using FilmStudio.Models;

namespace FilmStudio;

public class ApplicationContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<CastingList> CastingLists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer("");
}
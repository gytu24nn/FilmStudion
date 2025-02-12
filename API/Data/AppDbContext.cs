using System;
using System.Data.Common;
using API.Models;
using API.Models.DTO;
using API.Models.Film;
using API.Models.FilmCopy;
using API.Models.FilmStudio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Film> Films {get; set;}
    public DbSet<FilmStudio> FilmStudios {get; set;}
    public DbSet<FilmCopy> filmCopies {get; set;} 
    public DbSet<User> users {get; set;}
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Film>()
        .HasKey(f => f.MovieId);

        modelBuilder.Entity<FilmStudio>()
        .HasKey(Fs => Fs.FilmStudioId);

        modelBuilder.Entity<RegisterFilmStudioDTO>()
        .HasKey(Rfs => Rfs.id);
    }
}

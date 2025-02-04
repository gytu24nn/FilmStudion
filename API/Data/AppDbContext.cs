using System;
using System.Data.Common;
using API.Models.Film;
using API.Models.FilmStudio;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Film> Films {get; set;}
    public DbSet<FilmStudio> FilmStudios {get; set;}

    

}

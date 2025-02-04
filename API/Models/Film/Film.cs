using System;
using API.interfaces;

namespace API.Models.Film;

public class Film : IFilm
{
    public int MovieId {get; set;}
    public string MovieName {get; set;} =string.Empty;
    public string MovieDescription {get; set;} = string.Empty;
    public string MovieGenre {get; set;} = string.Empty;
    public int MovieAvailableCopies {get; set;}
    //Denna behövs senare
    //public List<FilmLoan> filmLoans {get; set;} = new List<FilmLoan>();

    public Film(string name, string description, string genre, int availableCopies)
    {
        MovieName = name;
        MovieDescription = description;
        MovieGenre = genre;
        MovieAvailableCopies = availableCopies;
    }
}

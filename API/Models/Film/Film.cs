using System;
using System.ComponentModel.DataAnnotations;
using API.interfaces;
using API.Models.FilmCopy;

namespace API.Models.Film;

public class Film : IFilm
{
    [Key]
    public int MovieId {get; set;}
    public string MovieName {get; set;} =string.Empty;
    public string MovieDescription {get; set;} = string.Empty;
    public string MovieGenre {get; set;} = string.Empty;
    public int MovieAvailableCopies {get; set;}
    public DateTime dateTimeCreatedOrUpdated  {get; set;}
    public List<API.Models.FilmCopy.FilmCopy> filmCopies { get; set; } = new List<API.Models.FilmCopy.FilmCopy>();

    //Denna behövs senare
    //public List<FilmLoan> filmLoans {get; set;} = new List<FilmLoan>();

    public Film() { }

    public Film(string name, string description, string genre, int availableCopies)
    {
        MovieName = name;
        MovieDescription = description;
        MovieGenre = genre;
        MovieAvailableCopies = availableCopies;
    }
}

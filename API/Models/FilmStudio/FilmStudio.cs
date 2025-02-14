using System;
using System.ComponentModel.DataAnnotations;
using API.interfaces;

namespace API.Models.FilmStudio;

public class FilmStudio : IFilmStudio
{
    [Key]
    public int FilmStudioId {get; set;}
    public string FilmStudioName {get; set;} = string.Empty;
    public string FilmStudioEmail {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;
    public string FilmStudioCity {get; set;}
    

    public FilmStudio() { }

    public FilmStudio(string name, string email, string hashedPassword, string city)
    {
        FilmStudioName = name;
        FilmStudioEmail = email;
        FilmStudioCity = city;
        Password = hashedPassword;
    }
}

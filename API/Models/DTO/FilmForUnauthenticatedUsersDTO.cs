using System;
using API.interfaces;

namespace API.Models.DTO;

public class FilmForUnauthenticatedUsersDTO : IFilm
{
    public string MovieName {get; set;} = string.Empty;
    public string MovieDescription {get; set;} = string.Empty;
    public string MovieGenre {get; set;} = string.Empty;
    public int MovieAvailableCopies {get; set;}
    public DateTime dateTime {get; set;}

    public List<API.Models.FilmCopy.FilmCopy> filmCopies { get; set; } = null; // Eller sätt den som null för oautentiserade



   
}

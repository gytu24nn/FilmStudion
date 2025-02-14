using System;
using API.Models.DTO;
using API.Models.FilmCopy;

namespace API.interfaces;

public interface IFilm
{
    public string MovieName {get; set;} 
    public string MovieDescription {get; set;}
    public string MovieGenre {get; set;}
    public int MovieAvailableCopies {get; set;}
    public DateTime dateTime {get; set;}

    public List<FilmCopy> filmCopies {get; set;} 
}

using System;

namespace API.interfaces;

public interface IFilm
{
    public string MovieName {get; set;} 
    public string MovieDescription {get; set;}
    public string MovieGenre {get; set;}
    public int MovieAvailableCopies {get; set;}
    public DateTime dateTime {get; set;}
}

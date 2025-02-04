using System;

namespace API.interfaces;

public interface IFilm
{
    public int MovieId {get; set;}
    public string MovieName {get; set;} 
    public string MovieDescription {get; set;}
    public string MovieGenre {get; set;}
    public int MovieAvailableCopies {get; set;}
}

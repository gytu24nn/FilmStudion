using System;

namespace API.Models.interfaces;

public interface ICreateFilm
{
    public int MovieId {get; set;}
    public string MovieName {get; set;}
    public string MovieDescription {get; set;}
    public string MovieGenre {get; set;}
    public int MovieAvailableCopies {get; set;}
}

using System;

namespace API.Models.interfaces;

public interface ICreateFilm
{
    public int MovieId {get; set;}
    public string MovieTitle {get; set;}
    public string Description {get; set;}
    public string Genre {get; set;}
    public int MovieAvailableCopies {get; set;}
}

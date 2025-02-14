using System;
using API.Models.interfaces;

namespace API.Models.DTO;

public class CreateFilmDTO : ICreateFilm
{
    public int MovieId {get; set;}
    public string MovieName {get; set;} = string.Empty;
    public string MovieDescription {get; set;} = string.Empty;
    public string MovieGenre {get; set;} = string.Empty;
    public int MovieAvailableCopies {get; set;}
}

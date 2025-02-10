using System;
using API.Models.interfaces;

namespace API.Models.DTO;

public class CreateFilmDTO : ICreateFilm
{
    public int MovieId {get; set;}
    public string MovieTitle {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public string Genre {get; set;} = string.Empty;
    public int MovieAvailableCopies {get; set;}
}

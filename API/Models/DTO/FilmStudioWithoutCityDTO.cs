using System;
using API.interfaces;

namespace API.Models.DTO;

public class FilmStudioWithoutCityDTO : IFilmStudio
{
    public int FilmStudioId {get; set;}
    public string FilmStudioName {get; set;} = string.Empty;
    public string FilmStudioEmail {get; set;} = string.Empty;
}

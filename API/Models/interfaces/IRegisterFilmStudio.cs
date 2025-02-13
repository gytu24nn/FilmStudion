using System;

namespace API.Models.interfaces;

public interface IRegisterFilmStudio
{
    public string FilmstudioName {get; set;}
    public string Email {get; set;}
    public string City {get; set;}
}

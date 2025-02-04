using System;

namespace API.Models.interfaces;

public interface IRegisterFilmStudio
{
    public string Name {get; set;}
    public string Email {get; set;}
    public string HashedPassword {get; set;}
}

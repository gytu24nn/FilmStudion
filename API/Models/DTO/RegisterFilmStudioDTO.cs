using System;
using System.ComponentModel.DataAnnotations;
using API.Models.interfaces;

namespace API.Models.DTO;

public class RegisterFilmStudioDTO : IRegisterFilmStudio
{
    public int id {get; set;}
    public required string FilmstudioName {get; set;} = string.Empty;
    public required string Email {get; set;} = string.Empty;
    public required string Password {get; set;} = string.Empty;
    public required string City {get; set;} 
}

using System;
using API.Models.interfaces;

namespace API.Models.DTO;

public class RegisterFilmStudioDTO : IRegisterFilmStudio
{
    public required string Name {get; set;} = string.Empty;
    public required string Email {get; set;} = string.Empty;
    public required string HashedPassword {get; set;} = string.Empty;
}

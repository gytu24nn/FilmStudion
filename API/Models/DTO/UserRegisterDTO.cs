using System;
using API.Models.interfaces;

namespace API.Models.DTO;

public class UserRegisterDTO : IUserRegister
{
    public string UserName {get; set;} = string.Empty; 
    public string Password {get; set;} = string.Empty;
    public string Email {get; set;} = string.Empty;
}

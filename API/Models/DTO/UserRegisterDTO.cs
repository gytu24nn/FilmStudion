using System;
using API.Models.interfaces;

namespace API.Models.DTO;

public class UserRegisterDTO : IUserRegister
{
    public int UserId {get; set;}
    public string UserName {get; set;} = string.Empty;
    public string UserEmail {get; set;} = string.Empty;
    public bool IsAdmin {get; set;}
    public string Password {get; set;} = string.Empty;
}

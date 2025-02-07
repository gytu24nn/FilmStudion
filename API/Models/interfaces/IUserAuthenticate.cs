using System;

namespace API.Models.interfaces;

public interface IUserAuthenticate
{
    public string Username {get; set;}
    public string Password {get; set;}
    public string Email {get; set;}
}

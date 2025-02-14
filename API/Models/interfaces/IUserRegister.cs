using System;

namespace API.Models.interfaces;

public interface IUserRegister
{
    string UserName { get; set; }
    string Password { get; set; }  
    public string Email {get; set;}
}

using System;

namespace API.Models.interfaces;

public interface IUserRegister
{
    string UserName { get; set; }  // Användarnamn (kan vara e-postadress)
    string Password { get; set; }  
    public string Email {get; set;}
}

using System;

namespace API.Models.interfaces;

public interface IUserRegister
{
    public int UserId {get; set;}
    public string UserName {get; set;}
    public string UserEmail {get; set;}
    public bool IsAdmin {get; set;}
}

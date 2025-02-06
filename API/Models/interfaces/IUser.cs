using System;

namespace API.Models.interfaces;

public interface IUser
{
    public int UserId {get; set;}
    public string Role {get; set;}
    public string UserName {get; set;}


}

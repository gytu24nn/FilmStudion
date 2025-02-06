using System;
using API.Models.interfaces;

namespace API.Models.DTO;

public class UserDTO :IUser
{
    public int UserId {get; set;}
    public string Role {get; set;} = string.Empty;
    public string UserName {get; set;} = string.Empty;
    public string UserEmail {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;
}

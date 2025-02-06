using System;
using System.ComponentModel.DataAnnotations;

namespace API.Models;

public class User
{
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string HashedPassword { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
}

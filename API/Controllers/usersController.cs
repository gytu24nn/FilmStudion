using API.Data;
using API.Models;
using API.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usersController : ControllerBase
    {
        private readonly AppDbContext _context; 

        public usersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> RegisterAdmin(UserRegisterDTO userRegisterDTO)
        {
            if(string.IsNullOrWhiteSpace(userRegisterDTO.UserName) ||
            string.IsNullOrWhiteSpace(userRegisterDTO.Password))
            {
                return BadRequest(new {message = "Användarnamn, E-postadress och lösenord krävs."});
            }

            if(!userRegisterDTO.IsAdmin)
            {
                return BadRequest(new {message = "det fungerade ej."});
            }

            var existingAdmin = await _context.users.FirstOrDefaultAsync(u => u.UserEmail == userRegisterDTO.UserName);
            if(existingAdmin != null)
            {
                return Conflict(new { message = "E-postadressen är redan registrerad till ett konto." });
            }

            var passwordHasher = new PasswordHasher<string>();
            string hashedPassword = passwordHasher.HashPassword(null, userRegisterDTO.Password);

            var newAdmin = new User 
            {
                UserName = userRegisterDTO.UserName,
                HashedPassword = hashedPassword,
                IsAdmin = true
            };

            _context.users.Add(newAdmin);
            await _context.SaveChangesAsync();

            var UserDTO = new UserDTO 
            {
                UserId = newAdmin.UserId,
                UserName = newAdmin.UserName,
                Role = "admin"
            };

            return Ok(UserDTO);
        }
    }
}

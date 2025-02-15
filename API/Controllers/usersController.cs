using API.Data;
using API.Models;
using API.Models.DTO;
using API.Models.FilmStudio;
using API.Models.interfaces;
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
        private readonly PasswordHasher<string> _passwordHasher = new();

        private readonly string AdminToken = "Admin-1234-4321";
        private readonly string FilmstudioToken = "Filmstudio-5678-8765";

        public usersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> RegisterAdmin(UserRegisterDTO userRegisterDTO)
        {
            if (string.IsNullOrWhiteSpace(userRegisterDTO.UserName) ||
            string.IsNullOrWhiteSpace(userRegisterDTO.Password) ||
            string.IsNullOrWhiteSpace(userRegisterDTO.Email))
            {
                return BadRequest(new { message = "Användarnamn, E-postadress och lösenord krävs." });
            }


            var existingAdmin = await _context.users.FirstOrDefaultAsync(u => u.UserEmail == userRegisterDTO.Email);
            if (existingAdmin != null)
            {
                return Conflict(new { message = "E-postadressen är redan registrerad till ett konto." });
            }

            var passwordHasher = new PasswordHasher<string>();
            string hashedPassword = passwordHasher.HashPassword(null, userRegisterDTO.Password);

            var newAdmin = new User
            {
                UserName = userRegisterDTO.UserName,
                UserEmail = userRegisterDTO.Email,
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

        [HttpPost("authenticate")]
        public async Task<ActionResult<IUser>> AutheticateFilmstudioAndAdmin(UserAuthenticateDTO userAuthenticateDTO)
        {
            if (string.IsNullOrWhiteSpace(userAuthenticateDTO.Email) ||
                string.IsNullOrWhiteSpace(userAuthenticateDTO.Username) ||
                string.IsNullOrWhiteSpace(userAuthenticateDTO.Password))
            {
                return BadRequest(new { message = "Användarnamn, E-postadress och lösenord krävs." });
            }

            // Autentisera Admin
            var admin = await _context.users.FirstOrDefaultAsync(u => u.UserEmail == userAuthenticateDTO.Email);
            if (admin != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(null, admin.HashedPassword, userAuthenticateDTO.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    return Ok(new
                    {
                        UserId = admin.UserId,
                        Role = "Admin",
                        UserName = admin.UserName,
                        Token = AdminToken
                    });
                }
            }

            // Autentisera Filmstudio
            var filmStudio = await _context.FilmStudios.FirstOrDefaultAsync(fs => fs.FilmStudioEmail == userAuthenticateDTO.Email);
            if (filmStudio != null)
            {
                var result = _passwordHasher.VerifyHashedPassword(null, filmStudio.Password, userAuthenticateDTO.Password);
                if (result == PasswordVerificationResult.Success)
                {
                    return Ok(new
                    {
                        UserId = filmStudio.FilmStudioId,
                        Role = "Filmstudio",
                        UserName = filmStudio.FilmStudioName,
                        filmStudio = new
                        {
                            FilmStudioId = filmStudio.FilmStudioId,
                            Name = filmStudio.FilmStudioName,
                            Email = filmStudio.FilmStudioEmail,
                            City = filmStudio.FilmStudioCity // Lägg till denna i din modell om det behövs
                        },
                        Token = FilmstudioToken
                    });
                }
            }

            return Unauthorized(new { message = "Felaktig E-postadress eller lösenord." });
        }

    }
}

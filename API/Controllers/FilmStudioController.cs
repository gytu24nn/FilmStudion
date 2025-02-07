using API.Data;
using API.interfaces;
using API.Models;
using API.Models.DTO;
using API.Models.FilmStudio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmStudioController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FilmStudioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]

public async Task<ActionResult<IFilmStudio>> RegisterFilmStudio(RegisterFilmStudioDTO FilmStudioDTO)
{
    if (string.IsNullOrWhiteSpace(FilmStudioDTO.Name) ||
        string.IsNullOrWhiteSpace(FilmStudioDTO.Email) ||
        string.IsNullOrWhiteSpace(FilmStudioDTO.Password) ||
        string.IsNullOrWhiteSpace(FilmStudioDTO.City))
    {
        return BadRequest(new { message = "Alla fälten måste vara ifyllda." });
    }

    var existingFilmStudio = await _context.FilmStudios
        .FirstOrDefaultAsync(fs => fs.FilmStudioEmail == FilmStudioDTO.Email);

    if (existingFilmStudio != null)
    {
        return Conflict(new { message = "E-postadressen är redan registrerad." });
    }

    var passwordHasher = new PasswordHasher<string>();
    var hashedPassword = passwordHasher.HashPassword(null, FilmStudioDTO.Password);

    var newFilmStudio = new FilmStudio(
        FilmStudioDTO.Name,
        FilmStudioDTO.Email,
        hashedPassword,
        FilmStudioDTO.City
    );

    _context.FilmStudios.Add(newFilmStudio);
    await _context.SaveChangesAsync();

    return Ok(new
    {
        filmstudioId = newFilmStudio.FilmStudioId,
        filmStudioName = newFilmStudio.FilmStudioName,
        filmStudioEmail = newFilmStudio.FilmStudioEmail,
        filmStudioCity = newFilmStudio.FilmStudioCity
    });
}
    }
}

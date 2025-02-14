using API.Data;
using API.interfaces;
using API.Models;
using API.Models.DTO;
using API.Models.FilmStudio;
using API.Models.RentedFilmsCopies;
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
        private readonly string AdminToken = "Admin-1234-4321";
        private readonly string FilmstudioToken = "Filmstudio-5678-8765";


        public FilmStudioController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]

        public async Task<ActionResult<IFilmStudio>> RegisterFilmStudio(RegisterFilmStudioDTO FilmStudioDTO)
        {
            if (string.IsNullOrWhiteSpace(FilmStudioDTO.FilmstudioName) ||
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
                FilmStudioDTO.FilmstudioName,
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

        [HttpGet("{id}")]
        public ActionResult GetASpecificFilmStudio(int id)
        {
            var headers = Request.Headers;
            string token = string.Empty;

            if (headers.ContainsKey("Authorization"))
            {
                token = headers["Authorization"].ToString();
                if (token.StartsWith("Bearer "))
                {
                    token = token.Substring("Bearer ".Length);
                }
            }

            var filmstudio = _context.FilmStudios.FirstOrDefault(studio => studio.FilmStudioId == id);
            if (filmstudio == null)
            {
                return NotFound(new { message = "Ingen filmstudio med det id hittades! Försök igen!" });
            }

            // Skapa FilmStudioDTO med grunddata (som alltid returneras)
            var filmStudioDTO = new FilmStudioDTO
            {
                FilmStudioId = filmstudio.FilmStudioId,
                FilmStudioName = filmstudio.FilmStudioName,
                FilmStudioEmail = filmstudio.FilmStudioEmail
            };

            // Om användaren är admin
            if (!string.IsNullOrEmpty(token) && token == AdminToken)
            {
                filmStudioDTO.FilmStudioCity = filmstudio.FilmStudioCity;
                filmStudioDTO.RentedFilmCopies = _context.filmCopies
                    .Where(copy => copy.FilmStudioId == filmstudio.FilmStudioId && copy.IsRented)
                    .Select(copy => new RentedFilmCopies
                    {
                        FilmCopyId = copy.FilmCopyId,
                        MovieId = copy.FilmId
                    }).ToList();
            }
            // Om användaren är en filmstudio som matchar id
            else if (!string.IsNullOrEmpty(token) && IsFilmStudioTokenValid(token, id))
            {
                filmStudioDTO.FilmStudioCity = filmstudio.FilmStudioCity;
                filmStudioDTO.RentedFilmCopies = _context.filmCopies
                    .Where(copy => copy.FilmStudioId == filmstudio.FilmStudioId && copy.IsRented)
                    .Select(copy => new RentedFilmCopies
                    {
                        FilmCopyId = copy.FilmCopyId,
                        MovieId = copy.FilmId
                    }).ToList();
            }
            // Annars (obehörig filmstudio)
            else
            {
                filmStudioDTO.FilmStudioCity = null;
                filmStudioDTO.RentedFilmCopies = new List<RentedFilmCopies>();
            }

            return Ok(filmStudioDTO);
        }

        private bool IsFilmStudioTokenValid(string token, int filmStudioId)
        {
            return token == FilmstudioToken;
        }




    }
}

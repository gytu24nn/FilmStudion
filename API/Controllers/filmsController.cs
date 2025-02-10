using API.Data;
using API.Models.DTO;
using API.Models.Film;
using API.Models.FilmCopy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class filmsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly string AdminToken = "Admin-1234-4321";
        private readonly string FilmstudioToken = "Filmstudio-5678-8765";

        public filmsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult> CreateFilm([FromBody] CreateFilmDTO createFilmDTO)
        {
            var headers = Request.Headers;

            if(!headers.ContainsKey("Authorization"))
            {
                return Unauthorized(new {message = "Ingen behörighet."});
            }

            var token = headers["Authorization"].ToString();

            if(token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length);
            }

            if(token != AdminToken)
            {
                return Unauthorized(new {message = "Felaktig token, försök igen!"});
            }

            var newFilm = new Film 
            {
                MovieName = createFilmDTO.MovieTitle,
                MovieDescription = createFilmDTO.Description,
                MovieGenre = createFilmDTO.Genre,
                dateTime = DateTime.UtcNow

            };

            _context.Films.Add(newFilm);
            await _context.SaveChangesAsync();

            for (int i = 0; i < createFilmDTO.MovieAvailableCopies; i++)
            {
                newFilm.filmCopies.Add(new FilmCopy
                {
                    FilmId = newFilm.MovieId
                });
            }

            newFilm.MovieAvailableCopies = newFilm.filmCopies.Count;

            await _context.SaveChangesAsync();

            return Ok(new 
            { 
                newFilm.MovieId,
                    newFilm.MovieName,
                    newFilm.MovieDescription,
                    newFilm.MovieGenre,
                    newFilm.MovieAvailableCopies,
                    newFilm.dateTime,
                    filmCopies = newFilm.filmCopies.Select(copy => new
                    {
                        copy.FilmCopyId,
                        copy.FilmId
                    }).ToList()
            });
            

        }
    }
}

// Här kan du också lägga till en kontroll för filmstudio-token om det behövs
    // if (token != "Filmstudio-5678-8765")
    // {
    //     return Unauthorized(new { message = "Felaktig token för filmstudio" });
    // }
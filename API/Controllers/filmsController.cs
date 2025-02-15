using System.Security.Cryptography.X509Certificates;
using System.Text;
using API.Data;
using API.interfaces;
using API.Models.DTO;
using API.Models.Film;
using API.Models.FilmCopy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.ValueGeneration.Internal;

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

            if (!headers.ContainsKey("Authorization"))
            {
                return Unauthorized(new { message = "Ingen behörighet." });
            }

            var token = headers["Authorization"].ToString();

            if (token.StartsWith("Bearer "))
            {
                token = token.Substring("Bearer ".Length);
            }

            if (token != AdminToken)
            {
                return Unauthorized(new { message = "Felaktig token, försök igen!" });
            }

            var newFilm = new Film
            {
                MovieName = createFilmDTO.MovieName,
                MovieDescription = createFilmDTO.MovieDescription,
                MovieGenre = createFilmDTO.MovieGenre,
                dateTimeCreatedOrUpdated = DateTime.UtcNow

            };

            _context.Films.Add(newFilm);
            await _context.SaveChangesAsync();

            for (int i = 0; i < createFilmDTO.MovieAvailableCopies; i++)
            {
                var newCopy = new FilmCopy
                {
                    FilmId = newFilm.MovieId,
                    IsAvailable = true
                };
                _context.filmCopies.Add(newCopy);
            }

            await _context.SaveChangesAsync();

            newFilm.MovieAvailableCopies = newFilm.filmCopies.Count;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                newFilm.MovieId,
                newFilm.MovieName,
                newFilm.MovieDescription,
                newFilm.MovieGenre,
                newFilm.MovieAvailableCopies,
                newFilm.dateTimeCreatedOrUpdated,
                filmCopies = newFilm.filmCopies.Select(copy => new FilmCopy
                {
                    FilmCopyId = copy.FilmCopyId,
                    FilmId = copy.FilmId
                }).ToList()
            });


        }

        [HttpGet]
        public ActionResult<IEnumerable<object>> GetAllFilms()
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

            var films = _context.Films.ToList();
            List<object> filmDTOs = new List<object>();

            if (!string.IsNullOrEmpty(token) && (token == AdminToken || token == FilmstudioToken))
            {
                foreach (var film in films)
                {
                    var filmDTO = new FilmForAuthenticatedUsersDTO
                    {
                        MovieName = film.MovieName,
                        MovieDescription = film.MovieDescription,
                        MovieGenre = film.MovieGenre,
                        MovieAvailableCopies = film.MovieAvailableCopies,
                        dateTimeCreatedOrUpdated = film.dateTimeCreatedOrUpdated,
                        filmCopies = _context.filmCopies
                        .Where(copy => copy.FilmId == film.MovieId) // Här ändras FilmCopyId till FilmId
                        .Select(copy => new FilmCopy
                        {
                            FilmCopyId = copy.FilmCopyId,
                        }).ToList()

                    };

                    filmDTOs.Add(filmDTO);
                }
            }
            else
            {
                foreach (var film in films)
                {
                    var filmDTO = new FilmForUnauthenticatedUsersDTO
                    {
                        MovieName = film.MovieName,
                        MovieDescription = film.MovieDescription,
                        MovieGenre = film.MovieGenre,
                        MovieAvailableCopies = film.MovieAvailableCopies,
                        dateTimeCreatedOrUpdated = film.dateTimeCreatedOrUpdated
                    };

                    filmDTOs.Add(filmDTO);
                }
            }

            return Ok(filmDTOs);

        }

        [HttpGet("{id}")]
        public ActionResult<object> GetFilmById(int id)
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

            var film = _context.Films.FirstOrDefault(f => f.MovieId == id);
            if (film == null)
            {
                return NotFound(new { message = "Ingen film med det ID finns. Försök igen" });
            }

            object filmDTO;

            if (!string.IsNullOrEmpty(token) && (token == AdminToken || token == FilmstudioToken))
            {
                filmDTO = new FilmForAuthenticatedUsersDTO
                {
                    MovieName = film.MovieName,
                    MovieDescription = film.MovieDescription,
                    MovieGenre = film.MovieGenre,
                    MovieAvailableCopies = film.MovieAvailableCopies,
                    dateTimeCreatedOrUpdated = film.dateTimeCreatedOrUpdated,
                    filmCopies = _context.filmCopies
                    .Where(copy => copy.FilmId == film.MovieId) // Korrekt filtrering
                    .Select(copy => new FilmCopy
                    {
                        FilmCopyId = copy.FilmCopyId,
                        FilmId = copy.FilmId,
                        IsAvailable = copy.IsAvailable
                    }).ToList()
                };
            }
            else
            {
                filmDTO = new FilmForUnauthenticatedUsersDTO
                {
                    MovieName = film.MovieName,
                    MovieDescription = film.MovieDescription,
                    MovieGenre = film.MovieGenre,
                    MovieAvailableCopies = film.MovieAvailableCopies,
                    dateTimeCreatedOrUpdated = film.dateTimeCreatedOrUpdated
                };
            }

            return Ok(filmDTO);
        }

        [HttpPatch("{id}")]
        public ActionResult<IFilm> UpdateFilm(int id, [FromBody] FilmForUnauthenticatedUsersDTO updatedFilm)
        {
            // Autentisering och tokenvalidering
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

            if (string.IsNullOrEmpty(token) || token != AdminToken)
            {
                return Unauthorized(new { message = "Ej behörig." });
            }

            // Hämta filmen från databasen
            var film = _context.Films.FirstOrDefault(f => f.MovieId == id);
            if (film == null)
            {
                return NotFound(new { message = "Ingen film med det ID finns. Försök igen!" });
            }

            film.MovieName = updatedFilm.MovieName ?? film.MovieName;
            film.MovieDescription = updatedFilm.MovieDescription ?? film.MovieDescription;
            film.MovieGenre = updatedFilm.MovieGenre ?? film.MovieGenre;

            if (updatedFilm.MovieAvailableCopies >= 0)
            {
                var existingCopies = _context.filmCopies.Where(copy => copy.FilmId == film.MovieId).ToList();
                _context.filmCopies.RemoveRange(existingCopies);
                _context.SaveChanges();

                for (int i = 0; i < updatedFilm.MovieAvailableCopies; i++)
                {
                    var newCopy = new FilmCopy
                    {
                        IsAvailable = true,
                        FilmId = film.MovieId
                    };
                    film.filmCopies.Add(newCopy);
                }

                film.MovieAvailableCopies = updatedFilm.MovieAvailableCopies;

                _context.SaveChanges();
            }

            film.dateTimeCreatedOrUpdated = DateTime.Now;

            _context.SaveChanges();

            var updatedFilmDTO = new FilmForUnauthenticatedUsersDTO
            {
                MovieName = film.MovieName,
                MovieDescription = film.MovieDescription,
                MovieGenre = film.MovieGenre,
                MovieAvailableCopies = film.MovieAvailableCopies,
                dateTimeCreatedOrUpdated = film.dateTimeCreatedOrUpdated
            };

            return Ok(updatedFilmDTO);
        }

        [HttpPost("rent")]
        public async Task<IActionResult> RentFilm([FromQuery] int id, [FromQuery] int studioid)
        {
            var headers = Request.Headers;
            string token = string.Empty;

            // Hämta token från Authorization headern
            if (headers.ContainsKey("Authorization"))
            {
                token = headers["Authorization"].ToString();

                // Om token börjar med "Bearer ", extrahera själva token
                if (token.StartsWith("Bearer "))
                {
                    token = token.Substring("Bearer ".Length);
                }
            }

            // Kontrollera om token är tom
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Ej behörig." });
            }

            // Kontrollera om token är för admin (admin får inte hyra filmer)
            if (token == AdminToken)
            {
                return Unauthorized(new { message = "Ej behörig som admin att låna filmer." });
            }

            // Kontrollera om token är för filmstudio
            if (token == FilmstudioToken)
            {
                var filmstudio = await _context.FilmStudios.FirstOrDefaultAsync(f => f.FilmStudioId == studioid);

                // Kontrollera om filmstudion finns
                if (filmstudio == null)
                {
                    return Unauthorized(new { message = "Ingen filmstudio matchar id:t." });
                }

                // Hämta filmen från databasen
                var film = await _context.Films
                    .Include(f => f.filmCopies)
                    .FirstOrDefaultAsync(f => f.MovieId == id);

                // Om filmen inte finns, returnera Conflict
                if (film == null)
                {
                    return Conflict(new { message = "Filmen kunde inte hittas." });
                }

                // Kontrollera om det finns lediga kopior av filmen
                var availableCopy = film.filmCopies.FirstOrDefault(c => c.IsAvailable);

                // Om det inte finns lediga kopior, returnera Conflict
                if (availableCopy == null)
                {
                    return Conflict(new { message = "Inga tillgängliga kopior av filmen finns." });
                }

                // Kontrollera om filmstudion redan hyr en kopia av samma film
                if (film.filmCopies.Any(c => c.FilmStudioId == studioid))
                {
                    return StatusCode(403, new { message = "Filmstudion hyr redan en kopia av denna film." });
                }

                // Markera filmen som uthyrd och tilldela filmstudion en kopia
                availableCopy.IsAvailable = false;
                availableCopy.FilmStudioId = studioid;

                // Spara ändringarna i databasen
                await _context.SaveChangesAsync();

                // Returnera OK om allt gick bra
                return Ok(new { message = "Filmen har lånats ut.", filmId = id, studioId = studioid });
            }

            // Om token inte matchar någon av de förväntade, returnera Unauthorized
            return Unauthorized(new { message = "Ogiltig token." });
        }

    }
}
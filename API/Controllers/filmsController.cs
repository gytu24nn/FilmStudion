using System.Text;
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

        [HttpGet]
        public ActionResult<IEnumerable<object>> GetAllFilms()
        {
            var headers = Request.Headers;
            string token = string.Empty;

            if(headers.ContainsKey("Authorization"))
            {
                token = headers["Authorization"].ToString();

                if(token.StartsWith("Bearer "))
                {
                    token = token.Substring("Bearer ".Length);
                }
            }

            var films = _context.Films.ToList();
            List<object> filmDTOs = new List<object>();

            if(!string.IsNullOrEmpty(token) && (token == AdminToken || token == FilmstudioToken))
            {
                foreach (var film in films)
                {
                    var filmDTO = new FilmForAuthenticatedUsersDTO
                    {
                        MovieName = film.MovieName,
                        MovieDescription = film.MovieDescription,
                        MovieGenre = film.MovieGenre,
                        MovieAvailableCopies = film.MovieAvailableCopies,
                        dateTime = film.dateTime,
                        FilmCopies = _context.filmCopies
                        .Where(copy => copy.FilmId == film.MovieId)
                        .Select(copy => new RentedFilmCopyDTO
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
                        dateTime = film.dateTime
                    };

                    filmDTOs.Add(filmDTO);                    
                }
            }

            return Ok(filmDTOs);

        }
    }
}
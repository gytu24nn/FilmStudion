using System.Text;
using API.Data;
using API.interfaces;
using API.Models.DTO;
using API.Models.Film;
using API.Models.FilmCopy;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
                MovieName = createFilmDTO.MovieName,
                MovieDescription = createFilmDTO.MovieDescription,
                MovieGenre = createFilmDTO.MovieGenre,
                dateTimeCreatedOrUpdated  = DateTime.UtcNow

            };

            _context.Films.Add(newFilm);
            await _context.SaveChangesAsync();

            for (int i = 0; i < createFilmDTO.MovieAvailableCopies; i++)
            {
                newFilm.filmCopies.Add(new FilmCopy
                {
                    FilmId = newFilm.MovieId,
                    IsAvailable = true
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
                    newFilm.dateTimeCreatedOrUpdated ,
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
                        dateTimeCreatedOrUpdated  = film.dateTimeCreatedOrUpdated ,
                        filmCopies = _context.filmCopies
                        .Where(copy => copy.FilmCopyId == film.MovieId)
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
                        dateTimeCreatedOrUpdated  = film.dateTimeCreatedOrUpdated 
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

            if(headers.ContainsKey("Authorization"))
            {
                token = headers["Authorization"].ToString();

                if(token.StartsWith("Bearer "))
                {
                    token = token.Substring("Bearer ".Length);
                }
            }

            var film = _context.Films.FirstOrDefault(f => f.MovieId == id);
            if(film == null)
            {
                return NotFound(new {message = "Ingen film med det ID finns. Försök igen"});
            }

            object filmDTO;

            if(!string.IsNullOrEmpty(token) && (token == AdminToken || token == FilmstudioToken))
            {
                filmDTO = new FilmForAuthenticatedUsersDTO
                {
                    MovieName = film.MovieName,
                    MovieDescription = film.MovieDescription,
                    MovieGenre = film.MovieGenre,
                    MovieAvailableCopies = film.MovieAvailableCopies,
                    dateTimeCreatedOrUpdated  = film.dateTimeCreatedOrUpdated ,
                    filmCopies = _context.filmCopies
                    .Where(copy => copy.FilmCopyId == film.MovieId)
                    .Select(copy => new FilmCopy
                    {
                        FilmCopyId = copy.FilmCopyId 
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
                    dateTimeCreatedOrUpdated  = film.dateTimeCreatedOrUpdated 
                };
            }

            return Ok(filmDTO);
        }

        [HttpPatch("{id}")]
        public ActionResult<IFilm> UpdateFilm(int id, [FromBody] FilmForUnauthenticatedUsersDTO updatedFilm)
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

            if(string.IsNullOrEmpty(token) || token != AdminToken)
            {
                return Unauthorized(new {message = "Ej behörig."});
            }

            var film = _context.Films.FirstOrDefault(f => f.MovieId == id);
            if(film == null)
            {
                return NotFound(new {message = "Ingen film med det ID finns. Försök igen!"});
            }

            int oldAvailableCopies = film.MovieAvailableCopies;

            film.MovieName = updatedFilm.MovieName ?? film.MovieName;
            film.MovieDescription = updatedFilm.MovieDescription ?? film.MovieDescription;
            film.MovieGenre = updatedFilm.MovieGenre ?? film.MovieGenre;
            film.MovieAvailableCopies = updatedFilm.MovieAvailableCopies;
            film.dateTimeCreatedOrUpdated  = DateTime.Now; 

            _context.SaveChanges();

            var updatedFilmDTO = new FilmForUnauthenticatedUsersDTO
            {
                MovieName = film.MovieName,
                MovieDescription = film.MovieDescription,
                MovieGenre = film.MovieGenre,
                MovieAvailableCopies = film.MovieAvailableCopies,
                dateTimeCreatedOrUpdated  = film.dateTimeCreatedOrUpdated 
            };

            return Ok(updatedFilmDTO);
        }
    }
}
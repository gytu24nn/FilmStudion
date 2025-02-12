using API.Data;
using API.Models.DTO;
 using Microsoft.AspNetCore.Http;
 using Microsoft.AspNetCore.Mvc;

 namespace API.Controllers
 {
     [Route("api/[controller]")]
     [ApiController]
     public class FilmstudiosController : ControllerBase
     {
        private readonly AppDbContext _context;
         private readonly string AdminToken = "Admin-1234-4321";

        public FilmstudiosController(AppDbContext context)
        {
            _context = context;
        }

[HttpGet]
public ActionResult GetAllFilmStudios()
{
    var headers = Request.Headers;
    string token = null;

    // Kontrollera om Authorization-headern finns
    if (headers.ContainsKey("Authorization"))
    {
        token = headers["Authorization"].ToString();

        // Ta bort "Bearer " om det finns
        if (token.StartsWith("Bearer "))
        {
            token = token.Substring("Bearer ".Length);
        }
    }

    // Kontrollera om användaren är admin
    if (token == AdminToken)
    {
        // Returnera all data inklusive RentedFilmCopies och City
        var adminFilmStudios = _context.FilmStudios
            .Select(studio => new AdminFilmStudioDTO
            {
                FilmStudioId = studio.FilmStudioId,
                FilmStudioName = studio.FilmStudioName,
                FilmStudioEmail = studio.FilmStudioEmail,
                FilmStudioCity = studio.FilmStudioCity,
                RentedFilmCopies = _context.filmCopies
                    .Where(copy => copy.FilmStudioId == studio.FilmStudioId && copy.IsRented)  // Filtret för uthyrda filmer
                    .Select(copy => new RentedFilmCopyDTO
                    {
                        FilmCopyId = copy.FilmCopyId,
                        MovieId = copy.FilmId
                    }).ToList()
            }).ToList();

        return Ok(adminFilmStudios);
    }

    // Om användaren är filmstudio eller oautentiserad, returnera bara nödvändiga egenskaper
    var filmStudios = _context.FilmStudios
        .Select(studio => new FilmStudioWithoutCityDTO
        {
            FilmStudioId = studio.FilmStudioId,
            FilmStudioName = studio.FilmStudioName,
            FilmStudioEmail = studio.FilmStudioEmail
        }).ToList();

    return Ok(filmStudios);
}



    }
}

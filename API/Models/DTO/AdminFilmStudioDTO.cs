using System;
using System.Linq.Expressions;
using API.interfaces;

namespace API.Models.DTO;

public class AdminFilmStudioDTO : IFilmStudio
{
    public int FilmStudioId { get; set; }
    public string FilmStudioName { get; set; } = string.Empty;
    public string FilmStudioEmail { get; set; } = string.Empty;

    // Utöver interfacet
    public string FilmStudioCity { get; set; } = string.Empty;
    public List<API.Models.RentedFilmsCopies.RentedFilmCopies> RentedFilmCopies { get; set; } = new List<API.Models.RentedFilmsCopies.RentedFilmCopies>();
}

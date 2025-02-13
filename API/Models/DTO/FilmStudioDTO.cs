using System;
using API.interfaces;

namespace API.Models.DTO;

public class FilmStudioDTO : IFilmStudio
{
    public int FilmStudioId {get; set;} 
    public string FilmStudioName {get; set;} = string.Empty;
    public string FilmStudioEmail {get; set;} = string.Empty;
    public string FilmStudioCity {get; set;} = string.Empty;

    public List<API.Models.DTO.RentedFilmCopyDTO> RentedFilmCopies { get; set; } = new List<API.Models.DTO.RentedFilmCopyDTO>();
}

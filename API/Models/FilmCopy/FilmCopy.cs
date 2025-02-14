using System;
using System.ComponentModel.DataAnnotations.Schema;
using API.Models.Film;
using API.Models.FilmStudio;  

namespace API.Models.FilmCopy
{
    public class FilmCopy
    {
        public int FilmCopyId { get; set; }
        public int FilmId { get; set; }  // Tidigare MovieId
    public bool IsAvailable { get; set; } = true;
    public int? FilmStudioId { get; set; } 
    public bool IsRented { get; set; } = false;
    }
}


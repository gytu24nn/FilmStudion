using System;
using System.ComponentModel.DataAnnotations.Schema;
using API.Models.Film;
using API.Models.FilmStudio;  

namespace API.Models.FilmCopy;

public class FilmCopy
{
    public int FilmCopyId {get; set;}
    public int FilmId {get; set;}
    [ForeignKey("FilmId")]
    public API.Models.Film.Film Film { get; set; }

    public int FilmStudioId {get; set;}
    public API.Models.FilmStudio.FilmStudio FilmStudio {get; set;}

    public bool IsRented {get; set;} 

}

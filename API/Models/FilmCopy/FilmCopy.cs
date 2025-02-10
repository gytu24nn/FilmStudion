using System;
using System.ComponentModel.DataAnnotations.Schema;
using API.Models.Film;

namespace API.Models.FilmCopy;

public class FilmCopy
{
    public int FilmCopyId {get; set;}
    public int FilmId {get; set;}
    [ForeignKey("FilmId")]
    public List<API.Models.Film.Film> Film { get; set; } = new List<API.Models.Film.Film>();

}

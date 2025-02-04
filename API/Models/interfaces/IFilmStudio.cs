using System;

namespace API.interfaces;

public interface IFilmStudio
{
    public int FilmStudioId {get; set;}
    public string FilmStudioName {get; set;}
    public string FilmStudioEmail {get; set;}
    string HashedPassword {get; set;}
    //Denna behövs senare
    //public List<FilmLoan> filmLoans {get; set; }
}

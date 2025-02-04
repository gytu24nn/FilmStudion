using System;
using API.interfaces;

namespace API.Models.FilmStudio;

public class FilmStudio : IFilmStudio
{
    public int FilmStudioId {get; set;}
    public string FilmStudioName {get; set;} = string.Empty;
    public string FilmStudioEmail {get; set;} = string.Empty;
    public string HashedPassword {get; set;}
    //Denna behövs senare
    //public List<FilmLoan> filmLoans {get; set;} = new List<FilmLoan>();

    public FilmStudio(string name, string email, string hashedPassword)
    {
        FilmStudioName = name;
        FilmStudioEmail = email;
        HashedPassword = hashedPassword;
    }
}
